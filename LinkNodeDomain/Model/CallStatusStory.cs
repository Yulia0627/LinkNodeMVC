using System;
using System.Collections.Generic;

namespace LinkNodeDomain.Model;

public partial class CallStatusStory : Entity
{
    public int IntroId { get; set; }

    public int OldStatusId { get; set; }

    public int NewStatusId { get; set; }

    public DateTime ChangedDate { get; set; }

    public virtual Interview Intro { get; set; } = null!;

    public virtual IntroStatus NewStatus { get; set; } = null!;

    public virtual IntroStatus OldStatus { get; set; } = null!;
}
