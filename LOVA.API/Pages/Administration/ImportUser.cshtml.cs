using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOVA.API.Areas.Identity.Pages.Account;
using LOVA.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace LOVA.API.Pages.Administration
{
    public class ImportUserModel : PageModel
    {

        private IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ImportUserModel(IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, ILogger<RegisterModel> logger, RoleManager<IdentityRole> roleManager)
        {
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        public void OnGet()
        {
        }

        public string password { get; set; }
        public async Task<ActionResult> OnPostImportFromExcel()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);

            var role = _roleManager.FindByIdAsync("27482134-79d4-4f3d-b4fb-02a0f6c3511a").Result;

            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")//This will read the Excel 97-2000 formats    
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                    else //This will read 2007 Excel format    
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                    IRow headerRow = sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;
                    // Start creating the html which would be displayed in tabular format on the screen  
                    sb.Append("<table class='table'><tr>");
                    for (int j = 0; j < cellCount; j++)
                    {
                        NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                        sb.Append("<th>" + cell.ToString() + "</th>");
                    }
                    sb.Append("</tr>");
                    sb.AppendLine("<tr>");
                    var user = new ApplicationUser();

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            string cellValue = row.GetCell(j) == null ? "" : row.GetCell(j).ToString();
                            sb.Append("<td>" + cellValue + "</td>");
                            switch (headerRow.GetCell(j).ToString())
                            {
                                case "Förnamn":
                                    user.ForeName = cellValue;
                                    break;
                                case "Efternamn":
                                    user.LastName = cellValue;
                                    break;
                                case "Förnamn2":
                                    user.ForeName2 = cellValue;
                                    break;
                                case "Efternamn2":
                                    user.LastName2 = cellValue;
                                    break;
                                case "Fastighet":
                                    user.Property = cellValue;
                                    break;
                                case "Inloggning":
                                    user.UserName = cellValue;
                                    break;
                                case "Lösenord":
                                    password = cellValue;
                                    break;
                                case "Address":
                                    user.Address = cellValue;
                                    break;
                                case "Postnummer":
                                    user.PostalCode = cellValue;
                                    break;
                                case "Ort":
                                    user.City = cellValue;
                                    break;
                                case "Telefon":
                                    user.Phone = cellValue;
                                    break;
                                case "Telefon2":
                                    user.Phone2 = cellValue;
                                    break;
                                case "epost":
                                    user.Email = cellValue;
                                    break;
                                case "epost2":
                                    user.Email2 = cellValue;
                                    break;

                                default:
                                    break;
                            }

                        }
                        sb.AppendLine("</tr>");

                        user.EmailConfirmed = true;


                        /// Saves the user in the Excel row
                        bool IsSaved = await SaveUser(user, role, password);
                    }
                    sb.Append("</table>");
                }               
            }
            return this.Content(sb.ToString());
        }


        private async Task<bool> SaveUser(ApplicationUser user, IdentityRole role, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                await _userManager.AddToRoleAsync(user, role.Name);
                return true;
            }
            else
            {
                return false;
            }

        }



    }
}
