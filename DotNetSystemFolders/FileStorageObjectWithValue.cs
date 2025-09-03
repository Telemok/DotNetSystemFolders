/* Version 2025.01.27

Это класс для сохранения настроек в файле так, чтобы можно было настройки разносить по разным компьютерам этого пользователя.




Как пользоваться классом:

[YouSettings.cs]
public readonly static Telemok.SystemFolders.FileStorageObjectWithValue<AppSettingsType> AppSettings = 
	new (Telemok.SystemFolders.ApplicationFoldersInfo.PathRoamingMy, "settings.json");

[Form1.cs]
AppSettings.EventOnChange += (T? obj)
{
	this.textBox1.Invoke(()=>{this.textBox1.Text = "" + obj.Value1});
}
AppSettings.Load();

[YouClass.cs]
AppSettings.Value.Value1 = 1;
AppSettings.Value.Value2 = 22;
AppSettings.Save();
*/


namespace Telemok.SystemFolders
{
	/**
	 You can define "Telemok_SystemFolders_FileStorageJsonWithValue_JSON_HUMAN_READABLE" to have human-readable JSON (with indents and spaces) in the saved file.
	 */
	public class FileStorageObjectWithValue<T> :FileStorageText where T : class/*for _writeObjectToFile(null)*/, new()
	{
		public FileStorageObjectWithValue(System.IO.DirectoryInfo di, string fileName) : base(di, fileName)
		{
		}
		public FileStorageObjectWithValue(System.IO.FileInfo fi) : base(fi)
		{
		}

		protected void _writeObjectToFile(T? obj)
		{
#pragma warning disable CA1869 // Cache and reuse 'JsonSerializerOptions' instances
/* CA1869: Suppressed intentionally. This is a deliberately rare operation (settings file save), so performance is not critical. */
#if Telemok_SystemFolders_FileStorageJsonWithValue_JSON_HUMAN_READABLE
		System.Text.Json.JsonSerializerOptions options = new()
			{
				WriteIndented = true,
				Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			};
#else
			System.Text.Json.JsonSerializerOptions options = new();
#endif
			string? allFileText = obj is null ? null : System.Text.Json.JsonSerializer.Serialize(obj, options);

			/* Write or delete (if null) file and raise events. */
			this._writeAllTextToFile(allFileText);
		}

		protected T? ReadObjectFromFile()
		{
			if( _readAllTextFromFile() is string allFileText )
			{
				return System.Text.Json.JsonSerializer.Deserialize<T>(allFileText) ?? throw new System.Exception($"FileStorageObjectWithValue from file «{this.MainFileInfo.FullName}» can not decode json «{allFileText}».");
			}
			return null;
		}

		public T Value { get; protected set; } = new T();

		public void Reset()
		{
			this.Value = new T();
			this.Save();
		}
		public void Save()
		{
			this._writeObjectToFile(Value);
		}
		//public enum LoadCallResult
		//{
		//	OK = 0,
		//	FILE_NOT_EXISTS = 1,
		//	FILE_BROKEN = 2,
		//	OTHER_ERROR = 7
		//}
		public class LoadResult
		{
			public bool Success { get; init; } 
			public bool FileNotExist { get; init; } 
			public string? FileText { get; init; }

		}
		public LoadResult Load()
		{
			LoadResult result = new();

			this.MainFileInfo.Refresh();
			if( !this.MainFileInfo.Exists )
			{
				this.Reset();
				return new() { Success = true, FileNotExist = true, FileText = null };
			}

			if( _readAllTextFromFile() is string allFileText )
			{
				T? val = System.Text.Json.JsonSerializer.Deserialize<T>(allFileText);
				if(val is null)
					return new() { Success = false, FileNotExist = false, FileText = allFileText };
				this.Value = val;
				return new() { Success = true, FileNotExist = false, FileText = allFileText };
			}
			return new() { Success = false, FileNotExist = false, FileText = null };
		}
	}
}