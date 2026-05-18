using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webbangiay.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không quá 100 ký tự")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
        [StringLength(500, ErrorMessage = "Mô tả không quá 500 ký tự")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(1000, 100000000, ErrorMessage = "Giá phải từ 1.000đ đến 100.000.000đ")]
        public decimal Price { get; set; }

        [Range(0, 100000000, ErrorMessage = "Giá cũ không hợp lệ")]
        public decimal? OldPrice { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Đánh giá không được để trống")]
        [Range(0, 5, ErrorMessage = "Đánh giá từ 0 đến 5 sao")]
        public double Rating { get; set; }

        public bool IsOnSale { get; set; }
        public int DiscountPercent { get; set; }
        public bool IsNew { get; set; }
        public List<string> Colors { get; set; }

        public string ColorsString
        {
            get => Colors != null ? string.Join(",", Colors) : "";
            set => Colors = string.IsNullOrEmpty(value)
                ? new List<string>()
                : value.Split(',').ToList();
        }

        public string BadgeText { get; set; }
        public string BadgeClass { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
