using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        ImageUploadResult AddImage(IFormFile file, string folderName);
        DeletionResult DeleleteImage(string publicId);
    }
}
