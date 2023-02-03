using BenchmarkDotNet.Attributes;
using CommunityToolkit.HighPerformance.Buffers;

namespace StringPoolExample;

[MemoryDiagnoser(false)]
public class Benchmarks
{
    [Params("https://blog.dotnetydd.com", "https://blog.dotnetydd.com/posts/1231123",
        "https://blog.dotnetydd.com/cates/23")]
    public string Url { get; set; }


    [Benchmark]
    public string GetHost_WithStringPool()
    {
        var prefixOffset = Url.AsSpan().IndexOf(stackalloc char[] {':', '/', '/'});
        var startIndex = prefixOffset == -1 ? 0 : prefixOffset + 3;
        var endIndex = Url.AsSpan(startIndex).IndexOf('/');

        var span = endIndex == -1 ? Url.AsSpan(startIndex) : Url.AsSpan(startIndex, endIndex);

        return StringPool.Shared.GetOrAdd(span);
    }
    
    
    [Benchmark]
    public string GetHost_WithSpan()
    {
        var prefixOffset = Url.AsSpan().IndexOf(stackalloc char[] {':', '/', '/'});
        var startIndex = prefixOffset == -1 ? 0 : prefixOffset + 3;
        var endIndex = Url.AsSpan(startIndex).IndexOf('/');

        var span = endIndex == -1 ? Url.AsSpan(startIndex) : Url.AsSpan(startIndex, endIndex);
        return span.ToString();
    }
    
    
    [Benchmark]
    public string GetHost()
    {
        var uri = new Uri(Url);
        return uri.Host;
    }
}