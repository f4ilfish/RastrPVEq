namespace RastrPVEq.Models.Topology
{
    /// <summary>
    /// Graph edge class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GraphEdge<T>
    {
        /// <summary>
        /// Gets connected vertex
        /// </summary>
        public GraphVertex<T> ConnectedVertex { get; }

        /// <summary>
        /// Gets edge weight
        /// </summary>
        public double EdgeWeight { get; }

        /// <summary>
        /// Graph edge class constructor
        /// </summary>
        /// <param name="connectedVertex">Connected vertex</param>
        /// <param name="weight">Weight</param>
        public GraphEdge(GraphVertex<T> connectedVertex, double weight)
        {
            ConnectedVertex = connectedVertex;
            EdgeWeight = weight;
        }
    }
}

