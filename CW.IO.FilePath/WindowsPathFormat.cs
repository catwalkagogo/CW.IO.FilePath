using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW.IO {
	public sealed class WindowsPathFormat : NoarchPathFormat, IEquatable<WindowsPathFormat>{
		internal WindowsPathFormat() { }

		public override IEnumerable<string> NormalizeAbsoluteFragments(IEnumerable<string> fragments) {
			var fragmentsArray = fragments.ToArray();

			if(fragmentsArray.Length > 0 && fragmentsArray[0].Length > 1) {
				// ドライブ文字を大文字に
				fragmentsArray[0] = fragmentsArray[0].ToUpper().TrimEnd(':');
			}

			return base.NormalizeAbsoluteFragments(fragments);
		}

		public string DirectorySeparator {
			get {
				return "\\";
			}
		}

		public IEnumerable<string> AltDirectorySeparators {
			get {
				return new string[]{"/"};
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

			// 絶対パスの場合、ドライブ名をチェック
			var isAbsolute = this.IsAbsolute(path);

			if(isAbsolute) {
				if(!(path.Length > 0 && (('A' <= path[0] && path[0] <= 'Z') || ('a' <= path[0] && path[0] <= 'z')))) {
					return false;
				}

				// ドライブ区切り除去
				path = path.Substring(0, 1) + path.Substring(2);
			}

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

		public bool IsAbsolute(string path){
			return (path.Length > 1 && path[1] == ':');
		}

		public override string GetPath(IFilePath path) {
			return
				(path.Fragments != null && path.Fragments.Count > 0) ?
					String.Join(this.DirectorySeparator, path.Fragments.Skip(1)) :
					null;
		}

		public override string GetFullPath(IFilePath path) {
			if(path == null) {
				throw new ArgumentNullException("path");
			}

			if(path.IsAbsolute){
				return path.Fragments[0] + @":\" + this.GetPath(path);
			}else{
				return null;
			}
		}

		public override IEqualityComparer<string> StringEqualityComparer {
			get {
				return StringComparer.OrdinalIgnoreCase;
			}
		}

		public override int GetHashCode(IFilePath path) {
			var hash = this.GetHashCode() ^ path.IsAbsolute.GetHashCode() ^ path.IsRelative.GetHashCode();
			return path.Fragments.Aggregate(hash, (_, frg) => _ ^ frg.ToUpper().GetHashCode());
		}

		public bool Equals(WindowsPathFormat other) {
			if(other == null) {
				return false;
			} else {
				return this.GetType().Equals(other.GetType());
			}
		}
	}
}
