using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Test.NUnit
{
    internal class SentenceUtilsTest
    {
        [Test]
        public void Character()
        {
            Assert.That(SentenceUtils.ToTitleCase("a"),
                        Is.EqualTo("A"),
                        "Your function should convert a single character.");
            Assert.That(SentenceUtils.ToTitleCase("A"),
                        Is.EqualTo("A"),
                        "Your function should convert a single character.");
        }

        [Test]
        public void Word()
        {
            Assert.That(SentenceUtils.ToTitleCase("abc"),
                        Is.EqualTo("Abc"),
                        "Your function should convert a single word.");
            Assert.That(SentenceUtils.ToTitleCase("ABC"),
                        Is.EqualTo("Abc"),
                        "Your function should convert a single word.");
            Assert.That(SentenceUtils.ToTitleCase("aBC"),
                        Is.EqualTo("Abc"),
                        "Your function should convert a single word.");
            Assert.That(SentenceUtils.ToTitleCase("Abc"),
                        Is.EqualTo("Abc"),
                        "Your function should convert a single word.");
        }

        [Test]
        public void Sentence()
        {
            Assert.That(SentenceUtils.ToTitleCase("abc def ghi"),
                        Is.EqualTo("Abc Def Ghi"),
                        "Your function should convert a sentence.");
        }

        [Test]
        public void MultipleWhitespace()
        {
            Assert.That(SentenceUtils.ToTitleCase("  abc  def ghi "),
                        Is.EqualTo("  Abc  Def Ghi "),
                        "Your function should keep multiple whitespaces.");
        }


        [Theory]
        [TestCase("a", "A")]
        [TestCase("A", "A")]
        [TestCase("abc", "Abc")]
        [TestCase("ABC", "Abc")]
        [TestCase("AbC", "Abc")]
        [TestCase("abC", "Abc")]
        [TestCase("aBC", "Abc")]
        [TestCase("aBc def GhI", "Abc Def Ghi")]
        [TestCase("   aBc   def GhI  ", "   Abc   Def Ghi  ")]
        public void ToTitleCase_AnyInput_TitleCaseOutput(string input, string output)
        {
            //Act
            var result = SentenceUtils.ToTitleCase(input);

            //Assert
            Assert.That(result, Is.EqualTo(output));
        }
    }
}
