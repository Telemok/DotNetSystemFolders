

namespace Telemok.SystemFolders
{
	/// <summary>
	/// Class for saving text to a file without keeping a memory copy of this text.
	/// Not suitable for: logging, frequent writes.
	/// Suitable for: storing settings, infrequent writes.
	/// </summary>
	public class FileStorageText
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FileStorageText"/> class with the specified file.
		/// </summary>
		/// <param name="fi">The file to use for storage.</param>
		public FileStorageText(System.IO.FileInfo fi)
		{
			MainFileInfo = fi;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FileStorageText"/> class with the specified directory and file name.
		/// </summary>
		/// <param name="di">The directory where the file is located.</param>
		/// <param name="fileName">The name of the file.</param>
		public FileStorageText(System.IO.DirectoryInfo di, string fileName)
		{
			MainFileInfo = new System.IO.FileInfo(System.IO.Path.Combine(di.FullName, fileName));
		}

		/// <summary>
		/// The System.IO.FileInfo for storing application settings.
		/// By default, this is C:\Users\user\AppData\Roaming\MyCompany\MyApp{\MyVersion}\filename.json
		/// </summary>
		public System.IO.FileInfo MainFileInfo { get; private init; }

		/// <summary>
		/// Event triggered after data has been written.
		/// Has no arguments because this class can be extended to a new type and to run the same function without arguments when initialized.
		/// </summary>
		public event System.Action? EventAfterSave;

		/// <summary>
		/// Event triggered after the file content has changed.
		/// Has no arguments because this class can be extended to a new type and to run the same function without arguments when initialized.
		/// </summary>
		public event System.Action? EventAfterChange;

		/// <summary>
		/// Writes all text to the file. Returns true if the file content was changed.
		/// </summary>
		/// <param name="allFileText">The text to write to the file. If null, the file will be deleted.</param>
		/// <returns>True if the file content was changed, otherwise false.</returns>
		protected bool _writeAllTextToFile(string? allFileText)
		{
			bool contentWasChanged = false;
			string? existingFileData = null;
			this.MainFileInfo.Refresh();
			if (MainFileInfo.Exists)
				existingFileData = System.IO.File.ReadAllText(MainFileInfo.FullName);

			if (!string.Equals(existingFileData, allFileText))
				contentWasChanged = true;

			if (allFileText == null)
			{
				// Delete settings file or throw, for example:
				// "The process cannot access the file 'C:\\Users\\user\\AppData\\Roaming\\MyCompany\\MyApp\\settings.json' because it is being used by another process."
				if (MainFileInfo.Exists)
					MainFileInfo.Delete();

				// .Refresh() can be skipped.
			}
			else
			{
				switch (MainFileInfo.Directory?.Exists)
				{
					case null:
						throw new System.Exception($"Directory {MainFileInfo.DirectoryName} is null.");
					case false:
						MainFileInfo.Directory.Create();
						break;
				}
				System.IO.File.WriteAllText(MainFileInfo.FullName, allFileText);
				MainFileInfo.Refresh(); // TODO: maybe delete and refresh are not needed
			}
			this.EventAfterSave?.Invoke();
			if (contentWasChanged)
				this.EventAfterChange?.Invoke();
			return contentWasChanged;
		}

		/// <summary>
		/// Reads all text from the file.
		/// </summary>
		/// <returns>The file content as a string, or null if the file does not exist.</returns>
		protected string? _readAllTextFromFile()
		{
			this.MainFileInfo.Refresh();

			if (!MainFileInfo.Exists)
				return null;
			string text = System.IO.File.ReadAllText(MainFileInfo.FullName);
			return text;
		}

		/// <summary>
		/// Deletes the storage file.
		/// </summary>
		public void Delete()
		{
			this._writeAllTextToFile(null);
		}

		/// <summary>
		/// Opens the storage file or its directory in Windows Explorer.
		/// </summary>
		public void OpenStorageFileInWindowsExplorer()
		{
			Telemok.SystemFolders.ApplicationFoldersInfo.OpenInExplorerSelectedFileOrItDirectory(MainFileInfo);
		}
	}
}
