﻿namespace Post.IRepository
{
	public interface IFileService
	{
		Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions);
		void DeleteFile(string fileNameWithExtension);
	}
}
