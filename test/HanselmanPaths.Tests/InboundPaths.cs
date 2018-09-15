using Microsoft.AspNetCore.Builder;
using System;
using Xunit;


public class InboundPaths
{
    [Theory]
    [InlineData(
        "/blog/a-complete-containerized-net-core-application-microservice-that-is-as-small-as-possible.aspx", 
        true, 
        "/blog/ACompleteContainerizedNETCoreApplicationMicroserviceThatIsAsSmallAsPossible.aspx")]
    public void KebabCase(string path, bool changed, string expectedPath)
    {
        Assert.Equal(changed, HanselmanPaths.TryKebabToPascalCase(path, out var newpath));
        Assert.Equal(expectedPath, newpath, ignoreCase: true);
    }
}