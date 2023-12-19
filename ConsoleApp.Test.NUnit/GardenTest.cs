namespace ConsoleApp.Test.NUnit
{
    public class GardenTest
    {
        private Garden Garden { get; set; }

        [SetUp]
        public void SetUp()
        {
            Garden = new Garden(0);
        }

        [TearDown]
        public void Clean()
        {
            Garden = null;
        }



        [Test]
        public void Plant_NotNullNotEmptyName_True()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 1;
            const string NOT_NULL_NOT_EMPTY_NAME = "_";
            var garden = new Garden(MINIMAL_VALID_SIZE);

            //Act
            var result = garden.Plant(NOT_NULL_NOT_EMPTY_NAME);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Plant_GardenOverflow_False()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            const string NOT_NULL_NOT_EMPTY_NAME = "_";
            var garden = new Garden(MINIMAL_VALID_SIZE);

            //Act
            var result = garden.Plant(NOT_NULL_NOT_EMPTY_NAME);

            //Assert
            Assert.That(result, Is.False);
        }

        [Theory]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase("\t")]
        public void Plant_InvalidName_ArgumentException(string? invalidName)
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            var garden = new Garden(MINIMAL_VALID_SIZE);

            //Act
            TestDelegate action = () => garden.Plant(invalidName);

            //Assert
            Assert.Throws(Is.InstanceOf<ArgumentException>().And.Property("ParamName").EqualTo("name"), action);
        }

        [Test]
        public void Plant_NotNullNotEmptyName_AddedToCollection()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 1;
            const string NOT_NULL_NOT_EMPTY_NAME = "_";
            var garden = new Garden(MINIMAL_VALID_SIZE);

            //Act
            _ = garden.Plant(NOT_NULL_NOT_EMPTY_NAME);

            //Assert
            var plants = garden.GetPlants();
            Assert.That(plants, Does.Contain(NOT_NULL_NOT_EMPTY_NAME));
        }

        [Test]
        public void Plant_ExistingName_AddedSameItemCountToName()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 2;
            const string NOT_NULL_NOT_EMPTY_NAME = "_";
            var garden = new Garden(MINIMAL_VALID_SIZE);
            _ = garden.Plant(NOT_NULL_NOT_EMPTY_NAME);
            const string POST_NAME = NOT_NULL_NOT_EMPTY_NAME + "2";

            //Act
            _ = garden.Plant(NOT_NULL_NOT_EMPTY_NAME);

            //Assert
            var plants = garden.GetPlants();
            Assert.That(plants, Does.Contain(POST_NAME));
        }

        [Test]
        public void GetPlants_CopyOfPlantsCollection()
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = 0;
            var garden = new Garden(INSIGNIFICANT_SIZE);

            //Act
            var result1 = garden.GetPlants();
            var result2 = garden.GetPlants();

            //Assert
            Assert.That(result1, Is.Not.SameAs(result2));
        }

        [Theory]
        [TestCase(0)]
        [TestCase(10)]
        [TestCase(4)]
        public void Garden_ValidSize_SizeInitialization(int size)
        {
            //Arrange & Act
            var garden = new Garden(size);

            //Assert
            Assert.That(garden.Size, Is.EqualTo(size));
        }


        [Theory]
        //testujemy warunki brzegowe
        [TestCase(int.MinValue)]
        [TestCase(-1)]
        [TestCase(11)]
        [TestCase(int.MaxValue)]
        public void Garden_InvalidSize_ArgumentOutOfRangeException(int size)
        {
            //Arrange & Act
            TestDelegate action = () => new Garden(size);

            //Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(action);
            Assert.That(exception.ParamName, Is.EqualTo("size"));
        }
    }
}