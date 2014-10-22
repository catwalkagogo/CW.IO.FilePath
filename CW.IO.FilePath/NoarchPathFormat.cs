using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW.IO {
	public class NoarchPathFormat : IFilePathFormat, IEquatable<NoarchPathFormat>{
		protected internal NoarchPathFormat() { }

		public virtual ISet<char> InvalidFileNameChars {
			get {
				return new HashSet<char>();
			}
		}

		public virtual string GetPath(IFilePath path) {
			throw new NotImplementedException();
		}

		public virtual string GetFullPath(IFilePath path) {
			throw new NotImplementedException();
		}

		public virtual IEqualityComparer<string> StringEqualityComparer {
			get {
				return StringComparer.Ordinal;
			}
		}

		public virtual int GetHashCode(IFilePath path) {
			if(path == null) {
				throw new ArgumentNullException("path");
			}

			var hash = this.GetHashCode() ^ path.IsAbsolute.GetHashCode() ^ path.IsRelative.GetHashCode();
			return path.Fragments.Aggregate(hash, (_, frg) => _ ^ frg.GetHashCode());
		}

		public virtual bool Equals(IFilePath a, IFilePath b) {
			if(a == null || b == null) {
				return Object.ReferenceEquals(a, b);
			} else {
				return
					a.Format.Equals(b.Format) &&
					a.IsAbsolute == b.IsAbsolute &&
					a.IsRelative == b.IsRelative &&
					a.Fragments.SequenceEqual(b.Fragments, this.StringEqualityComparer);
			}
		}

		public virtual IEnumerable<string> NormalizeRelativeFragments(IEnumerable<string> fragments) {
			return FilePath.ResolveFragments(fragments);
		}

		public virtual IEnumerable<string> NormalizeAbsoluteFragments(IEnumerable<string> fragments) {
			return FilePath.ResolveFragments(fragments);
		}

		public virtual bool IsValid(IFilePath path) {
			if(path == null) {
				throw new ArgumentNullException("path");
			}
			return this.IsValid(path.Fragments);
		}

		public virtual bool IsValid(RelativePath path) {
			return this.IsValid(path.Fragments);
		}

		public virtual bool IsValid(AbsolutePath path) {
			return this.IsValid(path.Fragments);
		}

		public virtual bool IsValid(IEnumerable<string> fragments) {
			var nameInvChars = this.InvalidFileNameChars;
			return !fragments.Any(name => name.ToCharArray().Any(c => nameInvChars.Contains(c)));
		}


		public virtual bool TryParse(string path, out IFilePath filePath) {
			throw new NotImplementedException();
		}

		public virtual IFilePath Parse(string path) {
			throw new NotImplementedException();
		}

		public bool Equals(NoarchPathFormat other) {
			if(other == null) {
				return false;
			} else {
				return this.GetType().Equals(other.GetType());
			}
		}

		public static bool operator==(NoarchPathFormat a, NoarchPathFormat b){
			if(Object.ReferenceEquals(a, null)) {
				if(Object.ReferenceEquals(b, null)) {
					return true;
				} else {
					return false;
				}
			} else {
				return a.Equals(b);
			}
		}

		public static bool operator !=(NoarchPathFormat a, NoarchPathFormat b) {
			return !(a == b);
		}

		public override bool Equals(object obj) {
			var format = obj as NoarchPathFormat;
			if(format != null) {
				return this.Equals(format);
			} else {
				return base.Equals(obj);
			}
		}

		public override int GetHashCode() {
			return this.GetType().GetHashCode();
		}
	}
}
