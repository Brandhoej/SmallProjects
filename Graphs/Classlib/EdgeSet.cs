using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Graphs {
    public interface IEdgeSet<TVertexKey, TVertex, TEdge>
        : IReadonlyEdgeSet<TVertexKey, TVertex, TEdge>,
        IMutableEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {
        // IReadonlyVertexSet<TVertexKey, TVertex> VertexSet
        // { get; }
    }

    public interface IReadonlyEdgeSet<TVertexKey, TVertex, TEdge>
        : IReadOnlyCollection<TEdge>,
        IEnumerable<TEdge>, IEnumerable
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {
        bool Contains(TEdge edge);
        bool Contains(TVertex source, TVertex destination);
        ICollection<TEdge> GetEdges(TVertex source);
        ICollection<TEdge> GetEdges(TVertex source, TVertex destination);
    }

    public interface IMutableEdgeSet<TVertexKey, TVertex, TEdge>
        : ICollection<TEdge>,
        IEnumerable<TEdge>, IEnumerable
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {
        new bool Add(TEdge edge);
        bool Contains(TVertex source, TVertex destination);
    }

    public abstract class EdgeSet<TVertexKey, TVertex, TEdge>
        : IEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {
        // public IReadonlyVertexSet<TVertexKey, TVertex> VertexSet
        // { get; }

        public abstract int Count 
        { get; }

        public bool IsReadOnly 
            => false;

        public EdgeSet() {
            // VertexSet = vertexSet;
        }

        public abstract bool Add(TEdge item);
        public abstract void Clear();
        public abstract bool Contains(TEdge item);
        public abstract bool Contains(TVertex source, TVertex destination);
        public abstract ICollection<TEdge> GetEdges(TVertex source);
        public abstract ICollection<TEdge> GetEdges(TVertex source, TVertex destination);
        public abstract void CopyTo(TEdge[] array, int arrayIndex);
        public abstract bool Remove(TEdge item);
        public abstract IEnumerator<TEdge> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() {
            return (IEnumerator)GetEnumerator();
        }

        void ICollection<TEdge>.Add(TEdge edge) {
            Add(edge);
        }
    }

    public interface IAdjacencyList<TVertexKey, TVertex, TEdge>
        : IEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {    }

    public abstract class AdjacencyList<TVertexKey, TVertex, TEdge>
        : EdgeSet<TVertexKey, TVertex, TEdge>,
        IAdjacencyList<TVertexKey, TVertex, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {
        // Edges are references as : class per definition of the generic
        private IDictionary<TVertex, ICollection<TEdge>> m_adjacencyList;

        public override int Count {
            get {
                int _amount = 0;
                foreach(ICollection<TEdge> edges in m_adjacencyList.Values) {
                    _amount += edges.Count;
                }
                return _amount;
            }
        }

        public AdjacencyList()
            : base() {
            m_adjacencyList = new Dictionary<TVertex, ICollection<TEdge>>();
        }

        public override bool Contains(TVertex source, TVertex destination) {
            if(!m_adjacencyList.ContainsKey(source)) {
                return false;
            }

            foreach(TEdge edge in m_adjacencyList[source]) {
                if(edge.Destination.Equals(destination)) {
                    return true;
                }
            }
            return false;
        }

        public override ICollection<TEdge> GetEdges(TVertex source) {
            if(!m_adjacencyList.ContainsKey(source)) {
                return null;
            }
            return m_adjacencyList[source];
        }

        public override ICollection<TEdge> GetEdges(TVertex source, TVertex destination) {
            ICollection<TEdge> edges = GetEdges(source);
            if(edges != null) {
                edges = edges.Where(edge => edge.Destination.Equals(destination)).ToList();
            }
            return edges;
        }

        public override void Clear() {
            m_adjacencyList.Clear();
        }

        public override void CopyTo(TEdge[] array, int arrayIndex) {
            // TODO: Convert to for loop for Keys in dictionary
            int currIndex = arrayIndex + 1;
            foreach(TEdge edge in this) {
                array.SetValue(edge, currIndex++);
            }
        }

        public override IEnumerator<TEdge> GetEnumerator() {
            foreach(ICollection<TEdge> edges in m_adjacencyList.Keys) {
                foreach(TEdge edge in edges) {
                    yield return edge;
                }
            }
        }

        protected bool Add(TVertex vertex, TEdge edge) {
            // if(!VertexSet.Contains(edge.Source) || !VertexSet.Contains(edge.Destination)) {
            //     return false;
            // }

            if(!m_adjacencyList.ContainsKey(vertex)) {
                m_adjacencyList.Add(vertex, new List<TEdge>());
            }

            m_adjacencyList[vertex].Add(edge);
            return true;
        }

        protected bool Remove(TVertex vertex, TEdge edge) {
            // if(!VertexSet.Contains(vertex)) {
            //     return false;
            // }

            if(!m_adjacencyList.ContainsKey(vertex)) {
                return false;
            }

            // Also returns false if item not found. Saves a Contains check
            return m_adjacencyList[vertex].Remove(edge);
        }
    }

    public interface IDirectedEdgeSet<TVertexKey, TVertex, TEdge>
        : IEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {    }

    public interface IDirectedAdjacencyList<TVertexKey, TVertex, TEdge>
        : IDirectedEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {    }

    public class DirectedAdjacencyList<TVertexKey, TVertex, TEdge>
        : AdjacencyList<TVertexKey, TVertex, TEdge>,
        IAdjacencyList<TVertexKey, TVertex, TEdge>,
        IDirectedAdjacencyList<TVertexKey, TVertex, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {
        public DirectedAdjacencyList()
            : base() {    }

        public override bool Contains(TEdge edge) {
            return Contains(edge.Source, edge.Destination);
        }

        public override bool Add(TEdge edge) {
            return Add(edge.Source, edge);
        }

        public override bool Remove(TEdge edge) {
            return Remove(edge.Source, edge);
        }
    }

    public interface IUndirectedEdgeSet<TVertexKey, TVertex, TEdge>
        : IEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {    }

    public interface IUndirectedAdjacencyList<TVertexKey, TVertex, TEdge>
        : IUndirectedEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {    }

    public class UndirectedAdjacencyList<TVertexKey, TVertex, TEdge>
        : AdjacencyList<TVertexKey, TVertex, TEdge>,
        IAdjacencyList<TVertexKey, TVertex, TEdge>,
        IUndirectedAdjacencyList<TVertexKey, TVertex, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {
        public UndirectedAdjacencyList()
            : base() {    }

        public override bool Contains(TEdge edge) {
            return Contains(edge.Source, edge.Destination) && Contains(edge.Destination, edge.Source);
        }

        public override bool Add(TEdge edge) {
            bool success = base.Add(edge.Source, edge);
            if(success) {
                success = Add(edge.Destination, edge);
                if(!success) {
                    Remove(edge.Source, edge);
                }
            }
            return success;
        }

        public override bool Remove(TEdge edge) {
            bool success = base.Remove(edge.Source, edge);
            if(success) {
                success = Remove(edge.Destination, edge);
                if(!success) {
                    Add(edge.Source, edge);
                }
            }
            return success;
        }

        public override ICollection<TEdge> GetEdges(TVertex source, TVertex destination) {
            return base.GetEdges(source, destination).Concat(base.GetEdges(destination, source)).ToList();
        }
    }
}