using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Il2CppTranslator.Util
{
    public class TopologicalSorting<TItem, TKey> : IEnumerator<TItem>
    {
        private readonly IEnumerator<TItem> source;
        private readonly Func<TItem, TKey> getKey;
        private readonly Func<TItem, IEnumerable<TKey>> getDependencies;
        private readonly HashSet<TKey> sortedItems;
        private readonly Queue<TItem> readyToOutput;
        private readonly WaitList<TItem, TKey> waitList = new WaitList<TItem, TKey>();

        private TItem current;

        public TopologicalSorting(IEnumerable<TItem> source, Func<TItem, TKey> getKey, Func<TItem, IEnumerable<TKey>> getDependencies)
        {
            this.source = source.GetEnumerator();
            this.getKey = getKey;
            this.getDependencies = getDependencies;

            readyToOutput = new Queue<TItem>();
            sortedItems = new HashSet<TKey>();
        }

        public TItem Current
        {
            get { return current; }
        }

        public void Dispose()
        {
            source.Dispose();
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            while (true)
            {
                if (readyToOutput.Count > 0)
                {
                    current = readyToOutput.Dequeue();
                    Release(current);
                    return true;
                }

                if (!source.MoveNext())
                {
                    break;
                }

                Process(source.Current);
            }

            if (waitList.Count > 0)
            {
                throw new ArgumentException("Cyclic dependency or missing dependency.");
            }

            return false;
        }

        public void Reset()
        {
            source.Reset();
            sortedItems.Clear();
            readyToOutput.Clear();
            current = default;
        }

        private void Process(TItem item)
        {
            var pendingDependencies = getDependencies(item)
                .Where(key => !sortedItems.Contains(key))
                .ToArray();

            if (pendingDependencies.Length > 0)
            {
                waitList.Add(item, pendingDependencies);
            }
            else
            {
                readyToOutput.Enqueue(item);
            }
        }

        private void Release(TItem item)
        {
            var key = getKey(item);
            sortedItems.Add(key);

            var releasedItems = waitList.Remove(key);
            if (releasedItems != null)
            {
                foreach (var releasedItem in releasedItems)
                {
                    readyToOutput.Enqueue(releasedItem);
                }
            }
        }
    }
}
