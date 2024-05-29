using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    /// <summary>
    /// Dette er en klasse for alle ansatte.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Henter eller setter ID for den ansatte.
        /// </summary>
        public int employeeId  { get; set; }

        /// <summary>
        /// Henter eller setter navnet til den ansatte.
        /// </summary>
        public string employeeName { get; set; }

        /// <summary>
        /// Henter eller setter en beskrivelse av den ansatte.
        /// </summary>
        public string employeeDescription { get; set; }

        /// <summary>
        /// Henter eller setter alderen til den ansatte.
        /// </summary>
        public int employeeAge { get; set; }

        /// <summary>
        /// Henter eller setter den personlige ID-en til den ansatte.
        /// </summary>
        public int employeePersonalId { get; set; }

        /// <summary>
        /// Henter eller setter adressen til den ansatte.
        /// </summary>
        public string employeeAddress { get; set; }

        /// <summary>
        /// Henter eller setter byen til den ansatte.
        /// </summary>
        public string employeeCity { get; set; }

        /// <summary>
        /// Henter eller setter telefonnummeret til den ansatte.
        /// </summary>
        public string employeeTelephoneNumber { get; set; }

        /// <summary>
        /// Henter eller setter en verdi som indikerer om den ansatte har autorisasjon til høyverdi varer.
        /// </summary>
        public bool employeeAuthorizationToHighValueGoods { get; set; } = false;

        /// <summary>
        /// Initialiserer en ny instans av Employee-klassen med to parametere.
        /// </summary>
        /// <param name="employeeId">ID for den ansatte.</param>
        /// <param name="employeeName">Navnet til den ansatte.</param>
        public Employee(int employeeId, string employeeName)
        {
            this.employeeId = employeeId;
            this.employeeName = employeeName;
            
        }

        /// <summary>
        /// Initialiserer en ny instans av Employee-klassen med alle parametere.
        /// </summary>
        /// <param name="employeeId">ID for den ansatte.</param>
        /// <param name="employeeName">Navnet til den ansatte.</param>
        /// <param name="employeeDescription">En beskrivelse av den ansatte, for eksempel at den ansatte er vikar.</param>
        /// <param name="employeeAge">Alderen til den ansatte.</param>
        /// <param name="employeePersonalId">Den personlige ID-en til den ansatte.</param>
        /// <param name="employeeAddress">Adressen til den ansatte.</param>
        /// <param name="employeeCity">Byen til den ansatte.</param>
        /// <param name="employeeTelephoneNumber">Telefonnummeret til den ansatte.</param>
        public Employee(
            int employeeId, 
            string employeeName, 
            string employeeDescription,
            int employeeAge, 
            int empployeePersonalId, 
            string employeeAddress, 
            string employeeCity,
            string employeeTelephoneNumber) 
        {
            this.employeeId = employeeId;
            this.employeeName = employeeName;
            this.employeeDescription = employeeDescription;
            this.employeeAge = employeeAge;
            this.employeePersonalId = empployeePersonalId;
            this.employeeAddress = employeeAddress;
            this.employeeCity = employeeCity;
            this.employeeTelephoneNumber = employeeTelephoneNumber;

        }

    }
}
