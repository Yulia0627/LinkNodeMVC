using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinkNodeDomain.Model;

public partial class UserRole : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Роль")]
    public string Role { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
