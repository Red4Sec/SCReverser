using SCReverser.Core.Types;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SCReverser.Core.Collections
{
    public class StackCollection : IEnumerable<StackItem>
    {
        List<StackItem> List = new List<StackItem>();
        public event EventHandler OnChange;

        /// <summary>
        /// Count
        /// </summary>
        public int Count { get { return List.Count; } }

        /// <summary>
        /// Copy elements from
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="it">It</param>
        public void CopyFrom<T>(IEnumerable<IEquatable<T>> it)
            where T : StackItem
        {
            bool change = false;
            int x = -1;

            foreach (T ob in it)
            {
                x++;

                if (List.Count > x)
                {
                    if (ob.Equals(List[x]))
                        continue;
                }

                List.Insert(x, ob);
                change = true;
            }

            // Remove extra
            while (List.Count > x + 1)
            {
                List.RemoveAt(x + 1);
                change = true;
            }

            if (change)
                OnChange?.Invoke(this, EventArgs.Empty);
        }

        public IEnumerator<StackItem> GetEnumerator() { return List.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return List.GetEnumerator(); }

        /// <summary>
        /// Notify changes
        /// </summary>
        public void NotifyChanges()
        {
            OnChange?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Clear
        /// </summary>
        public void Clear()
        {
            if (List.Count <= 0) return;

            List.Clear();
            OnChange?.Invoke(this, EventArgs.Empty);
        }
    }
}