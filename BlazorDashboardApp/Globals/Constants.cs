using System.IO;


namespace BlazorDashboardApp.Globals 
{
    static public class Constants
    {
        static public long MaxDatumUploadSize = 50 * 1024 * 1024;//50MB
        static public long MaxPhotoUploadSize = 1 * 1024 * 1024;//1MB
        static public string LocalWwwRootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        static public string DatumFileDirectory = "Data";
        static public string PhotoFileDirectory = "Photos";
        static public string LocalDatumFileDirectory = Path.Combine(LocalWwwRootDirectory, DatumFileDirectory);
        static public string LocalPhotoFileDirectory = Path.Combine(LocalWwwRootDirectory, PhotoFileDirectory);
        
        //!subject +posclaim -negclaim =anyclain @mentionedperson #subjectstate : %mentioneddate <beforedate >afterdate
        static public List<Char> TagReservedCharacters = 
                new List<Char> {/* '!',*/ '+', '-', '=', '@', '#', ':', '%' };
        static public string TagFormat = @"^.[a-zA-Z0-9 ]+$";
        static public void CreatePrerequisiteFileStructure()
        {
            try
            {
                if (!Directory.Exists(LocalDatumFileDirectory))
                    Directory.CreateDirectory(LocalDatumFileDirectory);
                if (!Directory.Exists(LocalPhotoFileDirectory))
                    Directory.CreateDirectory(LocalPhotoFileDirectory);
            }
            catch {
                throw;
            }

        }
    }
}
