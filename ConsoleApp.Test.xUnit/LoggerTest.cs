using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Test.xUnit
{
    public class LoggerTest
    {
        [Fact]
        public void Log_AnyMessage_EventInvoked()
        {
            //Arrange
            var logger = new Logger();
            const string ANY_MESSAGE = "_";
            bool result = false;
            logger.MessageLogged += (sender, args) => { result = true; };

            //Act
            logger.Log(ANY_MESSAGE);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void Log_AnyMessage_ValidEventInvoked()
        {
            //Arrange
            var logger = new Logger();
            const string ANY_MESSAGE = "_";
            object? eventSender = null;
            Logger.LoggerEventArgs? eventArgs = null;
            logger.MessageLogged += (sender, args) => { eventSender = sender; eventArgs = args as Logger.LoggerEventArgs;};
            var timeStart = DateTime.Now;

            //Act
            logger.Log(ANY_MESSAGE);

            //Assert
            var timeStop = DateTime.Now;
            Assert.Equal(logger, eventSender);
            Assert.NotNull(eventArgs);
            Assert.Equal(ANY_MESSAGE, eventArgs.Message);
            Assert.InRange(eventArgs.DateTime, timeStart, timeStop);
        }
    }
}
