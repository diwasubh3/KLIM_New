using System;

namespace YCM.CLO.DataAccess.Models
{
	public class DatabaseEntityOperationResult
	{
		public DatabaseEntityOperationResult(int id, bool success, Exception exception)
		{
			Id = id;
			Success = success;
			OperationException = exception;
		}
		public int Id { get; }
		public bool Success { get; }
		public Exception OperationException { get; }
	}
}
