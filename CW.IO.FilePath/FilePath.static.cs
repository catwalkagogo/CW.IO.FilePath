using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CW.IO {
	public static class FilePath{
		#region GetCommonRoot
		private static IFilePathFormat GetCommonFileFormat(IEnumerable<IFilePath> paths) {
			if(paths == null) {
				return null;
			}
			IFilePathFormat format = null;
			foreach(var path in paths) {
				if(format == null) {
					format = path.Format;
				}
				if(format != path.Format) {
					return FilePathFormats.Noarch;
				}
			}
			return format;
		}

		public static IFilePath GetCommonRoot(IEnumerable<IFilePath> paths) {
			var a = paths.ToArray();
			return GetCommonRoot(GetCommonFileFormat(a), paths.ToArray());
		}

		public static IFilePath GetCommonRoot(params IFilePath[] paths) {
			return GetCommonRoot(GetCommonFileFormat(paths), paths);
		}

		public static IFilePath GetCommonRoot(IFilePathFormat format, IEnumerable<IFilePath> paths) {
			return GetCommonRoot(format, paths.ToArray());
		}

		public static IFilePath GetCommonRoot(IFilePathFormat format, params IFilePath[] paths) {
			if(format == null) {
				throw new ArgumentNullException("format");
			}
			if(paths == null) {
				throw new ArgumentNullException("paths");
			} else if(paths.Length == 0) {
				throw new ArgumentException("paths is empty");
			} else if(paths.Length == 1) {
				return paths[0];
			}

			if(paths.All(p => p.IsAbsolute)) {
				return GetCommonRoot(paths.Cast<AbsolutePath>());
			} else if(paths.All(p => p.IsRelative)) {
				return GetCommonRoot(paths.Cast<RelativePath>());
			} else {
				throw new ArgumentException("paths must be all relative or absolute.", "paths");
			}
		}

		public static AbsolutePath GetCommonRoot(IEnumerable<AbsolutePath> paths) {
			var a = paths.ToArray();
			return GetCommonRoot(GetCommonFileFormat(a.Cast<IFilePath>()), a);
		}

		public static AbsolutePath GetCommonRoot(params AbsolutePath[] paths) {
			return GetCommonRoot(GetCommonFileFormat(paths.Cast<IFilePath>()), paths);
		}

		public static AbsolutePath GetCommonRoot(IFilePathFormat format, IEnumerable<AbsolutePath> paths) {
			return GetCommonRoot(format, paths.ToArray());
		}

		public static AbsolutePath GetCommonRoot(IFilePathFormat format, params AbsolutePath[] paths) {
			if(format == null) {
				throw new ArgumentNullException("format");
			}
			if(paths == null) {
				throw new ArgumentNullException("paths");
			} else if(paths.Length == 0) {
				throw new ArgumentException("paths is empty");
			} else if(paths.Length == 1) {
				return paths[0];
			}

			IEnumerable<string> fragments = format.NormalizeAbsoluteFragments(paths[0].Fragments);

			var cmp = format.StringEqualityComparer;
			foreach(var path in paths.Skip(1)) {
				var fragments2 = format.NormalizeAbsoluteFragments(path.Fragments);
				fragments = fragments
					.Zip(fragments2, (f1, f2) => Tuple.Create(f1, f2))
					.TakeWhile(t => cmp.Equals(t.Item1, t.Item2))
					.Select(t => t.Item1);
			}
			return new AbsolutePath(format, fragments);
		}

		public static RelativePath GetCommonRoot(IEnumerable<RelativePath> paths) {
			var a = paths.ToArray();
			return GetCommonRoot(GetCommonFileFormat(a.Cast<IFilePath>()), a);
		}

		public static RelativePath GetCommonRoot(params RelativePath[] paths) {
			return GetCommonRoot(GetCommonFileFormat(paths.Cast<IFilePath>()), paths);
		}

		public static RelativePath GetCommonRoot(IFilePathFormat format, IEnumerable<RelativePath> paths) {
			return GetCommonRoot(format, paths.ToArray());
		}

		public static RelativePath GetCommonRoot(IFilePathFormat format, params RelativePath[] paths) {
			if(format == null) {
				throw new ArgumentNullException("format");
			}
			if(paths == null) {
				throw new ArgumentNullException("paths");
			} else if(paths.Length == 0) {
				throw new ArgumentException("paths is empty");
			} else if(paths.Length == 1) {
				return paths[0];
			}

			IEnumerable<string> fragments = format.NormalizeRelativeFragments(paths[0].Fragments);

			var cmp = format.StringEqualityComparer;
			foreach(var path in paths.Skip(1)) {
				var fragments2 = format.NormalizeRelativeFragments(path.Fragments);
				fragments = fragments
					.Zip(fragments2, (f1, f2) => Tuple.Create(f1, f2))
					.TakeWhile(t => cmp.Equals(t.Item1, t.Item2))
					.Select(t => t.Item1);
			}
			return new RelativePath(format, fragments);
		}

		#endregion

		#region ResolveAll

		public static IFilePath ResolveAll(params IFilePath[] paths) {
			if(paths == null) {
				throw new ArgumentNullException("paths");
			}

			return paths.Aggregate((src, path) => ResolveInternal(src, path));
		}

		public static IFilePath ResolveAll(IEnumerable<IFilePath> paths) {
			if(paths == null) {
				throw new ArgumentNullException("paths");
			}

			return paths.Aggregate((src, path) => ResolveInternal(src, path));
		}

		public static AbsolutePath ResolveAll(IEnumerable<AbsolutePath> paths) {
			if(paths == null) {
				throw new ArgumentNullException("paths");
			}

			return paths.Aggregate((src, path) => (AbsolutePath)ResolveInternal(src, path));
		}

		public static AbsolutePath ResolveAll(params AbsolutePath[] paths) {
			if(paths == null) {
				throw new ArgumentNullException("paths");
			}

			return paths.Aggregate((src, path) => (AbsolutePath)ResolveInternal(src, path));
		}


		internal static IFilePath ResolveInternal(IFilePath src, IFilePath dest) {
			if(src == null) {
				throw new ArgumentNullException("src");
			}
			if(dest == null) {
				throw new ArgumentNullException("dest");
			}
			if(dest.IsAbsolute) {
				return dest;
			}
			var destFormat =
				(src.Format == dest.Format) ?
					src.Format :
					dest.Format.GetType().GetTypeInfo().IsAssignableFrom(src.Format.GetType().GetTypeInfo()) ?
						dest.Format :
						null;

			if(destFormat == null) {
				throw new ArgumentException("dest is an incompatible file path format.", "dest");
			}

			var baseNames = src.Fragments;
			var destNames = dest.Fragments;
			var outNames = new List<string>(baseNames.Count + destNames.Count);
			foreach(var names in new IReadOnlyList<string>[] { baseNames, destNames }) {
				ResolveInternal(names, outNames);
			}
			if(src.IsAbsolute) {
				return new AbsolutePath(destFormat, outNames);
			} else {
				return new RelativePath(destFormat, outNames);
			}
		}

		private static void ResolveInternal(IEnumerable<string> names, List<string> outNames) {
			var names2 = names.ToArray();
			foreach(var name in names2) {
				//Console.WriteLine(name);
				if(name == "..") {
					if(outNames.Count > 0 && outNames[outNames.Count - 1] != "..") {
						outNames.RemoveAt(outNames.Count - 1);
					} else {
						outNames.Add("..");
					}
				} else if(name != ".") {
					outNames.Add(name);
				}
				//Console.WriteLine(String.Join("/", outNames));
			}
		}

		public static IReadOnlyList<string> ResolveFragments(params IEnumerable<string>[] fragmentsList) {
			var outNames = new List<string>();
			foreach(var fragments in fragmentsList) {
				ResolveInternal(fragments, outNames);
			}
			return new ReadOnlyCollection<string>(outNames);
		}

		#endregion

		#region ConvertFormat

		public static IFilePath ConvertFormat(this IFilePath path, IFilePathFormat format) {
			if(format == null) {
				throw new ArgumentNullException("format");
			}

			if(path.Format == format) {
				return path;
			} else if(path.IsAbsolute) {
				return new AbsolutePath(format, path.Fragments);
			} else if(path.IsRelative) {
				return new RelativePath(format, path.Fragments);
			} else {
				return null;
			}
		}

		#endregion
	}
}
