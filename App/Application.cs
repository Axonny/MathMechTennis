using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Rating;
using MongoDB.Bson;
using TableTennisDomain;
using TableTennisDomain.DomainRepositories;

namespace App
{
    public class Application<TRatingRecord> : IApplication
        where TRatingRecord : class, IRatingRecord
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
                ObjectId.GenerateNewId(),
                playersRepository.GetPlayerIdByNickname(nickname1), 
                playersRepository.GetPlayerIdByNickname(nickname2), 
                gamesWon1,
                gamesWon2);
            
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
            return playersRepository.TryGetPlayerIdByChatId(chatId, out _);
        }
        
        public async Task<long> GetRatingValue(string nickname)
        {
            var record = await GetRating(nickname);

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
            var match = matchesRepository.GetById(matchId);
            var matchStatus = matchStatusRepository.GetById(matchId);
            var playerId = playersRepository.GetPlayerIdByNickname(nickname);

            if (match.FirstPlayerId == playerId)
                matchStatus.IsConfirmedByFirst = true;
            else if (match.SecondPlayerId == playerId)
                matchStatus.IsConfirmedBySecond = true;

            if (matchStatus.IsConfirmedByFirst && matchStatus.IsConfirmedBySecond)
            {
                RatingSystem.UpdateRating(match);
            }
            
            return Task.Run(() => matchStatusRepository.Update(matchStatus));
        }

        public Task<List<string>> GetMatchesInfos(IEnumerable<ObjectId> matchIds)
        {
            return Task.Run(() => matchIds.Select(id => GetMatchInfo(matchesRepository.GetById(id))).ToList());
        }

        public Task<List<ObjectId>> GetUnconfirmedMatchesIds(string nickname, int maxCount = 5)
        {
            var playerId = playersRepository.GetPlayerIdByNickname(nickname);

            return Task.Run(() =>
                matchesRepository
                    //TODO: GetByPlayerId without count (with IEnumerable)
                    .GetByPlayerId(playerId, maxCount)
                    .Where(match =>
                    {
                        var status = matchStatusRepository.GetById(match.Id);

                        return match.FirstPlayerId == playerId && !status.IsConfirmedByFirst
                               || match.SecondPlayerId == playerId && !status.IsConfirmedBySecond;
                    })
                    .Select(match => match.Id)
                    .ToList());
        }

        public Task<TRatingRecord> GetRating(string nickname)
        { 
             return Task.Run(() => 
                RatingSystem.RatingByPlayerId.GetById(playersRepository.GetPlayerIdByNickname(nickname)));
        }

        private string GetMatchInfo(Match match)
        {
            return $"{match.Date}\n" +
                   $"{playersRepository.GetUsernameByPlayerId(match.FirstPlayerId)} vs " +
                   $"{playersRepository.GetUsernameByPlayerId(match.SecondPlayerId)}\n" +
                   $"Result: {match.GamesWonByFirstPlayer}:{match.GamesWonBySecondPlayer}";
        }
    }
}