using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// A control that renders a comboxbox in the credential UI
    /// </summary>
    public class ComboboxControl : ControlBase
    {
        private int selectedItemIndex;

        /// <summary>
        /// Creates a new <c ref="ComboboxControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        public ComboboxControl(string key) : this(key, null) { }

        /// <summary>
        /// Creates a new <c ref="ComboboxControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        /// <param name="label">The label associated with the control</param>
        public ComboboxControl(string key, string label) : base(key, label, FieldType.ComboBox)
        {
            this.ComboBoxItems = new SimpleList();
            this.ComboBoxItems.ItemAdded += this.ComboBoxItems_ItemAdded;
            this.ComboBoxItems.ItemRemoved += this.ComboBoxItems_ItemRemoved;
        }

        private void ComboBoxItems_ItemRemoved(object sender, int e)
        {
            if (e == this.selectedItemIndex)
            {
                this.SelectedItemIndex = e - 1;
            }

            this.Events?.DeleteFieldComboBoxItem(this.Credential, this.Id, (uint)e);
        }

        private void ComboBoxItems_ItemAdded(object sender, string item)
        {
            this.Events?.AppendFieldComboBoxItem(this.Credential, this.Id, item);
            if (this.selectedItemIndex == -1)
            {
                this.SelectedItemIndex = 0;
            }
        }

        /// <summary>
        /// Get the list of items in the combobox
        /// </summary>
        public SimpleList ComboBoxItems { get; }

        /// <summary>
        /// Gets or sets the index of the currently selected item in the combobox
        /// </summary>
        public int SelectedItemIndex
        {
            get { return this.selectedItemIndex; }
            set
            {
                if (this.selectedItemIndex != value)
                {
                    this.selectedItemIndex = value;
                    this.Events?.SetFieldComboBoxSelectedItem(this.Credential, this.Id, (uint)this.selectedItemIndex);
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(this.SelectedItem));
                }
            }
        }

        /// <summary>
        /// Get the value of the currently selected item in the combobox
        /// </summary>
        public string SelectedItem
        {
            get
            {
                if (this.selectedItemIndex >= 0 && this.selectedItemIndex < this.ComboBoxItems.Count)
                {
                    return this.ComboBoxItems[this.selectedItemIndex];
                }

                return null;
            }
        }

        internal void SetComboboxSelectedItemIndexInternal(int index)
        {
            this.selectedItemIndex = index;
            this.RaisePropertyChanged(nameof(this.SelectedItemIndex));
            this.RaisePropertyChanged(nameof(this.SelectedItem));
        }

        private ComboboxControl(ComboboxControl source) : base(source)
        {
            this.ComboBoxItems = new SimpleList();
            this.ComboBoxItems.ItemAdded += this.ComboBoxItems_ItemAdded;
            this.ComboBoxItems.ItemRemoved += this.ComboBoxItems_ItemRemoved;
        }

        internal override ControlBase Clone()
        {
            var clone = new ComboboxControl(this);
            clone.ComboBoxItems.AddRange(this.ComboBoxItems);
            clone.SelectedItemIndex = this.SelectedItemIndex;
            return clone;
        }
    }
}