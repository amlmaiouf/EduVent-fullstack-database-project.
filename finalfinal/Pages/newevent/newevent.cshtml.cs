using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;



namespace finalfinal.Pages.newevent
{
    public class NewEventModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public DateTime Date { get; set; }
        [BindProperty]
        public string Location { get; set; }
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public string Status { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string connectionString = @"Data Source=Rana-Dief;Initial Catalog=Users;Integrated Security=True";
            string sql = "INSERT INTO Events (Event_Name, Event_Date, Event_Location, Event_Description, Event_Status) VALUES (@Name, @Date, @Location, @Description, @Status)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {

                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Date", Date);
                        cmd.Parameters.AddWithValue("@Location", Location);
                        cmd.Parameters.AddWithValue("@Description", Description);
                        cmd.Parameters.AddWithValue("@Status", Status);

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToPage("Success");
            }
            catch (Exception ex)
            {
                
                ModelState.AddModelError(string.Empty, "An error occurred while saving the event: " + ex.Message);
                return Page(); 
            }
        }
    }
}