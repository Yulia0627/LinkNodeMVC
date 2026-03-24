using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LinkNodeDomain.Model;

public partial class User : IdentityUser<int>
{
    //[Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    //[Display(Name = "Роль")]
    //public int RoleId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Ім'я")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Прізвище")]
    public string Surname { get; set; } = null!;

    //[Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    //[Display(Name = "Email")]
    //public string Email { get; set; } = null!;

    //[Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    //[Display(Name = "Логін")]
    //public string Login { get; set; } = null!;

    //[Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    //[Display(Name = "Пароль")]
    //public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Країна")]
    public string Country { get; set; } = null!;

    [Display(Name = "Статус")]
    public bool IsActive { get; set; }

    [Display(Name = "Дата створення")]
    public DateTime CreatedDate { get; set; }

    [Display(Name = "Дата оновлення")]
    public DateTime UpdatedDate { get; set; }

    public virtual ICollection<AdminAction> AdminTargetUsers { get; set; } = new List<AdminAction>();
    public virtual ICollection<AdminAction> AdminActions { get; set; } = new List<AdminAction>();

    public virtual Client? Client { get; set; }

    public virtual Freelancer? Freelancer { get; set; }

    //[Display(Name = "Роль")]
    //public virtual UserRole Role { get; set; } = null!;
}
