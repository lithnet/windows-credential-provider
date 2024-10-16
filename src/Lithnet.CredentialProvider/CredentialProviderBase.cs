using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// This class represents the base of a credential provider. Inherit from this class to create a new credential provider.
    /// </summary>
    public abstract partial class CredentialProviderBase
    {
        private readonly ICredentialProviderLogger logger;
        private ICredentialProviderEvents CredentialProviderEvents;
        private ICredentialProviderUserArray credentialProviderUsers;

        private IntPtr credentialProviderEventsAdviseContext;
        private bool notifyOnTileCollectionChange;
        private List<CredentialTile> tiles;

        internal ICredentialProviderLoggerFactory LoggerFactory { get; }

        /// <summary>
        /// Gets the GUID of the credential provider
        /// </summary>
        public Guid CredentialProviderId { get; private set; }

        /// <summary>
        /// Gets the list of users that were supplied to the credential provider from LogonUI
        /// </summary>
        public IReadOnlyList<CredentialProviderUser> SuppliedUsers { get; private set; }

        /// <summary>
        /// Gets the usage scenario communicated by LogonUI or CreDUI
        /// </summary>
        public UsageScenario UsageScenario { get; private set; }

        /// <summary>
        /// Gets the flags provided by CredUI
        /// </summary>
        public CredUIWinFlags CredUIFlags { get; private set; }

        /// <summary>
        /// Gets the list of controls used by this credential provider
        /// </summary>
        public ControlCollection Controls { get; private set; }

        /// <summary>
        /// Gets a list of the tiles created for this credential provider
        /// </summary>
        public IReadOnlyList<CredentialTile> Tiles { get; private set; }

        /// <summary>
        /// Gets a value that indicates if this credential provider is loaded by Logon UI
        /// </summary>
        public bool IsLogonUI => this.UsageScenario == UsageScenario.Logon;

        /// <summary>
        /// Gets a value that indicates if the credential provider is loaded by Consent UI (eg UAC prompt)
        /// </summary>
        public bool IsConsentUI => this.UsageScenario == UsageScenario.CredUI && ConsentUIData.IsConsentUIParent();

        /// <summary>
        /// Provides access to the serialized input data provided by CredUI
        /// </summary>
        public CredentialSerialization InboundSerialization { get; private set; }

        protected CredentialProviderBase()
        {
            this.LoggerFactory = this.GetLoggerFactory();


            this.logger = this.LoggerFactory.CreateLogger(this.GetType());
            var guidAttribute = (GuidAttribute)(this.GetType().GetCustomAttribute(typeof(GuidAttribute)));

            if (guidAttribute == null)
            {
                throw new InvalidOperationException("The Credential Provider must have a [Guid(\"xxx\")] attribute assigned to its class");
            }

            this.CredentialProviderId = Guid.Parse(guidAttribute.Value);
        }

        /// <summary>
        /// Gets a logger factory. Override this method and provide an implementation of <c ref="ILoggerFactory"/> to enable credential provider logging
        /// </summary>
        /// <returns>An ILoggerFactory instance</returns>
        protected virtual ICredentialProviderLoggerFactory GetLoggerFactory() { return TraceLoggerFactory.Instance; }

        /// <summary>
        /// Gets a value indicating if the credential provider supports the <c ref="UsageScenario"/> provided by LogonUI or CredUI
        /// </summary>
        /// <param name="cpus">The usage scenario</param>
        /// <param name="dwFlags">Additional flags provided by CredUI</param>
        /// <returns>True, if the credential provider can handle the specified usage scenario, or false if it cannot</returns>
        public abstract bool IsUsageScenarioSupported(UsageScenario cpus, CredUIWinFlags dwFlags);

        /// <summary>
        /// Gets the set of controls used by this provider to render the UI
        /// </summary>
        /// <param name="cpus">The usage scenario to obtain the controls for</param>
        /// <returns>A collection of ControlBase objects</returns>
        public abstract IEnumerable<ControlBase> GetControls(UsageScenario cpus);

        /// <summary>
        /// Gets a value that indicates if the specified user should have a tile rendered for them by this UI
        /// </summary>
        /// <param name="user">Details of the user provided by LogonUI or CredUI</param>
        /// <returns>A value indicating if this credential provider should show a tile for this user</returns>
        public abstract bool ShouldIncludeUserTile(CredentialProviderUser user);

        /// <summary>
        /// Gets a value that indicates if the credential provider should show a generic tile. That is, a tile that is not associated with a specific user.
        /// </summary>
        public abstract bool ShouldIncludeGenericTile();
        
        /// <summary>
        /// Notifies LogonUI that one of more of the tile items has been modified, and should be reloaded
        /// </summary>
        public void ReloadUserTiles()
        {
            this.NotifyHostOfTileCollectionChange();
        }

        /// <summary>
        /// Adds additional user tiles to the collection, and notifies LogonUI that new tiles are available
        /// </summary>
        /// <param name="tiles">One or more credential tiles to add</param>
        public void AddAdditionalUserTiles(params CredentialTile[] tiles)
        {
            if (tiles == null)
            {
                return;
            }

            foreach (var tile in tiles)
            {
                if (!this.tiles.Contains(tile))
                {
                    this.tiles.Add(tile);
                    tile.Initialize();
                }
            }

            this.NotifyHostOfTileCollectionChange();
        }

        /// <summary>
        /// Removes one or more user tiles, and notifies LogonUI that tiles have been removed
        /// </summary>
        /// <param name="tiles">The credential tiles to remove</param>
        public void RemoveUserTiles(params CredentialTile[] tiles)
        {
            if (tiles == null)
            {
                return;
            }

            foreach (var tile in tiles)
            {
                this.tiles.Remove(tile);
            }

            this.NotifyHostOfTileCollectionChange();
        }

        /// <summary>
        /// This method is used to generate the generic tile for this credential provider. This is called when <c ref="ShouldIncludeGenericTile"/> return true
        /// </summary>
        public abstract CredentialTile CreateGenericTile();

        /// <summary>
        /// Creates a credential tile for the specified user
        /// </summary>
        /// <param name="user">The user to create the tile for</param>
        public abstract CredentialTile2 CreateUserTile(CredentialProviderUser user);

        /// <summary>
        /// This method is called when the LogonUI or CredUI provides inbound credential data. Override this method to respond to the incoming data.
        /// </summary>
        /// <param name="inboundSerialization">The inbound serialized credential </param>
        public virtual void OnSetSerialization(CredentialSerialization inboundSerialization) { }

        /// <summary>
        /// This method is first called when the credential provider is initialized. Override this method to perform any initialization tasks
        /// </summary>
        public virtual void OnLoad() { }

        /// <summary>
        /// The method is called when the credential provider is being unloaded. Override this method to perform any cleanup tasks
        /// </summary>
        public virtual void OnUnload() { }

        private void BuildControls()
        {
            if (this.Controls == null)
            {
                this.Controls = new ControlCollection();

                foreach (var control in this.GetControls(this.UsageScenario))
                {
                    control.SetLogger(this.LoggerFactory);
                    this.Controls.Add(control);
                }

                this.Controls.Lock();
            }
        }

        private List<CredentialTile> GenerateSuppliedUserTiles()
        {
            this.BuildControls();

            var tiles = new List<CredentialTile>();

            var users = new List<CredentialProviderUser>();

            this.credentialProviderUsers.GetCount(out var count);

            for (uint i = 0; i < count; i++)
            {
                var result = this.credentialProviderUsers.GetAt(i, out var user);
                if (result != HRESULT.S_OK)
                {
                    this.logger.LogError($"Could not get user at index {i}");
                    continue;
                }

                user.GetSid(out var sid);

                this.logger.LogTrace($"Got supplied user {i}: with name {user.GetQualifiedUserName()} and SID {sid}");

                var credentialProviderUser = new CredentialProviderUser(this.LoggerFactory, user);
                users.Add(credentialProviderUser);

                try
                {
                    if (this.ShouldIncludeUserTile(credentialProviderUser))
                    {
                        var userTile = this.CreateUserTile(credentialProviderUser);
                        if (userTile != null)
                        {
                            tiles.Add(userTile);
                            userTile.Initialize();
                        }
                    }
                }
                catch (NotImplementedException)
                {
                }
            }

            try
            {
                if (this.ShouldIncludeGenericTile())
                {
                    var genericTile = this.CreateGenericTile();
                    if (genericTile != null)
                    {
                        tiles.Add(genericTile);
                        genericTile.Initialize();
                    }
                }
            }
            catch (NotImplementedException)
            {
            }

            this.SuppliedUsers = users.AsReadOnly();

            return tiles;
        }

        private void SetupTiles()
        {
            this.tiles = new List<CredentialTile>(this.GenerateSuppliedUserTiles());
            this.Tiles = this.tiles.AsReadOnly();
        }

        private void NotifyHostOfTileCollectionChange()
        {
            if (this.notifyOnTileCollectionChange)
            {
                this.CredentialProviderEvents?.CredentialsChanged(this.credentialProviderEventsAdviseContext);
            }
        }
    }
}