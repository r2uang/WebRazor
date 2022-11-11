using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebRazor.Helper;
using WebRazor.Models;

namespace WebRazor.Pages.Account
{
    public class ForgotModel : PageModel
    {
        private readonly PRN221DBContext dbContext;

        public ForgotModel(PRN221DBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [BindProperty]
        public string email { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (String.IsNullOrEmpty(email))
            {
                ViewData["msg"] = "Please enter your email to get password!";
                return Page();
            }

            var account = await dbContext.Accounts.FirstOrDefaultAsync(a => a.Email.Equals(email));
            if (account == null)
            {
                ViewData["msg"] = "Not found email in for system, please check again!";
                return Page();
            }

            String passwordGenerate = AccountHelper.GeneratePassword(8);
            account.Password = AccountHelper.HashPassWord(passwordGenerate);
            
            dbContext.Update(account);
            if (await dbContext.SaveChangesAsync() <= 0)
            {
                ViewData["msg"] = "System error, please try again!";
                return Page();
            }
            
            string bodyMail = "After login, please change your password! You new password is: " + passwordGenerate;
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("quangkthe153744@fpt.edu.vn");
                mail.To.Add(email);
                mail.Subject = "[Reset Password]";
                mail.Body = bodyMail;
                mail.IsBodyHtml = false;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("quangkthe153744@fpt.edu.vn", "quang@1213");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            return Page();
        }
    }
}
