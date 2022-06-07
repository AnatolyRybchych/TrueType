
namespace TrueType
{
    class Program
    {
        static void Main(string[] args)
        {
            Font font = new Font(File.ReadAllBytes(@"C:\Windows\Fonts\arial.ttf"), 0);

        }
    }
}