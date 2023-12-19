using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ConsoleApp.Test.MSTest
{
    [TestClass]
    public class GardenTest
    {
        private Garden Garden { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            Garden = new Garden(0);
        }

        [TestCleanup]
        public void Clean()
        {
            Garden = null;
        }



        [TestMethod]
        public void Plant_NotNullNotEmptyName_True()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 1;
            const string NOT_NULL_NOT_EMPTY_NAME = "_";
            var garden = new Garden(MINIMAL_VALID_SIZE);

            //Act
            var result = garden.Plant(NOT_NULL_NOT_EMPTY_NAME);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Plant_GardenOverflow_False()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            const string NOT_NULL_NOT_EMPTY_NAME = "_";
            var garden = new Garden(MINIMAL_VALID_SIZE);

            //Act
            var result = garden.Plant(NOT_NULL_NOT_EMPTY_NAME);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\n")]
        [DataRow("\t")]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void Plant_InvalidName_ArgumentException(string? invalidName)
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            var garden = new Garden(MINIMAL_VALID_SIZE);

            //Act
            garden.Plant(invalidName);
        }

        [TestMethod]
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
            Assert.IsTrue(plants.Contains(NOT_NULL_NOT_EMPTY_NAME));
        }

        [TestMethod]
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
            Assert.IsTrue(plants.Contains(POST_NAME));
        }

        [TestMethod]
        public void GetPlants_CopyOfPlantsCollection()
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = 0;
            var garden = new Garden(INSIGNIFICANT_SIZE);

            //Act
            var result1 = garden.GetPlants();
            var result2 = garden.GetPlants();

            //Assert
            Assert.AreNotSame(result1, result2);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(10)]
        [DataRow(4)]
        public void Garden_ValidSize_SizeInitialization(int size)
        {
            //Arrange & Act
            var garden = new Garden(size);

            //Assert
            Assert.AreEqual(size, garden.Size);
        }


        [TestMethod]
        //testujemy warunki brzegowe
        [DataRow(int.MinValue)]
        [DataRow(-1)]
        [DataRow(11)]
        [DataRow(int.MaxValue)]
        public void Garden_InvalidSize_ArgumentOutOfRangeException(int size)
        {
            Action action = () => new Garden(size);

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(action);
            Assert.AreEqual("size", exception.ParamName);
        }
    }
}