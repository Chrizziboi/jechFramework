using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    public class Shelf
    {
        private static int nextId = 1; // Statisk variabel for å holde styr på neste ID
        public int shelfId { get; set; } // Endret til int for sekvensiell ID
        public int length { get; set; }
        public int depth { get; set; }
        public int palletCapacity { get; set; }
        public int floors { get; set; } // Etasjer på reolen, kan være 0 hvis ikke spesifisert

        public List<Pallet> palletList = new();

        public Shelf() 
        {
        
        }

        // Konstruktør som forventer etasjer
        public Shelf(int length, int depth, int palletCapacity, int floors)
        {
            shelfId = nextId++; // Generer en unik ID når en ny instans opprettes
            this.length = length;
            this.depth = depth;
            this.palletCapacity = palletCapacity;
            this.floors = floors;
        }

        // Konstruktør uten etasjer, setter standardverdi for Floors til 0
        public Shelf(int length, int depth, int palletCapacity)
        {
            shelfId = nextId++; // Generer en unik ID her også
            this.length = length;
            this.depth = depth;
            this.palletCapacity = palletCapacity;
            floors = 0; // Standardverdi, indikerer at etasjedetaljer ikke er spesifisert
        }
    }
}