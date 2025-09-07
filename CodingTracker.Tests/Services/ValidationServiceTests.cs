using Xunit;
using Moq;
using CodingTracker.empty_codes.Services;

namespace CodingTracker.Tests.Services
{
    public class ValidationServiceTests
    {
        [Fact]
        public void IsMenuChoiceValid_InvalidThenValid_ReturnsChoice()
        {
            //Arrange of AAA
            var mockConsole = new Mock<IConsoleService>();
            mockConsole.SetupSequence(c => c.ReadLine())
                    .Returns("abc")  // invalid input
                    .Returns("5");   // valid input

            var validationService = new ValidationService(mockConsole.Object);

            // Act
            var result = validationService.IsMenuChoiceValid(1, 8);

            //Assert
            Assert.Equal(5, result);
            // returns abc first to simulate bad user input and confirm that the validation service
            // correctly handles invalid input before eventually accepting valid input
        }

        [Fact]
        public void IsDateValid_InvalidThenValid_ReturnsValidDate()
        {
            //Arrange
            var mockConsole = new Mock<IConsoleService>();
            mockConsole.SetupSequence(c => c.ReadLine())
               .Returns("01-01-2025")   // invalid format
               .Returns("2025-01-01 14:30"); // valid format

            var validationService = new ValidationService(mockConsole.Object, "yyyy-MM-dd HH:mm");

            //Act
            var result = validationService.IsDateValid("01-01-2025");

            // Assert
            Assert.Equal(new DateTime(2025, 1, 1, 14, 30, 0), result);
            mockConsole.Verify(c => c.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void IsEndDateValid_EndBeforeStart_ReturnsFalse()
        {
            //Arrange
            var mockConsole = new Mock<IConsoleService>();
            var validationService = new ValidationService(mockConsole.Object);

            var start = new DateTime(2025, 1, 1); 
            var end = new DateTime(2024, 12, 31);

            //Act
            var result = validationService.IsEndDateValid(start, end);

            //Assert
            Assert.False(result);
            mockConsole.Verify(c => c.WriteLine(It.IsAny<string>()), Times.Once);
        }
    }
}
