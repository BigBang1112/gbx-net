using GbxExplorerOld.Client.Components;
using GBX.NET.Attributes;
using GBX.NET.Crypto;
using GBX.NET.Engines.Game;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.JSInterop;
using System.Reflection;

namespace GbxExplorerOld.Tests;

public class PropertyMethodTests
{
    [Test]
    public async Task Click_ReplacesOnlyItsButtonAndInvokesMethodOnce()
    {
        using var services = CreateServices();
        using var renderer = new TestRenderer(services);
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var owner = new MethodOwner();
            var first = renderer.Add();
            var second = renderer.Add();
            await renderer.Show(first, owner, nameof(MethodOwner.Number));
            await renderer.Show(second, owner, nameof(MethodOwner.Empty));
            await Assert.That(owner.Calls).IsEqualTo(0);
            await Assert.That(renderer.Text(first)).IsEqualTo(nameof(MethodOwner.Number));

            await renderer.Click(first);

            await Assert.That(owner.Calls).IsEqualTo(1);
            await Assert.That(renderer.Element(first)).IsEqualTo("span");
            await Assert.That(renderer.Text(first)).IsEqualTo("42");
            await Assert.That(renderer.Element(second)).IsEqualTo("button");
            await renderer.Show(first, owner, nameof(MethodOwner.Number));
            await Assert.That(owner.Calls).IsEqualTo(1);
            await Assert.That(renderer.Text(first)).IsEqualTo("42");
        });
    }

    [Test]
    [Arguments(nameof(MethodOwner.Empty), "(empty)")]
    [Arguments(nameof(MethodOwner.Null), "null")]
    [Arguments(nameof(MethodOwner.Fails), "Error: Cannot compute result.")]
    [Arguments(nameof(MethodOwner.Custom), "custom result")]
    [Arguments(nameof(MethodOwner.Static), "static result")]
    public async Task Click_DisplaysReturnedTextOrMethodError(string methodName, string expected)
    {
        using var services = CreateServices();
        using var renderer = new TestRenderer(services);
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var id = renderer.Add();
            await renderer.Show(id, new MethodOwner(), methodName);
            await renderer.Click(id);
            await Assert.That(renderer.Element(id)).IsEqualTo("span");
            await Assert.That(renderer.Text(id)).IsEqualTo(expected);
        });
    }

    [Test]
    public async Task ChangedOwnerMethodOrValue_ResetsResultToButton()
    {
        using var services = CreateServices();
        using var renderer = new TestRenderer(services);
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var id = renderer.Add();
            var owner = new MethodOwner();
            await renderer.Show(id, owner, nameof(MethodOwner.Number));
            await renderer.Click(id);
            owner = new MethodOwner();
            await renderer.Show(id, owner, nameof(MethodOwner.Number));
            await Assert.That(renderer.Element(id)).IsEqualTo("button");

            await renderer.Click(id);
            await renderer.Show(id, owner, nameof(MethodOwner.Empty));
            await Assert.That(renderer.Element(id)).IsEqualTo("button");

            await renderer.Click(id);
            await renderer.Show(id, owner, nameof(MethodOwner.Empty), new object());
            await Assert.That(renderer.Element(id)).IsEqualTo("button");
        });
    }

    [Test]
    [Arguments("Missing")]
    [Arguments(nameof(MethodOwner.WithArgument))]
    [Arguments(nameof(MethodOwner.WithoutResult))]
    public async Task Click_InvalidMethod_DisplaysError(string methodName)
    {
        using var services = CreateServices();
        using var renderer = new TestRenderer(services);
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var id = renderer.Add();
            await renderer.Show(id, new MethodOwner(), methodName);
            await renderer.Click(id);
            await Assert.That(renderer.Text(id)).StartsWith("Error:");
        });
    }

    [Test]
    [Arguments(nameof(MethodOwner.Secret), "secret-value")]
    [Arguments(nameof(MethodOwner.Number), "42")]
    [Arguments(nameof(MethodOwner.Empty), "")]
    [Arguments(nameof(MethodOwner.Null), "")]
    public async Task Click_ProtectedMethod_CopiesActualResultWithoutRenderingIt(string methodName, string expected)
    {
        var clipboard = new Clipboard();
        using var services = CreateServices(clipboard);
        using var renderer = new TestRenderer(services);
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var id = renderer.Add();
            var owner = new MethodOwner();
            await renderer.Show(id, owner, methodName, isProtected: true);
            await Assert.That(clipboard.Values).IsEmpty();
            await renderer.Click(id);

            await Assert.That(clipboard.Values).IsEquivalentTo(new[] { expected });
            await Assert.That(renderer.Element(id)).IsEqualTo("button");
            await Assert.That(renderer.Content(id)).Contains("(copied)");
            if (expected.Length > 0) await Assert.That(renderer.Content(id)).DoesNotContain(expected);
        });
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Click_ProtectedMethod_KeepsCopiedStatusWhileCopyingAgain(bool fails)
    {
        var clipboard = new Clipboard();
        using var services = CreateServices(clipboard);
        using var renderer = new TestRenderer(services);
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var id = renderer.Add();
            var owner = new MethodOwner();
            await renderer.Show(id, owner, nameof(MethodOwner.Number), isProtected: true);
            await renderer.Click(id);

            var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            clipboard.PendingWrite = completion.Task;
            clipboard.Fail = fails;
            var click = renderer.Click(id);
            try
            {
                await Assert.That(click.IsCompleted).IsFalse();
                await Assert.That(renderer.Content(id)).Contains("(copied)");
            }
            finally
            {
                completion.SetResult();
                await click;
            }

            await Assert.That(owner.Calls).IsEqualTo(2);
            await Assert.That(clipboard.Values).Count().IsEqualTo(fails ? 1 : 2);
            await Assert.That(renderer.Content(id)).Contains(fails ? "(copy failed)" : "(copied)");
            if (fails) await Assert.That(renderer.Content(id)).DoesNotContain("(copied)");
        });
    }

    [Test]
    public async Task Click_ClipboardFailure_DoesNotRevealResultAndAllowsRetry()
    {
        var clipboard = new Clipboard { Fail = true };
        using var services = CreateServices(clipboard);
        using var renderer = new TestRenderer(services);
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var id = renderer.Add();
            await renderer.Show(id, new MethodOwner(), nameof(MethodOwner.Secret), isProtected: true);
            await renderer.Click(id);
            await Assert.That(renderer.Content(id)).Contains("(copy failed)");
            await Assert.That(renderer.Content(id)).DoesNotContain("secret-value");

            clipboard.Fail = false;
            await renderer.Click(id);
            await Assert.That(renderer.Content(id)).Contains("(copied)");
            await Assert.That(renderer.Content(id)).DoesNotContain("secret-value");
        });
    }

    [Test]
    public async Task Click_ProtectedMethodFailure_DoesNotRevealExceptionMessage()
    {
        var clipboard = new Clipboard();
        using var services = CreateServices(clipboard);
        using var renderer = new TestRenderer(services);
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var id = renderer.Add();
            await renderer.Show(id, new MethodOwner(), nameof(MethodOwner.FailsWithSecret), isProtected: true);
            await renderer.Click(id);
            await Assert.That(renderer.Content(id)).Contains("(copy failed)");
            await Assert.That(renderer.Content(id)).DoesNotContain("secret-value");
            await Assert.That(clipboard.Values).IsEmpty();
        });
    }

    [Test]
    public async Task ChangedProtection_ClearsDisplayedResultAndCopiesOnNextClick()
    {
        var clipboard = new Clipboard();
        using var services = CreateServices(clipboard);
        using var renderer = new TestRenderer(services);
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var id = renderer.Add();
            var owner = new MethodOwner();
            await renderer.Show(id, owner, nameof(MethodOwner.Secret));
            await renderer.Click(id);
            await Assert.That(renderer.Text(id)).IsEqualTo("secret-value");

            await renderer.Show(id, owner, nameof(MethodOwner.Secret), isProtected: true);
            await Assert.That(renderer.Element(id)).IsEqualTo("button");
            await Assert.That(renderer.Content(id)).DoesNotContain("secret-value");
            await renderer.Click(id);
            await Assert.That(clipboard.Values).IsEquivalentTo(new[] { "secret-value" });
        });
    }

    [Test]
    [Arguments(nameof(CGamePlayerProfile.OnlineValidationKeyHexa), nameof(CGamePlayerProfileExtensions.DecryptOnlineValidationKey))]
    [Arguments(nameof(CGamePlayerProfile.OnlineSupportKeyHexa), nameof(CGamePlayerProfileExtensions.DecryptOnlineSupportKey))]
    public async Task Click_ProfileKeyAttribute_InvokesCryptoExtensionAndCopiesWithoutRevealingResult(string propertyName, string methodName)
    {
        var clipboard = new Clipboard();
        using var services = CreateServices(clipboard);
        using var renderer = new TestRenderer(services);
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var profile = new CGamePlayerProfile();
            var property = typeof(CGamePlayerProfile).GetProperty(propertyName)!;
            property.SetValue(profile, "00010203040506071D64EF84BA1E1150F117FE2D82CC0C58");
            var attribute = property.GetCustomAttributes<MethodAttribute>().Single();
            await Assert.That(attribute.MethodName).IsEqualTo(methodName);
            await Assert.That(attribute.Protected).IsTrue();
            await Assert.That(Type.GetType(attribute.DeclaringTypeName!, throwOnError: true)).IsEqualTo(typeof(CGamePlayerProfileExtensions));

            var id = renderer.Add();
            await renderer.Show(id, profile, attribute.MethodName, value: property.GetValue(profile),
                isProtected: attribute.Protected, declaringTypeName: attribute.DeclaringTypeName);
            await Assert.That(clipboard.Values).IsEmpty();
            await renderer.Click(id);

            await Assert.That(clipboard.Values).IsEquivalentTo(new[] { "SupportKey" });
            await Assert.That(renderer.Element(id)).IsEqualTo("button");
            await Assert.That(renderer.Text(id)).IsEqualTo(methodName);
            await Assert.That(renderer.Content(id)).Contains("(copied)");
        });
    }

    [Test]
    [Arguments(nameof(CGamePlayerProfileChunk_AccountSettings.OnlinePassword), "DecryptOnlinePassword",
        "00010203040506079DD2E057932C92C473E804E0C3B7254F", "password-123")]
    [Arguments(nameof(CGamePlayerProfileChunk_AccountSettings.OnlineSupportKey), "DecryptOnlineSupportKey",
        "000102030405060727F9FC270D7E739E", "12345678")]
    [Arguments(nameof(CGamePlayerProfileChunk_AccountSettings.OnlineValidationCode), "DecryptOnlineValidationCode",
        "00010203040506076CBB6D73FA3787C98990CCC79394A3CD", "ABC-DEFGH-IJK")]
    [Arguments(nameof(CGamePlayerProfileChunk_AccountSettings.EncryptedKeyHexa), "DecryptKey",
        "0001020304050607DB004DE4B223D219BDE8983151234019", "account-key-data")]
    public async Task Click_AccountSettingsAttribute_InvokesCryptoExtensionAndCopiesWithoutRevealingResult(
        string propertyName, string methodName, string hex, string expected)
    {
        var clipboard = new Clipboard();
        using var services = CreateServices(clipboard);
        using var renderer = new TestRenderer(services);
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var settings = new CGamePlayerProfileChunk_AccountSettings { OnlineLogin = "test.player" };
            var property = typeof(CGamePlayerProfileChunk_AccountSettings).GetProperty(propertyName)!;
            property.SetValue(settings, hex);
            var attribute = property.GetCustomAttributes<MethodAttribute>().Single();
            await Assert.That(attribute.MethodName).IsEqualTo(methodName);
            await Assert.That(attribute.Protected).IsTrue();
            await Assert.That(Type.GetType(attribute.DeclaringTypeName!, throwOnError: true))
                .IsEqualTo(typeof(CGamePlayerProfileChunk_AccountSettingsExtensions));

            var id = renderer.Add();
            await renderer.Show(id, settings, attribute.MethodName, value: property.GetValue(settings),
                isProtected: attribute.Protected, declaringTypeName: attribute.DeclaringTypeName);
            await renderer.Click(id);

            await Assert.That(clipboard.Values).IsEquivalentTo(new[] { expected });
            await Assert.That(renderer.Element(id)).IsEqualTo("button");
            await Assert.That(renderer.Text(id)).IsEqualTo(methodName);
            await Assert.That(renderer.Content(id)).Contains("(copied)");
            await Assert.That(renderer.Content(id)).DoesNotContain(expected);
        });
    }

    private static ServiceProvider CreateServices(Clipboard? clipboard = null) => new ServiceCollection()
        .AddLogging().AddSingleton<IJSRuntime>(clipboard ?? new Clipboard()).BuildServiceProvider();

    private sealed class Clipboard : IJSRuntime
    {
        public List<string> Values { get; } = [];
        public bool Fail { get; set; }
        public Task? PendingWrite { get; set; }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) =>
            InvokeAsync<TValue>(identifier, CancellationToken.None, args);

        public async ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        {
            if (identifier != "navigator.clipboard.writeText") throw new InvalidOperationException(identifier);
            if (PendingWrite is not null) await PendingWrite;
            if (Fail) throw new JSException("Clipboard failure with secret-value.");
            Values.Add((string)args![0]!);
            return default!;
        }
    }

    public class MethodOwner
    {
        public int Calls { get; private set; }
        public int Number() { Calls++; return 42; }
        public string Empty() => "";
        public string? Null() => null;
        public string Secret() => "secret-value";
        public string FailsWithSecret() => throw new InvalidOperationException("secret-value");
        public string Fails() => throw new InvalidOperationException("Cannot compute result.");
        public object Custom() => new CustomResult();
        public static string Static() => "static result";
        public string WithArgument(int value) => value.ToString();
        public void WithoutResult() { }
    }

    private sealed class CustomResult
    {
        public override string ToString() => "custom result";
    }

    // Dispatch real Blazor click events without requiring a browser or WebAssembly host.
#pragma warning disable BL0006 // Render tree access is intentional in this test renderer.
    private sealed class TestRenderer(IServiceProvider services) : Renderer(services, NullLoggerFactory.Instance)
    {
        public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();
        public int Add() => AssignRootComponentId(InstantiateComponent(typeof(PropertyMethod)));
        public Task Show(int id, object owner, string methodName, object? value = null, bool isProtected = false, string? declaringTypeName = null) =>
            RenderRootComponentAsync(id, ParameterView.FromDictionary(new Dictionary<string, object?>
            {
                [nameof(PropertyMethod.Owner)] = owner,
                [nameof(PropertyMethod.MethodName)] = methodName,
                [nameof(PropertyMethod.DeclaringTypeName)] = declaringTypeName,
                [nameof(PropertyMethod.Protected)] = isProtected,
                [nameof(PropertyMethod.Value)] = value
            }));
        public string Element(int id) => Frames(id).First(x => x.FrameType == RenderTreeFrameType.Element).ElementName;
        public string Text(int id) => string.Concat(Frames(id).Where(x => x.FrameType == RenderTreeFrameType.Text).Select(x => x.TextContent));
        public string Content(int id) => string.Concat(Frames(id).Select(x => x.FrameType switch
        {
            RenderTreeFrameType.Text => x.TextContent,
            RenderTreeFrameType.Markup => x.MarkupContent,
            RenderTreeFrameType.Attribute => x.AttributeValue?.ToString(),
            _ => null
        }));
        public Task Click(int id) => DispatchEventAsync(Frames(id)
            .Single(x => x.FrameType == RenderTreeFrameType.Attribute && x.AttributeName == "onclick").AttributeEventHandlerId,
            fieldInfo: null, new MouseEventArgs());
        private IEnumerable<RenderTreeFrame> Frames(int id)
        {
            var frames = GetCurrentRenderTreeFrames(id);
            return frames.Array.Take(frames.Count);
        }
        protected override void HandleException(Exception exception) => throw exception;
        protected override Task UpdateDisplayAsync(in RenderBatch renderBatch) => Task.CompletedTask;
    }
#pragma warning restore BL0006
}
