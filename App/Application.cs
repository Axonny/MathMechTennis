using System;
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
            if (IsRegisteredPlayer(chatId))
                return Task.CompletedTask;

            return Task.Run(() =>
            {
                if (!playersRepository.TryGetPlayerIdByChatId(chatId, out var player))
                {
                    player = new Player(nickname, chatId);
                    playersRepository.Save(player);
                }
                RatingSystem.RegisterNewPlayer(player.Id);
            });
        }

        public bool IsRegisteredPlayer(long chatId)
        {
            if (!playersRepository.TryGetPlayerIdByChatId(chatId, out var player)) return false;
            try
            {
                RatingSystem.RatingByPlayerId.GetById(player.Id);
                return true;
            }
            catch
            {
                return false;
            }
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

        public Task<bool> IsConfirmed(ObjectId matchId)
        {
            return Task.FromResult(matchStatusRepository.GetById(matchId).IsConfirmedByEachOne);
        }

        public async Task<bool> TryConfirmMatchBy(string nickname, ObjectId matchId)
        {
            MatchStatusRecord matchStatus;
            try
            {
                matchStatus = matchStatusRepository.GetById(matchId);
            }
            catch
            {
                return false;
            }

            if (matchStatus.IsConfirmedByEachOne)
            {
                return false;
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
            
            await Task.Run(() => matchStatusRepository.Update(matchStatus));

            return true;
        }
        
        public async Task<(bool, Match)> TryRejectMatchBy(string nickname, ObjectId matchId)
        {
            var matchStatus = matchStatusRepository.GetById(matchId);

            if (matchStatus.IsConfirmedByEachOne)
            {
                return (false, default);
            }

            var match = matchesRepository.GetById(matchId);
            var playerId = playersRepository.GetPlayerIdByNickname(nickname);

            if (!(match.FirstPlayerId == playerId || match.SecondPlayerId == playerId))
            {
                return (false, default);
            }

            await Task.Run(() => matchStatusRepository.DeleteById(matchId));
            var deleteMatch = await Task.Run(() => matchesRepository.DeleteById(matchId));

            return (true, deleteMatch);
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

        public string GetMatchInfo(Match match)
        {
            var nickname1 = playersRepository.GetNicknameByPlayerId(match.FirstPlayerId);
            var nickname2 = playersRepository.GetNicknameByPlayerId(match.SecondPlayerId);
            var matchStatus = new MatchStatusRecord();
            try
            {
                matchStatus = matchStatusRepository.GetById(match.Id);
            }
            catch
            {
                // ignore
            }

            return $"MatchId: {match.Id}\n" +
                   $"Date: <b>{match.Date}</b>\n" +
                   $"Players: {nickname1} vs " +
                   $"<b>{nickname2}</b>\n" +
                   $"Confirmation: <b>{matchStatus.IsConfirmedByEachOne}</b>\n" +
                   $"Result: <b>{match.GamesWonByFirstPlayer}:{match.GamesWonBySecondPlayer}</b>";
        }
    }
}