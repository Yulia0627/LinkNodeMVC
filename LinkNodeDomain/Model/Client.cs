using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinkNodeDomain.Model;

public partial class Client : Entity
{
    [Display(Name = "Назва компанії")]
    public string? CompanyName { get; set; }
 
    [Display(Name = "Відсоток найму")]
    public int? HireRate { get; set; }

    [Display(Name = "Середня оплата за годину")]
    public decimal? AvgHourlyRatePaid { get; set; }

    public virtual User ClientNavigation { get; set; } = null!;

    public virtual ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
