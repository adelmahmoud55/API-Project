namespace Talabat.DashBoard.Helpers
{
	public class PictureSettings
	{
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Folder Path
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName);

            // Ensure folder exists
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // 2. Set Unique File Name
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            // 3. Get File Path
            var filePath = Path.Combine(folderPath, fileName);

            // 4. Save File as Stream
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fs);
            }

            // 5. Return relative path
            return Path.Combine("images", folderName, fileName).Replace("\\", "/");
        }


        public static void DeleteFile(string folderName, string fileName)
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", folderName, fileName);

			if (File.Exists(filePath))
				File.Delete(filePath);
		}
	}
}
