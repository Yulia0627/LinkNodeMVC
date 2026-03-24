using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinkNodeDomain.Model;

public partial class ActionType : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Дія")]
    public string Action { get; set; } = null!;

    public virtual ICollection<AdminAction> AdminActions { get; set; } = new List<AdminAction>();
}
