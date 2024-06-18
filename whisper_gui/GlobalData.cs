using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using whisper_gui.Models;

namespace whisper_gui
{
    public static class GlobalData
    {
        public static Options Options { get; } = new Options();

        public static void Initialize()
        {
            Options.LoadFile();
        }
    }
}
