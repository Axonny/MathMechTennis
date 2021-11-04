namespace TableTennisDomain
{
    public interface IRatingSystem
    {
        int CalculateRating(Match match, Player player1, Player player2);
    }
}