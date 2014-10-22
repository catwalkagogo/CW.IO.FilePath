/*
	$Id: FilePath.cs 315 2013-12-11 07:59:06Z catwalkagogo@gmail.com $
*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CW.IO {
	/*
	public abstract partial class FilePath : IEquatable<FilePath>, CW.IO.IFilePath {
		private readonly IReadOnlyList<string> _Fragments;

		/// <summary>
		/// パスの階層
		/// </summary>
		public IReadOnlyList<string> Fragments{
			get {
				return this._Fragments;
			}
		}

		/// <summary>
		/// パスのフォーマット(Windows/Unix)
		/// </summary>
		public IFilePathFormat Format { get; private set; }

		#region Constructor

		protected FilePath(IFilePathFormat format, IEnumerable<string> fragments){
			if(fragments == null) {
				throw new ArgumentNullException("fragments");
			}
			if(format == null) {
				throw new ArgumentNullException("format");
			}
			this.Format = format;
			this._Fragments = new ReadOnlyCollection<string>(this.NormalizeFragments(fragments).ToArray());

			this.Validate();
		}

		#endregion

		#region Validate

		protected virtual void Validate() {
			if(!this.Format.IsValid(this)) {
				throw new FormatException("Invalid path.");
			}
		}

		#endregion

		#region Normalize

		protected virtual IEnumerable<string> NormalizeFragments(IEnumerable<string> fragments) {
			return fragments;
		}

		#endregion

		#region Properties
		public bool IsAbsolute {
			get {
				return this is AbsolutePath;
			}
		}

		public bool IsRelative {
			get {
				return this is RelativePath;
			}
		}

		/// <summary>
		/// パス
		/// </summary>
		public string Path {
			get {
				return this.Format.GetPath(this);
			}
		}

		/// <summary>
		/// フルパスを取得する。
		/// </summary>
		public string FullPath{
			get{
				return this.Format.GetFullPath(this);
			}
		}

		/// <summary>
		/// ファイル名を取得する。
		/// </summary>
		public string FileName{
			get{
				if(this._Fragments.Count > 0) {
					return this._Fragments[this._Fragments.Count - 1];
				} else {
					return "";
				}
			}
		}

		public string Extension{
			get{
				var name = this.FileName;
				var idx = name.LastIndexOf('.');
				if(idx >= 0){
					return name.Substring(idx + 1);
				}else{
					return "";
				}
			}
		}

		public string FileNameWithoutExtension {
			get {
				var name = this.FileName;
				return name.Substring(0, name.Length - this.Extension.Length);
			}
		}

		#endregion

		#region IEquatable<FilePath> Members

		public bool Equals(FilePath other) {
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
			if(!(obj is FilePath)) {
				return false;
			}
			return this.Equals((FilePath)obj);
		}

		#endregion

		#region Operators

		public static bool operator ==(FilePath a, FilePath b){
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

		public static bool operator !=(FilePath a, FilePath b){
			return !(a == b);
		}

		#endregion

		public override string ToString(){
			return
				this.IsAbsolute ? this.FullPath :
				this.Path;
		}

	}*/
}
