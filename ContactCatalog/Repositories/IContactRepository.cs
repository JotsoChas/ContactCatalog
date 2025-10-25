using System.Collections.Generic;
using ContactCatalog.Models;

namespace ContactCatalog.Repositories
{
    // Interface som definierar vilka metoder ett kontakt repository ska innehålla.
    // Genom att använda interface blir koden lättare att testa och byta ut vid behov.
    public interface IContactRepository
    {
        // Lägger till en ny kontakt i lagringen
        void Add(Contact contact);

        // Hämtar alla kontakter som finns
        IEnumerable<Contact> GetAll();

        // Hämtar en kontakt via e-post (används för att undvika dubbletter)
        Contact GetByEmail(string email);


    }
}
