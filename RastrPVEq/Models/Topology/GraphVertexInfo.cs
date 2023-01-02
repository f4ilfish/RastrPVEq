namespace RastrPVEq.Models.Topology
{
    public class GraphVertexInfo<T>
    {
        /// <summary>
        /// Вершина
        /// </summary>
        public GraphVertex<T> Vertex { get; set; }

        /// <summary>
        /// Не посещенная вершина
        /// </summary>
        public bool IsUnvisited { get; set; }

        /// <summary>
        /// Сумма весов ребер
        /// </summary>
        public double EdgesWeightSum { get; set; }

        /// <summary>
        /// Предыдущая вершина
        /// </summary>
        public GraphVertex<T> PreviousVertex { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="vertex">Вершина</param>
        public GraphVertexInfo(GraphVertex<T> vertex)
        {
            Vertex = vertex;
            IsUnvisited = true;
            EdgesWeightSum = double.MaxValue;
            PreviousVertex = null;
        }
    }
}

