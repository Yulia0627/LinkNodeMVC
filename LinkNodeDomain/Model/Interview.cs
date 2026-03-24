using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinkNodeDomain.Model;

public partial class Interview : Entity
{
    public int PropId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Дата та час")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime DateTime { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Посилання")]
    public string Reference { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Етап")]
    public int InterviewRound { get; set; }

    public int IntroStatusId { get; set; }

    [Display(Name = "Дата створення")]
    public DateTime CreatedDate { get; set; }

    public virtual ICollection<CallStatusStory> CallStatusStories { get; set; } = new List<CallStatusStory>();

    [Display(Name = "Статус інтерв'ю")]
    public virtual IntroStatus IntroStatus { get; set; } = null!;

    [Display(Name = "Пропозиція")]
    public virtual Proposal Prop { get; set; } = null!;
}
