using System;

namespace ContactCatalog.Validators
{
    // Om en kontakt med samma e-post redan finns
    public class DuplicateEmailException : Exception
    {
        // Sparar den e-postadress som orsakade felet
        public string DuplicateEmail { get; }

        // Skickar vidare ett tydligt felmeddelande till användaren
        public DuplicateEmailException(string email)
            : base($"The email address - {email} - is already registered.")
        {
            DuplicateEmail = email;
        }
    }
}
