using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    internal class Employee
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
