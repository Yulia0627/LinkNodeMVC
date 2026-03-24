using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinkNodeDomain.Model;

public partial class IntroStatus : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Статус")]
    public string Status { get; set; } = null!;

    public virtual ICollection<CallStatusStory> CallStatusStoryNewStatuses { get; set; } = new List<CallStatusStory>();

    public virtual ICollection<CallStatusStory> CallStatusStoryOldStatuses { get; set; } = new List<CallStatusStory>();

    public virtual ICollection<Interview> Interviews { get; set; } = new List<Interview>();
}
