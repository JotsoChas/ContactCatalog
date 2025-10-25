using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactCatalog.Validators
{
    public class InvalidEmailException : Exception // Eget felmeddelande för ogiltig e-post
    {
        public InvalidEmailException(string email)
            : base($"E-postadressen - {email} - är inte giltig.") // Visar vilket värde som var fel
        {
        }
    }

}
