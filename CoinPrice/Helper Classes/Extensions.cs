using System;

namespace CoinPrice
{
	public static class Extensions
	{
		//========================================================
		//	String
		//========================================================

		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		/// <summary>
		///  Replaces the format item in a specified System.String with the text equivalent
		///  of the value of a specified System.Object instance.
		/// </summary>
		/// <param name="value">A composite format string</param>
		/// <param name="arg0">An System.Object to format</param>
		/// <returns>A copy of format in which the first format item has been replaced by the
		/// System.String equivalent of arg0</returns>
		public static string Format(this string value, object arg0)
		{
			return string.Format(value, arg0);
		}

		/// <summary>
		///  Replaces the format item in a specified System.String with the text equivalent
		///  of the value of a specified System.Object instance.
		/// </summary>
		/// <param name="value">A composite format string</param>
		/// <param name="args">An System.Object array containing zero or more objects to format.</param>
		/// <returns>A copy of format in which the format items have been replaced by the System.String
		/// equivalent of the corresponding instances of System.Object in args.</returns>
		public static string Format(this string value, params object[] args)
		{
			return string.Format(value, args);
		}

		/// <summary>
		/// Parses a string into an Enum
		/// </summary>
		/// <typeparam name="T">The type of the Enum</typeparam>
		/// <param name="value">String value to parse</param>
		/// <returns>The Enum corresponding to the stringExtensions</returns>
		public static T ToEnum<T>(this string value)
		{
			return ToEnum<T>(value, false);
		}

		/// <summary>
		/// Parses a string into an Enum
		/// </summary>
		/// <typeparam name="T">The type of the Enum</typeparam>
		/// <param name="value">String value to parse</param>
		/// <param name="ignorecase">Ignore the case of the string being parsed</param>
		/// <returns>The Enum corresponding to the stringExtensions</returns>
		public static T ToEnum<T>(this string value, bool ignorecase)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			value = value.Trim();

			if (value.Length == 0)
				throw new ArgumentNullException("Must specify valid information for parsing in the string.", "value");

			Type t = typeof(T);
			if (!t.IsEnum)
				throw new ArgumentException("Type provided must be an Enum.", "T");

			return (T)Enum.Parse(t, value, ignorecase);
		}

		//========================================================
		//	Class
		//========================================================

		public static bool IsNull<T>(this T obj) where T : class
		{
			return (obj == null);
		}

		/// <summary>
		/// Throws ArgumentNullException if the object called upon is null.
		/// </summary>
		/// <typeparam name="T">The calling class</typeparam>
		/// <param name="obj">The This object</param>
		/// <param name="paramName">The parameter name to be written on the ArgumentNullException</param>
		public static void ThrowIfArgumentIsNull<T>(this T obj, string paramName) where T : class
		{
			if (obj == null)
				throw new ArgumentNullException(paramName);
		}

		/// <summary>
		/// Throws NullReferenceException if the obejct called upon is null.
		/// </summary>
		/// <typeparam name="T">The calling class</typeparam>
		/// <param name="obj">The This object</param>
		/// <param name="paramName">The parameter name to be written on the NullReferenceException. [paramName +" can not be null."]</param>
		public static void ThrowIfReferenceIsNull<T>(this T obj, string paramName) where T : class
		{
			if (obj == null)
				throw new NullReferenceException(paramName + " can not be null.");
		}
	}
}
