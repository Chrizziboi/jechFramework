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
        public int employeeId 
        { get; set; }
        public string employeeName 
        { get; set; }
        public string employeeDescription
        { get; set; }
        public int employeeAge 
        { get; set; }
        public int employeePersonalId 
        { get; set; }
        public string employeeAddress 
        { get; set; }
        public string employeeCity 
        { get; set; }
        public string employeeTelephoneNumber 
        { get; set; }

        /// <summary>
        /// her har man bare en konstruktør fordi når man skal legge til en ny ansatt så må man legge til all informasjon.
        /// </summary>
        /// <param name="employeeId"></param> Ansatt id.
        /// <param name="employeeName"></param> Ansatt navn.
        /// <param name="employeeDescription"></param> Ansatt sin beskrivelse, her kan det muligens være kort ekstra informasjon f.eks at den ansatte er vikar.
        /// <param name="employeeAge"></param> Ansatt alder.
        /// <param name="empployeePersonalId"></param> Ansatt personlig id.
        /// <param name="employeeAddress"></param> Ansatt adresse.
        /// <param name="employeeCity"></param> Ansatt by.
        /// <param name="employeeTelephoneNumber"></param> Ansatt telefon nummer.
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
