using System;
using App.Rating;
using MongoDB.Bson;
using Ninject;
using TableTennisDomain.DomainRepositories;
using TableTennisDomain.Infrastructure;

namespace App
{
    public static class Program
    {
        private static void Main()
        {
            var token = Environment.GetEnvironmentVariable("TgBotToken", EnvironmentVariableTarget.User) 
                        ?? throw new InvalidOperationException("TgBotToken not found");
            
            ConfigureContainer(token).Get<TelegramBot<EloRecord>>(); // Or TelegramBot<TrueSkillRecord>
            Console.ReadLine();
        }

        private static StandardKernel ConfigureContainer(string token)
        {
            var container = new StandardKernel();

            container.Bind<MatchesRepository>().ToSelf();
            container.Bind<PlayersRepository>().ToSelf();
            container.Bind<MatchStatusRepository>().ToSelf();
            container.Bind<IRepository<ObjectId, IRatingRecord>>().To<MongoDbRepository<IRatingRecord>>();
            
            container.Bind<string>().ToConstant(token).WhenInjectedInto<TelegramBot<EloRecord>>();
            container.Bind<EloRating>().ToSelf();
            container.Bind<IRepository<ObjectId, EloRecord>>().To<EloRatingRepository>();
            container.Bind<RatingSystem<EloRecord>>().To<EloRating>().WhenInjectedInto<Application<EloRecord>>();

            container.Bind<string>().ToConstant(token).WhenInjectedInto<TelegramBot<TrueSkillRecord>>();
            container.Bind<TrueSkillRating>().ToSelf();
            container.Bind<IRepository<ObjectId, TrueSkillRecord>>().To<TrueSkillRatingRepository>();
            container.Bind<RatingSystem<TrueSkillRecord>>().To<TrueSkillRating>().WhenInjectedInto<Application<TrueSkillRecord>>();
            
            return container;
        }
    }
}