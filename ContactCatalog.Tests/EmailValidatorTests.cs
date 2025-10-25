using ContactCatalog.Validators;
using Xunit;

namespace ContactCatalog.Tests
{
    public class EmailValidatorTests
    {
        [Theory]
        [InlineData("test@example.com", true)]
        [InlineData("joco.borghol@ica.se", true)]
        [InlineData("invalid-email", false)]
        [InlineData("", false)]
        public void IsValidEmail_ReturnsExpectedResult(string email, bool expected)
        {
            // Arrange
            var validator = new EmailValidator();

            // Act
            var result = validator.IsValidEmail(email);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
