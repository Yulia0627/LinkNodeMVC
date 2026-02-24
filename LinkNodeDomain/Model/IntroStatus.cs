using System;
using System.Collections.Generic;

namespace LinkNodeDomain.Model;

public partial class IntroStatus : Entity
{
    public string Status { get; set; } = null!;

    public virtual ICollection<CallStatusStory> CallStatusStoryNewStatuses { get; set; } = new List<CallStatusStory>();

    public virtual ICollection<CallStatusStory> CallStatusStoryOldStatuses { get; set; } = new List<CallStatusStory>();

    public virtual ICollection<Interview> Interviews { get; set; } = new List<Interview>();
}
