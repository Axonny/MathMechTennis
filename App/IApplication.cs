using System.Threading.Tasks;

namespace App
{
    public interface IApplication
    {
        Task RegisterMatch(string player1, string player2, int gamesWon1, int gamesWon2);
        Task RegisterPlayer(string nickname, long chatId);
        bool IsRegisteredPlayer(long chatId);
        Task<long> GetRatingValue(string nickname);
    }
}