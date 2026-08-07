using System;
using System.Collections.Generic;

namespace TechRiders.Infrastructure.Data.DatabaseFirstSnapshot.Entities;

public partial class FPTours
{
    public Guid Id { get; set; }

    public Guid CenterId { get; set; }

    public Guid AmbassadorId { get; set; }

    public bool HasContactCenter { get; set; }

    public bool HasContactAmbassador { get; set; }

    public bool HasScheduledDate { get; set; }

    public bool HasFeedbackCenter { get; set; }

    public bool HasFeedbackAmbassador { get; set; }

    public bool HasPhotosCenter { get; set; }

    public bool HasPhotosAmbassador { get; set; }

    public bool HasDeliveredCenter { get; set; }

    public bool HasDeliveredAmbassador { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Ambassadors Ambassador { get; set; } = null!;

    public virtual Centers Center { get; set; } = null!;
}
