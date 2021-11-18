using System.Linq;
using TableTennisDomain.Infrastructure;

namespace App
{
    public class UserRepository : LongKeyRepository<User>
    {
        public long GetPlayerIdByNickName(string nickName)
        {
            return GetAll().First(user => user.Username == nickName).PlayerId;
        }
    }
}