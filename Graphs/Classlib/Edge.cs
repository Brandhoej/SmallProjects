namespace Graphs {
    public interface IEdge<TVertex>
        : IReadonlyEdge<TVertex>,
        IMutableEdge<TVertex>
        where TVertex : IVertex {    }

    public interface IReadonlyEdge<TVertex>
        where TVertex : IVertex {
        TVertex Source
        { get; }

        TVertex Destination
        { get; }
    }

    public interface IMutableEdge<TVertex>
        where TVertex : IVertex {
        TVertex Source
        { set; }

        TVertex Destination
        { set; }
    }

    public interface ILabeledEdge<TVertex, TLabel>
        : IEdge<TVertex>
        where TVertex : IVertex {
        TLabel Label
        { get; }
    }

    public interface IWeightedEdge<TVertex>
        : ILabeledEdge<TVertex, float>
        where TVertex : IVertex {    }

    public class Edge<TVertex>
        : IEdge<TVertex>
        where TVertex : IVertex
    {
        public TVertex Source 
        { get; set; }
        
        public TVertex Destination 
        { get; set; }
    }
}