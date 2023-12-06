
using FinalProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using System;

namespace FinalProject.Pages
{
    public class ReadEmailModel : PageModel
    {
        public List<EmailInfo> listEmails = new List<EmailInfo>();
        public void OnGet()
        {
            try
            {
                String emailid = Request.Query["emailid"];

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
                    String sql = "SELECT * FROM emails WHERE emailreceiver='" + username + "' AND emailid = @emailid ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@emailid", emailid);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmailInfo emailInfo = new EmailInfo();
                                emailInfo.EmailID = "" + reader.GetInt32(0);
                                emailInfo.EmailSubject = reader.GetString(1);
                                emailInfo.EmailMessage = reader.GetString(2);
                                emailInfo.EmailDate = reader.GetDateTime(3).ToString();
                                emailInfo.EmailIsRead = "" + reader.GetInt32(4);
                                emailInfo.EmailSender = reader.GetString(5);
                                emailInfo.EmailReceiver = reader.GetString(6);

                                listEmails.Add(emailInfo);
                            }
                        }

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
