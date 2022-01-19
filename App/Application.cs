using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Rating;
using MongoDB.Bson;
using TableTennisDomain;
using TableTennisDomain.DomainRepositories;
using TableTennisDomain.Infrastructure;

namespace App
{
    public class Application<TRatingRecord> : IApplication
        where TRatingRecord : IIdentifiable<ObjectId>
    {
        public RatingSystem<TRatingRecord> RatingSystem { get; }

        private readonly MatchesRepository matchesRepository;
        private readonly MatchStatusRepository matchStatusRepository;
        private readonly PlayersRepository playersRepository;

        public Application(
            MatchesRepository matchesRepository,
            MatchStatusRepository matchStatusRepository,
            PlayersRepository playersRepository,
            RatingSystem<TRatingRecord> ratingSystem)
        {
            this.matchesRepository = matchesRepository;
            this.matchStatusRepository = matchStatusRepository;
            this.playersRepository = playersRepository;
            RatingSystem = ratingSystem;
        }

        public async Task<ObjectId> RegisterMatch(string nickname1, string nickname2, int gamesWon1, int gamesWon2)
        {
            var match = new Match(
                playersRepository.GetPlayerIdByNickname(nickname1), 
                playersRepository.GetPlayerIdByNickname(nickname2), 
                gamesWon1,
                gamesWon2);
            
            match.Id = ObjectId.GenerateNewId();
            var matchStatus = new MatchStatusRecord {Id = match.Id};

            await Task.Run(() =>
            {
                matchesRepository.Save(match);
                matchStatusRepository.Save(matchStatus);
            });

            return match.Id;
        }

        public Task RegisterPlayer(string nickname, long chatId)
        {
            if (playersRepository.TryGetPlayerIdByChatId(chatId, out var _))
                return Task.CompletedTask;
            
            return Task.Run(() =>
            {
                var player = new Player(nickname, chatId);
                playersRepository.Save(player);
                RatingSystem.RegisterNewPlayer(player.Id);
            });
        }

        public bool IsRegisteredPlayer(long chatId)
        {
            return playersRepository.TryGetPlayerIdByChatId(chatId, out var player);
        }
        
        //TODO: rewrite. IRatingRecord?
        public async Task<long> GetRatingValue(string nickname)
        {
            var record = (await GetRating(nickname)) as EloRecord;

            return record.Rating;
        }

        public async Task<List<string>> GetLastMatchesInfos(string nickname, int count)
        {
            var matches = await Task.Run(() =>
                matchesRepository.GetLastMatches(playersRepository.GetPlayerIdByNickname(nickname), count));

            return matches.Select(GetMatchInfo).ToList();
        }

        public Task ConfirmMatchBy(string nickname, ObjectId matchId)
        {
            var matchStatus = matchStatusRepository.GetById(matchId);

            if (matchStatus.IsConfirmedByEachOne)
            {
                return Task.CompletedTask;
            }

            var match = matchesRepository.GetById(matchId);
            var playerId = playersRepository.GetPlayerIdByNickname(nickname);
            
            if (match.FirstPlayerId == playerId)
                matchStatus.IsConfirmedByFirst = true;
            else if (match.SecondPlayerId == playerId)
                matchStatus.IsConfirmedBySecond = true;

            if (matchStatus.IsConfirmedByEachOne)
            {
                RatingSystem.UpdateRating(match);
            }
            
            return Task.Run(() => matchStatusRepository.Update(matchStatus));
        }

        public Task<List<string>> GetMatchesInfos(IEnumerable<ObjectId> matchIds)
        {
            return Task.Run(() => matchIds.Select(GetMatchInfo).ToList());
        }

        public string GetMatchInfo(ObjectId matchId)
        {
            return GetMatchInfo(matchesRepository.GetById(matchId));
        }

        public async Task<long> GetChatIdByNickname(string nickname)
        {
            return (await Task.Run(
                () => playersRepository.GetById(playersRepository.GetPlayerIdByNickname(nickname)))).ChatId;
        }

        public Task<TRatingRecord> GetRating(string nickname)
        { 
             return Task.Run(() => 
                RatingSystem.RatingByPlayerId.GetById(playersRepository.GetPlayerIdByNickname(nickname)));
        }

        private string GetMatchInfo(Match match)
        {
            var nickname1 = playersRepository.GetNicknameByPlayerId(match.FirstPlayerId);
            var nickname2 = playersRepository.GetNicknameByPlayerId(match.SecondPlayerId);
            var matchStatus = matchStatusRepository.GetById(match.Id);

            return $"MatchId: {match.Id}\n" +
                   $"{match.Date}\n" +
                   $"{nickname1} vs " +
                   $"{nickname2}\n" +
                   $"Confirmation: {matchStatus.IsConfirmedByEachOne}\n" +
                   $"Result: {match.GamesWonByFirstPlayer}:{match.GamesWonBySecondPlayer}";
        }
    }
}