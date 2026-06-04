using Microsoft.AspNetCore.Identity;
using webbangiay.Models; // Bắt buộc phải using namespace chứa ApplicationUser

namespace webbangiay.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. ĐỔI THÀNH UserManager<ApplicationUser>
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Tạo role Admin
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Tạo user Admin
            var adminEmail = "admin@sneakerhub.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                // 2. ĐỔI THÀNH new ApplicationUser
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail, // Bạn có thể đặt "admin" nếu muốn dùng tên ngắn
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FullName = "Quản trị viên Hệ thống" // THÊM TRƯỜNG NÀY VÌ NÓ LÀ BẮT BUỘC
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}