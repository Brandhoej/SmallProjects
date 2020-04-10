namespace Graphs {
    public interface IGraph<TVertex, TEdge, TVertexSet, TEdgeSet>
        : IReadonlyGraph<TVertex, TEdge, TVertexSet, TEdgeSet>
        , IMutableGraph<TVertex, TEdge, TVertexSet, TEdgeSet>
        where TVertex : IVertex
        where TEdge : IEdge<TVertex>
        where TVertexSet : IVertexSet<TVertex>
        where TEdgeSet : IEdgeSet<TVertex, TEdge> {
        TVertexSet VertexSet
        { get; }
        TEdgeSet EdgeSet
        { get; }
    }

    public interface IReadonlyGraph<TVertex, TEdge, TVertexSet, TEdgeSet>
        where TVertex : IVertex
        where TEdge : IEdge<TVertex>
        where TVertexSet : IReadonlyVertexSet<TVertex>
        where TEdgeSet : IReadonlyEdgeSet<TVertex, TEdge> {
        IReadonlyVertexSet<TVertex> ReadonlyVertexSet
        { get; }
        IReadonlyEdgeSet<TVertex, TEdge> ReadonlyEdgeSet
        { get; }
    }

    public interface IMutableGraph<TVertex, TEdge, TVertexSet, TEdgeSet>
        where TVertex : IVertex
        where TEdge : IEdge<TVertex>
        where TVertexSet : IMutableVertexSet<TVertex>
        where TEdgeSet : IMutableEdgeSet<TVertex, TEdge> {
        IMutableVertexSet<TVertex> MutableVertexSet
        { get; }
        IMutableEdgeSet<TVertex, TEdge> MutableEdgeSet
        { get; }
    }

    public abstract class Graph<TVertex, TEdge, TVertexSet, TEdgeSet>
        : IGraph<TVertex, TEdge, TVertexSet, TEdgeSet>
        where TVertex : IVertex
        where TEdge : IEdge<TVertex>
        where TVertexSet : IVertexSet<TVertex>
        where TEdgeSet : IEdgeSet<TVertex, TEdge>
    {
        public TVertexSet VertexSet
        { get; }
        public TEdgeSet EdgeSet
        { get; }

        public IReadonlyVertexSet<TVertex> ReadonlyVertexSet 
            => VertexSet;
        public IReadonlyEdgeSet<TVertex, TEdge> ReadonlyEdgeSet 
            => EdgeSet;
        public IMutableVertexSet<TVertex> MutableVertexSet 
            => VertexSet;
        public IMutableEdgeSet<TVertex, TEdge> MutableEdgeSet 
            => EdgeSet;

        public Graph(TVertexSet vertexSet, TEdgeSet edgeSet) {
            VertexSet = vertexSet;
            EdgeSet = edgeSet;
        }
    }

    public class UndirectedGraph<TVertex, TEdge>
        : Graph<TVertex, TEdge, IVertexSet<TVertex>, IUndirectedEdgeSet<TVertex, TEdge>>
        where TVertex : IVertex
        where TEdge : IEdge<TVertex>
    {
        public UndirectedGraph(IVertexSet<TVertex> vertexSet)
            : base(vertexSet, new UndirectedEdgeSet<TVertex, TEdge>(vertexSet)) {    }
    }
}

namespace Graphs {
    public static class Program {
        public static void Main() {
            UndirectedGraph<Vertex, Edge<Vertex>> graph = new UndirectedGraph<Vertex, Edge<Vertex>>(new VertexSet<Vertex>());
        }
    }
}