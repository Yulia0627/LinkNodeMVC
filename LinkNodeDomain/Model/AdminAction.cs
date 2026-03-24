using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LinkNodeDomain.Model;

[Table("AdminActions")]
public partial class AdminAction : Entity
{
    public int AdminId { get; set; }

    // Навігаційна властивість для зв'язку з сутністю User
    [ForeignKey("AdminId")]
    public virtual User Admin { get; set; } = null!;
    public int ActionId { get; set; }

    public int? TargetUserId { get; set; }

    public int? TargetVacancyId { get; set; }

    public string Description { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual ActionType Action { get; set; } = null!;

    public virtual User? TargetUser { get; set; }

    public virtual Vacancy? TargetVacancy { get; set; }
}
