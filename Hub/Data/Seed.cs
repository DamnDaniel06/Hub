using Hub.Data.Enum;
using Hub.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Diagnostics;
using System.Net;

namespace Hub.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                //Users
                if (!context.Users.Any())
                {
                    context.Users.AddRange(new List<User>()
                    {
                        new User()
                        {
                            FirstName="John",
                            LastName="Cena",
                            Email="JCfarmer@gmail.com",
                            Password="password",

                         },
                        new User()
                        {
                            FirstName="John",
                            LastName="Wick",
                            Email="JWEmployee@gmail.com",
                            Password="superPass",

                         }
                    }) ;
                    context.SaveChanges();
                }
                // Fetch Farmer John 
                User farmerJohn = context.Users.Single(r => r.LastName== "Cena");

                var milk = nameof(ProductCategory.Diary);
                //Products
                if (!context.Products.Any())
                {
                    context.Products.AddRange(new List<Product>()
                    {
                        new Product()
                        {
                            Name="Milk",
                            Category=milk,
                            ProductionDate=DateTime.Now
                        }
                    });
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Employee))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Employee));
                if (!await roleManager.RoleExistsAsync(UserRoles.Farmer))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Farmer));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();

                if (userManager != null)
                {
                    string farmerUserEmail = "JCfarmer@gmail.com";
                    string password = "Coding@1234?";

                    var farmerUser = await userManager.FindByEmailAsync(farmerUserEmail);
                    if (farmerUser == null)
                    {
                        var newFarmerUser = new User()
                        {
                            FirstName = "John",
                            LastName = "Cena",
                            UserName = "Mister Cena",
                            Email = farmerUserEmail,
                            Password = password,
                        };
                        var newUserResponse = await userManager.CreateAsync(newFarmerUser, password);

                        if (!newUserResponse.Succeeded)
                        {
                            var error = newUserResponse.Errors;
                            Console.WriteLine(error);
                        }
                        else
                        {
                            await userManager.AddToRoleAsync(newFarmerUser, UserRoles.Farmer);
                        }

                    }
                    else
                    {
                        Console.WriteLine("Farmer not found");
                    }

                    string EmployeeUserEmail = "JWEmployee@gmail.com";
                    password = "passWord13!";

                    var employeeUser = await userManager.FindByEmailAsync(EmployeeUserEmail);
                    if (employeeUser == null)
                    {
                        var newFarmerUser = new User()
                        {
                            FirstName = "John",
                            LastName = "Wick",
                            UserName = "Mister Wick",
                            Email = farmerUserEmail,
                            Password = password,
                        };
                        await userManager.CreateAsync(newFarmerUser, password);
                        await userManager.AddToRoleAsync(newFarmerUser, UserRoles.Employee);
                    }


                }
                
            }
        }
    }
}
