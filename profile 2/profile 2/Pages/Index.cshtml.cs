using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace profile_2.Pages
{
    public partial class IndexModel : PageModel 
    {
        private bool IsPostBack;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string currentPassword = Request.Form["password"];
                string newPassword = Request.Form["newpassword"];
                string renewPassword = Request.Form["renewpassword"];

                if (newPassword != renewPassword)
                {
                    Response.WriteAsJsonAsync("<script>alert('New passwords do not match.');</script>");
                    return;
                }

                string username = "exampleUser"; // Replace with dynamic username retrieval if logged in
                ChangeUserPassword(username, currentPassword, newPassword);
            }
        }

        private void ChangeUserPassword(string username, string currentPassword, string newPassword)
        {
            string connectionString = "Server=NADA_WALEED\\SQLEXPRESS01;Database=DatabaseProject;Trusted_Connection=True;"; // Replace with your actual connection string
            string query = "UPDATE Users SET Password = @NewPassword WHERE Username = @Username AND Password = @CurrentPassword";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@CurrentPassword", currentPassword);
                command.Parameters.AddWithValue("@NewPassword", newPassword);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Response.WriteAsJsonAsync("<script>alert('Password changed successfully.');</script>");
                }
                else
                {
                    Response.WriteAsJsonAsync("<script>alert('Current password is incorrect.');</script>");
                }
            }
        }
    }
}
