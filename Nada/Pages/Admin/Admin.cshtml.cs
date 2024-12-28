using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Database_Project.Pages.Admin;

namespace Database_Project.Pages.Admin
{
    public class AdminEventsModel : PageModel
    {
        
        [BindProperty]
        public int Event_Id { get; set; }
        [BindProperty]
        public string Event_Name { get; set; }
        [BindProperty]
        public DateTime Event_Date { get; set; }
        [BindProperty]
        public string? Event_Description { get; set; } 
        [BindProperty]
        public string Event_Location { get; set; }
        [BindProperty]
        public string Event_Status { get; set; }

        
        public List<Event> Events { get; set; } = new List<Event>();

        
        public IActionResult OnGet()
        {
            Events = GetEventsFromDatabase();
            return Page();
        }

       
        private List<Event> GetEventsFromDatabase()
        {
            List<Event> events = new List<Event>();

            string connectionString = "Server=NADA_WALEED\\SQLEXPRESS01;Database=DatabaseProject;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Event_Id, Event_Name, Event_Date, Event_Location, Event_Description, Event_Status FROM Events";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events.Add(new Event
                            {
                                Event_Id = (int)reader["Event_ID"],
                                Event_Name = reader["Event_Name"].ToString(),
                                Event_Date = (DateTime)reader["Event_Date"],
                                Event_Location = reader["Event_Location"].ToString(),
                                Event_Description = reader["Event_Description"].ToString(),
                                Event_Status = reader["Event_Status"].ToString()
                            });
                        }
                    }
                }
            }

            return events;
        }

        
        public IActionResult OnPostDeleteEvent(int Event_Id)
        {
            DeleteEvent(Event_Id);
            return RedirectToPage(); 
        }

        public IActionResult OnDeleteEvent(int Event_Id)
        {
            DeleteEvent(Event_Id);
            return new JsonResult("Event deleted successfully");
        }


        private void DeleteEvent(int Event_Id)
        {
            string connectionString = "Server=NADA_WALEED\\SQLEXPRESS01;Database=DatabaseProject;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Events WHERE Event_Id = @Event_Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Event_Id", Event_Id);
                    command.ExecuteNonQuery();
                }
            }
        }


        public IActionResult OnPostUpdateEvent()
        {
            if (ModelState.IsValid)
            {
                UpdateEventInDatabase();
                return RedirectToPage(); 
            }

            return Page();
        }


        private void UpdateEventInDatabase()
        {
            string connectionString = "Server=NADA_WALEED\\SQLEXPRESS01;Database=DatabaseProject;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Events SET Event_Name = @Event_Name, Event_Date = @Event_Date, Event_Location = @Event_Location, Event_Description = @ Event_Description, Event_Status = @Event_Status WHERE Event_Id = @Event_Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Event_Id", Event_Id);
                    command.Parameters.AddWithValue("@Event_Name", Event_Name);
                    command.Parameters.AddWithValue("@Event_Date", Event_Date);
                    command.Parameters.AddWithValue("@Event_Location", Event_Location);
                    command.Parameters.AddWithValue("@Event_Description", Event_Description);
                    command.Parameters.AddWithValue("@Event_Status", Event_Status);
                    command.ExecuteNonQuery();
                }
            }
        }


        public IActionResult OnGetEventById(int Event_Id)
        {
            Event Event_Item = GetEventByIdFromDatabase(Event_Id);
            return new JsonResult(Event_Item);
        }

        private Event GetEventByIdFromDatabase(int Event_Id)
        {
            Event Event_Item = null;
            string connectionString = "Server=NADA_WALEED\\SQLEXPRESS01;Database=DatabaseProject;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Event_Id, Event_Name, Event_Date, Event_Location, Event_Description, Event_Status FROM Events WHERE Event_Id = @Event_Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Event_Id", Event_Id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Event_Item = new Event
                            {
                                Event_Id = (int)reader["Event_Id"],
                                Event_Name = reader["Event_Name"].ToString(),
                                Event_Date = (DateTime)reader["Event_Date"],
                                Event_Location = reader["Event_Location"].ToString(),
                                Event_Description = reader["Event_Description"].ToString(),
                                Event_Status = reader["Event_Status"].ToString()
                            };
                        }
                    }
                }
            }
            return Event_Item;
        }
    }
}