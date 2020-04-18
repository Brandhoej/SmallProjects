namespace Graphs {
    public interface IGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>
        : IReadonlyGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>,
        IMutableGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex>
        where TVertexSet : IVertexSet<TVertexKey, TVertex>
        where TEdgeSet : IEdgeSet<TVertexKey, TVertex, TEdge> {
        new TVertexSet VertexSet
        { get; }

        new TEdgeSet EdgeSet
        { get; }
    }

    public interface IReadonlyGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex>
        where TVertexSet : IReadonlyVertexSet<TVertexKey, TVertex>
        where TEdgeSet : IReadonlyEdgeSet<TVertexKey, TVertex, TEdge> {
        IReadonlyVertexSet<TVertexKey, TVertex> VertexSet
        { get; }

        IReadonlyEdgeSet<TVertexKey, TVertex, TEdge> EdgeSet
        { get; }
    }

    public interface IMutableGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex>
        where TVertexSet : IMutableVertexSet<TVertexKey, TVertex>
        where TEdgeSet : IMutableEdgeSet<TVertexKey, TVertex, TEdge> {
        IMutableVertexSet<TVertexKey, TVertex> VertexSet
        { get; }

        IMutableEdgeSet<TVertexKey, TVertex, TEdge> EdgeSet
        { get; }
    }

    public abstract class Graph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>
        : IGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex>
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

        IReadonlyVertexSet<TVertexKey, TVertex> IReadonlyGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>.VertexSet
            => VertexSet;

        IMutableVertexSet<TVertexKey, TVertex> IMutableGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>.VertexSet
            => VertexSet;

        IReadonlyEdgeSet<TVertexKey, TVertex, TEdge> IReadonlyGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>.EdgeSet
            => EdgeSet;

        IMutableEdgeSet<TVertexKey, TVertex, TEdge> IMutableGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>.EdgeSet
            => EdgeSet;
    }

    public interface IDirectedGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>
        : IGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex>
        where TVertexSet : IVertexSet<TVertexKey, TVertex>
        where TEdgeSet : IDirectedEdgeSet<TVertexKey, TVertex, TEdge> {    }

    public class DirectedGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>
        : Graph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>,
        IDirectedGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex>
        where TVertexSet : IVertexSet<TVertexKey, TVertex>
        where TEdgeSet : IDirectedEdgeSet<TVertexKey, TVertex, TEdge> {
        public DirectedGraph(TVertexSet vertexSet, TEdgeSet edgeSet)
            : base(vertexSet, edgeSet) {    }
    }

    public class DirectedSparseGraph<TVertexSet, TVertexKey, TVertex, TEdge>
        : DirectedGraph<TVertexSet, TVertexKey, TVertex, IDirectedAdjacencyList<TVertexKey, TVertex, TEdge>, TEdge>,
        IDirectedGraph<TVertexSet, TVertexKey, TVertex, IDirectedAdjacencyList<TVertexKey, TVertex, TEdge>, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex>
        where TVertexSet : IVertexSet<TVertexKey, TVertex> {
        public DirectedSparseGraph(TVertexSet vertexSet, IDirectedAdjacencyList<TVertexKey, TVertex, TEdge> edgeSet) 
            : base(vertexSet, edgeSet) {    }
    }

    public interface IUndirectedGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>
        : IGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex>
        where TVertexSet : IVertexSet<TVertexKey, TVertex>
        where TEdgeSet : IUndirectedEdgeSet<TVertexKey, TVertex, TEdge> {    }

    public class UndirectedGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>
        : Graph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>,
        IUndirectedGraph<TVertexSet, TVertexKey, TVertex, TEdgeSet, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex>
        where TVertexSet : IVertexSet<TVertexKey, TVertex>
        where TEdgeSet : IUndirectedEdgeSet<TVertexKey, TVertex, TEdge> {
        public UndirectedGraph(TVertexSet vertexSet, TEdgeSet edgeSet)
            : base(vertexSet, edgeSet) {    }
    }

    public class UndirectedSparseGraph<TVertexSet, TVertexKey, TVertex, TEdge>
        : UndirectedGraph<TVertexSet, TVertexKey, TVertex, IUndirectedAdjacencyList<TVertexKey, TVertex, TEdge>, TEdge>,
        IUndirectedGraph<TVertexSet, TVertexKey, TVertex, IUndirectedAdjacencyList<TVertexKey, TVertex, TEdge>, TEdge>
        where TVertex : class, IVertex
        where TEdge : class, IReadonlyEdge<TVertex>
        where TVertexSet : IVertexSet<TVertexKey, TVertex> {
        public UndirectedSparseGraph(TVertexSet vertexSet, IUndirectedAdjacencyList<TVertexKey, TVertex, TEdge> edgeSet) 
            : base(vertexSet, edgeSet) {    }
    }
}