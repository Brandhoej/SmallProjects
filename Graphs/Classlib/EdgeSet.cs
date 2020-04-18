using System.Collections.Generic;
using System.Linq;

namespace Graphs {
    public interface IEdgeSet<TVertexKey, TVertex, TEdge>
        : IReadonlyEdgeSet<TVertexKey, TVertex, TEdge>,
        IMutableEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {
        IVertexSet<TVertexKey, TVertex> VertexSet
        { get; }
    }

    public interface IReadonlyEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {
        int Count
        { get; }

        bool Contains(TVertex source, TVertex destination);
    }

    public interface IMutableEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex 
        where TEdge : class, IReadonlyEdge<TVertex> {
        bool Add(TEdge edge);
        bool Remove(TEdge edge);
    }

    public abstract class EdgeSet<TVertexKey, TVertex, TEdge>
        : IEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {
        public IVertexSet<TVertexKey, TVertex> VertexSet
        { get; }
        public abstract int Count { get; }

        public EdgeSet(IVertexSet<TVertexKey, TVertex> vertexSet) {
            VertexSet = vertexSet;
        }

        public abstract bool Contains(TVertex source, TVertex destination);
        public abstract bool Contains(TEdge edge);
        public abstract bool Add(TEdge edge);
        public abstract bool Remove(TEdge edge);
    }

    public interface IAdjacencyList<TVertexKey, TVertex, TEdge>
        : IEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {    }

    public abstract class AdjacencyList<TVertexKey, TVertex, TEdge>
        : EdgeSet<TVertexKey, TVertex, TEdge>,
        IAdjacencyList<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex
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

        public AdjacencyList(IVertexSet<TVertexKey, TVertex> vertexSet)
            : base(vertexSet) {
            m_adjacencyList = new Dictionary<TVertex, ICollection<TEdge>>();
        }

        public override bool Contains(TVertex source, TVertex destination) {
            foreach(TEdge edge in m_adjacencyList[source]) {
                if(edge.Destination.Equals(destination)) {
                    return true;
                }
            }
            return false;
        }

        protected bool Add(TVertex vertex, TEdge edge) {
            if(!VertexSet.Contains(edge.Source) || !VertexSet.Contains(edge.Destination)) {
                return false;
            }

            if(Contains(edge)) {
                return false;
            }

            if(!m_adjacencyList.ContainsKey(vertex)) {
                m_adjacencyList.Add(vertex, new List<TEdge>());
            }

            m_adjacencyList[vertex].Add(edge);
            return true;
        }

        protected bool Remove(TVertex vertex, TEdge edge) {
            if(!VertexSet.Contains(vertex)) {
                return false;
            }

            if(!m_adjacencyList.ContainsKey(vertex)) {
                return false;
            }

            // Also returns false if item not found. Saves a Contains check
            return m_adjacencyList[vertex].Remove(edge);
        }
    }

    public interface IDirectedAdjacencyList<TVertexKey, TVertex, TEdge>
        : IEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {    }

    public class DirectedAdjacencyList<TVertexKey, TVertex, TEdge>
        : AdjacencyList<TVertexKey, TVertex, TEdge>,
        IAdjacencyList<TVertexKey, TVertex, TEdge>,
        IDirectedAdjacencyList<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {
        public DirectedAdjacencyList(IVertexSet<TVertexKey, TVertex> vertexSet)
            : base(vertexSet) {    }

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

    public interface IUndirectedAdjacencyList<TVertexKey, TVertex, TEdge>
        : IEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {    }

    public class UndirectedAdjacencyList<TVertexKey, TVertex, TEdge>
        : AdjacencyList<TVertexKey, TVertex, TEdge>,
        IAdjacencyList<TVertexKey, TVertex, TEdge>,
        IUndirectedAdjacencyList<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {
        public UndirectedAdjacencyList(IVertexSet<TVertexKey, TVertex> vertexSet)
            : base(vertexSet) {    }

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
    }
}