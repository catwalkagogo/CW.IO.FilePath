using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CW.IO {
	public struct AbsolutePath : IFilePath{
		#region Fields
		private readonly IReadOnlyList<string> _Fragments;

		public IReadOnlyList<string> Fragments {
			get {
				return this._Fragments;
			}
		}

		public IFilePathFormat Format { get; private set; }

		#endregion

		public AbsolutePath(IFilePathFormat format, IEnumerable<string> fragments)
			: this() {
			if(fragments == null) {
				throw new ArgumentNullException("fragments");
			}
			if(format == null) {
				throw new ArgumentNullException("format");
			}
			this.Format = format;
			this._Fragments = new ReadOnlyCollection<string>(this.Format.NormalizeAbsoluteFragments(fragments).ToArray());

			if(!this.Format.IsValid(this)) {
				throw new FormatException("Invalid path.");
			}

		}

		#region Properties

		public string FullPath {
			get {
				return this.Format.GetFullPath(this);
			}
		}

		#endregion

		#region GetRelativePathTo

		public RelativePath GetRelativePathTo(AbsolutePath dest) {
			if(!dest.Format.GetType().GetTypeInfo().IsAssignableFrom(this.Format.GetType().GetTypeInfo())) {
				throw new ArgumentException("Given dest path is incompatible format", "dest");
			}

			var common = FilePath.GetCommonRoot(this, dest).Fragments;
			var fromRoute = this.Fragments.Skip(common.Count);
			var destRoute = dest.Fragments.Skip(common.Count);
			return new RelativePath(
				this.Format,
				Enumerable.Repeat(@"..", fromRoute.Count()).Concat(destRoute)
			);
		}

		#endregion

		#region IFilePath

		public bool IsAbsolute {
			get {
				return true;
			}
		}

		public bool IsRelative {
			get {
				return false;
			}
		}

		#endregion

		#region IEquatable<IFilePath> Members

		public bool Equals(IFilePath other) {
			if(other == null) {
				return false;
			} else {
				return this.Format.Equals(this, other);
			}
		}

		public override int GetHashCode() {
			return this.Format.GetHashCode(this);
		}

		public override bool Equals(object obj) {
			if(!(obj is IFilePath)) {
				return false;
			}
			return this.Equals((IFilePath)obj);
		}

		#endregion

		#region Operators

		public static bool operator ==(AbsolutePath a, AbsolutePath b) {
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

		public static bool operator !=(AbsolutePath a, AbsolutePath b) {
			return !(a == b);
		}

		#endregion

		public override string ToString() {
			return this.FullPath;
		}
	}
}
