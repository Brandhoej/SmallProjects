using System;
using System.Collections;
using System.Collections.Generic;

namespace Graphs
{
    public interface IVertexSet<TVertexKey, TVertex>
        : IReadonlyVertexSet<TVertexKey, TVertex>,
        IMutableVertexSet<TVertexKey, TVertex>
        where TVertex : class, IVertex {    }

    public interface IReadonlyVertexSet<TVertexKey, TVertex>
        : IReadOnlyDictionary<TVertexKey, TVertex>
        where TVertex : class, IVertex {    }

    public interface IMutableVertexSet<TVertexKey, TVertex>
        : IDictionary<TVertexKey, TVertex>
        where TVertex : class, IVertex {    }

    public interface IVertexList<TVertex>
        : IVertexSet<int, TVertex>
        where TVertex : class, IVertex {    }

    /// <summary>
    /// The property of this set is that the vertices in the list from 0 -> Amount is guranteed to be there.
    /// In turn this means that only the last element can be removed and addition can only happen
    /// if the key is equal to the last elements key in list + 1
    /// </summary>
    public class VertexList<TVertex>
        : IVertexList<TVertex>
        where TVertex : class, IVertex {

        private List<TVertex> m_vertices;

        public int Count
            => m_vertices.Count;

        public IEnumerable<int> Keys
            => Keys;

        public IEnumerable<TVertex> Values
            => Values;

        ICollection<int> IDictionary<int, TVertex>.Keys {
            get {
                int[] _keys = new int[Count];
                for(int _i = 0; _i < Count; _i++) {
                    _keys[_i] = _i;
                }
                return _keys;
            }
        }

        ICollection<TVertex> IDictionary<int, TVertex>.Values {
            get {
                TVertex[] _values = new TVertex[Count];
                for(int _i = 0; _i < Count; _i++) {
                    _values[_i] = m_vertices[_i];
                }
                return _values;
            }
        }

        public bool IsReadOnly
            => false;

        TVertex IDictionary<int, TVertex>.this[int key]
        {
            get => this[key];
            set => m_vertices[key] = value;
        }

        public TVertex this[int key]
            => m_vertices[key];

        public VertexList() {
            m_vertices = new List<TVertex>();
        }

        public bool ContainsKey(int key) {
            return key >= 0 && key < Count;
        }

        public bool TryGetValue(int key, out TVertex value) {
            bool _success = false;
            if(_success = ContainsKey(key)) {
                value = m_vertices[key];
            } else {
                value = null;
            }
            return _success;
        }

        public void Add(int key, TVertex value) {
            if(key == Count) {
                m_vertices.Add(value);
            } else if(ContainsKey(key)) {
                m_vertices[key] = value;
            } else {
                throw new IndexOutOfRangeException("Key is not valid");
            }
        }

        public bool Remove(int key) {
            bool _success = false;
            if(_success = ContainsKey(key)) {
                m_vertices.RemoveAt(key);
            }
            return _success;
        }

        public void Add(KeyValuePair<int, TVertex> item) {
            Add(item.Key, item.Value);
        }

        public void Clear() {
            m_vertices.Clear();
        }

        public bool Contains(KeyValuePair<int, TVertex> item) {
            bool _found = false;
            if(ContainsKey(item.Key)) {
                _found = m_vertices[item.Key] == item.Value;
            }
            return _found;
        }

        public void CopyTo(KeyValuePair<int, TVertex>[] array, int arrayIndex) {
            for(int _i = 0; _i < Count; _i++) {
                array.SetValue(m_vertices[_i], _i + arrayIndex + 1);
            }
        }

        public bool Remove(KeyValuePair<int, TVertex> item) {
            bool _success = false;
            if(Contains(item)) {
                _success = Remove(item.Key);
            }
            return _success;
        }

        public IEnumerator<KeyValuePair<int, TVertex>> GetEnumerator() {
            for(int _i = 0; _i < Count; _i++) {
                yield return new KeyValuePair<int, TVertex>(_i, m_vertices[_i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// The property of this set is that the vertices in the list from 0 -> Amount is guranteed to be there but some can be NULL.
    /// Removeable of a vertex sets it to NULL, adding an element is the same as setting the element at the index
    /// </summary>
    public interface IVertexArray<TVertex>
        : IVertexSet<int, TVertex>
        where TVertex : class, IVertex {
        bool InBounds(int key);
    }

    public class VertexArray<TVertex>
        : IVertexArray<TVertex>
        where TVertex : class, IVertex {

        private TVertex[] m_vertices;

        public IEnumerable<int> Keys
            => Keys;

        public IEnumerable<TVertex> Values
            => m_vertices;

        public int Count
            => m_vertices.Length;

        ICollection<int> IDictionary<int, TVertex>.Keys {
            get {
                int[] _keys = new int[Count];
                for(int _i = 0; _i < Count; _i++) {
                    _keys[_i] = _i;
                }
                return _keys;
            }
        }

        ICollection<TVertex> IDictionary<int, TVertex>.Values {
            get {
                TVertex[] _values = new TVertex[Count];
                for(int _i = 0; _i < Count; _i++) {
                    _values[_i] = m_vertices[_i];
                }
                return _values;
            }
        }

        public bool IsReadOnly
            => false;

        TVertex IDictionary<int, TVertex>.this[int key]
        {
            get => this[key];
            set => this[key] = value;
        }

        public TVertex this[int key]
        {
            get => m_vertices[key];
            private set => m_vertices[key] = value;
        }

        public VertexArray(int amount) {
            m_vertices = new TVertex[amount];
        }

        public bool InBounds(int key) {
            return key >= 0 && key < Count;
        }

        public bool ContainsKey(int key) {
            return InBounds(key);
        }

        public bool TryGetValue(int key, out TVertex value) {
            bool _found = false;
            value = null;
            if(_found = InBounds(key)) {
                value = this[key];
            }
            return _found;
        }

        public void Add(int key, TVertex value) {
            if(InBounds(key)) {
                this[key] = value;
            } else {
                throw new IndexOutOfRangeException("Key is invalid");
            }
        }

        public bool Remove(int key) {
            bool _success = false;
            if(_success = InBounds(key)) {
                this[key] = null;
            }
            return _success;
        }

        public void Add(KeyValuePair<int, TVertex> item) {
            Add(item.Key, item.Value);
        }

        public void Clear() {
            for(int i = 0; i < Count; i++) {
                Remove(i);
            }
        }

        public bool Contains(KeyValuePair<int, TVertex> item) {
            bool _found = false;
            if(InBounds(item.Key)) {
                _found = this[item.Key] == item.Value;
            }
            return _found;
        }

        public void CopyTo(KeyValuePair<int, TVertex>[] array, int arrayIndex) {
            for(int _i = 0; _i < Count; _i++) {
                array.SetValue(m_vertices[_i], _i + arrayIndex + 1);
            }
        }

        public bool Remove(KeyValuePair<int, TVertex> item) {
            bool _success = false;
            if(_success = Contains(item)) {
                Remove(item.Key);
            }
            return _success;
        }

        public IEnumerator<KeyValuePair<int, TVertex>> GetEnumerator() {
            for(int _i = 0; _i < Count; _i++) {
                yield return new KeyValuePair<int, TVertex>(_i, m_vertices[_i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// This set stores key value pairs. Vertices can be added and removed freely without any restraints.
    /// </summary>
    public interface IVertexDictionary<TVertexKey, TVertex>
        : IVertexSet<TVertexKey, TVertex>
        where TVertex : class, IVertex {    }

    public class VertexDictionary<TVertexKey, TVertex>
        : IVertexDictionary<TVertexKey, TVertex>
        where TVertex : class, IVertex {

        private Dictionary<TVertexKey, TVertex> m_vertices;

        public int Amount 
            => m_vertices.Count;

        public IEnumerable<TVertexKey> Keys
            => m_vertices.Keys;

        public IEnumerable<TVertex> Values
            => m_vertices.Values;

        public int Count
            => m_vertices.Count;

        ICollection<TVertexKey> IDictionary<TVertexKey, TVertex>.Keys
            => m_vertices.Keys;

        ICollection<TVertex> IDictionary<TVertexKey, TVertex>.Values
            => m_vertices.Values;

        public bool IsReadOnly
            => false;

        TVertex IDictionary<TVertexKey, TVertex>.this[TVertexKey key] {
            get => this[key];
            set => this[key] = value;
        }

        public TVertex this[TVertexKey key] {
            get => m_vertices[key];
            private set => m_vertices[key] = value;
        }

        public VertexDictionary() {
            m_vertices = new Dictionary<TVertexKey, TVertex>();
        }

        public bool ContainsKey(TVertexKey key) {
            return m_vertices.ContainsKey(key);
        }

        public bool TryGetValue(TVertexKey key, out TVertex value) {
            return m_vertices.TryGetValue(key, out value);
        }

        public void Add(TVertexKey key, TVertex value) {
            m_vertices.Add(key, value);
        }

        public bool Remove(TVertexKey key) {
            return m_vertices.Remove(key);
        }

        public void Add(KeyValuePair<TVertexKey, TVertex> item) {
            Add(item.Key, item.Value);
        }

        public void Clear() {
            m_vertices.Clear();
        }

        public bool Contains(KeyValuePair<TVertexKey, TVertex> item) {
            bool _found = false;
            if(m_vertices.ContainsKey(item.Key)) {
                _found = m_vertices[item.Key] == item.Value;
            }
            return _found;
        }

        public void CopyTo(KeyValuePair<TVertexKey, TVertex>[] array, int arrayIndex) {
            int _i = 0;
            foreach(KeyValuePair<TVertexKey, TVertex> pair in m_vertices) {
                array.SetValue(pair, _i + arrayIndex + 1);
                _i++;
            }
        }

        public bool Remove(KeyValuePair<TVertexKey, TVertex> item) {
            bool _success = false;
            if(ContainsKey(item.Key) && m_vertices[item.Key] == item.Value) {
                _success = Remove(item.Key);
            }
            return _success;
        }

        public IEnumerator<KeyValuePair<TVertexKey, TVertex>> GetEnumerator() {
            foreach(KeyValuePair<TVertexKey, TVertex> pair in m_vertices) {
                yield return pair;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}