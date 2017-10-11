using System;
using AutoMapper;
using AutoMapper.DataAccess;

namespace Automapper.DataAccess.UnitTests.Dto
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class CustomerDtoWithUniqueToken
    {
        public string UniqueToken { get; set; }
    }

    public class CustomerDtoWhichIsNothingLikeACustomer
    {
        public int Blah { get; set; }
        public string BlahToken { get; set; }
    }

    public class InvoiceDto
    {
        public decimal Total { get; set; }
    }

    public class CustomerConversion : ITypeConverter<Customer, CustomerDtoWhichIsNothingLikeACustomer>
    {
        public CustomerDtoWhichIsNothingLikeACustomer Convert(Customer source, CustomerDtoWhichIsNothingLikeACustomer destination, ResolutionContext context)
        {
            if (source == null || context == null)
            {
                return null;
            }

            return new CustomerDtoWhichIsNothingLikeACustomer
            {
                Blah = source.Id,
                BlahToken = $"{source.Id}_{source.Name}_{source.Email}"
            };
        }
    }

    public class InvoiceResolver : IValueResolver<Invoice, InvoiceDto, Decimal>
    {
        public decimal Resolve(Invoice source, InvoiceDto destination, decimal destMember, ResolutionContext context)
        {
            if (source == null || destination == null)
            {
                return 0;
            }

            return source.TotalFromItemsBought + source.TotalTaxes;
        }
    }


}
