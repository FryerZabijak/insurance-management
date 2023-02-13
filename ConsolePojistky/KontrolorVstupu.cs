using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePojistky
{
    static internal class KontrolorVstupu
    {

        public delegate bool KontrolaZadavani(string line);

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

            if (int.Parse(line.ToString()) > 0) return true;
            return false;
        }

        public static bool KontrolaDelky5(string line)
        {
            if (line.Length < 5) return false;
            return true;
        }

        public static bool ZkontrolujDelku(int input, int delka)
        {
            return input < delka;
        }

        public static dynamic ZadavaniOdUzivatele(string nazev = "", string podminky = "",params KontrolaZadavani[] kontroly)
        {
            string vstup = null;
            bool valid = false;

            while (!valid)
            {
                if (nazev != "") Console.Write("Nyní zadáváte \"{0}\" ", nazev);
                if (podminky != "") Console.Write("(Podmínky: {0})", podminky);
                if (nazev == "" && podminky == "") Console.Write("Zadejte hodnotu");
                Console.WriteLine();
                Console.Write("> ");
                vstup = Console.ReadLine();


                valid = true;
                for (int i = 0; i < kontroly.Length; i++)
                {
                    if (!kontroly[i](vstup))
                    {
                        valid = false;
                        break;
                    }
                }

                if (!valid)
                    Console.WriteLine("Neplatný vstup, zadejte prosím znovu.");
            }

            return vstup;
        }

        public static bool ZkontrolujPlatnostIndexu(int index, int delkaPole)
        {
            if (!KontrolaNeprazdnosti(index.ToString())) return false;
            if (!KontrolaCisla(index.ToString())) return false;
            if (index < 0) return false;
            if (!ZkontrolujDelku(index, delkaPole)) return false;
            return true;
        }


    }
}
