namespace GBX.NET.Extensions;

public static class LoggerExtensions
{
    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: "User {User} logged in from {Address}"</param>
    /// <param name="arg0"><see cref="uint"/> to format.</param>
    /// <param name="arg0ToStringFormat">Format to put on <paramref name="arg0"/>.</param>
    public static void LogDebug(this ILogger logger, string? message, uint arg0, string? arg0ToStringFormat = null)
    {
        if (logger.IsEnabled(LogLevel.Debug))
        {
#pragma warning disable CA2254
            if (arg0ToStringFormat is null)
            {
                Microsoft.Extensions.Logging.LoggerExtensions.LogDebug(logger, message, arg0);
                return;
            }
            Microsoft.Extensions.Logging.LoggerExtensions.LogDebug(logger, message, arg0.ToString(arg0ToStringFormat));
#pragma warning restore CA2254
        }
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <typeparam name="T0">Type of the object to format.</typeparam>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: "User {User} logged in from {Address}"</param>
    /// <param name="arg0">Object to format.</param>
    public static void LogDebug<T0>(this ILogger logger, string? message, T0 arg0)
    {
        if (logger.IsEnabled(LogLevel.Debug))
        {
#pragma warning disable CA2254
            Microsoft.Extensions.Logging.LoggerExtensions.LogDebug(logger, message, arg0);
#pragma warning restore CA2254
        }
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <typeparam name="T0">Type of the object to format.</typeparam>
    /// <typeparam name="T1">Type of the object to format.</typeparam>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: "User {User} logged in from {Address}"</param>
    /// <param name="arg0">Object to format.</param>
    /// <param name="arg1">Object to format.</param>
    public static void LogDebug<T0, T1>(this ILogger logger, string? message, T0 arg0, T1 arg1)
    {
        if (logger.IsEnabled(LogLevel.Debug))
        {
#pragma warning disable CA2254
            Microsoft.Extensions.Logging.LoggerExtensions.LogDebug(logger, message, arg0, arg1);
#pragma warning restore CA2254
        }
    }
}
