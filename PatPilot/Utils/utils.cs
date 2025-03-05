using PatPilot.Models;

namespace PatPilot.Utils
{
    public static class utils
    {
        public static bool CreateImageDirectory(string name)
        {
            string formattedName = FormatDirectoryName(name);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", formattedName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return true;
        }
        public static string FormatDirectoryName(string name)
        {
            return string.Join("_", name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
