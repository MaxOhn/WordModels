using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace WordModels.Utility
{
    public class SortedList<T> : ICollection<T>
    {
        private List<T> m_innerList;
        private Comparer<T> m_comparer;

        public SortedList() : this(Comparer<T>.Create(CompareByString))
        {
        }

        public static int CompareByString(T x, T y)
        {
            string xS = x.ToString(), yS = y.ToString();
            if (xS.Length < yS.Length)
                return -1;
            else if (xS.Length > yS.Length)
                return 1;
            return string.Compare(x.ToString(), y.ToString());
        }

        public SortedList(Comparer<T> comparer)
        {
            m_innerList = new List<T>();
            m_comparer = comparer;
        }

        public SortedList(IEnumerable<T> list) : this(list, Comparer<T>.Create(CompareByString))
        {
        }

        public SortedList(IEnumerable<T> list, Comparer<T> comparer)
        {
            m_comparer = comparer;
            m_innerList = list.OrderBy(elem => elem, comparer).ToList();
        }

        public void Add(T item)
        {
            int insertIndex = FindIndexForSortedInsert(m_innerList, m_comparer, item);
            m_innerList.Insert(insertIndex, item);
        }

        public bool Contains(T item) => IndexOf(item) != -1;

        public int IndexOf(T item)
        {
            int insertIndex = FindIndexForSortedInsert(m_innerList, m_comparer, item);
            if (insertIndex == m_innerList.Count)
                return -1;
            if (m_comparer.Compare(item, m_innerList[insertIndex]) == 0)
            {
                int index = insertIndex;
                while (index > 0 && m_comparer.Compare(item, m_innerList[index - 1]) == 0)
                    index--;
                return index;
            }
            return -1;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                m_innerList.RemoveAt(index);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index) => m_innerList.RemoveAt(index);

        public void CopyTo(T[] array) => m_innerList.CopyTo(array);

        public void CopyTo(T[] array, int arrayIndex) => m_innerList.CopyTo(array, arrayIndex);

        public void Clear() => m_innerList.Clear();

        public T this[int index]
        {
            get => m_innerList[index];
        }

        public IEnumerator<T> GetEnumerator() => m_innerList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => m_innerList.GetEnumerator();

        public int Count
        {
            get => m_innerList.Count;
        }

        public bool IsReadOnly
        {
            get => false;
        }

        public static int FindIndexForSortedInsert(List<T> list, Comparer<T> comparer, T item)
        {
            if (list.Count == 0)
                return 0;

            int lowerIndex = 0;
            int upperIndex = list.Count - 1;
            int comparisonResult;
            while (lowerIndex < upperIndex)
            {
                int middleIndex = (lowerIndex + upperIndex) / 2;
                T middle = list[middleIndex];
                comparisonResult = comparer.Compare(middle, item);
                if (comparisonResult == 0)
                    return middleIndex;
                else if (comparisonResult > 0)
                    upperIndex = middleIndex - 1;
                else
                    lowerIndex = middleIndex + 1;
            }

            comparisonResult = comparer.Compare(list[lowerIndex], item);
            if (comparisonResult < 0)
                return lowerIndex + 1;
            else
                return lowerIndex;
        }

        public override string ToString() => "{" + string.Join(", ", this) + "}";
    }
}