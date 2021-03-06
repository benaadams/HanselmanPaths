﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.AspNetCore.Builder;

public class HypenCounts
{
    [Theory]
    [InlineData((string)null, 0)]
    [InlineData("", 0)]
    [InlineData("a", 0)]
    [InlineData("-", 1)]
    [InlineData("-a", 1)]
    [InlineData("a-", 1)]
    [InlineData("a-a", 1)]
    [InlineData("--", 2)]
    [InlineData("-a-", 2)]
    [InlineData("--a", 2)]
    [InlineData("a--", 2)]
    [InlineData("a--a", 2)]
    [InlineData("a-a-a", 2)]
    [InlineData("---", 3)]
    public void Plain(string path, int count)
    {
        Assert.Equal(count, HanselmanPaths.CountHypensToRemove(path));
    }

    [Theory]
    [InlineData("/", 0)]
    [InlineData("/a", 0)]
    [InlineData("/-", 1)]
    [InlineData("/-a", 1)]
    [InlineData("/a-", 1)]
    [InlineData("/a-a", 1)]
    [InlineData("/--", 2)]
    [InlineData("/-a-", 2)]
    [InlineData("/--a", 2)]
    [InlineData("/a--", 2)]
    [InlineData("/a--a", 2)]
    [InlineData("/a-a-a", 2)]
    [InlineData("/---", 3)]
    public void LeadingSlash(string path, int count)
    {
        Assert.Equal(count, HanselmanPaths.CountHypensToRemove(path));
    }

    [Theory]
    [InlineData("a/", 0)]
    [InlineData("a/a", 0)]
    [InlineData("a/-", 1)]
    [InlineData("a/-a", 1)]
    [InlineData("a/a-", 1)]
    [InlineData("a/a-a", 1)]
    [InlineData("a/--", 2)]
    [InlineData("a/-a-", 2)]
    [InlineData("a/--a", 2)]
    [InlineData("a/a--", 2)]
    [InlineData("a/a--a", 2)]
    [InlineData("a/a-a-a", 2)]
    [InlineData("a/---", 3)]
    [InlineData("/a/", 0)]
    [InlineData("/a/a", 0)]
    [InlineData("/a/-", 1)]
    [InlineData("/a/-a", 1)]
    [InlineData("/a/a-", 1)]
    [InlineData("/a/a-a", 1)]
    [InlineData("/a/--", 2)]
    [InlineData("/a/-a-", 2)]
    [InlineData("/a/--a", 2)]
    [InlineData("/a/a--", 2)]
    [InlineData("/a/a--a", 2)]
    [InlineData("/a/a-a-a", 2)]
    [InlineData("/a/---", 3)]
    [InlineData("b/a/", 0)]
    [InlineData("b/a/a", 0)]
    [InlineData("b/a/-", 1)]
    [InlineData("b/a/-a", 1)]
    [InlineData("b/a/a-", 1)]
    [InlineData("b/a/a-a", 1)]
    [InlineData("b/a/--", 2)]
    [InlineData("b/a/-a-", 2)]
    [InlineData("b/a/--a", 2)]
    [InlineData("b/a/a--", 2)]
    [InlineData("b/a/a--a", 2)]
    [InlineData("b/a/a-a-a", 2)]
    [InlineData("b/a/---", 3)]
    [InlineData("/b/a/", 0)]
    [InlineData("/b/a/a", 0)]
    [InlineData("/b/a/-", 1)]
    [InlineData("/b/a/-a", 1)]
    [InlineData("/b/a/a-", 1)]
    [InlineData("/b/a/a-a", 1)]
    [InlineData("/b/a/--", 2)]
    [InlineData("/b/a/-a-", 2)]
    [InlineData("/b/a/--a", 2)]
    [InlineData("/b/a/a--", 2)]
    [InlineData("/b/a/a--a", 2)]
    [InlineData("/b/a/a-a-a", 2)]
    [InlineData("/b/a/---", 3)]
    public void Segments(string path, int count)
    {
        Assert.Equal(count, HanselmanPaths.CountHypensToRemove(path));
    }


    [Theory]
    [InlineData("a-a/", 0)]
    [InlineData("a-a/a", 0)]
    [InlineData("a-a/-", 1)]
    [InlineData("a-a/-a", 1)]
    [InlineData("a-a/a-", 1)]
    [InlineData("a-a/a-a", 1)]
    [InlineData("a-a/--", 2)]
    [InlineData("a-a/-a-", 2)]
    [InlineData("a-a/--a", 2)]
    [InlineData("a-a/a--", 2)]
    [InlineData("a-a/a--a", 2)]
    [InlineData("a-a/a-a-a", 2)]
    [InlineData("a-a/---", 3)]
    [InlineData("/a-/", 0)]
    [InlineData("/a-/a", 0)]
    [InlineData("/a-/-", 1)]
    [InlineData("/a-/-a", 1)]
    [InlineData("/a-/a-", 1)]
    [InlineData("/a-/a-a", 1)]
    [InlineData("/a-/--", 2)]
    [InlineData("/a-/-a-", 2)]
    [InlineData("/a-/--a", 2)]
    [InlineData("/a-/a--", 2)]
    [InlineData("/a-/a--a", 2)]
    [InlineData("/a-/a-a-a", 2)]
    [InlineData("/a-/---", 3)]
    [InlineData("-b/a-a/", 0)]
    [InlineData("-b/a-a/a", 0)]
    [InlineData("-b/a-a/-", 1)]
    [InlineData("-b/a-a/-a", 1)]
    [InlineData("-b/a-a/a-", 1)]
    [InlineData("-b/a-a/a-a", 1)]
    [InlineData("-b/a-a/--", 2)]
    [InlineData("-b/a-a/-a-", 2)]
    [InlineData("-b/a-a/--a", 2)]
    [InlineData("-b/a-a/a--", 2)]
    [InlineData("-b/a-a/a--a", 2)]
    [InlineData("-b/a-a/a-a-a", 2)]
    [InlineData("-b/a-a/---", 3)]
    [InlineData("/b-b/a/", 0)]
    [InlineData("/b-b/a/a", 0)]
    [InlineData("/b-b/a/-", 1)]
    [InlineData("/b-b/a/-a", 1)]
    [InlineData("/b-b/a/a-", 1)]
    [InlineData("/b-b/a/a-a", 1)]
    [InlineData("/b-b/a/--", 2)]
    [InlineData("/b-b/a/-a-", 2)]
    [InlineData("/b-b/a/--a", 2)]
    [InlineData("/b-b/a/a--", 2)]
    [InlineData("/b-b/a/a--a", 2)]
    [InlineData("/b-b/a/a-a-a", 2)]
    [InlineData("/b-b/a/---", 3)]
    public void SegmentsWithHyphens(string path, int count)
    {
        Assert.Equal(count, HanselmanPaths.CountHypensToRemove(path));
    }
}