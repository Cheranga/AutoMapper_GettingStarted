using Automapper.DataAccess.UnitTests.Dto;
using AutoMapper;
using AutoMapper.DataAccess;

namespace Automapper.DataAccess.UnitTests.Mappers
{
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
}