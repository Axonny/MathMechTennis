using System;
using System.Security.Cryptography;
using TableTennisDomain.Infrastructure;

namespace TableTennisDomain.DomainRepositories
{
    public class MatchesRepository : RuntimeRepository<string, Match>
    {
        public override string GetUniqueId()
        {
            // TODO: rewrite
            var matches = GetAll();
            
            return BitConverter.ToString(SHA256.HashData(BitConverter.GetBytes(matches.Count)));
        }
    }
}