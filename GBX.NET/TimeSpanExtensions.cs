using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
	public static class TimeSpanExtensions
	{
		public static int ToMilliseconds(this TimeSpan timeSpan)
		{
			return Convert.ToInt32(timeSpan.TotalMilliseconds);
		}
	}
}
