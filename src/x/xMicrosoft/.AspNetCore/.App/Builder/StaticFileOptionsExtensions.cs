﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System;

namespace Microsoft.AspNetCore.Builder {
  public static class StaticFileOptionsExtensions {

    public static StaticFileOptions AddRazorClassLibraryStaticFiles<T>(this StaticFileOptions options, IWebHostEnvironment environment, string rootPath = "wwwroot") {
      options = options ?? throw new ArgumentNullException(nameof(options));
      // Basic initialization in case the options weren't initialized by any other component
      options.ContentTypeProvider = options.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
      if (options.FileProvider == null && environment.WebRootFileProvider == null) {
        throw new InvalidOperationException("Missing FileProvider.");
      }
      options.FileProvider = options.FileProvider ?? environment.WebRootFileProvider;
      // Add our provider
      var filesProvider = new ManifestEmbeddedFileProvider(typeof(T).Assembly, rootPath);
      options.FileProvider = new CompositeFileProvider(options.FileProvider, filesProvider);
      return options;
    }

  }
}