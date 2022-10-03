namespace Models
{
    public class Employee : Person
    {
        public string Contract { get; set; }
        public int Salary { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Employee))
            {
                return false;
            }

            var employee = obj as Employee;
            return employee.FirstName == FirstName &
                employee.LastName == LastName &
                employee.PassportNumber == PassportNumber &
                employee.PhoneNumber == PhoneNumber &
                employee.DateOfBirth == DateOfBirth;
        }
        public static bool operator ==(Employee employee1, Employee employee2)
        {
            return employee1.Equals(employee2);
        }
        public static bool operator !=(Employee employee1, Employee employee2)
        {
            return !employee1.Equals(employee2);
        }
        public override int GetHashCode()
        {
            return PassportNumber.GetHashCode();
        }
    }
}