using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            var fixture = new Fixture();
            var logger = new Logger();
            string ANY_MESSAGE = fixture.Create<string>();
            bool result = false;
            logger.MessageLogged += (sender, args) => { result = true; };

            //Act
            logger.Log(ANY_MESSAGE);

            //Assert
            //Assert.True(result);
            result.Should().BeTrue();
        }

        [Fact]
        public void Log_AnyMessage_ValidEventInvoked()
        {
            //Arrange
            var logger = new Logger();
            string ANY_MESSAGE = new Fixture().Create<string>();
            object? eventSender = null;
            Logger.LoggerEventArgs? eventArgs = null;
            logger.MessageLogged += (sender, args) => { eventSender = sender; eventArgs = args as Logger.LoggerEventArgs;};
            var timeStart = DateTime.Now;

            //Act
            logger.Log(ANY_MESSAGE);

            //Assert
            var timeStop = DateTime.Now;
            /*Assert.Equal(logger, eventSender);
            Assert.NotNull(eventArgs);
            Assert.Equal(ANY_MESSAGE, eventArgs.Message);
            Assert.InRange(eventArgs.DateTime, timeStart, timeStop);*/

            using (new AssertionScope())
            {
                eventSender.Should().Be(logger);
                eventArgs.Should().NotBeNull();
                eventArgs.DateTime.Should().BeAfter(timeStart).And.BeBefore(timeStop);
                eventArgs.Message.Should().Be(ANY_MESSAGE);
            }
        }

        [Fact]
        public void Log_AnyMessage_ValidEventInvoked_WithMonitor()
        {
            //Arrange
            var logger = new Logger();
            string ANY_MESSAGE = new Fixture().Create<string>();
            using var monitor =  logger.Monitor();

            //Act
            logger.Log(ANY_MESSAGE);

            //Assert

            monitor.Should().Raise(nameof(Logger.MessageLogged))
                .WithSender(logger)
                .WithArgs<Logger.LoggerEventArgs>();
        }


        [Theory]
        [InlineData(0)]
        [InlineData(60)]
        public async void GetLogAsync_DateRange_LoggedMessage(int offsetSec)
        {
            //Arrange
            var logger = new Logger();
            string ANY_MESSAGE = new Fixture().Create<string>();
            DateTime RANGE_FROM = DateTime.Now.AddSeconds(-offsetSec);
            logger.Log(ANY_MESSAGE);
            DateTime RANGE_TO = DateTime.Now.AddSeconds(offsetSec);
            //bool isCompleted = false;
            //bool isCancelled = true;
            //string? result = null;

            //Act
            //var result = await logger.GetLogsAsync(RANGE_FROM, RANGE_TO);

            /*await logger.GetLogsAsync(RANGE_FROM, RANGE_TO)
                .ContinueWith(x => 
            { 
                isCompleted = x.IsCompleted;
                isCancelled = x.IsCanceled;
                result = x.Result;
            });*/

            var task = logger.GetLogsAsync(RANGE_FROM, RANGE_TO); ;
            await task;


            //Assert
            /*Assert.True(isCompleted);
            Assert.False(isCancelled);
            Assert.Contains(ANY_MESSAGE, result);
            Assert.True(DateTime.TryParseExact(result.Split(": ")[0], "dd.MM.yyyy hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
*/
            using (new AssertionScope())
            {
                task.IsCompletedSuccessfully.Should().BeTrue();
                task.Result.Should().Contain(ANY_MESSAGE);
                DateTime.TryParseExact(task.Result.Split(": ")[0], "dd.MM.yyyy hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _).Should().BeTrue();
            }
        }
    }
}
