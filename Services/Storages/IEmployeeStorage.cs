using Models;
using System.Collections.Generic;

namespace Services.Storages
{
    public interface IEmployeeStorage : IStorage<Employee>
    {
        public List<Employee> Data { get; }
    }
}
