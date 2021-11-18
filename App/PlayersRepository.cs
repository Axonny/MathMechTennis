using System.Linq;
using TableTennisDomain.Infrastructure;

namespace App
{
    public class PlayersRepository : LongKeyRepository<Player>
    {
        public long GetPlayerIdByUsername(string nickName)
        {
            return GetAll().First(user => user.Username == nickName).Id;
        }
    }
}