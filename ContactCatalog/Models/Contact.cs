using System;
using System.Collections.Generic;

namespace ContactCatalog.Models
{
    // Klassen representerar en enskild kontakt
    public class Contact
    {
        // Statisk räknare som ökar automatiskt vid varje ny kontakt
        private static int _nextId = 1;

        // Varje kontakt får ett unikt ID
        public int Id { get; private set; }

        // Namn, e-post och taggar
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Tags { get; set; } = new();

        // När en kontakt skapas får den automatiskt nästa ID
        public Contact()
        {
            Id = _nextId++;
        }
    }
}
