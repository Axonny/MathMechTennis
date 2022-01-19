using MongoDB.Bson;
using TableTennisDomain.Infrastructure;

namespace App
{
    public class MatchStatusRecord : IIdentifiable<ObjectId>
    {
        public ObjectId Id { get; set; } //MatchId == MatchStatusId
        public bool IsConfirmedByFirst { get; set; }
        public bool IsConfirmedBySecond { get; set; }
        public bool IsConfirmedByEachOne => IsConfirmedByFirst && IsConfirmedBySecond;
    }
}