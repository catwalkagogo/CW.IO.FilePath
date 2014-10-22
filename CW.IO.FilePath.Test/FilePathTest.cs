using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CW.IO;

namespace CatWalk.Test {
	[TestFixture]
	public class FilePathTest {
		[TestCase(@"c:\", true, true, true, 1)]
		[TestCase(@"c:\Program Files", true, true, true, 2)]
		[TestCase(@"c:\Program Files\", true, true, true, 2)]
		[TestCase(@"c:\\Program Files\\", true, true, true, 2)]
		[TestCase(@"_:\Program Files", true, false, true, 0)]
		
		// unix
		[TestCase(@"/usr/bin", false, true, true, 2)]
		[TestCase(@"/", false, true, true, 0)]

		// relatives
		[TestCase(@"/usr/../etc", false, true, true, 1)]
		[TestCase(@"./path", false, true, false, 1)]
		[TestCase(@"../path", false, true, false, 2)]
		[TestCase(@"./path/..", false, true, false, 0)]
		public void Init(string path, bool isWindows, bool isValid, bool isAbsolute, int frgCount) {
			var format = GetFilePathFormat(isWindows);

			IFilePath fp;
			var fpIsValid = format.TryParse(path, out fp);
			Assert.AreEqual(isValid, fpIsValid, "IsValid");
			if(fpIsValid) {
				Assert.AreEqual(isAbsolute, fp.IsAbsolute, "IsAbsolute");
				Assert.AreEqual(!isAbsolute, fp.IsRelative, "IsRelative");
				Assert.AreEqual(frgCount, fp.Fragments.Count, "Fragment Count");
			}
		}

		[TestCase("/usr/bin", false, "/", "../../")]
		[TestCase("/usr/bin", false, "/etc", "../../", "etc")]
		[TestCase("/usr/bin", false, "/var/www", "../../", "./etc/httpd", "/var/www")]
		public void Resolve(string path, bool isWindows, string expected, params string[] dest) {
			var format = GetFilePathFormat(isWindows);

			var fp = format.Parse(path);
			Assert.AreEqual(true, fp.IsAbsolute, "IsAbsolute");

			fp = fp.Resolve(dest.Select(p => format.Parse(p)));

			Assert.AreEqual(expected, fp.ToString());
		}

		[TestCase(true, @"c:\windows", @"c:\Windows\System32\drivers", @"c:\Windows\System32", @"c:\Windows\")]
		public void GetCommonRoot(bool isWindows, string expected, params string[] paths){
			var format = GetFilePathFormat(isWindows);

			var fp = FilePath.GetCommonRoot(paths.Select(p => format.Parse(p)));
			Assert.AreEqual(expected.ToLower(), fp.ToString().ToLower());
		}

		private static IFilePathFormat GetFilePathFormat(bool isWindows) {
			return isWindows ?
				FilePathFormats.Windows :
				FilePathFormats.Unix;
		}
	}
}
