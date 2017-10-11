using System.Linq;
using Automapper.DataAccess.UnitTests.Dto;
using AutoMapper;
using AutoMapper.DataAccess;
using AutoMapper.QueryableExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Automapper.DataAccess.UnitTests
{
    [TestClass]
    public class CustomerTests
    {
        [TestMethod]
        public void Projection_As_It_Is()
        {
            // Arrange
            Mapper.Initialize(cfg => cfg.CreateMap<Customer, CustomerDto>());

            var customers = Enumerable.Range(1, 10).Select(x => new Customer
            {
                Id = x,
                Name = $"Customer_{x}",
                Email = $"customer_{x}@blah.com"
            }).AsQueryable();

            // Act
            var customerDto = customers.ProjectTo<CustomerDto>().First();

            // Assert
            Assert.IsNotNull(customerDto);
            Assert.AreEqual("Customer_1", customerDto.Name);
            Assert.AreEqual("customer_1@blah.com", customerDto.Email);
        }

        [TestMethod]
        public void Projection_With_Ignored_Properties()
        {
            // Arrange (Projection can be done only for "IQueryable" interfaces
            Mapper.Initialize(cfg =>
                cfg.CreateMap<Customer, CustomerDto>().ForMember(y => y.Email, z => z.Ignore()));

            var customers = Enumerable.Range(1, 10).Select(x => new Customer
            {
                Id = x,
                Name = $"Customer_{x}",
                Email = $"customer_{x}@blah.com"
            }).AsQueryable();

            // Act
            var customerDto = customers.ProjectTo<CustomerDto>().First();

            // Assert
            Assert.IsNotNull(customerDto);
            Assert.AreEqual("Customer_1", customerDto.Name);
            Assert.IsNull(customerDto.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(AutoMapperConfigurationException))]
        public void CheckForConfiguration_When_There_Is_No_Configuration()
        {
            // Arrange
            Mapper.Initialize(cfg => cfg.CreateMap<Customer, CustomerDtoWithUniqueToken>());
            
            // Act (there's no act in here)

            // Assert
            Mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void CheckForConfiguration_When_There_Is_a_Valid_Configuration()
        {
            // Arrange
            Mapper.Initialize(cfg =>
                cfg.CreateMap<Customer, CustomerDtoWithUniqueToken>()
                    .ForMember(x => x.UniqueToken, expression => expression.MapFrom(cust => $"{cust.Id}_{cust.Email}")));

            // Act (there's no act in here)

            // Assert
            Mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void ConversionValidation_When_Properties_Does_Not_Match_At_All()
        {
            // Arrange
            Mapper.Initialize(cfg=>
            cfg.CreateMap<Customer, CustomerDtoWhichIsNothingLikeACustomer>().ConvertUsing(new CustomerConversion()));

            var customer = new Customer
            {
                Id = 1,
                Name = "Cheranga",
                Email = "cheranga@blah.com",
                Age = 35
            };

            // Act
            var dto = Mapper.Map<Customer, CustomerDtoWhichIsNothingLikeACustomer>(customer);

            // Assert
            Assert.IsNotNull(dto);
            Assert.AreEqual(1, dto.Blah);
            Assert.AreEqual("1_Cheranga_cheranga@blah.com", dto.BlahToken);
        }

        [TestMethod]
        public void ValueResolver()
        {
            // Arrange
            Mapper.Initialize(cfg=>
            cfg.CreateMap<Invoice, InvoiceDto>().ForMember(x=>x.Total, opt=>opt.ResolveUsing<InvoiceResolver>())
            );

            var invoice = new Invoice
            {
                TotalFromItemsBought = 1000,
                TotalTaxes = 200
            };

            // Act
            var dto = Mapper.Map<Invoice, InvoiceDto>(invoice);

            // Assert
            Assert.IsNotNull(dto);
            Assert.AreEqual(1200, dto.Total);

        }

        [TestMethod]
        public void NullSubstitution()
        {
            // Arrange
            Mapper.Initialize(cfg=>
            cfg.CreateMap<Customer, CustomerDto>().ForMember(x=>x.Email, opt=>opt.NullSubstitute("xxx@blah.com")));

            var customer = new Customer();

            // Act
            var dto = Mapper.Map<Customer, CustomerDto>(customer);

            // Assert
            Assert.IsNotNull(dto);
            Assert.AreEqual("xxx@blah.com", dto.Email);
        }
    }
}