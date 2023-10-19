using AutoMapper;
using ContactBookAPI.Data.Interface;
using ContactBookAPI.Model;
using ContactBookAPI.Model.DTOs;
using ContactBookAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IUserRepository _userRepository;

        public UserController(IMapper mapper, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, ITokenGenerator tokenGenerator, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenGenerator = tokenGenerator;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            
                return BadRequest(ModelState);
            

            var newUser = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
            };

            var UserCreationResponse = await _userManager.CreateAsync(newUser, model.Password);

            if (!UserCreationResponse.Succeeded)
            
                return BadRequest(UserCreationResponse.Errors);
            

            var rolesToAdd = new List<string> { "regular" }; 

            var addToRolesResult = await _userManager.AddToRolesAsync(newUser, rolesToAdd);

            if (!addToRolesResult.Succeeded)
            
                return BadRequest(addToRolesResult.Errors);
            

            return Ok("User registered and added to roles successfully");
        }

       
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto model)
        {
            var userExistInDb = await _userManager.FindByEmailAsync(model.Email);
            if (userExistInDb == null)
            
                return NotFound("User Not Found");
            

            var result = await _signInManager.PasswordSignInAsync(userExistInDb, model.Password, false, false);
            if (!result.Succeeded)
            
                return BadRequest("Invalid Credentials");
            
            var token = await _tokenGenerator.GenerateToken(userExistInDb);
            var responseToLogin = new LoginResponseDTO
            {
                Token = token,
                Message = "Login Successful",
                Success = true
            };

            return Ok(responseToLogin);
        }

       
        [HttpPost("add-new")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddNewUser([FromBody] UserToAddDTO model)
        {
            try
            {
                var result = await _userRepository.AddUser(model);
                if (result)
                {
                    return Ok("User Added Successfully");
                }
                else
                {
                    return BadRequest("Something went wrong");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding user: {ex.Message}");
            }
        }

      
        [HttpGet("get-users")]
        [Authorize(Roles = "admin")]
        public ActionResult GetUsers()
        {
            try
            {
                var adminUsers = _userManager.GetUsersInRoleAsync("admin").Result;
                var regularUsers = _userManager.GetUsersInRoleAsync("regular").Result.ToList();
                var users = new List<AppUser>();
                users.AddRange(adminUsers);
                users.AddRange(regularUsers);
                var usersToReturn = _mapper.Map<List<UserToReturnDTO>>(users);
                return Ok(usersToReturn);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving users: {ex.Message}");
            }
        }
    }
}
