CW.IO.FilePath
===========

# Overview

CW.IO.FilePath is a portable .Net library that

* provides utilities to parse and operate a file path.
* supports Unix/Windows file format and platform independed file format (Noarch).

# Usage


## Parse

```C#
var absolutePath = (AbsolutePath)FilePathFormats.Windows.Parse(@"C:\Windows\System32");
Debug.WriteLine(absolutePath.FullPath); // C:\Windows\System32

var relativePath = (RelativePath)FilePathFormats.Windows.Parse(@"./AppData/Roaming");
Debug.WriteLine(relativePath.Path); // AppData/Roaming
```

## Resolve

```C#
var a = FilePathFormats.Windows.Parse("/usr/bin");
var b = FilePathFormats.Windows.Parse("../local/bin");

var fp = a.Resolve(b);
Debug.WriteLine(fp.FullPath); // /usr/local/bin

```

## ResolveAll

```C#
var paths = (new string[]{
	@"/usr/bin",
	@"../../",
	@"./etc",
}).Select(p => FilePathFormats.Unix.Parse(p));

var fp = FilePath.ResolveAll(paths);
Debug.WriteLine(fp.FullPath); // /etc

```