using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace WebRazor.Pages.Admin.Product
{
    public class ImportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<List<Models.Product>> Import(IFormFile file)
        {
            var list = new List<Models.Product>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    for (int i = 2; i < rowcount; i++)
                    {
                        list.Add(new Models.Product
                        {
                            ProductId = int.Parse(worksheet.Cells[i, 1].Value.ToString().Trim()),
                            ProductName = worksheet.Cells[i, 2].Value.ToString().Trim(),
                            //CategoryId = worksheet.Cells[i, 3].Value.ToString().Trim(),
                            //QuantityPerUnit = decimal.Parse(worksheet.Cells[i, 4].Value.ToString().Trim()),
                            //UnitPrice = worksheet.Cells[i, 5].Value.ToString().Trim(),
                            //UnitsInStock = worksheet.Cells[i, 6].Value.ToString().Trim(),
                            //UnitsOnOrder = worksheet.Cells[i, 7].Value.ToString().Trim(),
                            //ReorderLevel = worksheet.Cells[i, 8].Value.ToString().Trim(),
                            //Discontinued = worksheet.Cells[i, 9].Value.ToString().Trim(),
                        });
                    }
                }
            }
            return list;
        }
    }
}
