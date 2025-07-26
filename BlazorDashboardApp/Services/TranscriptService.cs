using BlazorDashboardApp.Data;
using BlazorDashboardApp.Mappers;
using BlazorDashboardApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace BlazorDashboardApp.Services
{
    public class TranscriptService
    {
        private readonly ApplicationDbContext repository;
        private readonly UserService userService;
        private readonly DatumService datumService;
        private readonly TranscriptMapper transcriptMapper;
        private readonly TranscriptViewModelMapper transcriptViewModelMapper;

        public TranscriptService(ApplicationDbContext repo,
                                UserService user_service,
                                DatumService datum_service,
                                TranscriptMapper transcript_mapper,
                                TranscriptViewModelMapper transcriptvm_mapper)
        {
            repository = repo;
            userService = user_service;

            datumService = datum_service;
            transcriptMapper = transcript_mapper;
            transcriptViewModelMapper = transcriptvm_mapper;
        }

        public async Task<TranscriptViewModel> Get(int? transcriptid, bool includeDeleted = false)
        {
            if (transcriptid is null)
                throw new ArgumentException();

            var transcript = await repository.Transcript.FindAsync(transcriptid);
            if (transcript is null || (!includeDeleted && transcript.IsDeleted))
                return null;

            return transcriptViewModelMapper.Map(transcript);
        }

        public async Task<ICollection<TranscriptViewModel>> GetAll(int? datumid)
        {
            if (datumid is null)
                throw new ArgumentException();

            List<Transcript> transcripts = await repository.Transcript.Where(t => t.DatumId == datumid && !t.IsDeleted).ToListAsync();
            return transcriptViewModelMapper.MapAll(transcripts);
        }
        public async Task<ICollection<TranscriptViewModel>> GetAllDeleted(int? datumid)
        {
            if (!await userService.CurrentUserHasRole("Editor"))
                throw new AccessViolationException();
            
            if (datumid is null)
                throw new ArgumentException();

            List<Transcript> transcripts = await  repository.Transcript.Where(t => t.DatumId == datumid && t.IsDeleted).ToListAsync();
            return transcriptViewModelMapper.MapAll(transcripts);
        }

        public async Task<bool> Create(TranscriptViewModel transcriptvm)
        {
            if (!await userService.CurrentUserHasRole("Editor"))
                throw new AccessViolationException();

            if (transcriptvm.DatumId is null || datumService.Get(transcriptvm.DatumId) is null || String.IsNullOrWhiteSpace(transcriptvm.TranscriptString))
                throw new ArgumentException();
            
            var newtranscript = transcriptMapper.Map(transcriptvm);
            await repository.Transcript.AddAsync(newtranscript);
            repository.SaveChanges();

            return true;
        }

        public async Task<bool> Delete(int? transcriptid)
        {
            //softdelete based on Id
            if (!await userService.CurrentUserHasRole("Editor"))
                throw new AccessViolationException();

            if (transcriptid is null)
                throw new ArgumentException();


            var transcript = await repository.Transcript.FindAsync(transcriptid);
            if (transcript is null || transcript.IsDeleted)
                throw new ArgumentException();

            var user = await userService.GetCurrentUser();

            transcript.IsDeleted = true;
            transcript.DeletedByUserId = user.Id;
            transcript.DeletedDate = DateTime.Now;
            repository.Transcript.Update(transcript);
            repository.SaveChanges();
            return true;
        }

        public async Task<bool> Undelete(int? transcriptid)
        {
            //undelete based on Id
            if (!await userService.CurrentUserHasRole("Editor"))
                throw new AccessViolationException();

            if (transcriptid is null)
                throw new ArgumentException();

            //softdelete based on Id

            var transcript = await repository.Transcript.FindAsync(transcriptid);
            if (transcript is null || !transcript.IsDeleted)
                throw new ArgumentException();

            var user = await userService.GetCurrentUser();

            transcript.IsDeleted = false;
            transcript.DeletedByUserId = user.Id;
            transcript.DeletedDate = DateTime.Now;
            repository.Transcript.Update(transcript);
            repository.SaveChanges();
            return true;
        }
    }
}