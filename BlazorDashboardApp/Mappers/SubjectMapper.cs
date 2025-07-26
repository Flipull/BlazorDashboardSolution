using BlazorDashboardApp.Data;
using BlazorDashboardApp.ViewModels;

namespace BlazorDashboardApp.Mappers
{
    public class SubjectViewModelMapper : AbstractGenericMapper<Subject, SubjectViewModel>
    {
        public override SubjectViewModel Map(Subject source)
        {
            return new SubjectViewModel
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                Photofile = source.Photofile
            };
        }
    }
    public class SubjectMapper : AbstractGenericMapper<SubjectViewModel, Subject>
    {
        public override Subject Map(SubjectViewModel source)
        {
            return new Subject
            {
                Id = source.Id ?? 0,
                Name = source.Name,
                Description = source.Description,
                Photofile = source.Photofile                
            };
        }
    }
}
