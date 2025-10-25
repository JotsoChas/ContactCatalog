using ContactCatalog.Models;
using ContactCatalog.Repositories;
using ContactCatalog.Services;
using Microsoft.Extensions.Logging;
using System.Globalization;


namespace ContactCatalog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Skapar logger-fabrik som skriver till konsolen
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            // Skapar logger specifikt för ContactService
            ILogger<ContactService> logger = loggerFactory.CreateLogger<ContactService>();

            // Skapar vårt in-memory repository och skickar in det i tjänsten
            var repository = new InMemoryContactRepository();
            var service = new ContactService(repository, logger);

            RunMenu(service);
        }

        private static void RunMenu(ContactService service)
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("|=======================|");
                Console.WriteLine("|    CONTACT CATALOG    |");
                Console.WriteLine("|=======================|\n");
                Console.ResetColor();

                Console.WriteLine("1. Add Contact");
                Console.WriteLine("2. List Contacts");
                Console.WriteLine("3. Search by Name");
                Console.WriteLine("4. Filter by Tag");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("0. Exit\n");
                Console.ResetColor();

                Console.Write("Select option: ");
                string? choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        AddContact(service);
                        break;
                    case "2":
                        ListContacts(service);
                        break;
                    case "3":
                        SearchContacts(service);
                        break;
                    case "4":
                        FilterContacts(service);
                        break;
                    case "0":
                        Console.Clear();
                        Console.WriteLine("Exiting program...\n");
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.\n");
                        break;
                }

                if (isRunning)
                {
                    Console.WriteLine("Press any key to return to menu...");
                    Console.ReadKey();
                }
            }
        }

        private static void AddContact(ContactService service)
        {
            Console.Clear();
            Console.WriteLine("|=== ADD CONTACT ===|\n");

            var contact = new Contact();

            Console.Write("Enter name: ");
            string inputName = Console.ReadLine() ?? "";

            // Gör första bokstaven i varje ord stor
            contact.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(inputName.ToLower().Trim());

            Console.Write("Enter email: ");
            contact.Email = Console.ReadLine() ?? "";

            Console.WriteLine("\nChoose tag:");
            Console.WriteLine("1. Family");
            Console.WriteLine("2. Classmate");
            Console.WriteLine("3. Coworker");
            Console.WriteLine("4. Friend");
            Console.WriteLine("5. Other\n");
            Console.Write("Select number: ");
            string tagChoice = Console.ReadLine() ?? "";

            string selectedTag = tagChoice switch
            {
                "1" => "Family",
                "2" => "Classmate",
                "3" => "Coworker",
                "4" => "Friend",
                _ => "Other"    //allt utanför 1-4 blir other
            };

            contact.Tags = new List<string> { selectedTag };

            try
            {
                Console.Clear();
                Console.WriteLine("|=== ADD CONTACT ===|\n");

                // Låter ContactService göra validering och spara + skriva ut lite status
                service.AddContact(contact);

                Console.WriteLine($"Contact added (ID: {contact.Id})\n");
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("|=== ADD CONTACT ===|\n");
                Console.WriteLine($"Error: {ex.Message}\n");
            }
        }

        private static void ListContacts(ContactService service)
        {
            Console.Clear();
            Console.WriteLine("|=== ALL CONTACTS ===|\n");

            var contacts = service.GetAllContacts();

            if (contacts.Count == 0)
            {
                Console.WriteLine("No contacts found.\n");
                return;
            }

            foreach (var c in contacts)
            {
                Console.WriteLine($"[{c.Id}] {c.Name} - {c.Email} - Tags: {string.Join(", ", c.Tags)}\n");
            }

            Console.WriteLine($"Total contacts: {contacts.Count}\n");
        }

        private static void SearchContacts(ContactService service)
        {
            Console.Clear();
            Console.WriteLine("|=== SEARCH CONTACTS ===|\n");

            Console.Write("Enter name to search: ");
            string search = Console.ReadLine() ?? "";

            // Rensa och visa resultat
            Console.Clear();
            Console.WriteLine($"|=== SEARCH RESULTS FOR: {search.ToUpper()} ===|\n");

            var results = service.SearchByName(search);

            if (results.Count == 0)
            {
                Console.WriteLine("No contacts found.\n");
                return;
            }

            foreach (var c in results)
            {
                Console.WriteLine($"[{c.Id}] {c.Name} - {c.Email} - Tags: {string.Join(", ", c.Tags)}\n");
            }

            Console.WriteLine($"Found {results.Count} contact(s).\n");
        }

        private static void FilterContacts(ContactService service)
        {
            Console.Clear();
            Console.WriteLine("|=== FILTER CONTACTS ===|\n");

            Console.WriteLine("1. Family");
            Console.WriteLine("2. Classmate");
            Console.WriteLine("3. Coworker");
            Console.WriteLine("4. Friend");
            Console.WriteLine("5. Other\n");
            Console.Write("Select number: ");
            string tagChoice = Console.ReadLine() ?? "";

            string selectedTag = tagChoice switch
            {
                "1" => "Family",
                "2" => "Classmate",
                "3" => "Coworker",
                "4" => "Friend",
                _ => "Other"    ////allt utanför 1-4 blir other
            };

            // Rensa och visa resultat
            Console.Clear();
            Console.WriteLine($"|=== CONTACTS WITH TAG: {selectedTag.ToUpper()} ===|\n");

            var results = service.FilterByTag(selectedTag);

            if (results.Count == 0)
            {
                Console.WriteLine("No contacts found with that tag.\n");
                return;
            }

            foreach (var c in results)
            {
                Console.WriteLine($"[{c.Id}] {c.Name} - {c.Email} - Tags: {string.Join(", ", c.Tags)}\n");
            }

            Console.WriteLine($"Found {results.Count} contact with tag - {selectedTag}.\n");
        }
    }
}
