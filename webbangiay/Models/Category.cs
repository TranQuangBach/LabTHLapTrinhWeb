using System.ComponentModel.DataAnnotations;

namespace webbangiay.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100, ErrorMessage = "Tên danh mục không quá 100 ký tự")]
        public string Name { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
