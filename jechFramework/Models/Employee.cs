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
        public int employeeId  { get; set; }
        public string employeeName { get; set; }

        public string employeeDescription { get; set; }

        public int employeeAge { get; set; }

        public int employeePersonalId { get; set; }

        public string employeeAddress { get; set; }

        public string employeeCity { get; set; }

        public string employeeTelephoneNumber { get; set; }


        public Employee(int employeeId, string employeeName)
        {
            this.employeeId = employeeId;
            this.employeeName = employeeName;
            
        }
        /// <summary>
        /// her har man bare en konstruktør fordi når man skal legge til en ny ansatt så må man legge til all informasjon.
        /// </summary>
        /// <param name="employeeId">Ansatt id.</param>
        /// <param name="employeeName">Ansatt navn.</param> 
        /// <param name="employeeDescription">Ansatt sin beskrivelse, her kan det muligens være kort ekstra informasjon f.eks at den ansatte er vikar.</param> 
        /// <param name="employeeAge">Ansatt alder.</param> 
        /// <param name="empployeePersonalId">Ansatt personlig id.</param> 
        /// <param name="employeeAddress">Ansatt adresse.</param> 
        /// <param name="employeeCity">Ansatt by.</param> 
        /// <param name="employeeTelephoneNumber">Ansatt telefon nummer.</param> 
        public Employee(int employeeId, string employeeName, string employeeDescription,
            int employeeAge, int empployeePersonalId, string employeeAddress, string employeeCity,
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
