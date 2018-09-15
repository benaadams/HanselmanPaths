using System;
using System.Buffers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PathConverter;

namespace Microsoft.AspNetCore.Builder
{
    public class HanselmanPaths
    {
        private readonly RequestDelegate _next;

        public HanselmanPaths(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;

            if (TryKebabToPascalCase(path, out var newPath))
            {
                context.Request.Path = newPath;
            }

            return _next(context);
        }

#if NETCOREAPP2_1
        private static SpanAction<char, (string path, int offset)> s_createPascalCaseString = (span, args) => CreatePascalCaseString(span, args);

        private static void CreatePascalCaseString(Span<char> span, (string path, int offset) args)
        {
            var path = args.path.AsSpan();
            var offset = args.offset;
            if (offset > 0)
            {
                path.Slice(0, offset).CopyTo(span);

                path = path.Slice(offset);
                span = span.Slice(offset);
            }

            offset = path.IndexOf('-');
            while (offset >= 0)
            {
                if (offset > 0)
                {
                    // Capitalize the first letter
                    span[0] = char.ToUpper(path[0]);

                    if (offset > 1)
                    {
                        // Copy the rest
                        path.Slice(1, offset - 1).CopyTo(span.Slice(1));
                    }

                    span = span.Slice(offset);
                    // Skip the hyphen in path
                    path = path.Slice(offset + 1);
                }
                else
                {
                    // Skip the hyphen in path
                    path = path.Slice(1);
                }

                offset = path.IndexOf('-');
            }

            if (path.Length > 0)
            {
                // Capitalize the first letter
                span[0] = char.ToUpper(path[0]);
            }
            if (path.Length > 1)
            {
                // Copy the remaining
                path.Slice(1, path.Length - 1).CopyTo(span.Slice(1));
            }

        }

        public static bool TryKebabToPascalCase(string path, out string newPath)
        {
            newPath = path;
            if (path is null) return false;

            var count = CountHypensToRemove(path);
            int offset = path.LastIndexOf('/') + 1;
            if (count == 0 && offset == path.Length)
            {
                return false;
            }

            newPath = string.Create(path.Length - count, (path, offset), s_createPascalCaseString);
            return true;
        }
#else
        public static bool TryKebabToPascalCase(string path, out string newPath)
        {
            newPath = path;
            if (path is null) return false;

            var count = CountHypensToRemove(path);
            int offset = path.LastIndexOf('/') + 1;
            if (count == 0 && offset == path.Length)
            {
                return false;
            }

            var sb = StringBuilderCache.Acquire(capacity: path.Length - count);

            if (offset > 0)
            {
                sb.Append(path.Substring(0, offset));
            }

            int newOffset;
            int diff;
            while ((newOffset = path.IndexOf('-', offset)) >= 0)
            {
                diff = newOffset - offset;
                if (diff > 0)
                {
                    // Capitalize the first letter
                    sb.Append(char.ToUpper(path[offset]));
                }
                if (diff > 1)
                {
                    // Copy the rest
                    sb.Append(path.Substring(offset + 1, diff - 1));
                }

                offset = newOffset + 1;
            }

            diff = path.Length - offset;
            if (diff > 0)
            {
                // Capitalize the first letter
                sb.Append(char.ToUpper(path[offset]));
            }
            if (diff > 1)
            {
                // Copy the rest
                sb.Append(path.Substring(offset + 1, diff - 1));
            }

            newPath = StringBuilderCache.GetStringAndRelease(sb);
            return true;
        }
#endif
        public static int CountHypensToRemove(string path)
        {
            if (path is null) return 0;

            int count = 0;
            int offset = path.LastIndexOf('/') + 1;
            while ((offset = path.IndexOf('-', offset) + 1) > 0)
            {
                count++;
            }

            return count;
        }
    }

    public static class HanselmanPathsExtensions
    {
        public static IApplicationBuilder UseHanselmanPathConverter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HanselmanPaths>();
        }
    }
}

