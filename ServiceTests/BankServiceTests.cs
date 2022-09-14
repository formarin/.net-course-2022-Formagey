using Models;
using Services;
using Xunit;

namespace ServiceTests
{
    public class BankServiceTests
    {
        [Fact]
        public void AddBonus_PositiveTest()
        {
            //Arrange
            BankService bankService = new();
            Employee employee = new();

            //Act
            bankService.AddBonus(employee);

            //Assert
            Assert.Equal(1, employee.BonusCount);
        }

        [Fact]
        public void AddToBlackList_PositiveTest()
        {
            //Arrange
            BankService bankService = new();
            Employee employee = new();

            //Act
            bankService.AddToBlackList(employee);

            //Assert
            Assert.Contains(employee, bankService.BlackList);
        }

        [Fact]
        public void IsPersonInBlackList_PositiveTest()
        {
            //Arrange
            BankService bankService = new();
            Employee employee = new();
            bankService.AddToBlackList(employee);

            //Act Assert
            Assert.True(bankService.IsPersonInBlackList(employee));
        }
    }
}
