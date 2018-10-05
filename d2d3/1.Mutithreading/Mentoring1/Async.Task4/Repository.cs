using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Async.Task4
{
    public class Repository
    {
        private string _filePath;

        public Repository(string path = "store/users.store")
        {
            _filePath = path;
            if (Path.GetDirectoryName(_filePath) != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
            }
        }

        public async Task Add(User user)
        {
            var users = await ReadUsers();
            if (users.FirstOrDefault(u => u.Id == user.Id) != null)
            {
                throw new ArgumentException("User with same id already exists");
            }
            await WriteUsers(users.Concat(new[] { user }).ToArray());
        }

        public async Task Update(User user)
        {
            var users = await ReadUsers();
            var repUser = users.FirstOrDefault(u => u.Id == user.Id);
            if (repUser == null)
            {
                throw new ArgumentException("User is not exist");
            }
            repUser.Age = user.Age;
            repUser.Name = user.Name;
            repUser.Surname = user.Surname;
            await WriteUsers(users);
        }

        public async Task Delete(User user)
        {
            var users = await ReadUsers();
            var resultUsers = users.Except(users.Where(u => u.Id == user.Id)).ToArray();
            await WriteUsers(resultUsers);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await ReadUsers();
        }

        private async Task<IEnumerable<User>> ReadUsers()
        {
            using (var ms = new MemoryStream())
            {
                using (var fs = new FileStream(_filePath, FileMode.OpenOrCreate))
                {
                    await fs.CopyToAsync(ms);

                    if (ms.Length == 0) { return new User[0]; }
                    ms.Seek(0, SeekOrigin.Begin);
                    var formatter = new BinaryFormatter();
                    return (User[])formatter.Deserialize(ms);
                }
            }
        }

        private async Task WriteUsers(IEnumerable<User> users)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, users);
                ms.Seek(0, SeekOrigin.Begin);
                using (var fs = new FileStream(_filePath, FileMode.Create))
                {
                    await ms.CopyToAsync(fs);
                }
            }
        }
    }
}