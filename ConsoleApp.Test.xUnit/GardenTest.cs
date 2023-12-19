using AutoFixture;
using FluentAssertions;
using Moq;

namespace ConsoleApp.Test.xUnit
{
    public class GardenTest : IDisposable
    {

        //SetUp i TearDown uwa¿ane s¹ za z³¹ praktykê w testach jednostkowych
        //jeœli potrzebujemy podobnych rozwi¹zañ, powinniœmy skorzystaæ z metod prywatnych ujednolicaj¹cych powtarzaj¹cy siê kod inicjuj¹cy testy
        
        private Garden Garden { get; set; }

        //odpowiednik funkcjonalnoœci setup
        public GardenTest()
        {
            Garden = new Garden(0);
        }

        //odpowiednik funkcjonalnoœci teardown
        public void Dispose()
        {
            Garden = null;
        }



        [Fact]
        //public void MethodName_Scenario_Result()
        //public void MethodNameReturnValueWhenSthHappend()
        //public void Plant_PassingValidName_ReturnsTrue()
        //public void PlantGivesTrueWHenProvidedValidName()
        public void Plant_NotNullNotEmptyName_True()
        {
            //Arrange
            //u¿ywamy wartoœci z jak mniejmniejszym przekazem i opisujemy swoje intencje
            const int MINIMAL_VALID_SIZE = 1;
            const string NOT_NULL_NOT_EMPTY_NAME = "_";
            var garden = new Garden(MINIMAL_VALID_SIZE); 

            //Act
            var result = garden.Plant(NOT_NULL_NOT_EMPTY_NAME);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void Plant_GardenOverflow_False()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            const string NOT_NULL_NOT_EMPTY_NAME = "_";
            var garden = new Garden(MINIMAL_VALID_SIZE);

            //Act
            var result = garden.Plant(NOT_NULL_NOT_EMPTY_NAME);

            //Assert
            Assert.False(result);
        }

        /*[Fact]
        public void Plant_GardenOverflow_False()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 1;
            const string NOT_NULL_NOT_EMPTY_NAME = "_";
            var garden = new Garden(MINIMAL_VALID_SIZE);
            garden.Plant(NOT_NULL_NOT_EMPTY_NAME);

            //Act
            var result = garden.Plant(NOT_NULL_NOT_EMPTY_NAME);

            //Assert
            Assert.False(result);
        }*/

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\t")]
        public void Plant_InvalidName_ArgumentException(string? invalidName)
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            var garden = new Garden(MINIMAL_VALID_SIZE);

            //Act
            Action action = () => garden.Plant(invalidName);

            //Assert
            var exception = Assert.ThrowsAny<ArgumentException>(action); 
            Assert.Equal("name", exception.ParamName);
        }

        [Fact(Skip = "Replaced by Plant_InvalidName_ArgumentException")]
        public void Plant_NullName_ArgumentNullException()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            var garden = new Garden(MINIMAL_VALID_SIZE);
            const string? NULL_NAME = null;

            //Act
            Action action = () => garden.Plant(NULL_NAME);

            //Assert
            var exception = Assert.Throws<ArgumentNullException>(action); //throws sprawdza konkrenty wyj¹tek
            //Assert.ThrowsAny<ArgumentException>(action); //throwsAny sprawdza czy wyj¹tek jest z hierarchi dziedziczenia
            Assert.Equal("name", exception.ParamName);
        }

        [Fact(Skip = "Replaced by Plant_InvalidName_ArgumentException")]
        public void Plant_EmptyName_ArgumentException()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            var garden = new Garden(MINIMAL_VALID_SIZE);
            const string EMPTY_NAME = "";

            //Act
            var exception = Record.Exception(() => garden.Plant(EMPTY_NAME));

            //Assert
            Assert.NotNull(exception);
            var argumentException = Assert.IsType<ArgumentException>(exception);
            Assert.Equal("name", argumentException.ParamName);
            Assert.Contains(ConsoleApp.Properties.Resources.PlantNeedsName, argumentException.Message);
        }

        [Fact]
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
            Assert.Contains(NOT_NULL_NOT_EMPTY_NAME, plants);
        }

        [Fact]
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
            Assert.Contains(POST_NAME, plants);
        }

        [Fact]
        public void GetPlants_CopyOfPlantsCollection()
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = 0;
            var garden = new Garden(INSIGNIFICANT_SIZE);

            //Act
            var result1 = garden.GetPlants();
            var result2 = garden.GetPlants();

            //Assert
            Assert.NotSame(result1, result2); // NotSame/Same - sprawdza referencjê, a nie elementy na liœcie jak metoda Equal
        }

        [Theory]
        //sprzwdzamy warunki brzegowe
        [InlineData(0)]
        [InlineData(10)]
        //Jakaœ dodatkowa wartoœæ z wnêtrza przedzia³u
        [InlineData(4)]
        public void Garden_ValidSize_SizeInitialization(int size)
        {
            //Arrange & Act
            var garden = new Garden(size);

            //Assert
            Assert.Equal(size, garden.Size);
        }


        [Theory]
        //testujemy warunki brzegowe
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(11)]
        [InlineData(int.MaxValue)]
        public void Garden_InvalidSize_ArgumentOutOfRangeException(int size)
        {
            //Arrange & Act
            Action action = () => new Garden(size);

            //Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(action);
            Assert.Equal("size", exception.ParamName);
        }

        [Fact]
        public void Plant_NotNullNotEmptyName_MessageLogged()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 1;
            string NOT_NULL_NOT_EMPTY_NAME = new Fixture().Create<string>();
            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.Log(It.Is<string>(xx => xx.Contains(NOT_NULL_NOT_EMPTY_NAME)))).Verifiable();

            var garden = new Garden(MINIMAL_VALID_SIZE, loggerMock.Object);

            //Act
            garden.Plant(NOT_NULL_NOT_EMPTY_NAME);

            //Assert
            loggerMock.Verify();
        }

        [Fact]
        public void Plant_DuplicatedName_MessageLogged()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 2;
            string NOT_NULL_NOT_EMPTY_NAME = new Fixture().Create<string>();
            var loggerMock = new Mock<ILogger>();
            

            var garden = new Garden(MINIMAL_VALID_SIZE, loggerMock.Object);
            garden.Plant(NOT_NULL_NOT_EMPTY_NAME);

            //Act
            garden.Plant(NOT_NULL_NOT_EMPTY_NAME);

            //Assert
            loggerMock.Verify(x => x.Log(It.IsAny<string>()), Times.Exactly(3));
        }

        [Fact]
        public void GetLastLogFromLastHour_LastLog()
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = 0;
            var fixture = new Fixture();
            var logs = fixture.CreateMany<string>(2);

            var logger = new Mock<ILogger>();

            logger.Setup(x => x.GetLogsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(string.Join("\n", logs));
            var garden = new Garden(INSIGNIFICANT_SIZE, logger.Object);

            //Act
            var result = garden.GetLastLogFromLastHour();

            //Assert
            result.Should().Be(logs.Last());
        }
    }
}