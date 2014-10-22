using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CW.IO;
using System.Diagnostics;

namespace CW.IO.Test {
	class Samples {
		static void Parse() {
			var absolutePath = (AbsolutePath)FilePathFormats.Windows.Parse(@"C:\Windows\System32");
			Debug.WriteLine(absolutePath.ToString()); // C:\Windows\System32

			var relativePath = (RelativePath)FilePathFormats.Windows.Parse(@"./AppData/Roaming");
			Debug.WriteLine(relativePath.ToString()); // AppData/Roaming
		}
		static void Resolve() {
			var a = FilePathFormats.Windows.Parse("/usr/bin");
			var b = FilePathFormats.Windows.Parse("../local/bin");

			var fp = a.Resolve(b);
			Debug.WriteLine(fp.ToString()); // /usr/local/bin
		}

		static void ResolveAll() {
			var paths = (new string[]{
				@"/usr/bin",
				@"../../",
				@"./etc",
			}).Select(p => FilePathFormats.Unix.Parse(p));

			var fp = FilePath.ResolveAll(paths);
			Debug.WriteLine(fp.ToString()); // /etc
		}
	}
}
