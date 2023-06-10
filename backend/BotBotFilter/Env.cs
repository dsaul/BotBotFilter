using Serilog;

namespace TwilioCallScreening
{
	public static class Env
	{
		public static string CALDAV_URI
		{
			get
			{
				string? str = Environment.GetEnvironmentVariable("CALDAV_URI");
				if (string.IsNullOrWhiteSpace(str))
					throw new InvalidOperationException("CALDAV_URI is not set");
				return str;
			}
		}

		public static string CALDAV_AUTH_TYPE
		{
			get
			{
				string? str = Environment.GetEnvironmentVariable("CALDAV_AUTH_TYPE");
				if (string.IsNullOrWhiteSpace(str))
					throw new InvalidOperationException("CALDAV_AUTH_TYPE is not set");
				return str;
			}
		}

		public static string CALDAV_USER_NAME
		{
			get
			{
				string? str = Environment.GetEnvironmentVariable("CALDAV_USER_NAME");
				if (string.IsNullOrWhiteSpace(str))
					throw new InvalidOperationException("CALDAV_USER_NAME is not set");
				return str;
			}
		}

		public static string CALDAV_PASSWORD
		{
			get
			{
				string? str = Environment.GetEnvironmentVariable("CALDAV_PASSWORD");
				if (string.IsNullOrWhiteSpace(str))
					throw new InvalidOperationException("CALDAV_PASSWORD is not set");
				return str;
			}
		}

		public static string FORWARD_E164
		{
			get
			{
				string? str = Environment.GetEnvironmentVariable("FORWARD_E164");
				if (string.IsNullOrWhiteSpace(str))
					throw new InvalidOperationException("FORWARD_E164 is not set");
				return str;
			}
		}

		public static string SND_URI_INTRO
		{
			get
			{
				string? str = Environment.GetEnvironmentVariable("SND_URI_INTRO");
				if (string.IsNullOrWhiteSpace(str))
					throw new InvalidOperationException("SND_URI_INTRO is not set");
				return str;
			}
		}

		public static string SND_URI_NUMBER_PATTERN
		{
			get
			{
				string? str = Environment.GetEnvironmentVariable("SND_URI_NUMBER_PATTERN");
				if (string.IsNullOrWhiteSpace(str))
					throw new InvalidOperationException("SND_URI_NUMBER_PATTERN is not set");
				return str;
			}
		}


		public static string SND_URI_NO_RESPONSE_GOOD_BYE
		{
			get
			{
				string? str = Environment.GetEnvironmentVariable("SND_URI_NO_RESPONSE_GOOD_BYE");
				if (string.IsNullOrWhiteSpace(str))
					throw new InvalidOperationException("SND_URI_NO_RESPONSE_GOOD_BYE is not set");
				return str;
			}
		}


		public static string SND_URI_NOT_CORRECT_GOOD_BYE
		{
			get
			{
				string? str = Environment.GetEnvironmentVariable("SND_URI_NOT_CORRECT_GOOD_BYE");
				if (string.IsNullOrWhiteSpace(str))
					throw new InvalidOperationException("SND_URI_NOT_CORRECT_GOOD_BYE is not set");
				return str;
			}
		}
	}
}
