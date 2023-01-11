namespace RastrPVEq.Models.Topology
{
    /// <summary>
    /// Graph vertex info class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GraphVertexInfo<T>
    {
        /// <summary>
        /// Vertex
        /// </summary>
        public GraphVertex<T> Vertex { get;}

        /// <summary>
        /// Gets is unvisited 
        /// </summary>
        public bool IsUnvisited { get; set; }

        /// <summary>
        /// Gets or sets edges weight sum
        /// </summary>
        public double EdgesWeightSum { get; set; }

        /// <summary>
        /// Gets or sets previous visited vertex
        /// </summary>
        public GraphVertex<T> PreviousVisitedVertex { get; set; }

        /// <summary>
        /// Graph vertex info class constructor
        /// </summary>
        /// <param name="vertex">Vertex</param>
        public GraphVertexInfo(GraphVertex<T> vertex)
        {
            Vertex = vertex;
            IsUnvisited = true;
            EdgesWeightSum = double.MaxValue;
            PreviousVisitedVertex = null;
        }
    }
}

