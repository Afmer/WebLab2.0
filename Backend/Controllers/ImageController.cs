using Microsoft.AspNetCore.Mvc;
using Weblab.Architecture.Interfaces;

namespace Weblab.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;
    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }
    [HttpGet]
    public IActionResult Show(Guid id, string imageArea)
    {
        try
        {
            var path = _imageService.GetImagePath(imageArea, id);
            FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
            return File(fileStream, "image/jpeg");
        }
        catch
        {
            return NotFound();
        }
    }
}