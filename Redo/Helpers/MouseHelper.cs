using Redo.Libs;
using System.Drawing;

namespace Redo.Helpers
{
    internal class MouseHelper
    {
        public static Point GetCursorPosition()
        {
            MouseAPI.POINT lpPoint;
            MouseAPI.GetCursorPos(out lpPoint);
            return lpPoint;
        }
        public static bool SetCursorPosition(Point Location)
        {
           return MouseAPI.SetCursorPos(Location.X, Location.Y);
        }
    }
}