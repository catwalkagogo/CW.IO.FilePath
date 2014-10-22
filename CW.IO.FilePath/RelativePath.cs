using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW.IO {
	public struct RelativePath : IFilePath {
		#region Fields
		private readonly IReadOnlyList<string> _Fragments;

		public IReadOnlyList<string> Fragments {
			get {
				return this._Fragments;
			}
		}

		public IFilePathFormat Format { get; private set; }

		#endregion

		public RelativePath(IFilePathFormat format, IEnumerable<string> fragments)
			: this() {
			if(fragments == null) {
				throw new ArgumentNullException("fragments");
			}
			if(format == null) {
				throw new ArgumentNullException("format");
			}
			this.Format = format;
			this._Fragments = new ReadOnlyCollection<string>(this.Format.NormalizeRelativeFragments(fragments).ToArray());

			if(!this.Format.IsValid(this)) {
				throw new FormatException("Invalid path.");
			}

		}

		#region

		public string Path {
			get {
				return this.Format.GetPath(this);
			}
		}

		#endregion

		#region GetFullPath

		public IFilePath GetFullPath(IFilePath basePath) {
			if(basePath == null) {
				throw new ArgumentNullException("basePath");
			}
			if(!(basePath is AbsolutePath)) {
				throw new ArgumentNullException("basePath is not absolute");
			}

			return this.GetFullPath(basePath);
		}


		/// <summary>
		/// Get full path from base path.
		/// </summary>
		/// <param name="basePath">Base absolute path</param>
		/// <exception cref="System.InvalidOperationException">this path kind is not relative</exception>
		/// <exception cref="System.UriFormatException">The base path is not absolute.</exception>
		/// <returns>full path</returns>
		public AbsolutePath GetFullPath(AbsolutePath basePath) {
			if(basePath == null) {
				throw new ArgumentNullException("basePath");
			}

			return basePath.Resolve(this);
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

		public static bool operator ==(RelativePath a, RelativePath b) {
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

		public static bool operator !=(RelativePath a, RelativePath b) {
			return !(a == b);
		}

		#endregion

		#region IFilePath
		public bool IsAbsolute {
			get {
				return false;
			}
		}

		public bool IsRelative {
			get {
				return true;
			}
		}

		#endregion

		public override string ToString() {
			return this.Path;
		}
	}
}
