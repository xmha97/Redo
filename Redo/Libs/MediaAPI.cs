using System;
using System.Runtime.InteropServices;

namespace Redo.Libs
{
    internal class MediaAPI
    {
        #region PlaySound
        [DllImport("winmm.dll", SetLastError = true)]
        public static extern bool PlaySound(string pszSound, IntPtr hmod, uint fdwSound);
        public const uint SND_SYNC = 0x0000;         // Play sound synchronously (blocking)
        public const uint SND_ASYNC = 0x0001;        // Play sound asynchronously (non-blocking)
        public const uint SND_NODEFAULT = 0x0002;    // Do not play the default sound if the file is not found
        public const uint SND_MEMORY = 0x0004;       // The sound is a memory file, not a filename
        public const uint SND_LOOP = 0x0008;         // Loop the sound until PlaySound is called again with null
        public const uint SND_NOSTOP = 0x0010;       // Do not interrupt a sound currently playing
        public const uint SND_FILENAME = 0x00020000; // The sound name is a file name

        #endregion
    }
}
