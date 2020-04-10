namespace Graphs {
    public interface IEdgeSet<TVertex, TEdge>
        : IReadonlyEdgeSet<TVertex, TEdge>, IMutableEdgeSet<TVertex, TEdge>,
        where TVertex : IVertex 
        where TEdge : IEdge<TVertex> {
        IVertexSet<TVertex> VertexSet
        { get; }
    }

    public interface IReadonlyEdgeSet<TVertex, TEdge>
        where TVertex : IVertex 
        where TEdge : IEdge<TVertex> {    }

    public interface IMutableEdgeSet<TVertex, TEdge>
        where TVertex : IVertex 
        where TEdge : IEdge<TVertex> {    }

    public class EdgeSet<TVertex, TEdge>
        : IEdgeSet<TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : IEdge<TVertex> {
        public IVertexSet<TVertex> VertexSet
        { get; }

        public EdgeSet(IVertexSet<TVertex> vertexSet) {
            VertexSet = vertexSet;
        }
    }

    public interface IUndirectedEdgeSet<TVertex, TEdge>
        : IEdgeSet<TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : IEdge<TVertex> {    }

    public class UndirectedEdgeSet<TVertex, TEdge>
        : EdgeSet<TVertex, TEdge>, IUndirectedEdgeSet<TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : IEdge<TVertex> {
        public UndirectedEdgeSet(IVertexSet<TVertex> vertexSet)
            : base(vertexSet) {    }
    }
}