using BlazorDashboardApp.Data;
using BlazorDashboardApp.ViewModels;

namespace BlazorDashboardApp.Mappers
{
    public class TagViewModelMapper : AbstractGenericMapper<Tag, TagViewModel>
    {
        public override TagViewModel Map(Tag source)
        {
            return new TagViewModel
            {
                Id = source.Id,
                DatumId = source.DatumId,
                TagString = source.TagString,
                IsDeleted = source.IsDeleted,
                DeletedDate = source.DeletedDate,
                DeletedByUserId = source.DeletedByUserId
            };
        }
    }
    public class TagMapper : AbstractGenericMapper<TagViewModel, Tag>
    {
        public override Tag Map(TagViewModel source)
        {
            return new Tag
            {
                Id = source.Id ?? 0,
                DatumId = source.DatumId ?? 0,
                TagString = source.TagString,
                IsDeleted = source.IsDeleted,
                DeletedDate = source.DeletedDate,
                DeletedByUserId = source.DeletedByUserId
            };
        }
    }
}
