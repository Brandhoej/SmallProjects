namespace Graphs {
    public interface IGraph<TVertexKey, TVertex, TEdge, TVertexSet, TEdgeSet>
        : IReadonlyGraph<TVertexKey, TVertex, TEdge, TVertexSet, TEdgeSet>
        , IMutableGraph<TVertexKey, TVertex, TEdge, TVertexSet, TEdgeSet>
        where TVertex : IVertex
        where TEdge : IReadonlyEdge<TVertex>
        where TVertexSet : IVertexSet<TVertexKey, TVertex>
        where TEdgeSet : IEdgeSet<TVertexKey, TVertex, TEdge> {
        new TVertexSet VertexSet
        { get; }

        new TEdgeSet EdgeSet
        { get; }
    }

    public interface IReadonlyGraph<TVertexKey, TVertex, TEdge, TVertexSet, TEdgeSet>
        where TVertex : IVertex
        where TEdge : IReadonlyEdge<TVertex>
        where TVertexSet : IReadonlyVertexSet<TVertexKey, TVertex>
        where TEdgeSet : IReadonlyEdgeSet<TVertexKey, TVertex, TEdge> {
        IReadonlyVertexSet<TVertexKey, TVertex> VertexSet
        { get; }

        IReadonlyEdgeSet<TVertexKey, TVertex, TEdge> EdgeSet
        { get; }
    }

    public interface IMutableGraph<TVertexKey, TVertex, TEdge, TVertexSet, TEdgeSet>
        where TVertex : IVertex
        where TEdge : IReadonlyEdge<TVertex>
        where TVertexSet : IMutableVertexSet<TVertexKey, TVertex>
        where TEdgeSet : IMutableEdgeSet<TVertexKey, TVertex, TEdge> {
        IMutableVertexSet<TVertexKey, TVertex> VertexSet
        { get; }

        IMutableEdgeSet<TVertexKey, TVertex, TEdge> EdgeSet
        { get; }
    }

    public abstract class Graph<TVertexKey, TVertex, TEdge, TVertexSet, TEdgeSet>
        : IGraph<TVertexKey, TVertex, TEdge, TVertexSet, TEdgeSet>
        where TVertex : IVertex
        where TEdge : IReadonlyEdge<TVertex>
        where TVertexSet : IVertexSet<TVertexKey, TVertex>
        where TEdgeSet : IEdgeSet<TVertexKey, TVertex, TEdge>
    {
        public Graph(TVertexSet vertexSet, TEdgeSet edgeSet) {
            VertexSet = vertexSet;
            EdgeSet = edgeSet;
        }

        public TVertexSet VertexSet
        { get; private set; }

        public TEdgeSet EdgeSet
        { get; private set; }

        IReadonlyVertexSet<TVertexKey, TVertex> IReadonlyGraph<TVertexKey, TVertex, TEdge, TVertexSet, TEdgeSet>.VertexSet
            => VertexSet;

        IMutableVertexSet<TVertexKey, TVertex> IMutableGraph<TVertexKey, TVertex, TEdge, TVertexSet, TEdgeSet>.VertexSet
            => VertexSet;

        IReadonlyEdgeSet<TVertexKey, TVertex, TEdge> IReadonlyGraph<TVertexKey, TVertex, TEdge, TVertexSet, TEdgeSet>.EdgeSet
            => EdgeSet;

        IMutableEdgeSet<TVertexKey, TVertex, TEdge> IMutableGraph<TVertexKey, TVertex, TEdge, TVertexSet, TEdgeSet>.EdgeSet
            => EdgeSet;
    }

    public class UndirectedGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>
        : Graph<TVertexKey, TVertex, TEdge, IVertexSet<TVertexKey, TVertex>, IUndirectedEdgeSet<TVertexKey, TVertex, TEdge>>
        where TVertex : IVertex
        where TEdge : IReadonlyEdge<TVertex>
        where TVertexSet : IVertexSet<TVertexKey, TVertex>
        where TEdgeSet : IUndirectedEdgeSet<TVertexKey, TVertex, TEdge> {
        public UndirectedGraph(TVertexSet vertexSet, TEdgeSet edgeSet)
            : base(vertexSet, edgeSet) {    }

        public UndirectedGraph(TVertexSet vertexSet)
            : base(vertexSet, new UndirectedEdgeSet<TVertexKey, TVertex, TEdge>(vertexSet)) {    }
    }
}