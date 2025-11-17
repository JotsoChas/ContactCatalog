using System.Collections.Generic;
using System.Linq;
using ContactCatalog.Models;

namespace ContactCatalog.Repositories
{
    public class InMemoryContactRepository : IContactRepository
    {
        // Lista som fungerar som vår tillfälliga databas
        private readonly Dictionary<int, Contact> _contacts = new();

        private int _nextId = 1;

        // Lägger till en ny kontakt i listan
        public void Add(Contact contact)
        {   // Sätter Id endast här när kontakten faktiskt sparas
            contact.Id = _nextId++;
            _contacts[contact.Id] = contact;
        }

        // Hämtar alla kontakter som finns i listan
        public IEnumerable<Contact> GetAll()
        {
            return _contacts.Values;
        }

        // Hämtar en kontakt via e-post, för att undvika dubbletter
        public Contact GetByEmail(string email)
        {
            // Returnerar första matchningen eller null om ingen hittas
            return _contacts.Values
            .FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

    }
}
