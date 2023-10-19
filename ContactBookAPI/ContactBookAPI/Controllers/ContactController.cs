using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ContactBookAPI.Data.Interface;
using ContactBookAPI.Model;
using ContactBookAPI.Model.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ContactBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;
        private readonly IConfiguration _config;
        private readonly ILogger<ContactController> _logger;
        private readonly IContactRepository _contactRepository;
        private readonly UserManager<AppUser> _userManager;

        public ContactController(ILogger<ContactController> logger, IContactRepository contactRepository, IConfiguration config, UserManager<AppUser> userManager)
        {
            _config = config;
            Account account = new Account
            {
                Cloud = _config.GetSection("CloudinarySettings:CloudName").Value,
                ApiKey = _config.GetSection("CloudinarySettings:ApiKey").Value,
                ApiSecret = _config.GetSection("CloudinarySettings:ApiSecret").Value,
            };
            _cloudinary = new Cloudinary(account);
            _logger = logger;
            _contactRepository = contactRepository;
            _userManager = userManager;
        }

        [HttpGet("all-contacts")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> GetContacts([FromQuery] PaginationFilter filter)
        {
            var resp = await _contactRepository.Get(filter);
            return Ok(resp);

        }

        [HttpGet("{emailOrId}")]
        [Authorize(Roles = "admin,regular")]
        public async Task<IActionResult> GetByIdOrEmail(string emailOrId)
        {
            return Ok(await _contactRepository.GetByIdOrEmail(emailOrId));
        }

        [HttpPost("add-new")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> CreateUser([FromForm] ContactDto contact)
        {
            var result = await _contactRepository.Create(contact);
            if (result)
            {
                return Ok("Contact Successfully Added");
            }
            else
            {
                return BadRequest("Something went wrong, try again");
            }
        }

        [HttpPatch("photo/{Id}")]
        //[Authorize(Roles = "admin,regular")]
        public async Task<ActionResult> UpdatePhoto(int Id, [FromForm] PhotoToAddDto model)
        {
            var ddd = User.FindFirst(ClaimTypes.Role).Value;
            var imageUploadResult = new ImageUploadResult();

            if (ddd == "admin")
            {
                var file = model.PhotoFile;
                if (file.Length <= 0)
                    return BadRequest("Invalid file size");

                using (var fs = file.OpenReadStream())
                {
                    var imageUploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, fs),
                        Transformation = new Transformation().Width(300).Height(300)
                        .Crop("fill").Gravity("face")
                    };
                    imageUploadResult = _cloudinary.Upload(imageUploadParams);
                }
            }

            else
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var contact = await _contactRepository.GetByEmail(userEmail);

                if (Id != contact.Id)
                
                    return Unauthorized();
                
                
                var file = model.PhotoFile;
                if (file.Length <= 0)
                    return BadRequest("Invalid file size");

                using (var fs = file.OpenReadStream())
                {
                    var imageUploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, fs),
                        Transformation = new Transformation().Width(300).Height(300)
                        .Crop("fill").Gravity("face")
                    };
                    imageUploadResult = _cloudinary.Upload(imageUploadParams);
                }
            }

            var publicId = imageUploadResult.PublicId;
            var Url = imageUploadResult.Url.ToString();
            await _contactRepository.UpdatePhoto(Id, Url);

            return Ok(new { id = publicId, Url });

        }

        
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var contactToDelete = await _contactRepository.GetById(id);
            if (contactToDelete == null)
            {
                return NotFound();
            }
            await _contactRepository.Delete(contactToDelete);
            return NoContent();
        }

       
        [HttpPut("update/{Id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Update(int Id, [FromForm] ContactDto contact)
        {
            var foundContact = _contactRepository.GetById(Id).GetAwaiter().GetResult();
            if (foundContact == null)
            
                return NotFound("No such contact found");
            

            var result = await _contactRepository.Update(Id, contact);

            if (result)
            {
                return Ok("Contact Successfully Updated");
            }
            else
            {
                return BadRequest("Something went wrong, try again");
            }
        }

       
        [HttpGet("search")]
        [Authorize(Roles = "admin,regular")]
        public IActionResult Search([FromQuery] SearchDTO model)
        {
            var contactToReturn = _contactRepository.Search(model.Name, model.State, model.City);
            if (contactToReturn == null)
            {
                return NotFound("No Contact associated search ");
            }
            var output = new List<SearchResponseDTO>();
            foreach (var contact in contactToReturn)
            {
                var response = new SearchResponseDTO
                {
                    Name = $"{contact.FirstName} {contact.LastName}",
                    Email = contact.Email,
                    Address = $"{contact.Address.Street} {contact.Address.City}, {contact.Address.State}, {contact.Address.Country}"
                };
                output.Add(response);
            }
            return Ok(output);
        }
    }
}
