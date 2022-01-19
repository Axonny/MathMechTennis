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
            ConfigureContainer().Get <TelegramBot<EloRecord>>();
            Console.ReadLine();
        }

        private static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();
            
            container.Bind<string>()
                .ToConstant(
                    @"2135115038:AAGcArPVCMvLlVxf4X4sxyZk7_5noYVGePI")
                .WhenInjectedInto<TelegramBot<EloRecord>>();
            container.Bind<IRatingRecord>().To<EloRecord>();
            
            container.Bind<MatchesRepository>().ToSelf();
            container.Bind<MatchStatusRepository>().ToSelf();
            container.Bind<PlayersRepository>().ToSelf();
            
            container.Bind<EloRating>().ToSelf();
            container.Bind<RatingSystem<EloRecord>>().To<EloRating>().WhenInjectedInto<Application<EloRecord>>();
            container.Bind<IRepository<ObjectId, EloRecord>>().To<EloRatingRepository>();
            container.Bind<IRepository<ObjectId, IRatingRecord>>().To<MongoDbRepository<IRatingRecord>>();

            return container;
        }
    }
}