using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Embedded;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.FileProviders {
  public class EmbeddedFileResolvingProvider : IFileProvider {
    // public EmbeddedFileResolvingProvider(Assembly assembly) : this(assembly, assembly?.GetName()?.Name) { }

    public EmbeddedFileResolvingProvider(Assembly assembly, string baseNamespace) {
      _baseNamespace = string.IsNullOrEmpty(baseNamespace) ? string.Empty : baseNamespace + ".";
      _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
      _lastModified = DateTimeOffset.UtcNow;
      if (!string.IsNullOrEmpty(_assembly.Location)) {
        try {
          _lastModified = File.GetLastWriteTimeUtc(_assembly.Location);
        } catch (PathTooLongException) {
        } catch (UnauthorizedAccessException) {
        }
      }
    }

    static readonly char[] _invalidFileNameChars = Path.GetInvalidFileNameChars().Where(c => c != '/' && c != '\\').ToArray();
    readonly Assembly _assembly;
    readonly string _baseNamespace;
    readonly DateTimeOffset _lastModified;

    // from documentation you would think that - should always be mapped to _ which we do above
    // but from observation that seems only true for folder segments
    // for files it seems sometimes I have to swap _ back to - or I get a null resourceStream

    //https://stackoverflow.com/questions/14705211/how-is-net-renaming-my-embedded-resources
    // identifiers can't start with a digit, so _ is prepended by resource builder
    // so far have not run into things named that way in my static resources
    // made this an interface so it can be easily replaced if it doesn't work for other scenarios

    /// <summary>
    /// Enumerate a directory at the given path, if any.
    /// This file provider uses a flat directory structure. Everything under the base namespace is considered to be one
    /// directory.
    /// </summary>
    /// <param name="subpath">The path that identifies the directory</param>
    /// <returns>
    /// Contents of the directory. Caller must check Exists property. A <see cref="NotFoundDirectoryContents" /> if no
    /// resources were found that match <paramref name="subpath" />
    /// </returns>
    public IDirectoryContents GetDirectoryContents(string subpath) {
      // The file name is assumed to be the remainder of the resource name.
      if (subpath == null) {
        return NotFoundDirectoryContents.Singleton;
      }
      // Relative paths starting with a leading slash okay
      if (subpath.StartsWith("/", StringComparison.Ordinal)) {
        subpath = subpath.Substring(1);
      }
      // Non-hierarchal.
      if (!subpath.Equals(string.Empty)) {
        return NotFoundDirectoryContents.Singleton;
      }
      var entries = new List<IFileInfo>();
      // TODO: The list of resources in an assembly isn't going to change. Consider caching.
      var resources = _assembly.GetManifestResourceNames();
      for (var i = 0; i < resources.Length; i++) {
        var resourceName = resources[i];
        if (resourceName.StartsWith(_baseNamespace, StringComparison.Ordinal)) {
          entries.Add(new EmbeddedResourceFileInfo(
              _assembly,
              resourceName,
              resourceName.Substring(_baseNamespace.Length),
              _lastModified));
        }
      }
      return new EnumerableDirectoryContent(entries);
    }

    /// <summary>
    /// Locates a file at the given path.
    /// </summary>
    /// <param name="subpath">The path that identifies the file. </param>
    /// <returns>
    /// The file information. Caller must check Exists property. A <see cref="NotFoundFileInfo" /> if the file could
    /// not be found.
    /// </returns>
    public IFileInfo GetFileInfo(string subpath) {
      if (string.IsNullOrEmpty(subpath)) {
        return new NotFoundFileInfo(subpath);
      }
      subpath = ResolveResourceIdentifier(subpath);
      var builder = new StringBuilder(_baseNamespace.Length + subpath.Length);
      builder.Append(_baseNamespace);
      // Relative paths starting with a leading slash okay
      if (subpath.StartsWith("/", StringComparison.Ordinal)) {
        builder.Append(subpath, 1, subpath.Length - 1);
      } else {
        builder.Append(subpath);
      }
      for (var i = _baseNamespace.Length; i < builder.Length; i++) {
        if (builder[i] == '/' || builder[i] == '\\') {
          builder[i] = '.';
        }
      }
      var resourcePath = builder.ToString();
      if (HasInvalidPathChars(resourcePath)) {
        return new NotFoundFileInfo(resourcePath);
      }
      var name = Path.GetFileName(subpath);
      if (_assembly.GetManifestResourceInfo(resourcePath) == null) {
        return new NotFoundFileInfo(name);
      }
      return new EmbeddedResourceFileInfo(_assembly, resourcePath, name, _lastModified);
    }

    public virtual string ResolveResourceIdentifier(string inputIdentifier) {
      if (string.IsNullOrWhiteSpace(inputIdentifier))
        return inputIdentifier;
      var fileName = Path.GetFileName(inputIdentifier);
      //var identifier = inputIdentifier.Replace("/", ".");
      if (!inputIdentifier.Contains("-"))
        return inputIdentifier;
      // we need to not replace - from folder names but not file names
      var pathBeforeFileNaame = inputIdentifier.Replace(fileName, string.Empty).Replace("-", "_");
      return pathBeforeFileNaame + fileName;
    }

    /// <summary>
    /// Embedded files do not change.
    /// </summary>
    /// <param name="filter">This parameter is ignored</param>
    /// <returns>A <see cref="NullChangeToken" /></returns>
    public IChangeToken Watch(string filter) => NullChangeToken.Singleton;

    static bool HasInvalidPathChars(string path) => path.IndexOfAny(_invalidFileNameChars) != -1;

  }
}