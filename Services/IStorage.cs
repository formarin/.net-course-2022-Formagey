using Models;

namespace Services
{
    public interface IStorage
    {
        public void Add(Person person);
        public void Update(Person person);
        public void Delete(Person person);
    }
}
