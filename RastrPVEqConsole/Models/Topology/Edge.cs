namespace RastrPVEqConsole.Models.Topology
{
    public class Edge<T> : IEdge<T>
    {
        /// <summary>
        /// Vertex, edge starts from
        /// </summary>
        public IVertex<T> StartVertex { get; }

        /// <summary>
        /// Vertex, edge ends at
        /// </summary>
        public IVertex<T> EndVertex { get; }

        /// <summary>
        /// Weight of the edge
        /// </summary>
        public double Weight { get; }

        /// <summary>
        /// Gives a pointer to the graph edge belongs to
        /// </summary>
        public IGraph<T> CurrentGraph { get; set; }

        public Edge(IVertex<T> startVertex, IVertex<T> endVertex, double weight, IGraph<T> currentGraph)
        {
            StartVertex = startVertex;
            EndVertex = endVertex;
            Weight = weight;
            CurrentGraph = currentGraph;
        }

        public Edge(IVertex<T> startVertex, IVertex<T> endVertex, IGraph<T> currentGraph)
        {
            StartVertex = startVertex;
            EndVertex = endVertex;
            CurrentGraph = currentGraph;
        }

        public Edge(IVertex<T> startVertex, IVertex<T> endVertex)
        {
            StartVertex = startVertex;
            EndVertex = endVertex;
        }
    }
}
