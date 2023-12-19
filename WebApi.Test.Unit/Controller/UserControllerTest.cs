using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;

namespace WebApi.Test.Unit.Controller
{
    public class UserControllerTest
    {

        [Fact]
        public async Task Get_OkWithAllUsers()
        {
            //Arrage
            var service = new Mock<ICrudService<User>>();
            var expedtedList = new Fixture().CreateMany<User>();
            service.Setup(x => x.ReadAsync()).ReturnsAsync(expedtedList);
            var controller = new UsersController(service.Object);

            //Act
            var result = await controller.Get();

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var resultList = Assert.IsAssignableFrom<IEnumerable<User>>(actionResult.Value);
            Assert.Equal(expedtedList, resultList);
        }

        [Fact]
        public async Task Get_ExistingId_OkWithUser()
        {
            //Arrage
            var service = new Mock<ICrudService<User>>();
            var expectedUser = new Fixture().Create<User>();
            service.Setup(x => x.ReadAsync(expectedUser.Id)).ReturnsAsync(expectedUser);
            var controller = new UsersController(service.Object);

            //Act
            var result = await controller.Get(expectedUser.Id);


            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var resultUser = Assert.IsAssignableFrom<User>(actionResult.Value);
            Assert.Equal(expectedUser, resultUser);
        }

        [Fact]
        public Task Get_NotExistingId_NotFound()
        {
            return ReturnsNotFound((controller, id) => controller.Get(id));

            /*//Arrage
            var service = new Mock<ICrudService<User>>();
            var controller = new UsersController(service.Object);
            var id = new Fixture().Create<int>();

            //Act
            var result = await controller.Get(id);

            //Assert
            Assert.IsType<NotFoundResult>(result);*/
        }

        [Fact]
        public async Task Delete_ExistingId_NoContent()
        {
            //Arrage
            var service = new Mock<ICrudService<User>>();
            var expectedUser = new Fixture().Create<User>();
            service.Setup(x => x.ReadAsync(expectedUser.Id)).ReturnsAsync(expectedUser);
            service.Setup(x => x.DeleteAsync(expectedUser.Id)).Verifiable();
            var controller = new UsersController(service.Object);

            //Act
            var result =  await controller.Delete(expectedUser.Id);

            //
            Assert.IsType<NoContentResult>(result);
            service.Verify();
        }

        [Fact]
        public Task Delete_NotExistingId_NotFound()
        {
            return ReturnsNotFound((controller, id) => controller.Delete(id));

            /*//Arrage
            var service = new Mock<ICrudService<User>>();
            var controller = new UsersController(service.Object);
            var id = new Fixture().Create<int>();

            //Act
            var result = await controller.Delete(id);

            //Assert
            Assert.IsType<NotFoundResult>(result);*/
        }


        private static async Task ReturnsNotFound(Func<UsersController, int, Task<IActionResult>> funcAsync)
        {
            //Arrage
            var service = new Mock<ICrudService<User>>();
            int id = new Fixture().Create<int>();
            var controller = new UsersController(service.Object);

            //Act
            var result = await funcAsync(controller, id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public Task Put_NotExistingId_NotFound()
        {
            return ReturnsNotFound((controller, id) => controller.Put(id, new Fixture().Create<User>()));
        }
    }
}
