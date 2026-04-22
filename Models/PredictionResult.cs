
using System;

namespace HealthyBites.Api.Models
{
    public partial class PredictionResult
    {
        public Guid Id { get; set; }
        public Guid SugarPredictionId { get; set; }
        public double PredictedValue { get; set; }

        // العلاقة مع SugarPrediction
        public SugarPrediction SugarPrediction { get; set; } = null!;
    }
}
