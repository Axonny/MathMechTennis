using System.Collections.Generic;
using System.Linq;
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
        // ReSharper disable MemberCanBePrivate.Global
        public RatingSystem<TRatingRecord> RatingSystem { get; }
        // ReSharper restore MemberCanBePrivate.Global

        private readonly TelegramBot bot;
        private readonly MatchesRepository matchesRepository;
        private readonly UserRepository userRepository;

        public AppPresenter(
            TelegramBot bot,
            MatchesRepository matchesRepository,
            UserRepository userRepository,
            RatingSystem<TRatingRecord> ratingSystem)
        {
            this.bot = bot;
            this.matchesRepository = matchesRepository;
            this.userRepository = userRepository;
            RatingSystem = ratingSystem;
        }

        public void RegisterMatch(long userChatId1, string userName2, int gamesWon1, int gamesWon2)
        {
            var match = new Match(
                matchesRepository.GetUniqueId(), 
                userRepository.GetById(userChatId1).PlayerId, 
                userRepository.GetPlayerIdByNickName(userName2), 
                gamesWon1, 
                gamesWon2);
            
            matchesRepository.SaveOrUpdate(match);
            RatingSystem.UpdateRating(match);
        }

        public void RegisterPlayer(Chat chat)
        {
            var playerId = userRepository.GetAll().LongCount() + 1;
            
            userRepository.SaveOrUpdate(new User(chat.Id, playerId, chat.Username));
        }

        public TRatingRecord GetRating(long chatId)
        { 
            return RatingSystem.RatingByPlayerId.GetById(userRepository.GetById(chatId).PlayerId);
        }
    }
}