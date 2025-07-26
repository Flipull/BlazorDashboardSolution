using BlazorDashboardApp.Data;
using BlazorDashboardApp.ViewModels;

namespace BlazorDashboardApp.Mappers
{
    public class DatumViewModelMapper : AbstractGenericMapper<Datum, DatumViewModel>
    {
        public override DatumViewModel Map(Datum source)
        {
            return new DatumViewModel
            {
                Id = source.Id,
                SubjectId = source.SubjectId,
                Filename = source.Filename,
                Filetype = source.Filetype
            };
        }
    }
    public class DatumMapper : AbstractGenericMapper<DatumViewModel, Datum>
    {
        public override Datum Map(DatumViewModel source)
        {
            return new Datum
            {
                Id = source.Id ?? 0,
                SubjectId = source.SubjectId ?? 0,
                Filename = source.Filename,
                Filetype = source.Filetype
            };
        }
    }
}
