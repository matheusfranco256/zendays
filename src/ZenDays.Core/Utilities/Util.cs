using System.Globalization;

namespace ZenDays.Core.Utilities
{
	public static class Util
	{
		public static DateTime ConvertToDateTime(string value, string separador)
		{
			return DateTime.ParseExact(value, $"dd{separador}MM{separador}yyyy", CultureInfo.InvariantCulture);
		}
	}
}
