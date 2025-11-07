using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTicket.Models
{
    public class TicketType
    {
        public int TicketTypeId { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

     
    }

}
