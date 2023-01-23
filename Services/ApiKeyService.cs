using IShop.Data;
using IShop.Model;

namespace IShop.Services
{
    public class ApiKeyService
    {
        private readonly IShopContext _context;
        public ApiKeyService(IShopContext context)
        {
            _context = context;
        }
        public async Task<UserApiKey> CreateApiKey(User user)
        {
            var newApiKey = new UserApiKey
            {
                //User = user,
                UserId = user.Id,
                Value = GenerateApiKey()
            };
            _context.UserApiKeys.Add(newApiKey);
            await _context.SaveChangesAsync();
            return newApiKey;
        }
        private string GenerateApiKey()
        {
            return $"{Guid.NewGuid()}-{Guid.NewGuid()}";
        }
    }
}
