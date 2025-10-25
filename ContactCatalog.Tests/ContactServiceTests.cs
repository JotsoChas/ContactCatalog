using ContactCatalog.Models;
using ContactCatalog.Repositories;
using ContactCatalog.Services;
using ContactCatalog.Validators;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ContactCatalog.Tests
{
    public class ContactServiceTests
    {
        [Fact]
        public void AddContact_DuplicateEmail_ThrowsDuplicateEmailException()
        {
            // Arrange
            var mockRepo = new Mock<IContactRepository>();
            var logger = new Mock<ILogger<ContactService>>();

            // Skapar en kontakt som redan finns
            var existing = new Contact { Name = "Anna", Email = "test@example.com" };
            mockRepo.Setup(r => r.GetAll()).Returns(new List<Contact> { existing });
            mockRepo.Setup(r => r.GetByEmail(It.IsAny<string>())).Returns(existing); //gör att mocken låtsas att en kontakt redan finns, så testet kan upptäcka dubbletter


            var service = new ContactService(mockRepo.Object, logger.Object);
            var newContact = new Contact { Name = "New", Email = "test@example.com" };

            // Act & Assert
            Assert.Throws<DuplicateEmailException>(() => service.AddContact(newContact));
        }


        [Fact]
        public void AddContact_ValidEmail_AddsSuccessfully()
        {
            // Arrange
            var mockRepo = new Mock<IContactRepository>();
            var logger = new Mock<ILogger<ContactService>>();

            mockRepo.Setup(r => r.GetAll()).Returns(new List<Contact>());
            var service = new ContactService(mockRepo.Object, logger.Object);

            var newContact = new Contact { Name = "Test", Email = "unique@example.com" };

            // Act
            service.AddContact(newContact);

            // Assert
            mockRepo.Verify(r => r.Add(It.Is<Contact>(c => c.Email == "unique@example.com")), Times.Once);
        }

        [Fact]
        public void FilterByTag_ReturnsCorrectContacts()
        {
            // Arrange
            var mockRepo = new Mock<IContactRepository>();
            var logger = new Mock<ILogger<ContactService>>();

            // Skapar tre kontakter med olika taggar
            var contacts = new List<Contact>
            {
                new Contact { Name = "Anna", Email = "anna@test.com", Tags = new() { "Friend" } },
                new Contact { Name = "Bertil", Email = "bertil@test.com", Tags = new() { "Coworker" } },
                new Contact { Name = "Cecilia", Email = "cecilia@test.com", Tags = new() { "Friend" } }
            };

            // Mockar GetAll så den returnerar listan ovan
            mockRepo.Setup(r => r.GetAll()).Returns(contacts);

            var service = new ContactService(mockRepo.Object, logger.Object);

            // Act – filtrerar på taggen "Friend"
            var result = service.FilterByTag("Friend");

            // Assert – kontrollerar att rätt kontakter hittas
            Assert.Equal(2, result.Count);
            Assert.All(result, c => Assert.Contains("Friend", c.Tags));
        }


    }
}
