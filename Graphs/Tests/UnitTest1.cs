using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphs;

namespace Tests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void CheckGraphConversionPerperties() {
            UndirectedGraph<VertexList<Vertex>, int, Vertex, UndirectedAdjacencyList<int, Vertex, Edge<Vertex>>, Edge<Vertex>> graph = new UndirectedGraph<VertexList<Vertex>, int, Vertex, UndirectedAdjacencyList<int, Vertex, Edge<Vertex>>, Edge<Vertex>>(new VertexList<Vertex>());
            Assert.IsNotNull(graph.EdgeSet);
            Assert.IsNotNull(graph.VertexSet);
            Assert.IsNotNull(((IMutableGraph<int, Vertex, Edge<Vertex>, IVertexSet<int, Vertex>, IUndirectedAdjacencyList<int, Vertex, Edge<Vertex>>>)graph).EdgeSet);
            Assert.IsNotNull(((IMutableGraph<int, Vertex, Edge<Vertex>, IVertexSet<int, Vertex>, IUndirectedAdjacencyList<int, Vertex, Edge<Vertex>>>)graph).VertexSet);
            Assert.IsNotNull(((IReadonlyGraph<int, Vertex, Edge<Vertex>, IVertexSet<int, Vertex>, IUndirectedAdjacencyList<int, Vertex, Edge<Vertex>>>)graph).EdgeSet);
            Assert.IsNotNull(((IReadonlyGraph<int, Vertex, Edge<Vertex>, IVertexSet<int, Vertex>, IUndirectedAdjacencyList<int, Vertex, Edge<Vertex>>>)graph).VertexSet);
        }
    }
}
