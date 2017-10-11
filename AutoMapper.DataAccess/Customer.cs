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
}
