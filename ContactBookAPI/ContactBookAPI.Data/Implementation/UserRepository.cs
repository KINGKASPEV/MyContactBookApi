using ContactBookAPI.Data.Interface;
using ContactBookAPI.Model;
using ContactBookAPI.Model.DTOs;
using Microsoft.AspNetCore.Identity;

namespace ContactBookAPI.Data.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ContactBookDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(
            ContactBookDbContext dbContext,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AddUser(UserToAddDTO newUser)
        {
            try
            {
                var user = new AppUser
                {
                    Email = newUser.Email,
                    UserName = newUser.Email,
                    PhoneNumber = newUser.PhoneNumber,
                    Contact = new Contact
                    {
                        Email = newUser.Email,
                        FirstName = newUser.FirstName,
                        LastName = newUser.LastName,
                        Address = new Address
                        {
                            Street = newUser.Street,
                            City = newUser.City,
                            State = newUser.State,
                            Country = newUser.Country
                        }
                    }
                };

                var result = await _userManager.CreateAsync(user, newUser.Password);

                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, newUser.Role);

                    if (roleResult.Succeeded)
                    {
                        await _dbContext.SaveChangesAsync(); 
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Role assignment failed: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    Console.WriteLine($"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
                return false;
            }
        }

    }
}
