using System;
using System.Collections;
using System.Collections.Generic;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// A collection of control objects
    /// </summary>
    public class ControlCollection : IEnumerable<ControlBase>
    {
        private readonly Dictionary<uint, ControlBase> idControls = new Dictionary<uint, ControlBase>();
        private readonly Dictionary<string, ControlBase> keyedControls = new Dictionary<string, ControlBase>(StringComparer.OrdinalIgnoreCase);
        private readonly List<ControlBase> controls = new List<ControlBase>();

        private ICredentialProviderCredentialEvents events;
        private ICredentialProviderCredential credential;
        private bool locked;

        internal ControlCollection() { }

        internal ControlCollection(CredentialTile credential)
        {
            this.credential = credential;
        }

        internal void SetCredential(ICredentialProviderCredential credential)
        {
            if (this.credential != null && this.credential != credential)
            {
                throw new InvalidOperationException("Cannot set credential more than once");
            }

            this.credential = credential;
        }

        internal void Add(ControlBase control)
        {
            if (this.locked)
            {
                throw new InvalidOperationException("Cannot add controls after the provider has had GetFieldDescriptorCount called");
            }

            if (this.credential != null)
            {
                control.AssignCredential(this.credential);
            }

            if (this.events != null)
            {
                control.AssignEvents(this.events);
            }

            if (this.keyedControls.ContainsKey(control.Key))
            {
                throw new ArgumentException("An control with the same key already exists");
            }

            this.idControls.Add(control.Id, control);
            this.keyedControls.Add(control.Key, control);
            this.controls.Add(control);
        }

        internal void AssignEvents(ICredentialProviderCredentialEvents events)
        {
            if (this.credential == null)
            {
                throw new InvalidOperationException("Unable to assign events until a credential has been set");
            }

            this.events = events;

            foreach (var control in this.idControls.Values)
            {
                control.AssignEvents(events);
            }
        }

        internal void UnassignEvents()
        {
            this.events = null;

            foreach (var control in this.idControls.Values)
            {
                control.UnassignEvents();
            }
        }

        /// <summary>
        /// Gets a control from the collection
        /// </summary>
        /// <typeparam name="T">The type of control</typeparam>
        /// <param name="key">The unique ID of the control</param>
        /// <returns></returns>
        public T GetControl<T>(string key) where T : ControlBase
        {
            return (T)this.keyedControls[key];
        }

        internal T GetControl<T>(uint id) where T : ControlBase
        {
            return (T)this.idControls[id];
        }

        internal bool TryGetControl<T>(uint id, FieldType type, out T control) where T : ControlBase
        {
            if (this.idControls.ContainsKey(id))
            {
                control = this.idControls[id] as T;

                if (control != null)
                {
                    if (control.Type == type)
                    {
                        return true;
                    }
                }
            }

            control = null;
            return false;
        }

        internal bool TryGetControl(uint id, out ControlBase control)
        {
            control = null;

            if (this.idControls.ContainsKey(id))
            {
                control = this.idControls[id];
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the total number of controls in the collection
        /// </summary>
        public int Count => this.idControls.Count;

        internal ControlBase this[int index]
        {
            get
            {
                return this.controls[index];
            }
        }

        internal void Lock()
        {
            this.locked = true;
        }

        /// <summary>
        /// Gets a control by its unique ID
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ControlBase this[string key]
        {
            get
            {
                return this.keyedControls[key];
            }
        }

        /// <summary>
        /// Get all controls of the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal IEnumerable<ControlBase> GetByType(FieldType type)
        {
            foreach (var item in this.idControls)
            {
                if (item.Value.Type == type)
                {
                    yield return item.Value;
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerator<ControlBase> GetEnumerator()
        {
            return this.idControls.Values.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.idControls.Values.GetEnumerator();
        }
    }
}