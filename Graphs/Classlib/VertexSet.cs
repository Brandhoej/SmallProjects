using System.Collections.Generic;
using System.Linq;

namespace Graphs {
    public interface IVertexSet<TVertexKey, TVertex>
        : IReadonlyVertexSet<TVertexKey, TVertex>,
        IMutableVertexSet<TVertexKey, TVertex>
        where TVertex : IVertex {    }

    public interface IReadonlyVertexSet<TVertexKey, TVertex>
        where TVertex : IVertex {
        
        int Amount
        { get; }

        bool Contains(TVertexKey key);
        bool Contains(TVertex vertex);
        TVertex GetVertex(TVertexKey key);
    }

    public interface IMutableVertexSet<TVertexKey, TVertex>
        where TVertex : IVertex {
        
        bool Add(TVertexKey key, TVertex vertex);
        bool Remove(TVertexKey key);
    }

    public interface IVertexList<TVertex>
        : IVertexSet<int, TVertex>
        where TVertex : IVertex {    }

    /// <summary>
    /// The property of this set is that the vertices in the list from 0 -> Amount is guranteed to be there.
    /// In turn this means that only the last element can be removed and addition can only happen
    /// if the key is equal to the last elements key in list + 1
    /// </summary>
    public class VertexList<TVertex>
        : IVertexList<TVertex>
        where TVertex : IVertex {

        private List<TVertex> m_vertices;

        public int Amount 
            => m_vertices.Count;

        public VertexList() {
            m_vertices = new List<TVertex>();
        }

        public bool Add(int key, TVertex vertex) {
            if(key != Amount) {
                return false;
            }
            m_vertices.Add(vertex);
            return true;
        }

        public bool Contains(int key) {
            return key >= 0 && key < Amount;
        }

        public TVertex GetVertex(int key) {
            return m_vertices[key];
        }

        public bool Remove(int key) {
            if(key != Amount - 1) {
                return false;
            }
            // Remove the last element
            m_vertices.RemoveAt(key);
            return true;
        }

        public bool Contains(TVertex vertex) {
            for(int i = 0; i < Amount; i++) {
                if(GetVertex(i).Equals(vertex)) {
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// The property of this set is that the vertices in the list from 0 -> Amount is guranteed to be there but some can be NULL.
    /// Removeable of a vertex sets it to NULL, adding an element is the same as setting the element at the index
    /// </summary>
    public interface IVertexArray<TVertex>
        : IVertexSet<int, TVertex>
        where TVertex : IVertex {    }

    public class VertexArray<TVertex>
        : IVertexArray<TVertex>
        where TVertex : IVertex {

        private TVertex[] m_vertices;

        public int Amount 
            => m_vertices.Length;

        public VertexArray(int amount) {
            m_vertices = new TVertex[amount];
        }

        public bool Add(int key, TVertex vertex) {
            if(!Contains(key)) {
                return false;
            }
            m_vertices[key] = vertex;
            return true;
        }

        public bool Contains(int key) {
            return key >= 0 && key < Amount;
        }

        public bool Contains(TVertex vertex) {
            for(int i = 0; i < Amount; i++) {
                if(GetVertex(i).Equals(vertex)) {
                    return true;
                }
            }
            return false;
        }

        public TVertex GetVertex(int key) {
            return m_vertices[key];
        }

        public bool Remove(int key) {
            if(!Contains(key)) {
                return false;
            }
            m_vertices[key] = default;
            return true;
        }
    }

    /// <summary>
    /// This set stores key value pairs. Vertices can be added and removed freely without any restraints.
    /// </summary>
    public interface IVertexDictionary<TVertexKey, TVertex>
        : IVertexSet<TVertexKey, TVertex>
        where TVertex : IVertex {
        bool Contains(TVertexKey key, TVertex vertex);
        bool Contains(KeyValuePair<TVertexKey, TVertex> pair);
    }

    public class VertexDictionary<TVertexKey, TVertex>
        : IVertexDictionary<TVertexKey, TVertex>
        where TVertex : IVertex {

        private Dictionary<TVertexKey, TVertex> m_vertices;

        public int Amount 
            => m_vertices.Count;

        public VertexDictionary() {
            m_vertices = new Dictionary<TVertexKey, TVertex>();
        }

        public bool Add(TVertexKey key, TVertex vertex) {
            if(Contains(key)) {
                return false;
            }
            m_vertices.Add(key, vertex);
            return true;
        }

        public bool Contains(TVertexKey key) {
            return m_vertices.ContainsKey(key);
        }

        public bool Contains(TVertex vertex) {
            return m_vertices.ContainsValue(vertex);
        }

        public bool Contains(TVertexKey key, TVertex vertex) {
            return Contains(new KeyValuePair<TVertexKey, TVertex>(key, vertex));
        }

        public bool Contains(KeyValuePair<TVertexKey, TVertex> pair) {
            return m_vertices.Contains(pair);
        }

        public TVertex GetVertex(TVertexKey key) {
            return m_vertices[key];
        }

        public bool Remove(TVertexKey key) {
            return m_vertices.Remove(key);
        }
    }
}