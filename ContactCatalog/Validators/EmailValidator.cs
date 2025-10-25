using System.Text.RegularExpressions;

namespace ContactCatalog.Validators
{
    // Ansvarar för att kontrollera att epost är giltig
    public class EmailValidator
    {
        // Metod som returnerar true om epost innehåller ett @
        public bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, "@");
        }
    }
}
