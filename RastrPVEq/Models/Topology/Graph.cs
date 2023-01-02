using System.Collections.Generic;

namespace RastrPVEq.Models.Topology
{
    public class Graph<T>
    {
        /// <summary>
        /// Список вершин графа
        /// </summary>
        public List<GraphVertex<T>> Vertices { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Graph()
        {
            Vertices = new List<GraphVertex<T>>();
        }

        /// <summary>
        /// Добавление вершины
        /// </summary>
        /// <param name="vertexData">Имя вершины</param>
        public void AddVertex(T vertexData)
        {
            Vertices.Add(new GraphVertex<T>(vertexData));
        }

        /// <summary>
        /// Поиск вершины
        /// </summary>
        /// <param name="vertexData">Название вершины</param>
        /// <returns>Найденная вершина</returns>
        public GraphVertex<T> FindVertex(T vertexData)
        {
            foreach (var v in Vertices)
            {
                if (v.Data.Equals(vertexData))
                {
                    return v;
                }
            }

            return null;
        }

        /// <summary>
        /// Добавление ребра
        /// </summary>
        /// <param name="firstName">Имя первой вершины</param>
        /// <param name="secondName">Имя второй вершины</param>
        /// <param name="weight">Вес ребра соединяющего вершины</param>
        public void AddEdge(T startVertex, T endVertex, double weight)
        {
            var v1 = FindVertex(startVertex);
            var v2 = FindVertex(endVertex);
            if (v2 != null && v1 != null)
            {
                v1.AddEdge(v2, weight);
                v2.AddEdge(v1, weight);
            }
        }
    }
}
