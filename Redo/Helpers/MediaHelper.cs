using System;
using Redo.Libs;

namespace Redo.Helpers
{
    internal class MediaHelper
    {
        public static void RedoPlaySounds(bool mode)
        {
            var soundFile = string.Empty;
            if (mode == false)
                soundFile = System.IO.Path.Join("Assets", "Stop.wav");
            else
                soundFile = System.IO.Path.Join("Assets", "Start.wav");
            MediaAPI.PlaySound(soundFile, IntPtr.Zero, MediaAPI.SND_FILENAME | MediaAPI.SND_ASYNC | MediaAPI.SND_NOSTOP);
        }
    }
}
