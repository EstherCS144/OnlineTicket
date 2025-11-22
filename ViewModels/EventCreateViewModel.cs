using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace OnlineTicket.ViewModels
{
    public class EventCreateViewModel
    {
        [Required]
        public string Title { get; set; }


        [Required]
        public DateTime EventDate { get; set; }


        [Required]
        [Range(0, 999999)]
        public decimal TicketPrice { get; set; }


        public string? Description { get; set; }


        public IFormFile? ImageFile { get; set; }


        [Required]
        public int CategoryId { get; set; }


        [Required]
        public int VenueId { get; set; }


        [Required]
        public int TotalSeats { get; set; }


        public IEnumerable<SelectListItem>? Categories { get; set; }
        public IEnumerable<SelectListItem>? Venues { get; set; }
    }
}