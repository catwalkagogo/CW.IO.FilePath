using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW.IO {
	public static class FilePathFormats {
		private static Lazy<IFilePathFormat> _Noarch = new Lazy<IFilePathFormat>(() => new NoarchPathFormat());
		public static IFilePathFormat Noarch {
			get {
				return _Noarch.Value;
			}
		}

		private static Lazy<IFilePathFormat> _Windows = new Lazy<IFilePathFormat>(() => new WindowsPathFormat());
		public static IFilePathFormat Windows {
			get {
				return _Windows.Value;
			}
		}

		private static Lazy<IFilePathFormat> _Unix = new Lazy<IFilePathFormat>(() => new UnixPathFormat());
		public static IFilePathFormat Unix {
			get {
				return _Unix.Value;
			}
		}

		private static Lazy<IFilePathFormat> _Platform = new Lazy<IFilePathFormat>(() => {
			if(System.IO.Path.Combine("a", "b").Contains("\\")) {
				return Windows;
			} else {
				return Unix;
			}
		});

		public static IFilePathFormat PlatformPathFormat {
			get {
				return _Platform.Value;
			}
		}
	}
}
