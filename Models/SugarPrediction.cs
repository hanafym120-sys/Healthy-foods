using System;
using System.Collections.Generic;
using HealthyBites.Api.Models;

namespace HealthyBites.Api.Models
{
    public partial class SugarPrediction
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime PredictionDate { get; set; }
        public double ActualValue { get; set; }

        public virtual ICollection<PredictionResult> PredictionResults { get; set; } = new List<PredictionResult>();
        public virtual User User { get; set; } = null!;
    }
}
