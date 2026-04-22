namespace HealthyBites.Api.DTOs
{
    public class WeightDynamicsResponseDto
    {
        public double ExpectedWeightChange { get; set; }
        public double MinChange { get; set; }
        public double MaxChange { get; set; }
        public string Unit { get; set; } = "kg";
        public string Period { get; set; } = "30 days";
    }
}
