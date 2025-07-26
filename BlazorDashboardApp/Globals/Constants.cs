using System.IO;


namespace BlazorDashboardApp.Globals 
{
    static public class Constants
    {
        static public long MaxDatumUploadSize = 50 * 1024 * 1024;//50MB
        static public long MaxPhotoUploadSize = 1 * 1024 * 1024;//1MB
        static public string WwwRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        static public string DatumFileDirectory = Path.Combine(WwwRoot, "Data");
        static public string PhotoFileDirectory = Path.Combine(WwwRoot, "Photos");

        static public List<Char> ReservedTagCharacters = new List<Char> { '!', '+', '-', '=', '@', '#', ':' };
        
        static public void CreatePrerequisiteFileStructure()
        {
            try
            {
                if (!Directory.Exists(DatumFileDirectory))
                    Directory.CreateDirectory(DatumFileDirectory);
                if (!Directory.Exists(PhotoFileDirectory))
                    Directory.CreateDirectory(PhotoFileDirectory);
            }
            catch {
                throw;
            }

        }
    }
}
