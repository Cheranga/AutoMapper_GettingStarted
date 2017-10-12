using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper.DataAccess
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }

    public class Invoice
    {
        public decimal TotalFromItemsBought { get; set; }
        public decimal TotalTaxes { get; set; }
    }

    public class WeirdCustomer
    {
        public int _Id { get; set; }
        public string _Name { get; set; }
        public string Address { get; set; }
        public int Num_Of_Cars { get; set; }
        public int Blah_Blah_Blah_Blah_Blah { get; set; }

        public WeirdLocation Location { get; set; }
    }

    public class WeirdLocation
    {
        public int _Id { get; set; }
        public string _Name { get; set; }
        public string Address { get; set; }
        public int Num_Of_Cars { get; set; }
        public int Blah_Blah_Blah_Blah_Blah { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public string ParentName { get; set; }
    }

    public class NestedCustomer
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Student Student { get; set; }
    }
}
