using BlazorDashboardApp.Globals;
using BlazorDashboardApp.Data;
using BlazorDashboardApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using BlazorDashboardApp.Mappers;

namespace BlazorDashboardApp.Services
{
    public class SubjectService
    {

        private readonly ApplicationDbContext repository;
        private readonly UserService userService;
        private readonly SubjectMapper subjectMapper;
        private readonly SubjectViewModelMapper subjectViewModelMapper;
        public SubjectService(ApplicationDbContext repo,
                                UserService user_service,
                                SubjectMapper subject_mapper,
                                SubjectViewModelMapper subjectvm_mapper)
        {
            repository = repo;
            userService = user_service;

            subjectMapper = subject_mapper;
            subjectViewModelMapper = subjectvm_mapper;
        }


        public async Task<SubjectViewModel> Get(int? subjectid)
        {
            if (subjectid is null)
                throw new ArgumentException();

            var subject = await repository.Subject.FindAsync(subjectid);
            if (subject is null)
                return null;

            return subjectViewModelMapper.Map(subject);
        }
        public async Task<ICollection<SubjectViewModel>> GetAll()
        {
            return await GetAllQueryable().ToListAsync();
        }
        public IQueryable<SubjectViewModel> GetAllQueryable()
        {
            var subjectQueryable = repository.Subject.Select(s => subjectViewModelMapper.Map(s)).AsQueryable();
            return subjectQueryable;
        }
        
        public async Task<SubjectViewModel> Create(SubjectViewModel subjectvm)
        {
            if (!await userService.CurrentUserHasRole("Editor"))
                throw new AccessViolationException();

            //origfilename = subjectvm.UploadablePhoto.Name
            var photoFileName = CreatePhotoFilenameComplete(subjectvm);
            var photoFilePath = Path.Combine(Constants.PhotoFileDirectory, photoFileName);
            var collissions = await repository.Subject.Where(s => s.Photofile == photoFileName).Take(1).ToListAsync();

            if (collissions.Count > 0)
                throw new InvalidDataException("Photofile " + photoFileName + " DB file collission on the server's side");

            if (subjectvm.UploadablePhoto.Size > Constants.MaxPhotoUploadSize)
                throw new InvalidDataException($"Photofile is too large (Max {Constants.MaxPhotoUploadSize / 1024 / 1024} MB)");
            
            try
            {
                //store file on disk
                using (var filesaveStream = new FileStream(photoFilePath, FileMode.Create))
                {
                    await subjectvm.UploadablePhoto.OpenReadStream(Constants.MaxPhotoUploadSize).CopyToAsync(filesaveStream);
                }
                
            }
            catch
            {
                File.Delete(photoFilePath);
                throw;
            }
            //store subject in Db
            var subject = new Subject
            {
                Name = subjectvm.Name,
                Description = subjectvm.Description,
                Photofile = photoFileName
            };
            await repository.Subject.AddAsync(subject);
            await repository.SaveChangesAsync();
            return subjectViewModelMapper.Map(subject);
        }

        private string CreatePhotoFilenameComplete(SubjectViewModel subject)
        {
            return CreatePhotoFilenameFromSubjectName(subject) + CreatePhotoExtensionFromUploadName(subject);
        }
        private string CreatePhotoExtensionFromUploadName(SubjectViewModel subject)
        {
            return Path.GetExtension(subject.UploadablePhoto.Name).ToLower();
        }
        private string CreatePhotoFilenameFromSubjectName(SubjectViewModel subject)
        {
            var name = string.Join("_", subject.Name.Split(Path.GetInvalidFileNameChars()));
            name = name.Trim().Replace(' ', '_').ToLower();
            return name;
        }

        public async Task<SubjectViewModel> Update(SubjectViewModel subjectvm)
        {
            if (!await userService.CurrentUserHasRole("Editor"))
                throw new AccessViolationException();

            if (subjectvm.Id is null)
                throw new ArgumentException();

            var subject = await repository.Subject.FindAsync(subjectvm.Id);
            if (subject is null)
                throw new ArgumentException();

            var updatedsubject = subjectMapper.Map(subjectvm);
            repository.Subject.Update(subjectMapper.Map(subjectvm));
            repository.SaveChanges();

            return subjectViewModelMapper.Map(updatedsubject);

        }

        public async Task<bool> HardDelete(SubjectViewModel subjectvm)
        {
            if (!await userService.CurrentUserHasRole("Admin"))
                throw new AccessViolationException();
            
            if (subjectvm.Id is null)
                throw new ArgumentException();

            var subject = await repository.Subject.FindAsync(subjectvm.Id);
            if (subject is null)
                throw new ArgumentException();

            File.Delete(Path.Combine(Constants.PhotoFileDirectory, subject.Photofile));

            repository.Subject.Remove(subject);
            repository.SaveChanges();

            return true;
        }


    }
}
