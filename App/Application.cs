using System.Threading.Tasks;
using App.Rating;
using MongoDB.Bson;
using TableTennisDomain;
using TableTennisDomain.DomainRepositories;
using TableTennisDomain.Infrastructure;

namespace App
{
    public class Application<TRatingRecord> 
        where TRatingRecord : IIdentifiable<ObjectId>
    {
        public RatingSystem<TRatingRecord> RatingSystem { get; }

        private readonly MatchesRepository matchesRepository;
        private readonly PlayersRepository playersRepository;

        public Application(
            MatchesRepository matchesRepository,
            PlayersRepository playersRepository,
            RatingSystem<TRatingRecord> ratingSystem)
        {
            this.matchesRepository = matchesRepository;
            this.playersRepository = playersRepository;
            RatingSystem = ratingSystem;
        }

        public Task RegisterMatch(string nickname1, string nickname2, int gamesWon1, int gamesWon2)
        {
            var match = new Match(
                playersRepository.GetPlayerIdByUsername(nickname1), 
                playersRepository.GetPlayerIdByUsername(nickname2), 
                gamesWon1,
                gamesWon2);
            
            return Task.Run(() =>
            {
                matchesRepository.Save(match);
                RatingSystem.UpdateRating(match);
            });
        }

        public Task RegisterPlayer(string nickname, long chatId)
        {
            if (playersRepository.TryGetPlayerIdByChatId(chatId, out var _))
                return Task.CompletedTask;
            
            return Task.Run(() =>
            {
                var player = new Player(nickname, chatId);
                playersRepository.SaveOrUpdate(player);
                RatingSystem.RegisterNewPlayer(player.Id);
            });
        }

        public Task<TRatingRecord> GetRating(string username)
        { 
             return Task.Run(() => 
                RatingSystem.RatingByPlayerId.GetById(playersRepository.GetPlayerIdByUsername(username)));
        }
    }
}