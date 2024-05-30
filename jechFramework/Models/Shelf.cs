using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    /// <summary>
    /// Representerer en hylle i et lager.
    /// </summary>
    public class Shelf
    {

        private static int nextId = 1; // Statisk variabel for å holde styr på neste ID

        /// <summary>
        /// Henter eller setter lengden på hyllen.
        /// </summary>
        public int shelfId { get; set; }

        /// <summary>
        /// Henter eller setter lengden på hyllen.
        /// </summary>
        public int length { get; set; }

        /// <summary>
        /// Henter eller setter dybden på hyllen.
        /// </summary>
        public int depth { get; set; }

        /// <summary>
        /// Henter eller setter pallekapasiteten til hyllen.
        /// </summary>
        public int palletCapacity { get; set; }

        /// <summary>
        /// Henter eller setter antall etasjer på hyllen.
        /// </summary>
        public int floors { get; set; } // Etasjer på reolen, kan være 0 hvis ikke spesifisert

        /// <summary>
        /// Initialiserer en ny instans av <see cref="Shelf"/> klassen.
        /// </summary>
        public Shelf() 
        {
        
        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="Shelf"/> klassen med spesifiserte parametere inkludert etasjer.
        /// </summary>
        /// <param name="length">Lengden på hyllen.</param>
        /// <param name="depth">Dybden på hyllen.</param>
        /// <param name="palletCapacity">Pallekapasiteten til hyllen.</param>
        /// <param name="floors">Antall etasjer på hyllen.</param>
        public Shelf(int length, int depth, int palletCapacity, int floors)
        {
            shelfId = nextId++;
            this.length = length;
            this.depth = depth;
            this.palletCapacity = palletCapacity;
            this.floors = floors;
        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="Shelf"/> klassen med spesifiserte parametere uten etasjer.
        /// </summary>
        /// <param name="length">Lengden på hyllen.</param>
        /// <param name="depth">Dybden på hyllen.</param>
        /// <param name="palletCapacity">Pallekapasiteten til hyllen.</param>
        public Shelf(int length, int depth, int palletCapacity)
        {
            shelfId = nextId++; 
            this.length = length;
            this.depth = depth;
            this.palletCapacity = palletCapacity;
            floors = 0;
        }
    }
}