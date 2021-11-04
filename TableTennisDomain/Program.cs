namespace TableTennisDomain
{
    public sealed record Player(string Name, int Rating)
    {
        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}; {nameof(Rating)}: {Rating}";
        }
    }
}