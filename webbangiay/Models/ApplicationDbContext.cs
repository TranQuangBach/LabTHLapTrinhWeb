using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace webbangiay.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public
        ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // BẮT BUỘC: Dòng này phải nằm trên cùng của hàm
            base.OnModelCreating(builder);

            // Các cấu hình Fluent API khác của bạn (nếu có)
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
