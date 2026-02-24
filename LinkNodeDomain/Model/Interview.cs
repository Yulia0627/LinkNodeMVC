using System;
using System.Collections.Generic;

namespace LinkNodeDomain.Model;

public partial class Interview : Entity
{
    public int PropId { get; set; }

    public DateTime DateTime { get; set; }

    public string Reference { get; set; } = null!;

    public int InterviewRound { get; set; }

    public int IntroStatusId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<CallStatusStory> CallStatusStories { get; set; } = new List<CallStatusStory>();

    public virtual IntroStatus IntroStatus { get; set; } = null!;

    public virtual Proposal Prop { get; set; } = null!;
}
