using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    public class Shelf
    {
        public Guid ShelfId { get; private set; } // Unik identifikator for reolen
        public int length { get; set; }
        public int depth { get; set; }
        public int capacity { get; set; } // Antall paller reolen kan inneholde
        public int floors { get; set; } // Etasjer på reolen, kan være 0 hvis ikke spesifisert

        // Konstruktør som forventer etasjer
        public Shelf(int length, int depth, int capacity, int floors)
        {
            ShelfId = Guid.NewGuid(); // Generer en unik ID når en ny instans opprettes
            this.length = length;
            this.depth = depth;
            this.capacity = capacity;
            this.floors = floors;
        }

        // Konstruktør uten etasjer, setter standardverdi for Floors til 0
        public Shelf(int length, int depth, int capacity)
        {
            ShelfId = Guid.NewGuid(); // Generer en unik ID her også
            this.length = length;
            this.depth = depth;
            this.capacity = capacity;
            floors = 0; // Standardverdi, indikerer at etasjedetaljer ikke er spesifisert
        }
    }
}