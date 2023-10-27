using Weblab.Architecture.Enums;

namespace Weblab.Architecture.Interfaces;
public interface IImageService
{
    public Task<(Guid Id, ImageUploadStatusCode Status)> Upload(IFormFile uploadedFile, string area);
    public Task<bool> Delete(Guid id, string area);
    public string GetImagePath(string imageArea, Guid id);
}