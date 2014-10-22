using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW.IO {
	public interface IFilePathFormat : IEqualityComparer<IFilePath> {
		IEnumerable<string> NormalizeRelativeFragments(IEnumerable<string> fragments);
		IEnumerable<string> NormalizeAbsoluteFragments(IEnumerable<string> fragments);

		bool TryParse(string path, out IFilePath filePath);
		IFilePath Parse(string path);

		bool IsValid(IFilePath path);
		bool IsValid(RelativePath path);
		bool IsValid(AbsolutePath path);
		bool IsValid(IEnumerable<string> fragments);

		string GetPath(IFilePath path);

		string GetFullPath(IFilePath path);

		IEqualityComparer<string> StringEqualityComparer { get; }
	}
}
