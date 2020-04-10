namespace Graphs {
    public interface IVertex {    }

    public interface ILabeledVertex<TLabel>
        : IVertex {
        TLabel Label
        { get; }
    }

    public class Vertex
        : IVertex {    }
}