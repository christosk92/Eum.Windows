﻿using System.Text;
using Eum.Logging;
using Newtonsoft.Json;
using Nito.AsyncEx;

namespace Eum.UI.Helpers;

public class RuntimeParams
{
	private static RuntimeParams InternalInstance = null;
	private static string FileDir;

	private RuntimeParams()
	{
	}

	#region Business logic

	public static RuntimeParams Instance
	{
		get
		{
			if (InternalInstance is null)
			{
				throw new InvalidOperationException($"Not loaded! Use {nameof(LoadAsync)}() first!");
			}

			if (string.IsNullOrEmpty(FileDir))
			{
				throw new InvalidOperationException("Directory not set!");
			}

			return InternalInstance;
		}
	}

	private AsyncLock AsyncLock { get; } = new AsyncLock();
	private static string FilePath => Path.Combine(FileDir, "RuntimeParams.json");

	public static void SetDataDir(string dataDir)
	{
		FileDir = Path.Combine(dataDir);
	}

	public async Task SaveAsync()
	{
		try
		{
			using (await AsyncLock.LockAsync())
			{
				if (!Directory.Exists(FileDir))
				{
					Directory.CreateDirectory(FileDir);
				}

				string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
				await File.WriteAllTextAsync(
					FilePath,
					jsonString,
					Encoding.UTF8).ConfigureAwait(false);
			}
		}
		catch (Exception ex)
		{
			S_Log.Instance.LogInfo($"Could not save {nameof(RuntimeParams)}: {ex}.");
		}
	}

	public static async Task LoadAsync()
	{
		try
		{
			if (!File.Exists(FilePath))
			{
				var file = new RuntimeParams();
				await file.SaveAsync().ConfigureAwait(false);
			}

			string jsonString = await File.ReadAllTextAsync(FilePath, Encoding.UTF8).ConfigureAwait(false);
			InternalInstance = JsonConvert.DeserializeObject<RuntimeParams>(jsonString)
							?? throw new InvalidOperationException($"Couldn't deserialize {typeof(RuntimeParams)} from {FilePath}.");
			return;
		}
		catch (Exception ex)
		{
			S_Log.Instance.LogInfo($"Could not load {nameof(RuntimeParams)}: {ex}.");
		}
		InternalInstance = new RuntimeParams();
	}

	#endregion Business logic
}
