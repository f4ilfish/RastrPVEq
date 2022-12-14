namespace RastrPVEqConsole.Models.Topology
{
    public interface IEdge<T>
    {
        /// <summary>
        /// Vertex, edge starts from
        /// </summary>
        IVertex<T> StartVertex { get; }

        /// <summary>
        /// Vertex, edge ends at
        /// </summary>
        IVertex<T> EndVertex { get; }

        /// <summary>
        /// Weight of the edge
        /// </summary>
        double Weight { get; }

        /// <summary>
        /// Gives a pointer to the graph edge belongs to
        /// </summary>
        IGraph<T> CurrentGraph { get; set; }
    }
}
