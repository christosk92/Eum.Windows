using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using Eum.Logging;

namespace Eum.UI.Helpers;

public static class IoHelpers
{
	/// <summary>
	/// Attempts to delete <paramref name="directory"/> with retry feature to allow File Explorer or any other
	/// application that holds the directory handle of the <paramref name="directory"/> to release it.
	/// </summary>
	/// <see href="https://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true/14933880#44324346"/>
	public static async Task<bool> TryDeleteDirectoryAsync(string directory, int maxRetries = 10, int millisecondsDelay = 100)
	{
		Guard.NotNull(nameof(directory), directory);

		if (maxRetries < 1)
		{
			throw new ArgumentOutOfRangeException(nameof(maxRetries));
		}

		if (millisecondsDelay < 1)
		{
			throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));
		}

		for (int i = 0; i < maxRetries; ++i)
		{
			try
			{
				if (Directory.Exists(directory))
				{
					Directory.Delete(directory, recursive: true);
				}

				return true;
			}
			catch (DirectoryNotFoundException e)
			{
				S_Log.Instance.LogDebug($"Directory was not found: {e}");

				// Directory does not exist. So the operation is trivially done.
				return true;
			}
			catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
			{
				await Task.Delay(millisecondsDelay).ConfigureAwait(false);
			}
		}

		return false;
	}

	public static void EnsureContainingDirectoryExists(string fileNameOrPath)
	{
		string fullPath = Path.GetFullPath(fileNameOrPath); // No matter if relative or absolute path is given to this.
		string? dir = Path.GetDirectoryName(fullPath);
		EnsureDirectoryExists(dir);
	}

	/// <summary>
	/// It's like Directory.CreateDirectory, but does not fail when root is given.
	/// </summary>
	public static void EnsureDirectoryExists(string? dir)
	{
		if (!string.IsNullOrWhiteSpace(dir)) // If root is given, then do not worry.
		{
			Directory.CreateDirectory(dir); // It does not fail if it exists.
		}
	}

	public static void EnsureFileExists(string filePath)
	{
		if (!File.Exists(filePath))
		{
			EnsureContainingDirectoryExists(filePath);

			File.Create(filePath)?.Dispose();
		}
	}

	public static void OpenFolderInFileExplorer(string dirPath)
	{
		if (Directory.Exists(dirPath))
		{
			// RuntimeInformation.OSDescription on WSL2 reports a string like:
			// 'Linux 5.10.102.1-microsoft-standard-WSL2 #1 SMP Wed Mar 2 00:30:59 UTC 2022'
			if (!RuntimeInformation.OSDescription.ToString(CultureInfo.InvariantCulture).Contains("WSL2"))
			{
				using var process = Process.Start(new ProcessStartInfo
				{
					FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
						? "explorer.exe"
						: (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
							? "open"
							: "xdg-open"),
					Arguments = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"\"{dirPath}\"" : dirPath,
					CreateNoWindow = true
				});
			}
		}
	}

	public static async Task OpenBrowserAsync(string url)
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			// If no associated application/json MimeType is found xdg-open opens retrun error
			// but it tries to open it anyway using the console editor (nano, vim, other..)
			await EnvironmentHelpers.ShellExecAsync($"xdg-open {url}", waitForExit: false).ConfigureAwait(false);
		}
		else
		{
			using var process = Process.Start(new ProcessStartInfo
			{
				FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? url : "open",
				Arguments = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? $"-e {url}" : "",
				CreateNoWindow = true,
				UseShellExecute = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
			});
		}
	}

	public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
	{
		foreach (DirectoryInfo dir in source.GetDirectories())
		{
			CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
		}

		foreach (FileInfo file in source.GetFiles())
		{
			file.CopyTo(Path.Combine(target.FullName, file.Name));
		}
	}

	public static void CreateOrOverwriteFile(string path)
	{
		using var _ = File.Create(path);
	}
}
