using System;
using System.Collections.Generic;

namespace ContactCatalog.Models
{
    // Klassen representerar en enskild kontakt
    public class Contact
    {
        public int Id { get; set; }

        // Namn, e-post och taggar
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Tags { get; set; } = new();

       
    }
}
