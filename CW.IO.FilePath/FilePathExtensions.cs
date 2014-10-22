using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW.IO {
	public static class FilePathExtensions {

		#region Resolve

		public static IFilePath Resolve(this IFilePath path, params IFilePath[] paths) {
			if(path == null) {
				throw new ArgumentNullException("path");
			}
			if(paths == null) {
				throw new ArgumentNullException("paths");
			}

			return paths.Aggregate(path, (src, dest) => FilePath.ResolveInternal(src, dest));
		}

		public static IFilePath Resolve(this IFilePath path, IEnumerable<IFilePath> paths) {
			if(path == null) {
				throw new ArgumentNullException("path");
			}
			if(paths == null) {
				throw new ArgumentNullException("paths");
			}

			return paths.Aggregate(path, (src, dest) => FilePath.ResolveInternal(src, dest));
		}

		public static AbsolutePath Resolve(this AbsolutePath path, params RelativePath[] paths) {
			if(path == null) {
				throw new ArgumentNullException("path");
			}
			if(paths == null) {
				throw new ArgumentNullException("paths");
			}

			return paths.Aggregate(path, (src, dest) => (AbsolutePath)FilePath.ResolveInternal(src, dest));
		}

		public static AbsolutePath Resolve(this AbsolutePath path, IEnumerable<RelativePath> paths) {
			if(path == null) {
				throw new ArgumentNullException("path");
			}
			if(paths == null) {
				throw new ArgumentNullException("paths");
			}

			return paths.Aggregate(path, (src, dest) => (AbsolutePath)FilePath.ResolveInternal(src, dest));
		}

		public static RelativePath Resolve(this RelativePath path, params RelativePath[] paths) {
			if(path == null) {
				throw new ArgumentNullException("path");
			}
			if(paths == null) {
				throw new ArgumentNullException("paths");
			}

			return paths.Aggregate(path, (src, dest) => (RelativePath)FilePath.ResolveInternal(src, dest));
		}

		public static RelativePath Resolve(this RelativePath path, IEnumerable<RelativePath> paths) {
			if(path == null) {
				throw new ArgumentNullException("path");
			}
			if(paths == null) {
				throw new ArgumentNullException("paths");
			}

			return paths.Aggregate(path, (src, dest) => (RelativePath)FilePath.ResolveInternal(src, dest));
		}

		#endregion
	}
}
