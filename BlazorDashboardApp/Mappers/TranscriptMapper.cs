using BlazorDashboardApp.Data;
using BlazorDashboardApp.ViewModels;

namespace BlazorDashboardApp.Mappers
{
    public class TranscriptViewModelMapper : AbstractGenericMapper<Transcript, TranscriptViewModel>
    {
        public override TranscriptViewModel Map(Transcript source)
        {
            return new TranscriptViewModel
            {
                Id = source.Id,
                DatumId = source.DatumId,
                TranscriptString = source.TranscriptString,
                IsDeleted = source.IsDeleted,
                DeletedDate = source.DeletedDate,
                DeletedByUserId = source.DeletedByUserId
            };
        }
    }
    public class TranscriptMapper : AbstractGenericMapper<TranscriptViewModel, Transcript>
    {
        public override Transcript Map(TranscriptViewModel source)
        {
            return new Transcript
            {
                Id = source.Id ?? 0,
                DatumId = source.DatumId ?? 0,
                TranscriptString = source.TranscriptString,
                IsDeleted = source.IsDeleted,
                DeletedDate = source.DeletedDate,
                DeletedByUserId = source.DeletedByUserId
            };
        }
    }
}
