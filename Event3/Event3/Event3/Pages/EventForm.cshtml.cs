using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Event3.Pages
{
    public class Index1Model : PageModel
    {
        [BindProperty]
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string FullName { get; set; }
        public string ConfirmationMessage { get; set; }

        public IActionResult OnPost(string fullName, string eventName, DateTime eventDate)
        {

            TempData["ConfirmationMessage"] = $"Thank you, {FullName}. Your ticket(s) have been booked.";


            return RedirectToPage();
        }

        public void OnGet()
        {
            ConfirmationMessage = TempData["ConfirmationMessage"] as string;
        }

        
}
}
