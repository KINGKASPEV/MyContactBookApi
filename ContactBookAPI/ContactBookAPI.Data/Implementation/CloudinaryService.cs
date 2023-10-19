using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using ContactBookAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactBookAPI.Data.Interface;

namespace ContactBookAPI.Data.Implementation
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> cloudinaryConfig)
        {
            var account = new Account(
                cloudinaryConfig.Value.CloudName,
                cloudinaryConfig.Value.ApiKey,
                cloudinaryConfig.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("Invalid file");
                }

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream())
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    Console.WriteLine("Cloudinary Error: " + uploadResult.Error.Message);


                    throw new Exception("Image upload failed: " + uploadResult.Error.Message);
                }

                return uploadResult.SecureUri.AbsoluteUri;
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error in UploadImageAsync: " + ex.Message);


                throw;
            }
        }
    }
}
