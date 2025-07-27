using BlazorDashboardApp.Data;
using BlazorDashboardApp.Globals;
using BlazorDashboardApp.Mappers;
using BlazorDashboardApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Text.RegularExpressions;

namespace BlazorDashboardApp.Services
{
    public class TagService
    {
        private readonly ApplicationDbContext repository;
        private readonly UserService userService;
        private readonly DatumService datumService;
        private readonly TagMapper tagMapper;
        private readonly TagViewModelMapper tagViewModelMapper;

        public TagService(ApplicationDbContext repo,
                                UserService user_service,
                                DatumService datum_service,
                                TagMapper tag_mapper,
                                TagViewModelMapper tagvm_mapper)
        {
            repository = repo;
            userService = user_service;

            datumService = datum_service;
            tagMapper = tag_mapper;
            tagViewModelMapper = tagvm_mapper;
        }

        public bool IsValidTag(string tagstring)
        {
            return tagstring.Length >= 3 && Constants.TagReservedCharacters.Contains(tagstring[0])
                    && Regex.IsMatch(tagstring, Constants.TagFormat);
        }

        public async Task<TagViewModel> Get(int? tagid, bool includeDeleted = false)
        {
            if (tagid is null)
                throw new ArgumentException();

            var tag = await repository.Tag.FindAsync(tagid);
            if (tag is null || (!includeDeleted && tag.IsDeleted))
                return null;

            return tagViewModelMapper.Map(tag);
        }
        private async Task<Tag> GetIfExists(int? datumid, string tagstring, bool includeDeleted = false)
        {
            //all checks commented out, taken care of internally by caller's function

            //if (!await userService.CurrentUserHasRole("Editor"))
            //    throw new AccessViolationException();

            //if (datumid is null || await datumService.Get(datumid) is null || String.IsNullOrWhiteSpace(tagString))
            //    throw new ArgumentException();

            //var tags = await repository.Tag.Where(t => t.DatumId == datumid && t.TagString.ToLower() == tagstring.ToLower()).ToListAsync();
            
            var tag = await repository.Tag.Where(t => t.DatumId == datumid 
                                                    && t.TagString.ToLower() == tagstring.ToLower())
                                            .FirstOrDefaultAsync();
            if (tag is null || (!includeDeleted && tag.IsDeleted))
                //throw new ArgumentException();
                return null;

            return tag;
        }

        public async Task<ICollection<TagViewModel>> GetAll(int? datumid)
        {
            if (datumid is null)
                throw new ArgumentException();

            List<Tag> tags = await repository.Tag.Where(t => t.DatumId == datumid && !t.IsDeleted)
                                                .OrderBy(t => t.TagString)
                                                .ToListAsync();
            return tagViewModelMapper.MapAll(tags);
        }
        public async Task<ICollection<TagViewModel>> GetAllDeleted(int? datumid)
        {
            if (!await userService.CurrentUserHasRole("Editor"))
                throw new AccessViolationException();
            
            if (datumid is null)
                throw new ArgumentException();

            List<Tag> tags = await  repository.Tag.Where(t => t.DatumId == datumid && t.IsDeleted)
                                                .OrderBy(t => t.TagString)
                                                .ToListAsync();
            return tagViewModelMapper.MapAll(tags);
        }

        public async Task<bool> Create(TagViewModel tagvm)
        {
            if (!await userService.CurrentUserHasRole("Editor"))
                throw new AccessViolationException();
            
            if (tagvm.DatumId is null || await datumService.Get(tagvm.DatumId) is null 
                    || String.IsNullOrWhiteSpace(tagvm.TagString) || !IsValidTag(tagvm.TagString) )
                throw new ArgumentException();

            //if tag already exists
            var tag = await GetIfExists(tagvm.DatumId, tagvm.TagString, includeDeleted: true);
            if (tag is not null)
            {
                if (tag.IsDeleted)
                {
                    //if tag is soft-deleted, undelete it
                    await Undelete(tag);
                }
                else { };//already exists, so done
            }
            else
            {
                //else create new
                var newtag = tagMapper.Map(tagvm);
                await repository.Tag.AddAsync(newtag);
                repository.SaveChanges();
            }

            return true;
        }

        public async Task<bool> Delete(int? tagid)
        {
            //softdelete based on Id
            if (!await userService.CurrentUserHasRole("Editor"))
                throw new AccessViolationException();

            if (tagid is null)
                throw new ArgumentException();


            var tag = await repository.Tag.FindAsync(tagid);
            if (tag is null || tag.IsDeleted)
                throw new ArgumentException();

            var user = await userService.GetCurrentUser();

            tag.IsDeleted = true;
            tag.DeletedByUserId = user.Id;
            tag.DeletedDate = DateTime.Now;
            repository.Tag.Update(tag);
            repository.SaveChanges();
            return true;
        }

        public async Task<bool> Undelete(int? tagid)
        {
            //undelete based on Id
            if (!await userService.CurrentUserHasRole("Editor"))
                throw new AccessViolationException();

            if (tagid is null)
                throw new ArgumentException();

            //softdelete based on Id

            var tag = await repository.Tag.FindAsync(tagid);
            if (tag is null || !tag.IsDeleted)
                throw new ArgumentException();

            return await Undelete(tag);
        }

        private async Task<bool> Undelete(Tag tag)
        {
            var user = await userService.GetCurrentUser();

            tag.IsDeleted = false;
            tag.DeletedByUserId = user.Id;
            tag.DeletedDate = DateTime.Now;
            repository.Tag.Update(tag);
            repository.SaveChanges();
            return true;
        }
    }
}