using Microsoft.AspNetCore.Components.Forms;
using TangyWeb_Server.Service.IService;

namespace TangyWeb_Server.Service
{
	public class FileUpload : IFileUpload
	{
		private readonly IWebHostEnvironment _webHostEnvironment;

		public FileUpload(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}

		public bool DeleteFile(string filePath)
		{
			if(File.Exists(_webHostEnvironment.WebRootPath+filePath))
			{
				File.Delete(_webHostEnvironment.WebRootPath + filePath);
				return true;
			}
			return false;
		}

		public async Task<string> UploadFile(IBrowserFile file)
		{
			FileInfo fileInfo = new FileInfo(file.Name);
			var fileName = Guid.NewGuid().ToString() + fileInfo.Extension; // to create new file name
			var folderDirectory = $"{_webHostEnvironment.WebRootPath}\\images\\product";   //nex loacte that to where we want
			if(!Directory.Exists(folderDirectory))   //if we have not that directory, but we created it manually
			{
				Directory.CreateDirectory(folderDirectory);
			}
			var filePath = Path.Combine(folderDirectory, fileName);

			await using FileStream fs = new FileStream(filePath, FileMode.Create);
			await file.OpenReadStream().CopyToAsync(fs);

			var fullPath = $"/images/product/{fileName}";
			return fullPath;
		}
	}
}
