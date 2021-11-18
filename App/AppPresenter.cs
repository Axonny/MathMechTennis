using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Rating;
using TableTennisDomain;
using TableTennisDomain.DomainRepositories;
using TableTennisDomain.Infrastructure;
using Telegram.Bot.Types;

namespace App
{
    public class AppPresenter<TRatingRecord> 
        where TRatingRecord : IIdentifiable<long>
    {
        public RatingSystem<TRatingRecord> RatingSystem { get; }

        private readonly MatchesRepository matchesRepository;
        private readonly PlayersRepository playersRepository;

        public AppPresenter(
            MatchesRepository matchesRepository,
            PlayersRepository playersRepository,
            RatingSystem<TRatingRecord> ratingSystem)
        {
            this.matchesRepository = matchesRepository;
            this.playersRepository = playersRepository;
            RatingSystem = ratingSystem;
        }

        public Task RegisterMatch(string userName1, string userName2, int gamesWon1, int gamesWon2)
        {
            var match = new Match(
                matchesRepository.GetUniqueId(), 
                playersRepository.GetPlayerIdByUsername(userName1), 
                playersRepository.GetPlayerIdByUsername(userName2), 
                gamesWon1,
                gamesWon2);
            
            return Task.Run(() =>
            {
                matchesRepository.SaveOrUpdate(match);
                RatingSystem.UpdateRating(match);
            });
        }

        public Task RegisterPlayer(Chat chat)
        {
            return Task.Run(() => 
                playersRepository.SaveOrUpdate(new Player(playersRepository.GetUniqueId(), chat.Id, chat.Username)));
        }

        public Task<TRatingRecord> GetRating(string username)
        { 
             return Task.Run(() => 
                RatingSystem.RatingByPlayerId.GetById(playersRepository.GetPlayerIdByUsername(username)));
        }
    }
}