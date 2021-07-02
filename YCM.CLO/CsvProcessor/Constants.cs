using System.Configuration;

namespace CsvProcessor
{
	public static class ConfigurationConstants
	{
		static ConfigurationConstants()
		{
			ArchiveFiles = bool.Parse(ConfigurationManager.AppSettings[nameof(ArchiveFiles)]);
			AgeOfFilesToArchive = int.Parse(ConfigurationManager.AppSettings[nameof(AgeOfFilesToArchive)]);
		}

		public static bool ArchiveFiles { get; }
		public static int AgeOfFilesToArchive { get; }
	}
	public enum OperationType
	{
		MetadataLoad = 0, DataLoad = 1
	}
	public enum ExitCode
	{
		Success = 0, MissingParameter = 1
	}
}
