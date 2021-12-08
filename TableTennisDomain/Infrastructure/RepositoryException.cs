using System;

namespace TableTennisDomain.Infrastructure
{
    public class RepositoryException : Exception
    {
        public RepositoryException(Exception innerException) : base("", innerException)
        {
        }
    }
}