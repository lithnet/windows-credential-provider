using System;
using System.Collections;
using System.Collections.Generic;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents a simple list of strings with events
    /// </summary>
    public class SimpleList : IEnumerable<string>
    {
        private readonly List<string> backingList = new List<string>();

        public event EventHandler<string> ItemAdded;
        public event EventHandler<int> ItemRemoved;

        /// <summary>
        /// Adds the specified value to the list
        /// </summary>
        /// <param name="value">A string value to add to the list</param>
        public void Add(string value)
        {
            this.backingList.Add(value);
            this.ItemAdded?.Invoke(this, value);
        }

        /// <summary>
        /// Removes the specified value to the list, if found
        /// </summary>
        /// <param name="value">A string value to remove from the list</param>
        public void Remove(string value)
        {
            var index = this.backingList.IndexOf(value);
            if (index >= 0)
            {
                this.backingList.RemoveAt(index);
                this.ItemRemoved?.Invoke(this, index);
            }
        }

        /// <summary>
        /// Adds the specified values to the list
        /// </summary>
        /// <param name="items">Items to add to the list</param>
        public void AddRange(IEnumerable<string> items)
        {
            foreach (var item in items)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// Gets the total number of items in the list
        /// </summary>
        public int Count => this.backingList.Count;

        /// <inheritdoc/>
        public IEnumerator<string> GetEnumerator()
        {
            return this.backingList.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.backingList.GetEnumerator();
        }

        /// <summary>
        /// Gets an item from the list based on its index
        /// </summary>
        /// <param name="index">The index of the item to get</param>
        /// <returns>The item matching the index provided</returns>
        public string this[int index]
        {
            get
            {
                return this.backingList[index];
            }
        }
    }
}