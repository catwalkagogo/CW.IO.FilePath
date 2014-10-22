using System;
namespace CW.IO {
	public interface IFilePath : IEquatable<IFilePath>{
		IFilePathFormat Format { get; }
		System.Collections.Generic.IReadOnlyList<string> Fragments { get; }
		bool IsAbsolute { get; }
		bool IsRelative { get; }
		string ToString();
	}
}
