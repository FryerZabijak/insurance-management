using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePojistky
{
    static internal class KontrolorVstupu
    {

        public static bool KontrolaNeprazdnosti(string line)
        {
            if (line == null || line == "") return false;

            return true;
        }

        public static bool KontrolaCisla(string line)
        {
            if (int.TryParse(line, out int n)) return true;
            return false;
        }

        public static bool KontrolaKladnosti(dynamic line)
        {
            if (!KontrolaCisla(line.ToString())) throw new ArgumentException("Nelze provést Kontrolu Kladnosti, z důvodu že zadaná hodnota není číslo");

            if (int.Parse(line) > 0) return true;
            return false;
        }

        public static bool KontrolaDelky5(string line)
        {
            if (line.Length < 5) return false;
            return true;
        }

    }
}
