namespace ConsoleApp.Test.xUnit
{
    public class GardenTest
    {
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

        [Fact]
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

        [Fact]
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
    }
}