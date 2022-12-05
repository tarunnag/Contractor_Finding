using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Persistence;
using Domain;
using Domain.Models;

namespace ContractorFinding_Test.Services
{
    public class DataFixture : IDisposable
    {

        private static DbContextOptions<ContractorFindingContext> dbContextOptions = new DbContextOptionsBuilder<ContractorFindingContext>()
         .UseInMemoryDatabase(databaseName: "Contractor_Finding")
         .Options;
        public ContractorFindingContext context;

        public DataFixture()
        {
            context = new ContractorFindingContext(dbContextOptions);
            context.Database.EnsureCreated();
            SeedDatabase();
        }
        public void SeedDatabase()
        {
            var role = new List<UserType>()
            {
                new UserType(){ TypeId=1, Usertype1="Customer"  },
                new UserType(){ TypeId=2, Usertype1="Contractor" }
            };
            context.UserTypes.AddRange(role);
            context.SaveChanges();

            var user = new List<TbUser>()
            {
                new TbUser(){ UserId=1, TypeUser=1, FirstName="khadeeja", LastName="shirin", EmailId="shirin@gmail.com", Password="shirin@123", PhoneNumber=9876567898, CreatedDate=DateTime.Now, UpdatedDate=null, Active=true },
                new TbUser(){ UserId=2, TypeUser=1, FirstName="tarun", LastName="na", EmailId="tarunag@gmail.com", Password="tarun123", PhoneNumber=223456789, CreatedDate=DateTime.Now, UpdatedDate=null, Active=true },
                new TbUser(){ UserId=3, TypeUser=2, FirstName="vineeth", LastName="red", EmailId="vineeth@gmail.com", Password="vinny@123", PhoneNumber=1234567890, CreatedDate=DateTime.Now, UpdatedDate=null, Active=true },
                new TbUser(){ UserId=4, TypeUser=2, FirstName="pavan", LastName="mani", EmailId="pavan@gmail.com", Password="pavan@123", PhoneNumber=45678908765, CreatedDate=DateTime.Now, UpdatedDate=null, Active=true },
                new TbUser(){ UserId=5,TypeUser=2,FirstName="ram",LastName="das",EmailId="ram@gmail.com",Password="ram@123",PhoneNumber=23456789,CreatedDate=DateTime.Now,UpdatedDate=null, Active=true }
            };
            context.TbUsers.AddRange(user);
            context.SaveChanges();

            var services = new List<ServiceProviding>()
            {
                new ServiceProviding{ ServiceId =1 ,ServiceName="Interior"},
                new ServiceProviding{ ServiceId=2,ServiceName="Exterior"},
                new ServiceProviding{ ServiceId=3, ServiceName="exterior and Interior"}
            };
            context.ServiceProvidings.AddRange(services);
            context.SaveChanges();

            var gender = new List<TbGender>()
            {
                new TbGender(){ GenderId=1,GenderType="Male"},
                new TbGender(){ GenderId=2,GenderType="Female"},
                new TbGender(){ GenderId=3,GenderType="others"}
            };
            context.TbGenders.AddRange(gender);
            context.SaveChanges();

            var contractor = new List<ContractorDetail>()
            {
                new ContractorDetail(){ContractorId=3,CompanyName="vinnyConstruction",Gender=1,License="AP-23456789",Services=3,Lattitude=3.45,Longitude=5.34,Pincode=676563,PhoneNumber=1234567890},
                new ContractorDetail(){ContractorId=4,CompanyName="pavanTraders",Gender=1,License="KA-8765437",Services=2,Lattitude=9.54,Longitude=4.36,Pincode=864357,PhoneNumber=45678908765},
                //new ContractorDetail(){ContractorId=5,CompanyName="ramtraders",Gender=2,License="KL-456789",Services=1,Lattitude=7.45,Longitude=7.14,Pincode=765432,PhoneNumber=9876543322}
            };
            context.ContractorDetails.AddRange(contractor);
            context.SaveChanges();

            var customer = new List<TbCustomer>()
            {
                new TbCustomer(){LandSqft = 5.34, RegistrationNo = "1232334", BuildingType = 2, Lattitude = 7.45, Longitude = 7.14, Pincode = 765432},
                new TbCustomer(){LandSqft = 5.24, RegistrationNo = "1232335", BuildingType = 1, Lattitude = 7.75, Longitude = 7.15, Pincode = 765432},
                //new ContractorDetail(){ContractorId=5,CompanyName="ramtraders",Gender=2,License="KL-456789",Services=1,Lattitude=7.45,Longitude=7.14,Pincode=765432,PhoneNumber=9876543322}
            };
            context.TbCustomers.AddRange(customer);
            context.SaveChanges();

            var buildings = new List<TbBuilding>()
            {
                new TbBuilding(){Id =2, Building="main Building"},
                new TbBuilding(){Id =1, Building="sub Building"}
                //new ContractorDetail(){ContractorId=5,CompanyName="ramtraders",Gender=2,License="KL-456789",Services=1,Lattitude=7.45,Longitude=7.14,Pincode=765432,PhoneNumber=9876543322}
            };
            context.TbBuildings.AddRange(buildings);
            context.SaveChanges();
            //TbBuildings
        }
        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
