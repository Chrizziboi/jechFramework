using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Services
{

    /// <summary>
    /// Representerer feil som oppstår i tjenestelagene i applikasjonen.
    /// </summary>
    public class ServiceException : Exception
    {

        /// <summary>
        /// Initialiserer en ny instans av <see cref="ServiceException"/> klassen.
        /// </summary>
        public ServiceException() 
        { 

        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="ServiceException"/> klassen med en spesifisert feilmelding.
        /// </summary>
        /// <param name="message">Feilmeldingen som forklarer årsaken til unntaket.</param>
        public ServiceException(string message)
            : base(message) 
        {

        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="ServiceException"/> klassen med en spesifisert feilmelding og en referanse til den indre unntaket som er årsaken til dette unntaket.
        /// </summary>
        /// <param name="message">Feilmeldingen som forklarer årsaken til unntaket.</param>
        /// <param name="innerException">Unntaket som er årsaken til det nåværende unntaket, eller en nullreferanse (Nothing i Visual Basic) hvis ingen indre unntak er spesifisert.</param>
        public ServiceException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
