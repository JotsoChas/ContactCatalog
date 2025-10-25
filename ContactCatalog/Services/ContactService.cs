using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ContactCatalog.Models;
using ContactCatalog.Repositories;
using ContactCatalog.Validators;
using Microsoft.Extensions.Logging;

namespace ContactCatalog.Services
{
    public class ContactService
    {
        // Repository som håller datat i minnet
        private readonly IContactRepository _repository;

        // Logger används för att skriva info/varningar till konsolen (via LoggerFactory)
        private readonly ILogger<ContactService> _logger;

        // Enkel e-postvalidering (min egen validator)
        private readonly EmailValidator _emailValidator = new();

        public ContactService(IContactRepository repository, ILogger<ContactService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // Lägger till en ny kontakt
        // Viktigt här: jag låter den fortfarande skriva ut lite feedback till användaren
        // eftersom "Add Contact"-flödet i menyn känns bättre när man ser vad som händer.
        public void AddContact(Contact contact)
        {
            // Säkerställer att namnet alltid formateras korrekt
            contact.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(contact.Name.ToLower().Trim());

            Console.WriteLine($"Adding new contact: {contact.Name} ({contact.Email})\n");


            // Kontroll att fält inte är tomma
            if (string.IsNullOrWhiteSpace(contact.Name) || string.IsNullOrWhiteSpace(contact.Email))
            {
                _logger.LogWarning("Contact missing name or email. Name: {Name}, Email: {Email}", contact.Name, contact.Email);
                Console.WriteLine("Name and email cannot be empty.\n");
                throw new ArgumentException("Name and email cannot be empty.");
            }

            // Validera e-postformat
            if (!_emailValidator.IsValidEmail(contact.Email))
            {
                _logger.LogWarning("Invalid email format detected: {Email}", contact.Email);
                Console.WriteLine($"Invalid email format: {contact.Email}\n");
                throw new InvalidEmailException(contact.Email);
            }
            else
            {
                _logger.LogInformation("Email validated successfully for: {Email}", contact.Email);
                Console.WriteLine($"Email validated successfully for: {contact.Email}\n");
            }

            // Kolla om e-post redan finns
            var existingContact = _repository.GetByEmail(contact.Email);
            if (existingContact != null)
            {
                _logger.LogWarning("Duplicate email detected: {Email}", contact.Email);
                Console.WriteLine($"Duplicate email: {contact.Email}\n");
                throw new DuplicateEmailException(contact.Email);
            }

            // Lägg till i repository
            _repository.Add(contact);

            _logger.LogInformation("Contact added: {Name} ({Email}) with ID {Id}", contact.Name, contact.Email, contact.Id);
            Console.WriteLine($"Contact added successfully (ID: {contact.Id})\n");
        }

        // Hämtar alla kontakter (ingen utskrift här längre)
        public List<Contact> GetAllContacts()
        {
            var contacts = _repository.GetAll().ToList();

            if (contacts.Count == 0)
            {
                _logger.LogWarning("No contacts found when listing all.");
            }
            else
            {
                _logger.LogInformation("Listing {Count} contacts.", contacts.Count);
            }

            return contacts;
        }

        // Söker kontakter baserat på namn (ingen utskrift här)
        public List<Contact> SearchByName(string name)
        {
            _logger.LogInformation("Search started for name: {Name}", name);

            var results = _repository
                .GetAll()
                .Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (results.Count == 0)
            {
                _logger.LogWarning("No contacts found with name: {Name}", name);
            }

            return results;
        }

        // Filtrerar kontakter baserat på tagg (ingen utskrift här)
        // Jag använder LINQ Where + OrderBy så som kravet säger
        public List<Contact> FilterByTag(string tag)
        {
            _logger.LogInformation("Filtering started for tag: {Tag}", tag);

            var results = _repository
                .GetAll()
                .Where(c => c.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase))
                .OrderBy(c => c.Name)
                .ToList();

            if (results.Count == 0)
            {
                _logger.LogWarning("No contacts found with tag: {Tag}", tag);
            }
            else
            {
                _logger.LogInformation("{Count} contacts matched tag: {Tag}", results.Count, tag);
            }

            return results;
        }
    }
}
