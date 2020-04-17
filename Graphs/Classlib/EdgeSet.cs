namespace Graphs {
    public interface IEdgeSet<TVertexKey, TVertex, TEdge>
        : IReadonlyEdgeSet<TVertexKey, TVertex, TEdge>,
        IMutableEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : IReadonlyEdge<TVertex> {
        IVertexSet<TVertexKey, TVertex> VertexSet
        { get; }
    }

    public interface IReadonlyEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : IReadonlyEdge<TVertex> {    }

    public interface IMutableEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex 
        where TEdge : IReadonlyEdge<TVertex> {    }

    public class EdgeSet<TVertexKey, TVertex, TEdge>
        : IEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : IReadonlyEdge<TVertex> {
        public IVertexSet<TVertexKey, TVertex> VertexSet
        { get; }

        public EdgeSet(IVertexSet<TVertexKey, TVertex> vertexSet) {
            VertexSet = vertexSet;
        }
    }

    public interface IUndirectedEdgeSet<TVertexKey, TVertex, TEdge>
        : IEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : IReadonlyEdge<TVertex> {    }

    public class UndirectedEdgeSet<TVertexKey, TVertex, TEdge>
        : EdgeSet<TVertexKey, TVertex, TEdge>,
        IUndirectedEdgeSet<TVertexKey, TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : IReadonlyEdge<TVertex> {
        public UndirectedEdgeSet(IVertexSet<TVertexKey, TVertex> vertexSet)
            : base(vertexSet) {    }
    }
}