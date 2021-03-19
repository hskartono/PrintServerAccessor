using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrintServerAccessor
{
	public class printRequest
	{
		public requestContent request { get; set; }

	}

	public class requestContent
	{
		public string Document { get; set; }
		public string FileName { get; set; }
		public string PrinterName { get; set; }
		public string UserName { get; set; }

	}
}
