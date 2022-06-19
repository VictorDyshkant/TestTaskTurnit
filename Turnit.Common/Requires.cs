using System.Diagnostics;

namespace Turnit.Common;

[DebuggerStepThrough]
public static class Requires
{
    /// <summary>Throws an exception if the specified parameter's value is <see langword="null" />.</summary>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    /// <param name="value">The value of the argument.</param>
    /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public static void NotNull<T>(T value, string parameterName)
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName);
        }
    }
}