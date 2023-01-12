using System.Collections.Generic;

namespace RastrPVEq.Models.Topology
{
    /// <summary>
    /// Dijkstra class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Dijkstra<T>
    {
        /// <summary>
        /// Graph field
        /// </summary>
        private Graph<T> _graph;

        /// <summary>
        /// Gets or sets vertexes info
        /// </summary>
        private List<GraphVertexInfo<T>> _vertexesInfo;

        /// <summary>
        /// Dijkstra class constructor
        /// </summary>
        /// <param name="graph">Graph</param>
        public Dijkstra(Graph<T> graph)
        {
            _graph = graph;
            _vertexesInfo = new List<GraphVertexInfo<T>>();
            AddVertexesInfo();
        }

        /// <summary>
        /// Add vertexes info
        /// </summary>
        private void AddVertexesInfo()
        {
            foreach (var vertex in _graph.Vertexes)
            {
                _vertexesInfo.Add(new GraphVertexInfo<T>(vertex));
            }
        }

        /// <summary>
        /// Get vertex info
        /// </summary>
        /// <param name="vertex">Vertex</param>
        /// <returns></returns>
        private GraphVertexInfo<T> GetVertexInfo(GraphVertex<T> vertex)
        {
            foreach (var vertexInfo in _vertexesInfo)
            {
                if (vertexInfo.Vertex.Equals(vertex))
                {
                    return vertexInfo;
                }
            }

            return null;
        }

        /// <summary>
        /// Find unvisited vertex with min edges weight sum
        /// </summary>
        /// <returns></returns>
        private GraphVertexInfo<T> FindUnvisitedVertexWithMinEdgesWeightSum()
        {
            var maxVertexEdgesWeightSum = double.MaxValue;
            
            GraphVertexInfo<T> vertexWithMinEdgesWeightSum = null;
            
            foreach (var vertexInfo in _vertexesInfo)
            {
                if (!vertexInfo.IsUnvisited || 
                    !(vertexInfo.EdgesWeightSum < maxVertexEdgesWeightSum)) continue;
                
                vertexWithMinEdgesWeightSum = vertexInfo;
                maxVertexEdgesWeightSum = vertexInfo.EdgesWeightSum;
            }

            return vertexWithMinEdgesWeightSum;
        }

        /// <summary>
        /// Find shortest path
        /// </summary>
        /// <param name="startVertex">Start vertex</param>
        /// <param name="endVertex">End vertex</param>
        /// <returns></returns>
        public List<GraphVertex<T>> FindShortestPath(T startVertex, T endVertex)
        {
            return FindShortestPath(_graph.FindVertex(startVertex), 
                                    _graph.FindVertex(endVertex));
        }

        /// <summary>
        /// Find shortest path
        /// </summary>
        /// <param name="startVertex">Start vertex</param>
        /// <param name="finishVertex">Start vertex</param>
        /// <returns></returns>
        public List<GraphVertex<T>> FindShortestPath(GraphVertex<T> startVertex, GraphVertex<T> finishVertex)
        {
            var startVertexInfo = GetVertexInfo(startVertex);
            startVertexInfo.EdgesWeightSum = 0;
            
            while (true)
            {
                var unvisitedVertexWithMinEdgesWeightSum = FindUnvisitedVertexWithMinEdgesWeightSum();
                
                if (unvisitedVertexWithMinEdgesWeightSum == null)
                {
                    break;
                }

                SetVertexEdgesWeightSum(unvisitedVertexWithMinEdgesWeightSum);
            }

            return GetPath(startVertex, finishVertex);
        }

        /// <summary>
        /// Set vertex edges weight sum
        /// </summary>
        /// <param name="vertexInfo"></param>
        private void SetVertexEdgesWeightSum(GraphVertexInfo<T> vertexInfo)
        {
            vertexInfo.IsUnvisited = false;
            
            foreach (var edge in vertexInfo.Vertex.VertexEdges)
            {
                var connectedVertexInfo = GetVertexInfo(edge.ConnectedVertex);
                
                var connectedVertexEdgesWeightSum = vertexInfo.EdgesWeightSum + edge.EdgeWeight;

                if (!(connectedVertexEdgesWeightSum < connectedVertexInfo.EdgesWeightSum)) continue;
                
                connectedVertexInfo.EdgesWeightSum = connectedVertexEdgesWeightSum;
                connectedVertexInfo.PreviousVisitedVertex = vertexInfo.Vertex;
            }
        }

        /// <summary>
        /// Get path
        /// </summary>
        /// <param name="startVertex">Start vertex</param>
        /// <param name="endVertex">End vertex</param>
        /// <returns></returns>
        private List<GraphVertex<T>> GetPath(GraphVertex<T> startVertex, GraphVertex<T> endVertex)
        {
            var path = new List<GraphVertex<T>>
            {
                endVertex
            };
            
            while (startVertex != endVertex)
            {
                if (GetVertexInfo(endVertex).PreviousVisitedVertex == null)
                {
                    break;
                }

                endVertex = GetVertexInfo(endVertex).PreviousVisitedVertex;
                path.Add(endVertex);
            }

            return path;
        }
    }
}

