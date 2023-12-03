using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace TalabatAPIs.DTOs
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }
        [Required]
        public int DeliveryMethodId { get; set; }
        public AddressDto ShippingAddress { get; set; }
    }
}
