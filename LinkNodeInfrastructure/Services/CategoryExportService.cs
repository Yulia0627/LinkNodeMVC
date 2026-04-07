using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;

namespace LinkNodeInfrastructure.Services
{

    public class CategoryExportService : IExportService<LinkNodeDomain.Model.Category>
    {
        private const string RootWorksheetName = "";

        private static readonly IReadOnlyList<string> HeaderNames =
            new string[]
            {
                "Title",
                "EmpType",
                "Price",
                "Description",
            };
        private readonly DbLinkNodeContext _context;

        private static void WriteHeader(IXLWorksheet worksheet)
        {
            for (int columnIndex = 0; columnIndex < HeaderNames.Count; columnIndex++)
            {
                worksheet.Cell(1, columnIndex + 1).Value = HeaderNames[columnIndex];
            }
            worksheet.Row(1).Style.Font.Bold = true;
        }

        private void WriteVacancy(IXLWorksheet worksheet, LinkNodeDomain.Model.Vacancy vacancy, int rowIndex)
        {
            var columnIndex = 1;
            worksheet.Cell(rowIndex, columnIndex++).Value = vacancy.Title;

            worksheet.Cell(rowIndex, columnIndex++).Value = GetEmpTypeName(vacancy.EmpTypeId);

            worksheet.Cell(rowIndex, columnIndex++).Value = vacancy.Price;

            worksheet.Cell(rowIndex, columnIndex++).Value = vacancy.Description;


        }

        private void WriteVacancies(IXLWorksheet worksheet, ICollection<LinkNodeDomain.Model.Vacancy> vacancies)
        {
            WriteHeader(worksheet);
            int rowIndex = 2;
            foreach (var vacancy in vacancies)
            {
                WriteVacancy(worksheet, vacancy, rowIndex);
                rowIndex++;
            }
            var descriptionColumn = worksheet.Column(4); 
            descriptionColumn.Width = 60; 
            descriptionColumn.Style.Alignment.WrapText = true; 
            descriptionColumn.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top; 

           
            worksheet.Columns(1, 3).AdjustToContents();
        }

        private void WriteCategories(XLWorkbook workbook, ICollection<LinkNodeDomain.Model.Category> categories)
        {
            foreach (var cat in categories)
            {

                if (cat is not null)
                {
                    string sheetName = cat.Category1.Length > 31
                ? cat.Category1.Substring(0, 31)
                : cat.Category1;
                    var worksheet = workbook.Worksheets.Add(sheetName);
                    WriteVacancies(worksheet, cat.Vacancies.ToList());
                }
            }
        }

        public CategoryExportService(DbLinkNodeContext context)
        {
            _context = context;
        }

        public async Task WriteToAsync(Stream stream, int clientId, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Input stream is not writable");
            }

           
            var categories = await _context.Categories
                .Include(c => c.Vacancies)
                .Where(c => c.Vacancies.Any(v => v.ClientId == clientId)) 
                .ToListAsync(cancellationToken);

           
            foreach (var cat in categories)
            {
                cat.Vacancies = cat.Vacancies.Where(v => v.ClientId == clientId).ToList();
            }

            using (var workbook = new XLWorkbook())
            {
                WriteCategories(workbook, categories);

               
                workbook.SaveAs(stream);
            }
        }
        private static string GetEmpTypeName(int? empTypeId)
        {
            if (empTypeId == 1)
            {
                return "full-time";
            }

            else if (empTypeId == 2)
            {
                return "part-time";
            }
           
            else
            {
                return "unknown";
            }
        }

    }
}

