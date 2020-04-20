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

            foreach(KeyValuePair<int, Vertex> pair in vl) {
                Assert.IsNotNull(pair);
                Assert.IsNotNull(pair.Value);
            }

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
            VertexArray<Vertex> va = new VertexArray<Vertex>(2);
            Assert.AreEqual(va.Count, 2);
            Assert.IsNull(va[0]);
            Assert.IsNull(va[1]);
            Vertex v0 = new Vertex();
            Vertex v1 = new Vertex();
            va.Add(0, v0);
            va.Add(1, v1);

            foreach(KeyValuePair<int, Vertex> pair in va) {
                Assert.AreNotEqual(pair, null);
                Assert.AreNotEqual(pair.Value, null);
            }

            Assert.AreEqual(va.Count, 2);
            Assert.IsTrue(va.ContainsKey(0));
            Assert.IsTrue(va.ContainsKey(1));
            Assert.IsTrue(va.Contains(new KeyValuePair<int, Vertex>(0, v0)));
            Assert.IsTrue(va.Contains(new KeyValuePair<int, Vertex>(1, v1)));

            Vertex re = null;
            Assert.IsTrue(va.TryGetValue(0, out re));
            Assert.AreEqual(re, v0);
            re = null;
            Assert.IsFalse(va.TryGetValue(2, out re));
            Assert.IsNull(re);

            Assert.IsTrue(va.Remove(new KeyValuePair<int, Vertex>(0, v0)));
            Assert.IsTrue(va.ContainsKey(0));
            Assert.IsTrue(va.ContainsKey(1));
            Assert.IsNull(va[0]);
            Assert.IsNotNull(va[1]);
            Assert.AreEqual(va[1], v1);
            Assert.IsTrue(va.Remove(1));
            Assert.IsNull(va[1]);
            Assert.AreEqual(va.Count, 2);
        }

        [TestMethod]
        public void TestVertexDictionary() {
            VertexDictionary<int, Vertex> vd = new VertexDictionary<int, Vertex>();
            Assert.AreEqual(vd.Count, 0);
            Vertex v0 = new Vertex();
            Vertex v1 = new Vertex();
            vd.Add(0, v0);
            vd.Add(1, v1);

            foreach(KeyValuePair<int, Vertex> pair in vd) {
                Assert.IsNotNull(pair);
                Assert.IsNotNull(pair.Value);
            }

            Assert.AreEqual(vd.Count, 2);
            Assert.IsTrue(vd.ContainsKey(0));
            Assert.IsTrue(vd.ContainsKey(1));
            Assert.IsTrue(vd.Contains(new KeyValuePair<int, Vertex>(0, v0)));
            Assert.IsTrue(vd.Contains(new KeyValuePair<int, Vertex>(1, v1)));

            Vertex re = null;
            Assert.IsTrue(vd.TryGetValue(0, out re));
            Assert.AreEqual(re, v0);
            re = null;
            Assert.IsFalse(vd.TryGetValue(2, out re));
            Assert.IsNull(re);

            Assert.IsTrue(vd.Remove(new KeyValuePair<int, Vertex>(0, v0)));
            Assert.IsFalse(vd.Remove(0));
            Assert.AreEqual(vd.Count, 1);
            Assert.IsFalse(vd.ContainsKey(0));
            Assert.IsTrue(vd.ContainsKey(1));
            Assert.AreEqual(vd[1], v1);
        }

        [TestMethod]
        public void TestUndirectedGraph() {
            IVertexList<Vertex> vertices = new VertexList<Vertex>();
            IUndirectedAdjacencyList<int, Vertex, Edge<Vertex>> edges = new UndirectedAdjacencyList<int, Vertex, Edge<Vertex>>();
            IUndirectedGraph<IVertexList<Vertex>, int, Vertex, IUndirectedAdjacencyList<int, Vertex, Edge<Vertex>>, Edge<Vertex>> graph = new UndirectedGraph<IVertexList<Vertex>, int, Vertex, IUndirectedAdjacencyList<int, Vertex, Edge<Vertex>>, Edge<Vertex>>(vertices, edges);
            Assert.IsNotNull(graph.EdgeSet);
            Assert.IsNotNull(graph.VertexSet);
            Assert.IsNotNull(((IMutableGraph<VertexList<Vertex>, int, Vertex, IUndirectedAdjacencyList<int, Vertex, Edge<Vertex>>, Edge<Vertex>>)graph).EdgeSet);
            Assert.IsNotNull(((IMutableGraph<VertexList<Vertex>, int, Vertex, IUndirectedAdjacencyList<int, Vertex, Edge<Vertex>>, Edge<Vertex>>)graph).VertexSet);
            Assert.IsNotNull(((IReadonlyGraph<VertexList<Vertex>, int, Vertex, IUndirectedAdjacencyList<int, Vertex, Edge<Vertex>>, Edge<Vertex>>)graph).EdgeSet);
            Assert.IsNotNull(((IReadonlyGraph<VertexList<Vertex>, int, Vertex, IUndirectedAdjacencyList<int, Vertex, Edge<Vertex>>, Edge<Vertex>>)graph).VertexSet);
        }

        [TestMethod]
        public void TestUndirectedSparseGraph() {
            IVertexList<IVertex> vertices = new VertexList<Vertex>();
            IUndirectedSparseGraph<IVertexList<IVertex>, int, IVertex, IEdge<IVertex>> graph = new UndirectedSparseGraph<IVertexList<IVertex>, int, IVertex, IEdge<IVertex>>(vertices);
            Assert.IsNotNull(graph.EdgeSet);
            Assert.IsNotNull(graph.VertexSet);
            Assert.IsNotNull(((IMutableGraph<IVertexList<IVertex>, int, IVertex, IUndirectedAdjacencyList<int, IVertex, IEdge<Vertex>>, IEdge<Vertex>>)graph).EdgeSet);
            Assert.IsNotNull(((IMutableGraph<IVertexList<IVertex>, int, IVertex, IUndirectedAdjacencyList<int, IVertex, IEdge<Vertex>>, IEdge<Vertex>>)graph).VertexSet);
            Assert.IsNotNull(((IReadonlyGraph<IVertexList<IVertex>, int, IVertex, IUndirectedAdjacencyList<int, IVertex, IEdge<Vertex>>, IEdge<Vertex>>)graph).EdgeSet);
            Assert.IsNotNull(((IReadonlyGraph<IVertexList<IVertex>, int, IVertex, IUndirectedAdjacencyList<int, IVertex, IEdge<Vertex>>, IEdge<Vertex>>)graph).VertexSet);
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
    }
}
