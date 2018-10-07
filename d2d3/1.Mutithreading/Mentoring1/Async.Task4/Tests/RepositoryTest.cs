using System.Linq;
using Xunit;

namespace Async.Task4.Tests
{
    public class RepositoryTest
    {
        private Repository _repository;
        public RepositoryTest()
        {
            _repository = new Repository();
            foreach(var user in _repository.GetAll().Result)
            {
                _repository.Delete(user).Wait();

            }
            var userToAdd = new User() { Age = 10, Id = 2, Surname = "Sur", Name = "Name" };

            _repository.Add(userToAdd).Wait();
        }

        [Fact]
        public void AssertInitial()
        {
            Assert.Single(_repository.GetAll().Result);
            Assert.Equal("Name", _repository.GetAll().Result.First().Name);
            Assert.Equal("Sur", _repository.GetAll().Result.First().Surname);
            Assert.Equal(10, _repository.GetAll().Result.First().Age);
        }

        [Fact]
        public void WriteUser()
        {
            var user = new User() { Age = 10, Id = 1, Surname = "Sur", Name = "Name" };

            _repository.Add(user).Wait();

            Assert.Equal(2, _repository.GetAll().Result.Count());
        }

        [Fact]
        public void DeleteUser()
        {
            var user = new User() { Id = 2 };

            _repository.Delete(user).Wait();

            Assert.Empty(_repository.GetAll().Result);
        }

        [Fact]
        public void UpdateUser()
        {
            var user = new User() { Age = 11, Id = 2, Surname = "Sur1", Name = "Name1" };

            _repository.Update(user).Wait();
            var actual = _repository.GetAll().Result.First();


            Assert.Equal(11, actual.Age);
            Assert.Equal("Sur1", actual.Surname);
            Assert.Equal("Name1", actual.Name);
        }
    }
}