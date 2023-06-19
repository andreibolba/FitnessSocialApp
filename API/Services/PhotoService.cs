using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
        }

        public ImageUploadResult AddImage(IFormFile file, string folderName)
        {
            var uploadResult = new ImageUploadResult();
            if(file.Length>0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = folderName
                };
                uploadResult = _cloudinary.Upload(uploadParams);
            }

            return uploadResult;
        }

        public DeletionResult DeleleteImage(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            return _cloudinary.Destroy(deleteParams);   
        }
    }
}
