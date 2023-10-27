using SixLabors.ImageSharp.Formats.Jpeg;
using Weblab.Architecture.Enums;
using Weblab.Architecture.Interfaces;

namespace MVCSite.Features.Services;

public class ImageService : IImageService
{
    private readonly string _hostEnviroment;
    public ImageService(IWebHostEnvironment hostEnvironment)
    {
        _hostEnviroment = hostEnvironment.ContentRootPath;
    }
    public async Task<(Guid Id, ImageUploadStatusCode Status)> Upload(IFormFile uploadedFile, string area)
    {
        if(uploadedFile == null)
            return (Guid.Empty, ImageUploadStatusCode.Error);
        if(!IsImage(uploadedFile))
            return (Guid.Empty, ImageUploadStatusCode.Error);
        var id = Guid.NewGuid();
        var imagePath = _hostEnviroment + $"/AppData/{area}/" + id.ToString() + ".jpg";
        var imageFolderPath = _hostEnviroment + $"/AppData/{area}/";
        if (Directory.Exists(imageFolderPath))
        {
            Console.WriteLine("Путь уже существует.");
        }
        else
        {
            try
            {
                Directory.CreateDirectory(imageFolderPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании пути: {ex.Message}");
                return (Guid.Empty, ImageUploadStatusCode.PathCreationError);
            }
        }
        if(await SaveAsJpg(uploadedFile, imagePath))
            return (id, ImageUploadStatusCode.Success);
        else
            return (Guid.Empty, ImageUploadStatusCode.Error);
    }
    public async Task<bool> Delete(Guid id, string area)
    {
        var imagePath = _hostEnviroment + $"/AppData/{area}/" + id.ToString() + ".jpg";
        try
        {
            if (File.Exists(imagePath))
            {
                await Task.Run(() => File.Delete(imagePath));
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine(ex.Message);
            return  false;
        }
    }
    private static bool IsImage(IFormFile file)
    {
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        string fileExtension = Path.GetExtension(file.FileName).ToLower();
        return allowedExtensions.Contains(fileExtension);
    }

    private static async Task<bool> SaveAsJpg(IFormFile file, string targetImagePath)
    {
        try
        {
            using var image = Image.Load(file.OpenReadStream());
            // Сохраняем изображение в формате JPEG с максимальным качеством (100)
            var jpegEncoder = new JpegEncoder { Quality = 100 };
            await image.SaveAsync(targetImagePath, jpegEncoder);
            return true;
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
}