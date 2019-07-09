using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoCTools
{
    public sealed class PriorityQueue<T>
    {
        private readonly List<PriorityQueueItem> items;
        private readonly Func<T, int> priorityCalculator;

        public int Count => items.Count;

        public PriorityQueue(Func<T, int> priorityCalculator)
        {
            items = new List<PriorityQueueItem>();
            this.priorityCalculator = priorityCalculator;
        }

        public PriorityQueue(Func<T, int> priorityCalculator, int initialCapacity)
        {
            items = new List<PriorityQueueItem>(initialCapacity);
            this.priorityCalculator = priorityCalculator;
        }

        public void Clear() => items.Clear();

        public bool Contains(T item) => items.Any(x => x.item.Equals(item));

        public void Enqueue(T item)
        {
            items.Add(new PriorityQueueItem(item, priorityCalculator(item)));
            items.Sort((x, y) => y.priority.CompareTo(x.priority));
        }

        public void EnqueueOrUpdate(T item)
        {
            if (Contains(item))
            {
                UpdatePriority(item);
            }
            else
            {
                Enqueue(item);
            }
        }

        public T Dequeue()
        {
            T item = items[items.Count - 1].item;
            items.RemoveAt(items.Count - 1);
            return item;
        }

        public void UpdatePriority(T item)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item.Equals(item))
                {
                    items[i] = items[i].UpdatePriority(priorityCalculator(items[i].item));
                }
            }
        }

        public void UpdateAllPriorities()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i] = items[i].UpdatePriority(priorityCalculator(items[i].item));
            }
        }

        private readonly struct PriorityQueueItem
        {
            public readonly T item;
            public readonly int priority;

            public PriorityQueueItem(T item, int priority)
            {
                this.item = item;
                this.priority = priority;
            }

            public PriorityQueueItem UpdatePriority(int newPriority) =>
                new PriorityQueueItem(item, newPriority);
        }
    }
}
