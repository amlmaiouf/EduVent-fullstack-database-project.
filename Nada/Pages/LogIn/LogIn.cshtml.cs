using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Database_Project.Pages.LogIn
{
    public class LogInModel : PageModel
    {
        [BindProperty]
        public required string email { get; set; }
        [BindProperty]
        public required string password { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
           
        }

        public IActionResult OnPost()
        {
          
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Email and password are required."; 
                return RedirectToPage();  
            }

            try
            {
                
                string connectionString = "Server=NADA_WALEED\\SQLEXPRESS01;Database=DatabaseProject;Trusted_Connection=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT role FROM Users WHERE email = @Email AND password = @Password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);

                       
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var userRole = reader["role"].ToString();

                               
                                if (userRole == "Admin")
                                {
                                    return RedirectToPage("/Admin/Admin");
                                }
                                else if (userRole == "Organizer")
                                {
                                    return RedirectToPage("/OrganizerDashboard"); 
                                }
                                else if (userRole == "Participant")
                                {
                                    return RedirectToPage("/ParticipantDashboard");  
                                }
                                else
                                {
                                    return RedirectToPage("/UserDashboard"); 
                                }
                            }
                            else
                            {
                                TempData["Error"] = "Invalid email or password."; 
                                return RedirectToPage();  
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred: " + ex.Message; 
                return RedirectToPage(); 
            }
        }
    }
}


