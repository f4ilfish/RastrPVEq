using System.Collections.Generic;

namespace RastrPVEq.Models.Topology
{
    /// <summary>
    /// Graph vertex class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GraphVertex<T>
    {
        /// <summary>
        /// Gets vertex data
        /// </summary>
        public T VertexData { get; }

        /// <summary>
        /// Gets vertex edges
        /// </summary>
        public List<GraphEdge<T>> VertexEdges { get; }

        /// <summary>
        /// Graph vertex class constructor
        /// </summary>
        /// <param name="vertexData">Vertex data</param>
        public GraphVertex(T vertexData)
        {
            VertexData = vertexData;
            VertexEdges = new List<GraphEdge<T>>();
        }

        /// <summary>
        /// Add edge
        /// </summary>
        /// <param name="edge">Edge</param>
        public void AddEdge(GraphEdge<T> edge)
        {
            VertexEdges.Add(edge);
        }

        /// <summary>
        /// Add edge
        /// </summary>
        /// <param name="vertex">Вершина</param>
        /// <param name="edgeWeight">Вес</param>
        public void AddEdge(GraphVertex<T> vertex, double edgeWeight)
        {
            AddEdge(new GraphEdge<T>(vertex, edgeWeight));
        }
    }
}
