using System.Collections.Generic;

namespace RastrPVEq.Models.Topology
{
    /// <summary>
    /// Graph class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Graph<T>
    {
        /// <summary>
        /// Gets vertexes
        /// </summary>
        public List<GraphVertex<T>> Vertexes { get; }

        /// <summary>
        /// Graph class constructor
        /// </summary>
        public Graph()
        {
            Vertexes = new List<GraphVertex<T>>();
        }

        /// <summary>
        /// Add vertex
        /// </summary>
        /// <param name="vertexData">Vertex data</param>
        public void AddVertex(T vertexData)
        {
            Vertexes.Add(new GraphVertex<T>(vertexData));
        }

        /// <summary>
        /// Find vertex
        /// </summary>
        /// <param name="vertexData">Vertex data</param>
        /// <returns></returns>
        public GraphVertex<T> FindVertex(T vertexData)
        {
            foreach (var vertex in Vertexes)
            {
                if (vertex.VertexData.Equals(vertexData))
                {
                    return vertex;
                }
            }

            return null;
        }

        /// <summary>
        /// Add edge
        /// </summary>
        /// <param name="startVertex">Start vertex</param>
        /// <param name="endVertex">End vertex</param>
        /// <param name="weight">Weight</param>
        public void AddEdge(T startVertex, T endVertex, double weight)
        {
            var startGraphVertex = FindVertex(startVertex);
            var endGraphVertex = FindVertex(endVertex);

            if (endGraphVertex == null || startGraphVertex == null) return;
            
            startGraphVertex.AddEdge(endGraphVertex, weight);
            endGraphVertex.AddEdge(startGraphVertex, weight);
        }
    }
}
