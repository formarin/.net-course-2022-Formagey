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
            BankService bankService = new BankService();
            Employee employee = new Employee();

            //Act
            bankService.AddBonus(employee);

            //Assert
            Assert.Equal(1, employee.BonusCount);
        }

        [Fact]
        public void AddToBlackList_PositiveTest()
        {
            //Arrange
            BankService bankService = new BankService();
            Employee employee = new Employee();

            //Act
            bankService.AddToBlackList(employee);

            //Assert
            Assert.Contains(employee, bankService.BlackList);
        }

        [Fact]
        public void IsPersonInBlackList_PositiveTest()
        {
            //Arrange
            BankService bankService = new BankService();
            Employee employee = new Employee();
            bankService.AddToBlackList(employee);

            //Act Assert
            Assert.True(bankService.IsPersonInBlackList(employee));
        }
    }
}
