using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DiscordBotJarvis.Helpers;

namespace DiscordBotJarvis.Tests.TextRecognitionModule.Helpers
{
    [TestClass]
    public class GetSentenceHelperUnitTests
    {
        public string PathFile { get; set; }

        [TestInitialize]
        public void Init()
        {
            PathFile = @"..\..\..\DiscordBotJarvis\ResourcePacks\Exemple_fr-FR\Resources\SayGoodbye.txt";
        }

        [TestMethod]
        public void ReadFile_T001()
        {
            // Arrange
            string actual;
            StringBuilder expected = new StringBuilder();
            expected = expected.AppendLine("Bonne nuit {0} !").AppendLine("Au revoir {0} !").Append("A plus {0} !");

            // Act
            actual = GetSentenceHelper.ReadFile(PathFile);

            // Assert
            Assert.AreEqual(expected.ToString(), actual);
        }
    }
}
