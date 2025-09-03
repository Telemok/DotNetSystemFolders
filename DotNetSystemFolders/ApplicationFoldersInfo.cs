/* Version 2025.01.27

This class about folder to store user settings.

Где хранить настройки:

app.config или user.config - это настройки приложения от программиста, а не сохраняемые самим приложением.

Microsoft.Win32.Registry - не позволяет сохранять гигантские настройки, загаживает систему и не всегда даёт доступ.

Environment - Данные сохраняются на уровне ОС и не получится сохранять много данных.
 
Класс IsolatedStorageFile предоставляет безопасное хранилище, привязанное к пользователю.
    Скрытое от пользователя расположение.
    Хорошо подходит для небольших данных.

AppData/Local: Для данных, зависящих от устройства или не нуждающихся в синхронизации.
Пример: кэш, временные файлы.
string localPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
В Windows это C:\Users\<user>\AppData\Local or %localappdata%
В Linux это ~/.local/share.
В macOS это ~/Library/Application Support

AppData/LocalLow: Для приложений с ограниченными правами.
Пример: веб-приложения, плагины.
%userprofile%\AppData\LocalLow
string localLowPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData)+"Low"; // Нет прямого свойства

AppData/Roaming: Для синхронизируемых пользовательских настроек.
string roamingPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
Linux: ~/.config
macOS: ~/Library/Application Support
Windows: C:\Users\<user>\AppData\Roaming or %appdata%

string path = System.IO.Path.GetTempPath();
%USERPROFILE%\AppData\Local\Temp
Используется для временных файлов, которые не должны сохраняться между запусками приложения.
Операционная система не отслеживает, какие файлы принадлежат конкретному приложению, а какие просто находятся в папке Temp. Единственное, что делает Windows — это периодически запускает автоматическую очистку диска (Disk Cleanup или Storage Sense), которая может удалять файлы, не использовавшиеся в течение определенного времени (обычно 7-30 дней). 🗑️

Вывод:
лучше всего хранить настройки в roamingPath.

System.Windows.Forms.Application // плохо использовать потому, что тогда эта баблиотека перестанет работать в не windows.forms программах.
{
	//UserAppDataPath создаётся при запуске WinForms программы автоматически и это не отменить.
  "UserAppDataPath": "C:\\Users\\user\\AppData\\Roaming\\MyCompanyName\\MyProductName\\0.0.0.0",
  "ProductName": "MyProductName",
  "CompanyName": "MyCompanyName",
  "StartupPath": "C:\\KMotion5.3.3\\KMotion\\Debug\\",
  "CommonAppDataPath": "C:\\ProgramData\\MyCompanyName\\MyProductName\\0.0.0.0",
  "LocalUserAppDataPath": "C:\\Users\\user\\AppData\\Local\\MyCompanyName\\MyProductName\\0.0.0.0",
  "ExecutablePath": "C:\\KMotion5.3.3\\KMotion\\Debug\\MyProductName.exe",
  "ProductVersion": "0.0.0.0"
}
*/

namespace Telemok.SystemFolders
{
	public class ApplicationFoldersInfo
	{
		/** For example: C:/Users/user/AppData/Roaming/ or %appdata%/ */
		public static readonly System.IO.DirectoryInfo PathRoaming0ForAnyApplications;

		/** For example: C:/Users/user/AppData/Roaming/MyCompany/ or %appdata%/MyCompany/ */
		public static readonly System.IO.DirectoryInfo PathRoaming1Company;

		/** For example: C:/Users/user/AppData/Roaming/MyCompany/MyApp/ or %appdata%/MyCompany/MyApp/ */
		public static readonly System.IO.DirectoryInfo PathRoaming2Product;

		/** For example: C:/Users/user/AppData/Roaming/MyCompany/MyApp/0.0.0.0/ or %appdata%/MyCompany/MyApp/0.0.0.0/ */
		public static readonly System.IO.DirectoryInfo PathRoaming3Version;

		/** This folder used for store data with synchronization between devices.
		 * Usable for configurations, settings.
		 * On compile time selected between PathRoaming2Product and PathRoaming3Version. */
		public static readonly System.IO.DirectoryInfo PathRoamingMy;

		/** For example: C:/Users/user/AppData/Local/ or %localappdata%/ */
		public static readonly System.IO.DirectoryInfo PathLocal0ForAnyApplications;

		/** For example: C:/Users/user/AppData/Local/MyCompany/ or %localappdata%/MyCompany/ */
		public static readonly System.IO.DirectoryInfo PathLocal1Company;

		/** For example: C:/Users/user/AppData/Local/MyCompany/MyApp/ or %localappdata%/MyCompany/MyApp/ */
		public static readonly System.IO.DirectoryInfo PathLocal2Product;

		/** For example: C:/Users/user/AppData/Local/MyCompany/MyApp/0.0.0.0/ or %localappdata%/MyCompany/MyApp/0.0.0.0/ */
		public static readonly System.IO.DirectoryInfo PathLocal3Version;

		/** This folder used for store data without synchronization between devices.
		 * Usable for cache, temporary files.
		 * On compile time selected between PathLocal2Product and PathLocal3Version. */
		public static readonly System.IO.DirectoryInfo PathLocalMy;

		/** For example:
		 * Windows: /Users/user/AppData/Local/Temp/ or  %localappdata%/Temp
		 * macOs: /var/folders/.../T/
		 * Linux: /tmp/ or /var/tmp/ (depends on distribution or TMPDIR, TEMP или TMP)
		 * Android: /data/local/tmp/ or /data/data/<package_name>/cache/ or /data/user/0/<PackageName>/cache
		 *  */
		public static readonly System.IO.DirectoryInfo PathTemp0ForAnyApplications;

		/** For example: C:/Users/user/AppData/Local/Temp/MyCompany/ or %localappdata%/Temp/MyCompany/ */
		public static readonly System.IO.DirectoryInfo PathTemp1Company;

		/** For example: C:/Users/user/AppData/Local/Temp/MyCompany/MyApp/ or %localappdata%/Temp/MyCompany/MyApp/ */
		public static readonly System.IO.DirectoryInfo PathTemp2Product;

		/** For example: C:/Users/user/AppData/Local/Temp/MyCompany/MyApp/0.0.0.0/ or %localappdata%/Temp/MyCompany/MyApp/0.0.0.0/ */
		public static readonly System.IO.DirectoryInfo PathTemp3Version;

		/** This folder used for store temporary data without synchronization between devices and which must be deleted before application close.
		 * Usable for short time cache, temporary files.
		 * On compile time selected between PathTemp2Product and PathTemp3Version. */
		public static readonly System.IO.DirectoryInfo PathTempMy;


		/** Company name from assembly attribute [assembly: AssemblyCompany("MyCompany")] */
		public readonly static string MyCompanyName;
		/** Product name from assembly attribute [assembly: AssemblyProduct("MyProd")] */
		public readonly static string MyProductName;
		/** Product version from assembly attribute [assembly: AssemblyVersion("0.0.0.0")] */
		public readonly static string MyProductVersion;


		static ApplicationFoldersInfo()
		{
			/* Get System.IO.DirectoryInfo same %appdata% */
		PathRoaming0ForAnyApplications = new System.IO.DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));
			/* Get System.IO.DirectoryInfo same %localappdata% */
			PathLocal0ForAnyApplications = new System.IO.DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData));
			/* Get System.IO.DirectoryInfo same System.IO.Path.GetTempPath() */
			PathTemp0ForAnyApplications = new System.IO.DirectoryInfo(System.IO.Path.GetTempPath());

			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			/* Get companyName, productName, productVersion from assembly attributes.
			 * Better then only windows compatible System.Windows.Forms.Application.CompanyName;
			 * https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.fileversioninfo?view=net-8.0
			 * https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.fileversioninfo.getversioninfo?view=net-8.0#system-diagnostics-fileversioninfo-getversioninfo(system-string) */
			System.Diagnostics.FileVersionInfo thisAppVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
			string? companyName = thisAppVersionInfo.CompanyName;
			string? productName = thisAppVersionInfo.ProductName;
			string? productVersion = thisAppVersionInfo.ProductVersion;

			if( string.IsNullOrEmpty(companyName) )
			{
				throw new System.Exception($"static ApplicationFoldersInfo() error: Developer error: create file in app source /Properties/AssemblyInfo.cs" +
					$"\n[assembly: AssemblyCompany(\"MyCompany\")]"
				+ "Check if you have correct AssemblyCompany, AssemblyProduct and AssemblyVersion in /Properties/AssemblyInfo.cs");
				
				//companyName = "Error, can not initialize companyName";
			}
			if( string.IsNullOrEmpty(productName) )
			{
				throw new System.Exception($"static ApplicationFoldersInfo() error: Developer error: create file in app source /Properties/AssemblyInfo.cs" +
					$"\n[assembly: AssemblyProduct(\"MyProd\")]"
				+ "Check if you have correct AssemblyCompany, AssemblyProduct and AssemblyVersion in /Properties/AssemblyInfo.cs");
				//Application.Exit();
				//productName = "Error, can not initialize productName";
			}
			if( string.IsNullOrEmpty(productVersion) )
			{
				throw new System.Exception($"static ApplicationFoldersInfo() error: Developer error: create file in app source /Properties/AssemblyInfo.cs" +
					$"\n[assembly: AssemblyVersion(\"0.1.2.3\")]"
				+ "Check if you have correct AssemblyCompany, AssemblyProduct and AssemblyVersion in /Properties/AssemblyInfo.cs");
				//Application.Exit();
				//productVersion = "Error, can not initialize productVersion";
			}

		MyCompanyName = companyName;
		MyProductName = productName;
		MyProductVersion = productVersion;



		PathRoaming1Company = new System.IO.DirectoryInfo(System.IO.Path.Combine(PathRoaming0ForAnyApplications.FullName, companyName));
			PathRoaming2Product = new System.IO.DirectoryInfo(System.IO.Path.Combine(PathRoaming1Company.FullName, productName));
			PathRoaming3Version = new System.IO.DirectoryInfo(System.IO.Path.Combine(PathRoaming2Product.FullName, productVersion));
#if ApplicationFoldersInfo_SEPARATE_ROAMING_PATH_FOR_EACH_VERSION
			PathRoaming = PathRoaming3Version;
#else
			PathRoamingMy = PathRoaming2Product;
#endif

			PathLocal1Company = new System.IO.DirectoryInfo(System.IO.Path.Combine(PathLocal0ForAnyApplications.FullName, companyName));
			PathLocal2Product = new System.IO.DirectoryInfo(System.IO.Path.Combine(PathLocal1Company.FullName, productName));
			PathLocal3Version = new System.IO.DirectoryInfo(System.IO.Path.Combine(PathLocal2Product.FullName, productVersion));
			PathLocalMy = PathLocal3Version;

			PathTemp1Company = new System.IO.DirectoryInfo(System.IO.Path.Combine(PathTemp0ForAnyApplications.FullName, companyName));
			PathTemp2Product = new System.IO.DirectoryInfo(System.IO.Path.Combine(PathTemp1Company.FullName, productName));
			PathTemp3Version = new System.IO.DirectoryInfo(System.IO.Path.Combine(PathTemp2Product.FullName, productVersion));
			PathTempMy = PathTemp3Version;
		}
		/** Удаляет папку с настройками и другими файлами пользователя версии приложения.
		 * Потом удаляет папку приложения если она пустая.
		 * Потом удаляет папку продукта если она пустая.
		 * Потом удаляет папку фирмы если она пустая.
		 * Возвращает папку с доказательствами докуда получилось удалить.*/
		public static System.IO.DirectoryInfo RemovePathRoamingMyAndEmptyParents(bool openInExplorerIfHaveProblems = false)
		{
			try
			{
				PathRoamingMy.Delete(true);
			}
			catch
			{
				if( openInExplorerIfHaveProblems )
					OpenInExplorerDirectoryRecur(PathRoamingMy);
				return PathRoamingMy;
			}
#if ApplicationFoldersInfo_SEPARATE_ROAMING_PATH_FOR_EACH_VERSION
			try
			{
				PathRoaming2Product.Delete(false);
			}
			catch
			{
				if( openInExplorerIfHaveProblems )
					OpenInExplorerDirectoryRecur(PathRoaming2Product);
				return PathRoaming2Product;
			}
#endif

			try
			{
				PathRoaming1Company.Delete(false);
			}
			catch
			{
				if( openInExplorerIfHaveProblems )
					OpenInExplorerDirectoryRecur(PathRoaming1Company);
				return PathRoaming1Company;
			}
			return PathRoaming0ForAnyApplications;
		}
	


		/* Open in system file explorer folder or if file not exists, open closest parent folder. */
		public static void OpenInExplorerDirectoryRecur(System.IO.DirectoryInfo? di, int deep = 10)
		{
			for (int i = 0; i < deep ; i++)
			{
				if (di is null)
					return;
				if (di.Exists == true)
				{
					if( System.OperatingSystem.IsWindows() )
					{
						System.Diagnostics.Process.Start("explorer.exe", di.FullName);
					}
					else if(System.OperatingSystem.IsMacOS() )
					{
						System.Diagnostics.Process.Start("open", di.FullName ?? throw new System.Exception("fi?.DirectoryName"));
					}
					else if(System.OperatingSystem.IsAndroid() )
					{
						/* On Android, try to use the default file manager*/
						System.Diagnostics.Process.Start("am", $"start -a android.intent.action.VIEW -d file://{(di.FullName ?? throw new System.Exception("fi?.DirectoryName"))}");
					}
					else// if( OperatingSystem.IsLinux() )
					{
						/* Fallback: try xdg-open*/
						System.Diagnostics.Process.Start("xdg-open", di.FullName ?? throw new System.Exception("fi?.DirectoryName"));
					}
					return;
				}
				di = di.Parent;
			}
		}
		/* Open in system file explorer file and select it, or if file not exists, open closest parent folder. */
		public static void OpenInExplorerSelectedFileOrItDirectory(System.IO.FileInfo? fi)
		{
			if (fi?.Exists == true)
			{
				if(System.OperatingSystem.IsWindows() )
				{
					System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + fi.FullName + "\"");
				}
				else if(System.OperatingSystem.IsMacOS() )
				{
					System.Diagnostics.Process.Start("open", fi?.DirectoryName ?? throw new System.Exception("fi?.DirectoryName"));
				}
				else if(System.OperatingSystem.IsAndroid() )
				{
					/* On Android, try to use the default file manager*/
					System.Diagnostics.Process.Start("am", $"start -a android.intent.action.VIEW -d file://{(fi?.DirectoryName ?? throw new System.Exception("fi?.DirectoryName"))}");
				}
				else// if( OperatingSystem.IsLinux() )
				{
					/* Fallback: try xdg-open*/
					System.Diagnostics.Process.Start("xdg-open", fi?.DirectoryName ?? throw new System.Exception("fi?.DirectoryName"));
				}
				return;
			}
			OpenInExplorerDirectoryRecur(fi?.Directory);
		}
	}
}