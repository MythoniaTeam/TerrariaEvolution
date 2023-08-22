
namespace TerrariaRebirth.Utilities
{
    internal class MDebug
    {
        public static void Print(string text, Color? color = null)
        {
            Color color1 = color ?? Color.White;
            Main.NewText(text, color1.R, color1.G, color1.B);
        }
    }
}
