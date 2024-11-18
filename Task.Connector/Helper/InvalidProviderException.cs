using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.Connector.Helper
{
	internal class InvalidProviderException : Exception
	{
		public InvalidProviderException(string message) : base(message) { }
	}
}
