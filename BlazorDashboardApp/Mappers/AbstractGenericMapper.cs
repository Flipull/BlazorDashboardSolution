namespace BlazorDashboardApp.Mappers
{
    public abstract class AbstractGenericMapper<S, T> : IMapper<S, T>
    {
        public abstract T Map(S source);
        
        public ICollection<T> MapAll(IEnumerable<S> source)
        {
            var list = new List<T>();
            foreach (S s in source)
                list.Add(Map(s));
            return list;
        }
    }
}
