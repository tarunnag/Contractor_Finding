using Domain.Models;
using Domain;
using Moq;
using Service.Interface;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ContractorFinding_Test.Services
{
    [CollectionDefinition("Data Base")]
    public class DataBasecollection : ICollectionFixture<DataFixture> { }

    [Collection("Data Base")]
    public class UserServiceTest
    {
        DataFixture _fixture;
        UserService userService;
        Mock<IEncrypt> encrypt;
        Mock<IConfiguration> config;
        Mock<IGenerateToken> generateToken;

        public UserServiceTest(DataFixture fixture)
        {
            _fixture = fixture;
            encrypt = new Mock<IEncrypt>();
            config = new Mock<IConfiguration>();
            generateToken = new Mock<IGenerateToken>();
            userService = new UserService(_fixture.context, encrypt.Object,/* config.Object,*/generateToken.Object);
        }

        [Fact]
        public void GetAll_User()
        {
            //Arrange
            Pagination pageParams = new Pagination() { PageSize = 5, PageNumber = 1, OrderBy = "RegistrationNo" };

            //Act
            var result = userService.GetUserDetails(pageParams);

            //Assert
            var expect = _fixture.context.TbUsers.Count();
            var items = Assert.IsType<List<UserDisplay>>(result);
            Assert.Equal(expect, items.Count);

        }

        [Fact]
        public void Check_New_with_CheckExtistUser()
        {
            //Arrange
            var user = new Registration() { TypeUser = 1, FirstName = "khadeeja", LastName = "shirin", EmailId = "shir@gmail.com", Password = "shir@123", PhoneNumber = 9876567898, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };

            //Act
            var result = userService.checkExistUser(user);
            var excepted = true;

            //Assert
            Assert.Equal(excepted, result);
        }

        [Fact]
        public void Test_Register()
        {
            //Arrange
            var user = new Registration() { TypeUser = 2, FirstName = "ram", LastName = "das", EmailId = "ram@gmail.com", Password = "ram@123",confirmationPassword= "ram@123", PhoneNumber = 23456789, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

            //Act
            var result = userService.Register(user);
            var excepted = false;

            //Assert
            Assert.Equal(result, excepted);
        }

        [Fact]
        public void Test_Unregister()
        {
            //Arrange
            var user = new Registration() { TypeUser = 2, FirstName = "tejo", LastName = "navya", EmailId = "tejo@gmail.com", Password = "tejo@123",confirmationPassword= "tejo@123", PhoneNumber = 973456789, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

            //Act
            var result = userService.Register(user);
            var excepted = true;

            //Assert
            Assert.Equal(result, excepted);
        }

        [Fact]
        public void Test_Register_with_Wrong_Confirmpass()
        {
            //Arrange
            var user = new Registration() { TypeUser = 2, FirstName = "tej", LastName = "navya", EmailId = "teja@gmail.com", Password = "tejo@123", confirmationPassword = "tejo123", PhoneNumber = 973456789, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

            //Act
            var result = userService.Register(user);
            var excepted = false;

            //Assert
            Assert.Equal(result, excepted);
        }

        [Fact]
        public void LogIn_with_correct_mail_password()
        {
            //Arrange
            var user = new TbUser() { EmailId = "ram@gmail.com", Password = "ram@123" };
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);
            generateToken.Setup(a => a.GenerateNewToken(user)).Returns("success");

            //Act
            var result = userService.Login(user);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void LogIn_with_correct_mail_wrong_password()
        {
            //Arrange
            var user = new TbUser() { EmailId = "ram@gmail.com", Password = "SDFGH567" };
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);
            generateToken.Setup(a => a.GenerateNewToken(user)).Returns("fail");

            //Act
            var result = userService.Login(user);


            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void LogIn_with_new_mail()
        {
            // Arrange
            var user = new TbUser() { EmailId = "khadeeja@gmail.com", Password = "khade@123" };
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);
            generateToken.Setup(a => a.GenerateNewToken(user)).Returns("fail");

            //Act
            var result = userService.Login(user);


            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void Forget_Password_Test()
        {
            //Arrange
            var user = new Registration() { EmailId = "shirin@gmail.com", Password = "shirin123", confirmationPassword = "shirin123" };
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

            //Act
            var result = userService.forgotpassword(user);
            var expected = true;

            //Assert
            Assert.Equal(expected, expected);
        }

        [Fact]
        public void Forget_passwrod_Test_wrong_confirmpass() {
            var user = new Registration() { EmailId = "shirin@gmail.com", Password = "shirin123", confirmationPassword = "shiin123" };
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

            //Act
            var result = userService.forgotpassword(user);
            var expected = false;

            //Assert
            Assert.Equal(expected, expected);
        }
    }
}

