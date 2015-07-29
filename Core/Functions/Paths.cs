using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DotNEET.Extensions;

namespace DotNEET.Functions
{
    public static class Paths
    {
        public static string GetRelativePath(string fromPath, string toPath, bool fromIsDirectory = false, bool toIsDirectory = false)
        {
            fromPath.ThrowIfNullOrWhiteSpace("fromPath");
            toPath.ThrowIfNullOrWhiteSpace("toPath");
            // Most likely a hacky quick fix
            fromPath = fromIsDirectory ? fromPath + Path.DirectorySeparatorChar : fromPath;
            toPath = toIsDirectory ? toPath + Path.DirectorySeparatorChar : toPath;

            var fromUri = new Uri(fromPath);
            var toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.ToUpperInvariant() == "FILE")
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }
    }
}