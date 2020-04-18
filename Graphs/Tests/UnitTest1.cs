using System.Net.Http.Headers;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphs;
using System.Collections.Generic;

namespace Tests {
    [TestClass]
    public class Tests {
        [TestMethod]
        public void TestVertexList() {
            VertexList<Vertex> vl = new VertexList<Vertex>();
            Assert.AreEqual(vl.Count, 0);
            Vertex v0 = new Vertex();
            Vertex v1 = new Vertex();
            vl.Add(0, v0);
            vl.Add(1, v1);
            Assert.AreEqual(vl.Count, 2);
            Assert.IsTrue(vl.ContainsKey(0));
            Assert.IsTrue(vl.ContainsKey(1));
            Assert.IsTrue(vl.Contains(new KeyValuePair<int, Vertex>(0, v0)));
            Assert.IsTrue(vl.Contains(new KeyValuePair<int, Vertex>(1, v1)));
            Vertex re = null;
            Assert.IsTrue(vl.TryGetValue(0, out re));
            Assert.AreEqual(re, v0);
            re = null;
            Assert.IsFalse(vl.TryGetValue(2, out re));
            Assert.IsNull(re);
            Assert.IsTrue(vl.Remove(new KeyValuePair<int, Vertex>(0, v0)));
            Assert.IsTrue(vl.ContainsKey(0));
            Assert.IsFalse(vl.ContainsKey(1));
            Assert.AreEqual(vl[0], v1);
            Assert.IsTrue(vl.Remove(0));
            Assert.AreEqual(vl.Count, 0);
        }

        [TestMethod]
        public void TestVertexArray() {
            // VertexArray<Vertex> va = new VertexArray<Vertex>(2);
            // Assert.IsFalse(va.Contains(0));
            // Assert.IsFalse(va.Contains(1));
            // Vertex v1 = new Vertex();
            // Vertex v2 = new Vertex();
            // Assert.IsTrue(va.Add(0, v1));
            // Assert.IsTrue(va.Add(1, v2));
            // Assert.IsTrue(va.Contains(0));
            // Assert.IsTrue(va.Contains(1));
            // Assert.AreEqual(va.GetVertex(0), v1);
            // Assert.AreNotEqual(va.GetVertex(1), v1);
            // Assert.AreEqual(va.GetVertex(1), v2);
            // Assert.AreNotEqual(va.GetVertex(0), v2);
            // Assert.IsTrue(va.Remove(0));
            // Assert.IsFalse(va.Remove(0));
            // Assert.IsTrue(va.Remove(1));
            // Assert.IsFalse(va.Remove(1));
        }

        [TestMethod]
        public void TestVertexDictionary() {

        }

        [TestMethod]
        public void TestDirectedSparseGraph() {

        }

        [TestMethod]
        public void TestGraphConversionPerperties() {
            // VertexList<Vertex> vertices = new Graphs.VertexList<Vertex>();
            // UndirectedGraph<VertexList<Vertex>, int, Vertex, IUndirectedAdjacencyList<int, Vertex, Edge<Vertex>>, Edge<Vertex>> graph = new UndirectedGraph<VertexList<Vertex>, int, Vertex, IUndirectedAdjacencyList<int, Vertex, Edge<Vertex>>, Edge<Vertex>>(new VertexList<Vertex>(), new UndirectedAdjacencyList<int, Vertex, Edge<Vertex>>());
            // Assert.IsNotNull(graph.EdgeSet);
            // Assert.IsNotNull(graph.VertexSet);
            // Assert.IsNotNull(((IMutableGraph<VertexList<Vertex>, int, Vertex, IUndirectedAdjacencyList<int, Vertex, Edge<Vertex>>, Edge<Vertex>>)graph).EdgeSet);
            // Assert.IsNotNull(((IMutableGraph<VertexList<Vertex>, int, Vertex, IUndirectedAdjacencyList<int, Vertex, Edge<Vertex>>, Edge<Vertex>>)graph).VertexSet);
            // Assert.IsNotNull(((IReadonlyGraph<VertexList<Vertex>, int, Vertex, IUndirectedAdjacencyList<int, Vertex, Edge<Vertex>>, Edge<Vertex>>)graph).EdgeSet);
            // Assert.IsNotNull(((IReadonlyGraph<VertexList<Vertex>, int, Vertex, IUndirectedAdjacencyList<int, Vertex, Edge<Vertex>>, Edge<Vertex>>)graph).VertexSet);
        }

        [TestMethod]
        public void TestUndirectedSparseGraph() {
            // VertexList<Vertex> vertices = new VertexList<Vertex>();
            // Vertex va = new Vertex();
            // Vertex vb = new Vertex();
            // Assert.AreNotEqual(va, vb);
            // vertices.Add(0, va);
            // vertices.Add(1, vb);

            // Assert.AreEqual(2, vertices.Count);

            // UndirectedAdjacencyList<int, Vertex, Edge<Vertex>> adjList = new UndirectedAdjacencyList<int, Vertex, Edge<Vertex>>();
            // Edge<Vertex> edge = new Edge<Vertex>(va, vb);
            // bool added = adjList.Add(edge);
            // Assert.AreEqual(adjList.Count, 2);
            // Assert.AreEqual(adjList.GetEdges(va).Count, 1);
            // Assert.AreEqual(adjList.GetEdges(vb).Count, 1);
            // Assert.AreEqual(adjList.GetEdges(va).First(), edge);

            // UndirectedSparseGraph<IVertexList<Vertex>, int, Vertex, Edge<Vertex>> graph = new Graphs.UndirectedSparseGraph<IVertexList<Vertex>, int, Vertex, Edge<Vertex>>(vertices, adjList);
            // Assert.AreEqual(graph.VertexSet, vertices);
            // Assert.AreEqual(graph.EdgeSet, adjList);
        }
    }
}
