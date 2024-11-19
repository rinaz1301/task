using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.Connector.Helper
{
	internal class InvalidProviderException : ArgumentException
	{
		public string Name { get => "InvalidProviderException"; }
		public InvalidProviderException(string message) : base(message) {  }
	}
}
