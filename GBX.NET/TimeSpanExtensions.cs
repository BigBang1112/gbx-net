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

		/// <summary>
		/// Converts the value of the current <see cref="TimeSpan"/> to a Trackmania familiar time format.
		/// </summary>
		/// <param name="timeSpan">TimeSpan</param>
		/// <returns></returns>
		public static string ToStringTM(this TimeSpan timeSpan)
		{
			char? minus = null;
			if (timeSpan.Ticks < 0)
				minus = '-';

			if (timeSpan.TotalDays >= 1)
				return minus + timeSpan.ToString("d':'hh':'mm':'ss'.'fff");
			if (timeSpan.TotalHours >= 1)
				return minus + timeSpan.ToString("h':'mm':'ss'.'fff");
			return minus + timeSpan.ToString("m':'ss'.'fff");
		}
	}
}
