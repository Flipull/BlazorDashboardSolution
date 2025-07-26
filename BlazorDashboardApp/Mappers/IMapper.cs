using System.Collections.Generic;

namespace BlazorDashboardApp.Mappers
{
    //no co/contra-variance; not needed to support sub- and/or super-types
    public interface IMapper<S, T>
    {
        T Map(S source);
        ICollection<T> MapAll(IEnumerable<S> source);
    }
}
