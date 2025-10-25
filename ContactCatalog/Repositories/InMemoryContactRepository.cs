using System.Collections.Generic;
using System.Linq;
using ContactCatalog.Models;

namespace ContactCatalog.Repositories
{
    public class InMemoryContactRepository : IContactRepository
    {
        // Lista som fungerar som vår tillfälliga databas
        private readonly List<Contact> _contacts = new();

        // Lägger till en ny kontakt i listan
        public void Add(Contact contact)
        {
            _contacts.Add(contact);
        }

        // Hämtar alla kontakter som finns i listan
        public IEnumerable<Contact> GetAll()
        {
            return _contacts;
        }

        // Hämtar en kontakt via e-post, för att undvika dubbletter
        public Contact GetByEmail(string email)
        {
            // Returnerar första matchningen eller null om ingen hittas
            return _contacts.FirstOrDefault(c => c.Email.Equals(email, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}
