using System.Collections.Generic;

namespace RastrPVEq.Models.Topology
{
    public class GraphVertex<T>
    {
        /// <summary>
        /// Данные по вершине
        /// </summary>
        public T Data { get; }

        /// <summary>
        /// Список ребер
        /// </summary>
        public List<GraphEdge<T>> Edges { get; }

        public GraphVertex() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="vertexName">Название вершины</param>
        public GraphVertex(T vertex)
        {
            Data = vertex;
            Edges = new List<GraphEdge<T>>();
        }

        /// <summary>
        /// Добавить ребро
        /// </summary>
        /// <param name="newEdge">Ребро</param>
        public void AddEdge(GraphEdge<T> newEdge)
        {
            Edges.Add(newEdge);
        }

        /// <summary>
        /// Добавить ребро
        /// </summary>
        /// <param name="vertex">Вершина</param>
        /// <param name="edgeWeight">Вес</param>
        public void AddEdge(GraphVertex<T> vertex, double edgeWeight)
        {
            AddEdge(new GraphEdge<T>(vertex, edgeWeight));
        }
    }
}
