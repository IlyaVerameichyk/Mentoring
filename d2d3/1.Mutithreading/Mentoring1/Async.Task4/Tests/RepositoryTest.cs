using Xunit;

namespace Async.Task4.Tests
{
    public class RepositoryTest
    {
        private Repository _repository;
        public RepositoryTest()
        {
            _repository = new Repository();
        }

        [Fact]
        public void WriteUser()
        {
            var user = new User() {Age = 10, Id = 2, Surname = "Sur", Name = "Name"};

            _repository.Add(user).Wait();

            var users = _repository.GetAll().Result;
        }

        [Fact]
        public void DeleteUser()
        {
            var user = new User() {Id = 2};

            _repository.Delete(user).Wait();

            var users = _repository.GetAll().Result;
        }

        [Fact]
        public void UpdateUser()
        {
            var user = new User() { Age = 11, Id = 2, Surname = "Sur1", Name = "Name1" };

            _repository.Update(user).Wait();

            var users = _repository.GetAll().Result;
        }
    }
}