using System;
using System.Collections.Generic;

namespace LinkNodeDomain.Model;

public partial class Client : Entity
{
    public string? CompanyName { get; set; }

    public int? HireRate { get; set; }

    public decimal? AvgHourlyRatePaid { get; set; }

    public virtual User ClientNavigation { get; set; } = null!;

    public virtual ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
