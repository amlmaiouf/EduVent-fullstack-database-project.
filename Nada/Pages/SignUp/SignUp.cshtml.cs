using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;


namespace Database_Project.Pages.SignUp
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public required string full_name { get; set; }
        [BindProperty]
        public required string email { get; set; }
        [BindProperty]
        public required string password { get; set; }
        [BindProperty]
        public required string confirm_password { get; set; }
        [BindProperty]
        public required string role { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
            // This is triggered when the page is first loaded.
        }

        public void OnPost()
        {
            // Validate inputs
            if (string.IsNullOrEmpty(full_name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                ErrorMessage = "All fields are required.";
                return;
            }

            if (password != confirm_password)
            {
                ErrorMessage = "Passwords do not match.";
                return;
            }

            try
            {

                
                string connectionString = "Server=NADA_WALEED\\SQLEXPRESS01;Database=DatabaseProject;Trusted_Connection=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Users (FullName, Email, Password, Role) VALUES (@FullName, @Email, @Password, @Role)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", full_name);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password); 
                        command.Parameters.AddWithValue("@Role", role);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            SuccessMessage = "Account created successfully!";
                        }
                        else
                        {
                            ErrorMessage = "Something went wrong. Please try again.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred: " + ex.Message;
            }
        }
    }
}
