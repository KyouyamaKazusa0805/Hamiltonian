namespace System;

/// <summary>
/// Provides with extension methods on concrete <see cref="Array"/> types.
/// </summary>
public static class ArrayExtensions
{
	/// <summary>
	/// Flats the specified 2D array into an 1D array.
	/// </summary>
	/// <typeparam name="T">The type of each element.</typeparam>
	/// <param name="this">An array of elements of type <typeparamref name="T"/>.</param>
	/// <returns>An 1D array.</returns>
	public static T[] Flat<T>(this T[,] @this)
	{
		var result = new T[@this.Length];
		for (var i = 0; i < @this.GetLength(0); i++)
		{
			for (var (j, l2) = (0, @this.GetLength(1)); j < l2; j++)
			{
				result[i * l2 + j] = @this[i, j];
			}
		}
		return result;
	}
}
