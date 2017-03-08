using DiscordBotJarvis.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiscordBotJarvis.Tests.TextRecognitionModule.Extensions
{
    [TestClass]
    public class StringExtensionUnitTests
    {
        [TestMethod]
        public void AddWhiteSpaceAroundString_Sentence()
        {
            // Arrange
            string actual = "This is a sentence.";
            string expected = " This is a sentence. ";

            // Act
            actual = actual.AddWhiteSpaceAroundString();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReplaceSpecialsChar()
        {
            // Arrange
            string actual = "How-are-you ?";
            string expected = "How are you ?";

            // Act
            actual = actual.ReplaceSpecialsChar();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveDiacritics()
        {
            // Arrange
            string actual = "ÀÁÂÃÄÅÇÑñÇçÈÉÊËÌÍÎÏÒÓÔÕÖØÙÚÛÜÝàáâãäåçèéêëìíîïðòóôõöøùúûüýÿ";
            string expected = "AAAAAACNnCcEEEEIIIIOOOOOØUUUUYaaaaaaceeeeiiiiðoooooøuuuuyy";

            // Act
            actual = actual.RemoveDiacritics();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ProcessingUserRequest_Sentence_001()
        {
            // Arrange
            string actual = "This is a sentence.";
            string expected = " this is a sentence. ";

            // Act
            actual = actual.NormalizeUserQuery();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ProcessingUserRequest_Sentence_002()
        {
            // Arrange
            string actual = "   Aenean tempus donéc éléfantid euismod nequé, tristique libéro porté maécènas convallis, " 
                + "facîlisis malesuada convallis. Euismod blandit scelerisque pulvinar donéc dictumst hac in sociosqu ïn "
                + "rhoncüs, variûs adipiscing éu sit elementum. ";
            string expected = " aenean tempus donec elefantid euismod neque, tristique libero porte maecenas convallis, "
                + "facilisis malesuada convallis. euismod blandit scelerisque pulvinar donec dictumst hac in sociosqu in "
                + "rhoncus, varius adipiscing eu sit elementum. ";

            // Act
            actual = actual.NormalizeUserQuery();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ProcessingUserRequest_Sentence_003()
        {
            // Arrange
            string actual = @"Sociosqu commodoé in l'suspendisse mattis massè cél a magnès, mauris j'duis pélléntésque "
                + "diam magnès pérès curàbitur donec, fusce ùrci diam massa vehicula elementum tùrpus non, velit dictumst pésuéré. "
                + "Suscipit torquent per donéc mattis tellus aptenté cél dès, duis dolor dès dui juséo porta ùrci curabitur, hâc "
                + "nullä tùrpus venesatis egestas ét ïd justo.";
            string expected = " sociosqu commodoe in l'suspendisse mattis masse cel a magnes, mauris j'duis pellentesque "
                + "diam magnes peres curabitur donec, fusce urci diam massa vehicula elementum turpus non, velit dictumst pesuere. "
                + "suscipit torquent per donec mattis tellus aptente cel des, duis dolor des dui juseo porta urci curabitur, hac "
                + "nulla turpus venesatis egestas et id justo. ";

            // Act
            actual = actual.NormalizeUserQuery();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
