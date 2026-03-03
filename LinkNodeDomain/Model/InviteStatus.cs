using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinkNodeDomain.Model;

public partial class InviteStatus : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Статус запрошення")]
    public string InviteStatus1 { get; set; } = null!;

    public virtual ICollection<Invite> Invites { get; set; } = new List<Invite>();
}
