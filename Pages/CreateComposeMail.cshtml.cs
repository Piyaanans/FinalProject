using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FinalProject.Pages
{
    public class CreateComposeMailModel : PageModel
    {
        public EmailInfo emailInfo = new EmailInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            emailInfo.EmailSubject = Request.Form["emailsubject"];
            emailInfo.EmailMessage = Request.Form["emailmessage"];
            emailInfo.EmailReceiver = Request.Form["emailreceiver"];

            if (emailInfo.EmailSubject.Length == 0 || emailInfo.EmailMessage.Length == 0 ||
                emailInfo.EmailReceiver.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                String connectionString = "Server=tcp:finalprojectcs436.database.windows.net,1433;Initial Catalog=FinalProjectBuemail;Persist Security Info=False;User ID=bigb;Password=Softengineer006;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string username = "";
                    if (User.Identity.Name == null)
                    {
                        username = "";
                    }
                    else
                    {
                        username = User.Identity.Name;
                    }

                    String sql = "INSERT INTO emails " +
                                 "(emailsubject, emailmessage, emailisread, emailsender, emailreceiver) VALUES " +
                                 "(@emailsubject, @emailmessage, @emailisread, @emailsender, @emailreceiver)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@emailsubject", emailInfo.EmailSubject);
                        command.Parameters.AddWithValue("@emailmessage", emailInfo.EmailMessage);
                        command.Parameters.AddWithValue("@emailisread", "0");
                        command.Parameters.AddWithValue("@emailsender", username);
                        command.Parameters.AddWithValue("@emailreceiver", emailInfo.EmailReceiver);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            emailInfo.EmailSubject = "";
            emailInfo.EmailMessage = "";
            emailInfo.EmailSender = "";
            emailInfo.EmailReceiver = "";
            successMessage = "New Item Added Correctly";

            Response.Redirect("/Index");
        }
    }
}