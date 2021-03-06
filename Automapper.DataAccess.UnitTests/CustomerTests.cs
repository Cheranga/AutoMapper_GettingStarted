﻿using System;
using System.Linq;
using Automapper.DataAccess.UnitTests.Dto;
using Automapper.DataAccess.UnitTests.Mappers;
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
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDto>();
            });

            var customers = Enumerable.Range(1, 10).Select(x => new Customer
            {
                Id = x,
                Name = $"Customer_{x}",
                Email = $"customer_{x}@blah.com"
            }).AsQueryable();

            // Act
            var customerDto = customers.ProjectTo<CustomerDto>(mapperConfig).First();

            // Assert
            Assert.IsNotNull(customerDto);
            Assert.AreEqual("Customer_1", customerDto.Name);
            Assert.AreEqual("customer_1@blah.com", customerDto.Email);
        }

        [TestMethod]
        public void Projection_With_Ignored_Properties()
        {
            // Arrange (Projection can be done only for "IQueryable" interfaces
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDto>().ForMember(prop => prop.Email, opt => opt.Ignore());
            });

            var customers = Enumerable.Range(1, 10).Select(x => new Customer
            {
                Id = x,
                Name = $"Customer_{x}",
                Email = $"customer_{x}@blah.com"
            }).AsQueryable();

            // Act
            var customerDto = customers.ProjectTo<CustomerDto>(mapperConfig).First();

            // Assert
            Assert.IsNotNull(customerDto);
            Assert.AreEqual("Customer_1", customerDto.Name);
            Assert.IsNull(customerDto.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(AutoMapperConfigurationException))]
        public void CheckForConfiguration_When_They_Cannot_be_Mapped_With_Default_Conventions()
        {
            // Arrange
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDtoWithUniqueToken>();
            });
            
            // Act (there's no act in here)

            // Assert
            mapperConfig.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void CheckForConfiguration_When_They_Can_Be_Mapped_With_Default_Conventions()
        {
            // Arrange
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDtoWithUniqueToken>()
                    .ForMember(prop => prop.UniqueToken, expression => expression.MapFrom(cust => $"{cust.Id}_{cust.Email}"));
            });

            // Act (there's no act in here)

            // Assert
            mapperConfig.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void ConversionValidation_When_Properties_Does_Not_Match_At_All()
        {
            // Arrange
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDtoWhichIsNothingLikeACustomer>()
                .ConvertUsing(new CustomerConversion());
            });
            var mapper = mapperConfig.CreateMapper();

            var customer = new Customer
            {
                Id = 1,
                Name = "Cheranga",
                Email = "cheranga@blah.com",
                Age = 35
            };

            // Act
            var dto = mapper.Map<Customer, CustomerDtoWhichIsNothingLikeACustomer>(customer);

            // Assert
            Assert.IsNotNull(dto);
            Assert.AreEqual(1, dto.Blah);
            Assert.AreEqual("1_Cheranga_cheranga@blah.com", dto.BlahToken);
        }

        [TestMethod]
        public void ValueResolver()
        {
            // Arrange
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Invoice, InvoiceDto>()
                    .ForMember(prop => prop.Total, opt => opt.ResolveUsing<InvoiceResolver>());
            });
            var mapper = mapperConfig.CreateMapper();

            var invoice = new Invoice
            {
                TotalFromItemsBought = 1000,
                TotalTaxes = 200
            };

            // Act
            var dto = mapper.Map<Invoice, InvoiceDto>(invoice);

            // Assert
            Assert.IsNotNull(dto);
            Assert.AreEqual(1200, dto.Total);

        }

        [TestMethod]
        public void NullSubstitution()
        {
            // Arrange
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDto>()
                    .ForMember(prop => prop.Email, opt => opt.NullSubstitute("xxx@blah.com"));
            });
            var mapper = mapperConfig.CreateMapper();

            var customer = new Customer();

            // Act
            var dto = mapper.Map<Customer, CustomerDto>(customer);

            // Assert
            Assert.IsNotNull(dto);
            Assert.AreEqual("xxx@blah.com", dto.Email);
        }

        [TestMethod]
        public void Test()
        {
            var sourceToDestinationConfig = new MapperConfiguration(cfg =>
            {
                cfg.RecognizeAlias("_", "C_"); //cfg.RecognizePrefixes("_");
                cfg.SourceMemberNamingConvention = new RemoveUnderscoreConvention();

                RegisterMappings(cfg);
            });

            var mapper = sourceToDestinationConfig.CreateMapper();

            var weirdCustomer = new WeirdCustomer
            {
                _Id = 1,
                _Name = "Cheranga",
                Address = "Walhalla",
                Num_Of_Cars = 2,
                Blah_Blah_Blah_Blah_Blah = 10,
                Location = new WeirdLocation
                {
                    _Id = 2,
                    _Name = "Hatangala",
                    Address = "Vesteros",
                    Num_Of_Cars = 100,
                    Blah_Blah_Blah_Blah_Blah = 100,
                }
            };

            sourceToDestinationConfig.AssertConfigurationIsValid();

            var dto = mapper.Map<WeirdCustomer, WeirdCustomerDto>(weirdCustomer);
            Assert.IsNotNull(dto);
        }

        private void RegisterMappings(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<WeirdCustomer, WeirdCustomerDto>();
            cfg.CreateMap<WeirdLocation, WeirdLocationDto>();
        }
    }
}