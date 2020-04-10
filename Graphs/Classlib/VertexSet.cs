namespace Graphs {
    public interface IVertexSet<TVertex>
        : IReadonlyVertexSet<TVertex>, IMutableVertexSet<TVertex>
        where TVertex : IVertex {    }

    public interface IReadonlyVertexSet<TVertex>
        where TVertex : IVertex {    }

    public interface IMutableVertexSet<TVertex>
        where TVertex : IVertex {    }

    public class VertexSet<TVertex>
        : IVertexSet<TVertex>
        where TVertex : IVertex {    }
}