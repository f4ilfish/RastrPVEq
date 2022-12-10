namespace RastrPVEq.Models.Topology
{
    public class DistanceModel<T>
    {
        public IVertex<T> Vertex { get; set; }
        public IVertex<T> PreviousVertex { get; set; }
        public double Distance { get; set; }

        public DistanceModel(IVertex<T> vertex, IVertex<T> previousVertex, double distance)
        {
            Vertex = vertex;
            PreviousVertex = previousVertex;
            Distance = distance;
        }

        public DistanceModel() {}

        public override string ToString() =>
            $"Vertex: {Vertex} - Distance: {Distance} - Previous: {PreviousVertex}";
    }
}
