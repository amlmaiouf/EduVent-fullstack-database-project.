using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace finalfinal.Pages
{

    public class IndexModel : PageModel
    {

        public List<Events> listevents= new List<Events>();
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [Obsolete]
        public void OnGet()
        {
            string connectionString = @"Data Source=Rana-Dief;Initial Catalog=Users;Integrated Security=True";
            string sql = "SELECT Event_ID, Event_Name, Event_Date, Event_Location, Event_Description, Event_Status FROM Events";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open(); 

                    using (SqlCommand cmd = new SqlCommand(sql, connection)) 
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader()) 
                        {
                            while (reader.Read())
                            {
                                
                                Events events = new Events(
                                    reader.GetInt32(0), 
                                    reader.GetString(1), 
                                    reader.GetDateTime(2).ToString("yyyy-MM-dd"), 
                                    reader.GetString(3), 
                                    reader.GetString(4), 
                                    reader.GetString(5)  
                                );

                                listevents.Add(events); 
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                Console.WriteLine("An error occurred: " + ex.Message);
            }
          
        }

        public class Events
        {
            public int id { get; set;} 
            public string name { get; set; }
            public string date { get; set; }
            public string location { get; set; }
            public string description { get; set; }
            public string status { get; set; }

            // Constructor
            public Events(int id, string name, string date, string location, string description, string status)
            {
                this.id = id; // Ensure id is set
                this.name = name;
                this.date = date;
                this.location = location;
                this.description = description;
                this.status = status;
            }
        }
    }
    }

