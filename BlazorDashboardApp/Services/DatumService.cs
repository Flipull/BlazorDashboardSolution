using BlazorDashboardApp.Data;
using BlazorDashboardApp.Globals;
using BlazorDashboardApp.Mappers;
using BlazorDashboardApp.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace BlazorDashboardApp.Services
{
    public class DatumService
    {
        private readonly ApplicationDbContext repository;
        private readonly UserService userService;
        private readonly SubjectService subjectService;
        private readonly DatumMapper datumMapper;
        private readonly DatumViewModelMapper datumViewModelMapper;

        public DatumService(ApplicationDbContext repo,
                            UserService user_service,
                            SubjectService subject_service,
                            DatumMapper datum_mapper,
                            DatumViewModelMapper datumvm_mapper)
        {
            repository = repo;
            userService = user_service;

            subjectService = subject_service;
            datumMapper = datum_mapper;
            datumViewModelMapper = datumvm_mapper;
            
        }

        public async Task<DatumViewModel> Get(int? datumid, bool includeDeleted = false)
        {
            if (datumid is null)
                throw new ArgumentException();
            
            var datum = await repository.Datum.FindAsync(datumid);
            if (datum is null || (!includeDeleted && datum.IsDeleted))
                return null;

            return datumViewModelMapper.Map(datum);
        }
        public async Task<DatumViewModel> Create(DatumViewModel datumvm)
        {
            if (!await userService.CurrentUserHasRole("Editor"))
                throw new AccessViolationException();

            if (datumvm.SubjectId is null || await subjectService.Get(datumvm.SubjectId) is null || datumvm.UploadableDatum is null)
                throw new ArgumentException();

            string datumFileName = CreateDatumFilenameComplete(datumvm);
            string datumFilePath = Path.Combine(Constants.DatumFileDirectory, datumFileName);

            if (datumvm.UploadableDatum.Size > Constants.MaxDatumUploadSize)
                throw new InvalidDataException($"Datumfile is too large (Max {Constants.MaxDatumUploadSize / 1024 / 1024} MB)");

            try
            {
                //store file on disk
                using (var filesaveStream = new FileStream(datumFilePath, FileMode.Create))
                {
                    await datumvm.UploadableDatum.OpenReadStream(Constants.MaxDatumUploadSize).CopyToAsync(filesaveStream);
                }
            }
            catch
            {
                File.Delete(datumFilePath);
                throw;
            }

            var datum = new Datum
            {
                SubjectId = datumvm.SubjectId.Value,
                Filetype = datumvm.UploadableDatum.ContentType,
                Filename = datumFileName
            };
            await repository.Datum.AddAsync(datum);
            repository.SaveChanges();

            return datumViewModelMapper.Map(datum);
        }

        private string CreateDatumFilenameComplete(DatumViewModel datum)
        {
            return CreateDatumFilenameFromSubjectName(datum) + CreateDatumExtensionFromUploadName(datum);
        }
        private string CreateDatumExtensionFromUploadName(DatumViewModel datum)
        {
            return Path.GetExtension(datum.UploadableDatum.Name).ToLower();
        }
        private string CreateDatumFilenameFromSubjectName(DatumViewModel datum)
        {
            var name = DateTime.Now.ToString("yyyyMMdd-HHmm") + "_"
                        + Regex.Replace(Path.GetFileNameWithoutExtension(datum.UploadableDatum.Name).Trim(),
                                            @"[^a-zA-Z0-9]", "_");
            return name;
        }

        public async Task<bool> Delete(int? datumid)
        {
            //softdelete based on Id
            if (!await userService.CurrentUserHasRole("Editor"))
                throw new AccessViolationException();

            if (datumid is null)
                throw new ArgumentException();


            var datum = await repository.Datum.FindAsync(datumid);
            if (datum is null || datum.IsDeleted)
                throw new ArgumentException();

            var user = await userService.GetCurrentUser();

            datum.IsDeleted = true;
            datum.DeletedByUserId = user.Id;
            datum.DeletedDate = DateTime.Now;
            repository.Datum.Update(datum);
            repository.SaveChanges();
            return true;
        }
        public async Task<bool> Undelete(int? datumid)
        {
            //undelete based on Id
            if (!await userService.CurrentUserHasRole("Editor"))
                throw new AccessViolationException();

            if (datumid is null)
                throw new ArgumentException();


            var datum = await repository.Datum.FindAsync(datumid);
            if (datum is null || !datum.IsDeleted)
                throw new ArgumentException();

            var user = await userService.GetCurrentUser();

            datum.IsDeleted = false;
            datum.DeletedByUserId = user.Id;
            datum.DeletedDate = DateTime.Now;
            repository.Datum.Update(datum);
            repository.SaveChanges();
            return true;
        }

        public async Task<bool> HardDelete(int? datumid)
        {
            //hard delete based on Id, includes the file, and cascading to transcripts and tags. Admin only
            if (!await userService.CurrentUserHasRole("Admin"))
                throw new AccessViolationException();

            if (datumid is null)
                throw new ArgumentException();


            var datum = await repository.Datum.FindAsync(datumid);
            if (datum is null)
                throw new ArgumentException();


            string datumFilePath = Path.Combine(Constants.DatumFileDirectory, datum.Filename);
            File.Delete(datumFilePath);

            repository.Datum.Remove(datum);//remove item we fetched
            repository.Tag.RemoveRange(repository.Tag.Where(t => t.DatumId == datum.Id));
            repository.Transcript.RemoveRange(repository.Transcript.Where(t => t.DatumId == datum.Id));
            repository.SaveChanges();

            return true;
        }


    }
}