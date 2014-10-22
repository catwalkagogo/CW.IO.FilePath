using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW.IO {
	public sealed class UnixPathFormat : NoarchPathFormat, IEquatable<UnixPathFormat>{
		internal UnixPathFormat() { }

		public string DirectorySeparator {
			get {
				return "/";
			}
		}

		public IEnumerable<string> AltDirectorySeparators {
			get {
				return new string[]{"\\"};
			}
		}


		private static readonly ISet<char> _InvalidFileNameChars = new HashSet<char>(
			new char[] { '\"', '<', '>', '|', '\0', (Char)1, (Char)2, (Char)3, (Char)4, (Char)5, (Char)6, (Char)7, (Char)8, (Char)9, (Char)10, (Char)11, (Char)12, (Char)13, (Char)14, (Char)15, (Char)16, (Char)17, (Char)18, (Char)19, (Char)20, (Char)21, (Char)22, (Char)23, (Char)24, (Char)25, (Char)26, (Char)27, (Char)28, (Char)29, (Char)30, (Char)31, ':', '*', '?', '\\', '/' });
		public override ISet<char> InvalidFileNameChars {
			get {
				return base.InvalidFileNameChars;
			}
		}

		public override bool TryParse(string path, out IFilePath filePath) {
			filePath = null;

			var isAbsolute = this.IsAbsolute(path);

			var fragments = FilePath.ResolveFragments(
				path.Split(
					this.AltDirectorySeparators
						.Concat(new string[] { this.DirectorySeparator })
						.ToArray(),
					StringSplitOptions.RemoveEmptyEntries
				)
			);

			try {
				filePath = isAbsolute ? (IFilePath)(new AbsolutePath(this, fragments)) : (IFilePath)(new RelativePath(this, fragments));
			} catch(FormatException) {
				return false;
			}

			return true;
		}

		public override IFilePath Parse(string path) {
			IFilePath filePath;
			if(this.TryParse(path, out filePath)) {
				return filePath;
			} else {
				throw new FormatException();
			}
		}

		public bool IsAbsolute(string path) {
			return (path != null) && path.StartsWith(this.DirectorySeparator);
		}

		public override string GetPath(IFilePath path) {
			return String.Join(this.DirectorySeparator, path.Fragments);
		}

		public override string GetFullPath(IFilePath path) {
			if(path.IsAbsolute) {
				return this.DirectorySeparator + String.Join(this.DirectorySeparator, path.Fragments);
			} else {
				return null;
			}
		}

		public override IEqualityComparer<string> StringEqualityComparer {
			get {
				return StringComparer.Ordinal;
			}
		}

		public bool Equals(UnixPathFormat other) {
			if(other == null) {
				return false;
			} else {
				return this.GetType().Equals(other.GetType());
			}
		}
	}
}
