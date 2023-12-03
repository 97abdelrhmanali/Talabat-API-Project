using System.ComponentModel.DataAnnotations;

namespace TalabatAPIs.DTOs
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        [Range(0.1,double.MaxValue,ErrorMessage ="The Price must be greater than Zero")]
        public decimal Price { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        [Range(0,int.MaxValue,ErrorMessage ="The Quantity must be greater than Zero")]
        public int Quantity { get; set; }
    }
}