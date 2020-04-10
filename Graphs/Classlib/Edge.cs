namespace Graphs {
    public interface IEdge<TVertex>
        where TVertex : IVertex {
        TVertex Source
        { get; set; }

        TVertex Destination
        { get; set; }
    }

    public interface ILabeledEdge<TVertex, TLable>
        : IEdge<TVertex>
        where TVertex : IVertex {
        TLable Label
        { get; }
    }

    public interface IWeightedEdge<TVertex>
        : ILabeledEdge<TVertex, float>
        where TVertex : IVertex {    }

    public class Edge<TVertex>
        : IEdge<TVertex>
        where TVertex : IVertex
    {
        public TVertex Source { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public TVertex Destination { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}