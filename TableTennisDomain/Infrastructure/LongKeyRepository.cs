using System.Linq;

namespace TableTennisDomain.Infrastructure
{
    public class LongKeyRepository<TItem> : RuntimeRepository<long, TItem> 
        where TItem : IIdentifiable<long>
    {
        public override long GetUniqueId()
        {
            var prevId = 0L;
            foreach (var id in GetAll().Select(item => item.Id).OrderBy(item => item))
            {
                if (id - prevId > 1)
                    return prevId + 1;

                prevId = id;
            }

            return prevId + 1;
        }
    }
}