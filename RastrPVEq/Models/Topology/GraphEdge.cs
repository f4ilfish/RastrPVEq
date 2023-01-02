namespace RastrPVEq.Models.Topology
{
    public class GraphEdge<T>
    {
        /// <summary>
        /// Связанная вершина
        /// </summary>
        public GraphVertex<T> ConnectedVertex { get; }

        /// <summary>
        /// Вес ребра
        /// </summary>
        public double EdgeWeight { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectedVertex">Связанная вершина</param>
        /// <param name="weight">Вес ребра</param>
        public GraphEdge(GraphVertex<T> connectedVertex, double weight)
        {
            ConnectedVertex = connectedVertex;
            EdgeWeight = weight;
        }
    }
}

