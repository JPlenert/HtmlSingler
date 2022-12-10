using HtmlSingler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSinglerTest
{
    [TestClass]
    public class SinglerTests
    {
        [TestMethod]
        public void TestFileConsole()
        {
            string tempPath = ExtractTestFiles();

            using (MemoryStream memStream = new MemoryStream())
            {
                using (TextWriter writer = new StreamWriter(memStream))
                {
                    TextWriter oldOut = Console.Out;
                    Console.SetOut(writer);
                    try
                    {
                        Singler singler = new Singler();
                        singler.Execute(Path.Join(tempPath, "test.html"), null);
                    }
                    finally
                    {
                        Console.SetOut(oldOut);
                    }
                }

                CheckTestOutput(UTF8Encoding.UTF8.GetString(memStream.ToArray()), 4699);
            }
        }

        [TestMethod]
        public void TestFileFile()
        {
            string tempPath = ExtractTestFiles();
            string outFileName = Path.Join(tempPath, "test.html.out");

            Singler singler = new Singler();
            singler.Execute(Path.Join(tempPath, "test.html"), outFileName);

            CheckTestOutput(File.ReadAllText(outFileName), 4699);
        }

        [TestMethod]
        public void TestOnlineFile()
        {
            string tempPath = ExtractTestFiles();
            string outFileName = Path.Join(tempPath, "test.html.out");

            Singler singler = new Singler();
            singler.Execute(Path.Join(tempPath, "online.html"), outFileName);

            CheckTestOutput(File.ReadAllText(outFileName), 254740);
        }

        private void CheckTestOutput(string testOutput, int len)
        {
            // May not contain any Tag
            Assert.IsFalse(testOutput.Contains(Singler.TAGPrefix));
            // +4 because of CR(LF) on console
            Assert.IsTrue(testOutput.Length >= len && testOutput.Length <= len+4);
        }

        private string ExtractTestFiles()
        {
            const string resPrefix = "HtmlSinglerTest._Resources.";
            Random random = new Random();
            string tempPath = Path.Join(Path.GetTempPath(), random.NextInt64().ToString());
            Directory.CreateDirectory(tempPath);

            Assembly ass = Assembly.GetExecutingAssembly();
            foreach (var embFileName in ass.GetManifestResourceNames())
            {
                using (var readStream = ass.GetManifestResourceStream(embFileName))
                {
                    using (var writeStream = File.OpenWrite(Path.Join(tempPath, embFileName.Substring(resPrefix.Length))))
                        readStream.CopyTo(writeStream);
                }
            }

            return tempPath;
        }
    }
}