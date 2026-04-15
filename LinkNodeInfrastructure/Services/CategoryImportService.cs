using ClosedXML.Excel;
using LinkNodeDomain.Model;
using LinkNodeInfrastructure.Services;
using Microsoft.EntityFrameworkCore;
namespace LinkNodeInfrastructure.Services
{
    public class CategoryImportService : IImportService<Category>
    {
        private readonly DbLinkNodeContext _context;

        public CategoryImportService(DbLinkNodeContext context)
        {
            _context = context;
        }

        public async Task ImportFromStreamAsync(Stream stream, int clientId, CancellationToken cancellationToken)
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException("Дані не можуть бути прочитані.", nameof(stream));
            }

            using (XLWorkbook workVacancy = new XLWorkbook(stream))
            {
                
                foreach (IXLWorksheet worksheet in workVacancy.Worksheets)
                {
                    

                    var catname = worksheet.Name;
                    var category = await _context.Categories.FirstOrDefaultAsync(cat => cat.Category1 == catname, cancellationToken);
                    if (category == null)
                    {
                        category = new Category();
                        category.Category1 = catname;
                       
                        _context.Categories.Add(category);
                    }
                                       
                    foreach (var row in worksheet.RowsUsed().Skip(1))
                  
                    {
                        await AddVacancyAsync(row, cancellationToken, category, clientId);
                    }
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task AddVacancyAsync(IXLRow row, CancellationToken cancellationToken, Category category, int clientId)
        {
            Vacancy vacancy = new Vacancy();
            vacancy.ClientId = clientId;
            vacancy.Title = GetVacancyTitle(row);
            vacancy.EmpTypeId = GetEmpTypeId(row);
            vacancy.Category = category;
            vacancy.Price = GetVacancyPrice(row);
            vacancy.Description = GetVacancyDescription(row);
            vacancy.CreatedDate = DateTime.Now;
            vacancy.ClosedDate = null;
            _context.Vacancies.Add(vacancy);

        }

        private static string GetVacancyTitle(IXLRow row)
        {
            var value = row.Cell(1).Value;

            if (value.IsBlank)
            {
                throw new ArgumentException($"назва вакансії не може бути порожньою.");
            }

            string title = value.ToString().Trim();

            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException($"назва містить лише пробіли.");
            }

            return title;
        }

        private static int GetEmpTypeId(IXLRow row)
        {
            string type = row.Cell(2).Value.ToString().ToLower();
            if (type == "full-time")
                return 1;
            else if (type == "part-time")
                return 2;
            else
                throw new ArgumentException($"невідомий тип зайнятості: {type}");
        }

        private static decimal GetVacancyPrice(IXLRow row)
        {
            
            var value = row.Cell(3);

          
            if (value.Value.IsBlank)
            {
                throw new ArgumentException("ціна не може бути порожньою.");
            }

           
            if (!value.TryGetValue(out decimal price))
            {
                throw new ArgumentException("некоректний формат ціни. Очікується число.");
            }

            
            if (price < 0)
            {
                throw new ArgumentException("ціна не може бути меншою за 0.");
            }

            return price;
        }

        private static string GetVacancyDescription(IXLRow row)
        {
            var value = row.Cell(4).Value;

            if (value.IsBlank)
            {
                throw new ArgumentException($"опис вакансії не можу бути порожнім.");
            }

            string desc = value.ToString();

            if (string.IsNullOrEmpty(desc))
            {
                throw new ArgumentException($"опис вакансії містить лише пробіли.");
            }

            return desc;
        }
    }
}
