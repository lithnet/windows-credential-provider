using System;
using System.Runtime.InteropServices;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents a 'v1' user credential tile that implements the minimum functionality required by the credential provider framework
    /// </summary>
    /// <remarks>Inheriting from this class enables you to provide a v1 credential tile. V1 credential tiles were introduced in Windows Vista. These tiles are not personalized. See the Microsoft documentation on ICredentialProviderCredential for more information</remarks>
    public abstract partial class CredentialTile
    {
        private protected readonly ICredentialProviderLogger logger;
        private protected ICredentialProviderCredentialEvents events;
        private protected ICredentialProviderCredentialEvents2 events2;

        private protected ControlCollection controls;
        private bool isAutoLogon;

        protected CredentialTile(CredentialProviderBase credentialProvider)
        {
            this.CredentialProvider = credentialProvider;
            this.logger = credentialProvider.LoggerFactory.CreateLogger(this.GetType());
        }

        internal CredentialProviderBase CredentialProvider { get; }

        /// <summary>
        /// Gets a value indicating if this is a generic, as opposed to a personalized tile
        /// </summary>
        public virtual bool IsGenericTile => true;

        /// <summary>
        /// Gets a value that indicates if this tile is currently selected by the user
        /// </summary>
        public bool IsSelected { get; private set; }

        /// <summary>
        /// Gets a value that indicates if the user should be automatically logged on when the tile is selected. The tile must also have IsDefault set to true.
        /// </summary>
        public bool IsAutoLogon
        {
            get => this.isAutoLogon;
            set
            {
                this.isAutoLogon = value;
                this.CredentialProvider.ReloadUserTiles();

            }
        }

        /// <summary>
        /// Gets a value indicating if this should be the default time
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets the current usage scenario
        /// </summary>
        public UsageScenario UsageScenario => this.CredentialProvider.UsageScenario;

        /// <summary>
        /// Gets the flags provided by CredUI
        /// </summary>
        public CredUIWinFlags CredUIFlags => this.CredentialProvider.CredUIFlags;

        /// <summary>
        /// Gets a list of controls assigned to this tile
        /// </summary>
        public ControlCollection Controls
        {
            get
            {
                if (this.controls == null)
                {
                    this.controls = this.GenerateCredentialControls();
                }

                return this.controls;
            }
        }

        private ControlCollection GenerateCredentialControls()
        {
            ControlCollection list = new ControlCollection(this);

            foreach (var control in this.CredentialProvider.Controls)
            {
                list.Add(control.Clone());
            }

            list.Lock();
            return list;
        }

        /// <summary>
        /// Gets the HWND of the parent of the credential provider, and notifies LogonUI or CredUI that we need to create a Window
        /// </summary>
        /// <returns>A HWND to the parentobject</returns>
        /// <exception cref="InvalidOperationException">The method was called before the host has advised that is ready to provide events</exception>
        /// <exception cref="COMException">The request to obtain the parent window HWND failed</exception>
        public IntPtr CreateParentWindowHwnd()
        {
            if (this.events == null)
            {
                throw new InvalidOperationException("The Advise method has not yet been called by the host");
            }

            var result = this.events.OnCreatingWindow(out IntPtr phwndOwner);
            if (result == HRESULT.S_OK)
            {
                return phwndOwner;
            }

            throw new COMException("Unable to obtain parent window handle", result);
        }

        /// <summary>
        /// Indicates to the host that multiple updates need to be made to the fields, and that it should delay updating the UI until <see cref="EndBulkFieldUpdate" is called/>
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void BeginBulkFieldUpdate()
        {
            if (this.events2 == null)
            {
                throw new InvalidOperationException("The credential provider has not provided an ICredentialProviderCredentialEvents2 interface");
            }

            this.events2.BeginFieldUpdates();

        }

        /// <summary>
        /// Indicates to the host that the bulk update to fields has completed, and it can now update the UI according to the new values
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>

        public void EndBulkFieldUpdate()
        {
            if (this.events2 == null)
            {
                throw new InvalidOperationException("The credential provider has not provided an ICredentialProviderCredentialEvents2 interface");
            }

            this.events2.EndFieldUpdates();
        }


        /// <summary>
        /// This method is called when the credential provider tile is initialized. Override this method to perform any initialization tasks
        /// </summary>
        public virtual void OnLoad() { }

        /// <summary>
        /// The method is called when the credential provider tile is being unloaded. Override this method to perform any cleanup tasks
        /// </summary>
        public virtual void OnUnload() { }

        /// <summary>
        /// Called after the tile has been initialized. Override this method to perform post-initialization actions.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Called when the user selects this tile
        /// </summary>
        /// <param name="autoLogon">A value that indicates if logon should be performed immediately, without waiting for further user input</param>
        /// <remarks>
        /// In Windows 10, if a credential provider wants to automatically log the user on in a situation Windows does not think is appropriate, the system will display a sign in button as a speed bump. One example of this is when a user with an empty password locks the computer or signs out. In that scenario, Windows does not directly log the user back in.
        /// </remarks>
        protected virtual void OnSelected(out bool autoLogon)
        {
            autoLogon = false;
        }

        /// <summary>
        /// Called when a user deselects this tile
        /// </summary>
        protected virtual void OnDeselected() { }

        /// <summary>
        /// Called just before credentials are serialized and returned to the host
        /// </summary>
        protected virtual void OnBeforeSerialize() { }

        /// <summary>
        /// Constructs the credential set from tile for serialization
        /// </summary>
        /// <remarks>Override this method, and provide either a  <see cref="CredentialResponseSecure"/> or <see cref="CredentialResponseInsecure"/> response.
        /// </remarks>
        /// <returns>The credential set ready to be serialized and returned to LogonUI/CredUI</returns>
        protected abstract CredentialResponseBase GetCredentials();

        /// <summary>
        /// This method is called when the <see cref="UsageScenario"/> is set to <see cref="UsageScenario.ChangePassword"/> and the user clicks the submit button. If you are supporting this scenario, you should override this method, and perform the password change operation using the information in the tile controls. Return a <see cref="ChangePasswordResponse"/> object to indicate if the operation was successful or not.
        /// </summary>
        /// <returns>A <see cref="ChangePasswordResponse"/> object</returns>
        protected virtual ChangePasswordResponse ChangePassword()
        {
            return null;
        }

        /// <summary>
        /// Called by LogonUI to translates a received error status code into the appropriate user-readable message. The Credential UI does not call this method.
        /// </summary>
        /// <param name="ntStatusCode">The NTSTATUS value that reflects the return value of the Winlogon call to LsaLogonUser.</param>
        /// <param name="ntSubstatusCode">The NTSTATUS value that reflects the value pointed to by the SubStatus parameter of LsaLogonUser when that function returns after being called by Winlogon.</param>
        /// <param name="optionalStatusText">Optional. The error message that will be displayed to the user.</param>
        /// <param name="optionalStatusIcon">Optional. An icon that will shown on the credential</param>
        protected virtual void OnLogonStatusReported(int ntStatusCode, int ntSubstatusCode, out string optionalStatusText, out StatusIcon optionalStatusIcon)
        {
            optionalStatusText = null;
            optionalStatusIcon = StatusIcon.None;
        }

        /// <summary>
        /// Performs serialization of the credentials by first calling <see cref="GetCredentials"/> and then serializing the response for return to LogonUI/CredUI. You may override this method to perform the serialization yourself. In that case, <see cref="GetCredentials"/> will not be called and does not need to be implemented.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="GetCredentials"/> returned an invalid response</exception>
        protected virtual NativeSerializationResponse OnGetSerialization()
        {
            this.OnBeforeSerialize();

            var response = new NativeSerializationResponse();

            if (this.UsageScenario == UsageScenario.Logon || this.UsageScenario == UsageScenario.UnlockWorkstation || this.UsageScenario == UsageScenario.CredUI || this.UsageScenario == UsageScenario.PLAP)
            {
                var credentials = this.GetCredentials();

                response.OptionalStatusText = credentials?.StatusText;
                response.OptionalStatusIcon = credentials?.StatusIcon ?? StatusIcon.None;

                if (credentials?.IsSuccess != true)
                {
                    response.SerializationResponse = SerializationResponse.NoCredentialNotFinished;
                    response.HResult = HRESULT.S_OK;
                    return response;
                }

                var serializer = new CredentialSerializer(this.CredentialProvider.LoggerFactory);

                if (credentials is CredentialResponseSecure s)
                {
                    response.SerializedCredentials = serializer.GenerateCredentialSerialization(credentials.Domain, credentials.Username, s.Password, this.UsageScenario == UsageScenario.UnlockWorkstation, this.CredentialProvider.CredentialProviderId);
                }
                else if (credentials is CredentialResponseInsecure i)
                {
                    response.SerializedCredentials = serializer.GenerateCredentialSerialization(credentials.Domain, credentials.Username, i.Password, this.UsageScenario == UsageScenario.UnlockWorkstation, this.CredentialProvider.CredentialProviderId);
                }
                else
                {
                    throw new InvalidOperationException($"Unknown response type from {nameof(GetCredentials)}");
                }

                response.HResult = HRESULT.S_OK;
                response.SerializationResponse = SerializationResponse.ReturnCredentialFinished;
                return response;
            }
            else if (this.UsageScenario == UsageScenario.ChangePassword)
            {
                var result = this.ChangePassword();

                if (result?.IsSuccess == true)
                {
                    response.SerializationResponse = SerializationResponse.NoCredentialFinished;
                    response.HResult = HRESULT.S_OK;
                }
                else
                {
                    response.SerializationResponse = SerializationResponse.NoCredentialNotFinished;
                    response.HResult = HRESULT.S_OK;
                }

                response.OptionalStatusText = result?.StatusText;
                response.OptionalStatusIcon = result?.StatusIcon ?? StatusIcon.None;

                return response;
            }

            response.HResult = HRESULT.E_NOTIMPL;
            return response;
        }

    }
}
