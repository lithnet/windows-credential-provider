using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Lithnet.CredentialProvider.Interop;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents the base of all control types
    /// </summary>
    public abstract class ControlBase : INotifyPropertyChanged
    {
        private static uint fieldIdCounter;

        private FieldState state;
        private FieldInteractiveState interactiveState;
        private string label;
        private FieldOptions options;
        private protected ILogger logger = NullLogger.Instance;

        public event PropertyChangedEventHandler PropertyChanged;

        private protected ControlBase(ControlBase source)
        {
            this.Id = source.Id;
            this.Key = source.Key;
            this.Type = source.Type;
            this.FieldTypeGuid = source.FieldTypeGuid;

            this.state = source.State;
            this.interactiveState = source.InteractiveState;
            this.label = source.Label;
            this.options = source.options;
        }

        private protected ControlBase(string key, FieldType type)
         : this(key, null, type, Guid.Empty) { }

        private protected ControlBase(string key, string label, FieldType type)
            : this(key, label, type, Guid.Empty) { }

        private protected ControlBase(string key, string label, FieldType type, Guid guidType)
        {
            this.Id = fieldIdCounter++;
            this.Type = type;
            this.FieldTypeGuid = guidType;

            this.label = label;
            this.state = FieldState.DisplayInSelectedTile;
            this.interactiveState = FieldInteractiveState.None;
            this.options = FieldOptions.None;

            this.Key = key;
            this.logger = CredentialProviderBase.LoggerFactory.CreateLogger(this.GetType());
        }

        internal void AssignCredential(ICredentialProviderCredential credential)
        {
            this.Credential = credential;
        }

        internal void AssignEvents(ICredentialProviderCredentialEvents events)
        {
            this.Events = events;
        }

        internal void UnassignEvents()
        {
            this.Events = null;
        }

        internal ICredentialProviderCredentialEvents Events { get; private set; }

        internal ICredentialProviderCredential Credential { get; private set; }

        /// <summary>
        /// Gets the unique ID associated with this control
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets the internal numerical ID associated with this control
        /// </summary>
        public uint Id { get; }

        /// <summary>
        /// Gets or sets the label text for the control
        /// </summary>
        public string Label
        {
            get { return this.label; }
            set
            {
                if (this.label != value)
                {
                    this.label = value;
                    if (this.Type != FieldType.PasswordText &&
                        this.Type != FieldType.EditText)
                    {
                        this.Events?.SetFieldString(this.Credential, this.Id, Marshal.StringToCoTaskMemUni(this.label));
                    }

                    this.RaisePropertyChanged();
                }
            }
        }

        internal FieldType Type { get; }

        internal Guid FieldTypeGuid { get; }

        /// <summary>
        /// Gets or sets the options that control the rendering of this field
        /// </summary>
        public FieldOptions Options
        {
            get
            {
                return this.options;
            }
            set
            {
                if (this.options != value)
                {
                    this.options = value;

                    if (this.Events is ICredentialProviderCredentialEvents2 e)
                    {
                        e.SetFieldOptions(this.Credential, this.Id, this.options);
                    }

                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the states in which this control should be displayed
        /// </summary>
        public FieldState State
        {
            get
            {
                return this.state;
            }
            set
            {
                if (this.state != value)
                {
                    this.state = value;

                    this.Events?.SetFieldState(this.Credential, this.Id, this.state);
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the interactivity status of the control
        /// </summary>
        public FieldInteractiveState InteractiveState
        {
            get { return this.interactiveState; }
            set
            {
                if (this.interactiveState != value)
                {
                    this.interactiveState = value;
                    this.Events?.SetFieldInteractiveState(this.Credential, this.Id, this.interactiveState);
                    this.RaisePropertyChanged();
                }
            }
        }

        internal FieldDescriptor GetDescriptor()
        {
            return new FieldDescriptor
            {
                FieldType = this.Type,
                FieldID = this.Id,
                FieldTypeGuid = this.FieldTypeGuid,
                Label = this.Label
            };
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{this.Id}:{this.Type}:{this.Label}";
        }

        internal void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        internal abstract ControlBase Clone();
    }
}