﻿using Domain;
using Domain.Models;
using Service;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractorFinding_Test.Services
{
    [Collection("Data Base")]
    public class CustomerServiceTest
    {
        DataFixture _fixture;
        CustomerService customerService;

        public CustomerServiceTest(DataFixture fixture)
        {
            _fixture = fixture;
            customerService = new CustomerService(_fixture.context);
        }

        [Fact]
        public void Test_AddCustomerDetails()
        {
            //Arrange
            var customer = new TbCustomer() { LandSqft = 5.34, RegistrationNo = "12323364", BuildingType = 2, Lattitude = 7.45, Longitude = 7.14, Pincode = 765432 };

            //Act
            var result = customerService.CreateCustomer(customer);
            var expected = "Successfully Added";

            //Assert
            Assert.Equal(expected, result);
        }
        [Fact]
        public void Test_AddCustomerDetailsFailcondition()
        {
            //Arrange
            var customer = new TbCustomer() { LandSqft = 5.34, RegistrationNo = "", BuildingType = 2, Lattitude = 7.45, Longitude = 7.14, Pincode = 765432 };

            //Act
            var result = customerService.CreateCustomer(customer);
           
            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void Get_All_Customer()
        {
            //Arrange

            //Act
            var result = customerService.GetCustomerDetails();

            //Assert
            var expect = _fixture.context.TbCustomers.Count();
            var items = Assert.IsType<List<CustomerDisplay>>(result);
            Assert.Equal(expect, items.Count);
        }


        [Fact]
        public void UpdateDetails_Test_WithCorrectData()
        {
            //Arrange
            var customer = new TbCustomer() { LandSqft = 5.34, RegistrationNo = "1232334", BuildingType = 2, Lattitude = 7.45, Longitude = 7.14, Pincode = 765432 };

            //Act
            var result = customerService.UpdateCustomerDetails(customer);
            var expected = "Successfully Updated!";

            //Assret

            Assert.Equal(expected, result);
        }

        [Fact]
        public void UpdateDetails_Test_With_WrongLandSqft()
        {
            //Arrange
            var customer = new TbCustomer() { LandSqft = null, RegistrationNo = "1232334", BuildingType = 2, Lattitude = 7.45, Longitude = 7.14, Pincode = 765432 };
          

            //Act
            var result = customerService.UpdateCustomerDetails(customer);

            //Assert
            Assert.Null(result);

        }
        [Fact]
        public void UpdateDetails_Test_With_WrongRegistrationNo()
        {
            //Arrange
            var customer = new TbCustomer() { LandSqft = 0, RegistrationNo = "", BuildingType = 2, Lattitude = 7.45, Longitude = 7.14, Pincode = 765432 };


            //Act
            var result = customerService.UpdateCustomerDetails(customer);

            //Assert
            Assert.Null(result);

        }

        [Fact]
        public void DeleteCustomer_Test()
        {
            var customer = new TbCustomer() { LandSqft = 123, RegistrationNo = "1232335", BuildingType = 2, Lattitude = 7.45, Longitude = 7.14, Pincode = 3454545 };


            //Act
            var result = customerService.DeleteCustomer(customer);

         
            //Assret

            Assert.True(result);
        }
    }
}
