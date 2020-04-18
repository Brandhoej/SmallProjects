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

        bool Contains(TVertex a, TVertex b);
    }

    public interface IMutableEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex 
        where TEdge : class, IReadonlyEdge<TVertex> {
        bool AddEdge(TEdge edge);
        bool RemoveEdge(TEdge edge);
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
        public abstract bool AddEdge(TEdge edge);
        public abstract bool RemoveEdge(TEdge edge);
    }

    public interface IUndirectedEdgeSet<TVertexKey, TVertex, TEdge>
        : IEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {    }

    public class UndirectedEdgeSet<TVertexKey, TVertex, TEdge>
        : EdgeSet<TVertexKey, TVertex, TEdge>,
        IUndirectedEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : class, IReadonlyEdge<TVertex> {
        private ICollection<TEdge> m_edges;
        // Edges are references as : class per definition of the generic
        private IDictionary<TVertex, ICollection<TEdge>> m_adjacencyList;

        public override int Count
            => m_edges.Count;

        public UndirectedEdgeSet(IVertexSet<TVertexKey, TVertex> vertexSet)
            : base(vertexSet) {
            m_edges = new HashSet<TEdge>();
            m_adjacencyList = new Dictionary<TVertex, ICollection<TEdge>>();
            //TODO: INIT COLLECTIONS
        }

        public override bool Contains(TVertex source, TVertex destination) {
            foreach(TEdge edge in m_adjacencyList[source]) {
                if(edge.Destination.Equals(destination)) {
                    return true;
                }
            }
            return false;
        }

        public override bool Contains(TEdge edge) {
            return m_edges.Contains(edge);
        }

        public override bool AddEdge(TEdge edge) {
            if(!VertexSet.Contains(edge.Source) || !VertexSet.Contains(edge.Destination)) {
                return false;
            }

            if(Contains(edge)) {
                return false;
            }

            if(!m_adjacencyList.ContainsKey(edge.Source)) {
                m_adjacencyList.Add(edge.Source, new List<TEdge>());
            }

            if(!m_adjacencyList.ContainsKey(edge.Destination)) {
                m_adjacencyList.Add(edge.Destination, new List<TEdge>());
            }

            m_edges.Add(edge);
            m_adjacencyList[edge.Source].Add(edge);
            m_adjacencyList[edge.Destination].Add(edge);
            return true;
        }

        public override bool RemoveEdge(TEdge edge) {
            if(!Contains(edge.Source, edge.Destination)) {
                return false;
            }
            m_adjacencyList[edge.Source].Remove(edge);
            m_adjacencyList[edge.Destination].Remove(edge);
            return true;
        }
    }
}