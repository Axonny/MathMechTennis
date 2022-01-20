using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using TableTennisDomain;

namespace App
{
    public interface IApplication
    {
        Task<ObjectId> RegisterMatch(string player1, string player2, int gamesWon1, int gamesWon2);
        Task RegisterPlayer(string nickname, long chatId);
        bool IsRegisteredPlayer(long chatId);
        Task<long> GetRatingValue(string nickname);
        Task<List<string>> GetLastMatchesInfos(string nickname, int count);
        Task<bool> IsConfirmed(ObjectId matchId);
        Task<bool> TryConfirmMatchBy(string nickname, ObjectId matchId);
        Task<(bool, Match)> TryRejectMatchBy(string nickname, ObjectId matchId);
        Task<List<string>> GetMatchesInfos(IEnumerable<ObjectId> matchIds);
        Task<long> GetChatIdByNickname(string nickname);
        string GetMatchInfo(ObjectId matchId);
        string GetMatchInfo(Match match);
    }
}