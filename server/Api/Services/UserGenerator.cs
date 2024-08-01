using Bogus;
using Api.Repositories;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services
{
    public interface IUserGenerator
    {
        Task<string> GenerateUniqueUsernameAsync();
    }

    public class UserGenerator : IUserGenerator
    {
        private readonly Faker _faker;
        private readonly IUsersRepository _usersRepository;

        public UserGenerator(IUsersRepository usersRepository)
        {
            _faker = new Faker();
            _usersRepository = usersRepository;
        }

        public async Task<string> GenerateUniqueUsernameAsync()
        {
            string username;
            do
            {
                username = GenerateUsername();
            } while (await _usersRepository.GetUserByIdAsync(username) != null);

            return username;
        }

        private string GenerateUsername()
        {
            var word1 = ToPascalCase(_faker.Random.Word());
            var word2 = ToPascalCase(_faker.Random.Word());
            var number = _faker.Random.Int(1000, 9999);
            return $"{word1}{word2}{number}";
        }

        private string ToPascalCase(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return word;

            var stringBuilder = new StringBuilder(word);
            stringBuilder[0] = char.ToUpper(stringBuilder[0]);

            for (int i = 1; i < stringBuilder.Length; i++)
            {
                stringBuilder[i] = char.ToLower(stringBuilder[i]);
            }

            return stringBuilder.ToString();
        }
    }
}
