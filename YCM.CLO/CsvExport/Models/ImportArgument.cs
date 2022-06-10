using System;
using static ExportCsv.Utilities;

namespace ExportCsv.Models
{
	public class ImportArgument
	{
		public ImportArgument(string[] args)
		{
			if (args.Length <= 0)
				throw new ArgumentException("At least one parameter must be passed to this application!");
			ImportDate = DateTime.Today;
			DateId = GetDateId(ImportDate);
			int id;
			DateTime test;
			if (int.TryParse(args[0], out id))
				ImportId = id;
			if (args.Length > 1)
				if (int.TryParse(args[1], out id))
				{
					DateId = id;
					ImportDate = GetDateFromDateId(DateId);
				}
				else if (DateTime.TryParse(args[1], out test))
				{
					ImportDate = test;
					DateId = GetDateId(ImportDate);
				}

			OpType = OperationType.DataLoad;
			OperationType opType;
			if (args.Length > 2 && Enum.TryParse(args[2], out opType)
			                    && (int)opType <= (int)OperationType.DataLoad)
				OpType = opType;
		}

		public int ImportId { get; }
		public int DateId { get; }
		public DateTime ImportDate { get; }
		public OperationType OpType { get; }

		public override string ToString()
			=> $"Import Id: {ImportId} Date Id: {DateId} Date Id Date: {ImportDate} Op Type: {OpType}";
	}
}
