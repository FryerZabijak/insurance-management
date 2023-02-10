using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConsolePojistky
{
    internal class Program
    {
        static HashSet<ClassPojistka> seznamPojistek = new HashSet<ClassPojistka>();
        delegate bool KontrolaZadavani(string line);
        static void Main(string[] args)
        {
            bool chod_programu = true;
            while (chod_programu)
            {
                Console.Clear();
                NapisMenu();
                char step = Console.ReadKey(false).KeyChar;
                switch (step)
                {
                    case '1':
                        seznamPojistek.Add(VratPojistku());
                        break;
                    case '2':

                        break;
                }
            }
        }

        static ClassPojistka VratPojistku()
        {
            string jmenoPojistitele = ZadavaniOdUzivatele(KontrolaNeprazdnosti, "Jméno pojistitele", "Nesmí být prázdné");
            string prijmeniPojistitele = ZadavaniOdUzivatele(KontrolaNeprazdnosti, "Příjmení pojistitele", "Nesmí být prázdné");
            ClassOsoba pojistitel = new ClassOsoba(jmenoPojistitele,prijmeniPojistitele);

            string cisloPojistky = ZadavaniOdUzivatele(KontrolaCislaPojistky, "Číslo Pojistky", "Minimálně 5 znaků");
            int cilovaCastka = ZadavaniOdUzivatele(KontrolaCiloveCastky, "Cílová částka", "Musí být číslo, vyplněná a kladná");
            ClassPojistka pojistka = new ClassPojistka(cisloPojistky, pojistitel, cilovaCastka);
            return pojistka;
        }

        static dynamic ZadavaniOdUzivatele(KontrolaZadavani kontrola, string nazev="", string podminky="")
        {
            string vstup;
            do
            {
                if (nazev != "") Console.Write("Nyní zadáváte \"{0}\" ",nazev);
                if (podminky != "") Console.Write("(Podmínky: {0})",podminky);
                if (nazev == "" && podminky == "") Console.Write("Zadejte hodnotu");
                Console.WriteLine();
                Console.Write("> ");
                vstup = Console.ReadLine();
            } while (kontrola(vstup));

            return vstup;
        }

        static bool KontrolaCislaPojistky(string line)
        {
            return line.Length >= 5;
        }

        static bool KontrolaCiloveCastky(string line)
        {
            double number = 0;
            if (line == null) return false;
            if (!double.TryParse(line, out number)) return false;
            if (number <= 0) return false;

            return true;
        }

        static bool KontrolaNeprazdnosti(string line)
        {
            if (line == null) return false;

            return true;
        }



        static void NapisMenu()
        {
            Console.WriteLine("1..Přidání pojistky");
            Console.WriteLine("2..Přidání pojištěné osoby k pojistce");
            Console.WriteLine("3..Výpis všech pojistek");
            Console.WriteLine("4..Výpis údajů konkrétního pojistky vč. pojištěných osob");
            Console.WriteLine("U..Uložení do souboru");
            Console.WriteLine("N..Načtení ze souboru");
            Console.WriteLine("K..Konec");
            Console.Write("\nVolba: ");
        }
    }
}
