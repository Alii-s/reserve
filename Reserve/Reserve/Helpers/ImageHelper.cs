namespace Reserve.Helpers;

public static class ImageHelper
{
    public static string SaveImage(IFormFile? image, IWebHostEnvironment webHostEnvironment)
    {
        string webRootPath = webHostEnvironment.WebRootPath;
        string imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
        string imagePath = Path.Combine(webRootPath, @"images\event");
        using (var fileStream = new FileStream(Path.Combine(imagePath, imageName), FileMode.Create))
        {
            image.CopyTo(fileStream);
        }
        return @"\images\event\" + imageName;
    }
}
