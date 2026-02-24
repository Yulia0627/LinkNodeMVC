using System;
using System.Collections.Generic;

namespace LinkNodeDomain.Model;

public partial class Admin : Entity
{
    public int ActionId { get; set; }

    public int? TargetUserId { get; set; }

    public int? TargetVacancyId { get; set; }

    public string Description { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual ActionType Action { get; set; } = null!;

    public virtual User AdminNavigation { get; set; } = null!;

    public virtual User? TargetUser { get; set; }

    public virtual Vacancy? TargetVacancy { get; set; }
}
