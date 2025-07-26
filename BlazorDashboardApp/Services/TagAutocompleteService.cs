using BlazorDashboardApp.Data;
using BlazorDashboardApp.Mappers;
using BlazorDashboardApp.ViewModels;
using NuGet.Protocol.Core.Types;

namespace BlazorDashboardApp.Services
{
    public class TagAutocompleteService
    {
        private readonly ApplicationDbContext repository;
        private readonly SubjectService subjectService;
        private readonly DatumService datumService;
        private readonly TagMapper tagMapper;
        private readonly TagViewModelMapper tagViewModelMapper;

        public TagAutocompleteService(ApplicationDbContext repo,
                                        SubjectService subject_service,
                                        DatumService datum_service,
                                        TagMapper tag_mapper,
                                        TagViewModelMapper tagvm_mapper)
        {
            repository = repo;
            subjectService = subject_service;

            datumService = datum_service;
            tagMapper = tag_mapper;
            tagViewModelMapper = tagvm_mapper;
        }
        public async Task<ICollection<TagViewModel>> GetAllAutocomplete(int? subjectid, string partial_tagstring)
        {
            if (subjectid is null || await subjectService.Get(subjectid) is null || String.IsNullOrWhiteSpace(partial_tagstring))
                throw new ArgumentException();

                var tags = repository.Tag.Where(t => t.Datum.SubjectId == subjectid 
                                                        && t.TagString.ToLower().StartsWith(partial_tagstring.ToLower())
                                                        && !t.IsDeleted)
                                                    .OrderBy(t => t.TagString)
                                                    .Take(20)
                                                    .ToList();
            return tagViewModelMapper.MapAll(tags);
        }
    }
}
