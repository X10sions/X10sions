using Microsoft.Extensions.FileProviders;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.FileProviders {
  public class EnumerableDirectoryContent : IDirectoryContents {
    public EnumerableDirectoryContent(IEnumerable<IFileInfo> entries) {
      _entries = entries ?? throw new ArgumentNullException(nameof(entries));
    }
    private readonly IEnumerable<IFileInfo> _entries;
    public bool Exists => true;
    public IEnumerator<IFileInfo> GetEnumerator() => _entries.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _entries.GetEnumerator();
  }

}