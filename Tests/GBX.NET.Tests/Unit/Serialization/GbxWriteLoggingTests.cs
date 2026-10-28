using GBX.NET.Engines.Game;
using GBX.NET.Serialization.Chunking;
using Microsoft.Extensions.Logging;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class GbxWriteLoggingTests
{
    [Test]
    public async Task Save_WithLogger_LogsHeaderChunksAndBody()
    {
        // Arrange
        var node = new CGameCtnMediaBlockEntity { TimeScale = 2 };
        node.Chunks.Add(new CGameCtnMediaBlockEntity.Chunk0329F003());
        node.Chunks.Add(new SkippableChunk(0x0329F0FE) { Data = [0x12, 0x34, 0x56, 0x78] });
        var gbx = new Gbx<CGameCtnMediaBlockEntity>(node) { BodyCompression = GbxCompression.Uncompressed };
        var logger = new RecordingLogger();
        using var output = new MemoryStream();

        // Act
        gbx.Save(output, new GbxWriteSettings { Logger = logger });

        // Assert
        await Assert.That(logger.Messages).Contains("Gbx Save (IMPLICIT)");
        await Assert.That(logger.Messages).Contains("Class ID: 0x0329F000 (CGameCtnMediaBlockEntity)");
        await Assert.That(logger.Messages).Contains("UserData: Empty");
        await Assert.That(logger.Messages).Contains("Writing main node...");
        await Assert.That(logger.Messages).Contains("0x0329F003 (skippable)");
        await Assert.That(logger.Messages.Any(static m => m.StartsWith("0x0329F003 DONE (", StringComparison.Ordinal) && m.EndsWith("ms, size: 4)", StringComparison.Ordinal))).IsTrue();
        await Assert.That(logger.Messages).Contains("0x0329F0FE (unknown skippable, size: 4, stored data)");
        await Assert.That(logger.Messages).Contains("Main node complete.");
        await Assert.That(logger.Messages).Contains("Number of nodes: 1");
        await Assert.That(logger.Messages).Contains("Number of external nodes: 0");
        await Assert.That(logger.Messages).Contains("Uncompressed body.");
        await Assert.That(logger.Messages).Contains("Known Gbx completed.");
        await Assert.That(logger.Scopes).Contains("Header");
        await Assert.That(logger.Scopes).Contains("CGameCtnMediaBlockEntity (main)");
    }

    private sealed class RecordingLogger : ILogger
    {
        public List<string> Messages { get; } = [];
        public List<string> Scopes { get; } = [];

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            Scopes.Add(state.ToString() ?? "");
            return null;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Messages.Add(formatter(state, exception));
        }
    }
}
