using System.Runtime.InteropServices;
using System.Drawing;

namespace Redo.Libs
{
    internal class MouseAPI
    {
        #region SetCursorPos
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int X, int Y);
        #endregion
        #region GetCursorPos
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);
        #endregion
    }
}
