using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePojistky
{
    [Serializable]
    internal class ClassPojistka
    {
        string _cisloPojistky;
        ClassOsoba _pojistitel;
        int _cilovaCastka;
        public string CisloPojistky
        {
            get { return _cisloPojistky; }
        }
        public ClassOsoba Pojistitel
        {
            get { return _pojistitel; }
        }
        public int CilovaCastka
        {
            get { return _cilovaCastka; }
        }

        /// <summary>
        /// Konstruktor Třídy ClassPojistka
        /// </summary>
        /// <param name="cisloPojistky">Zadejte Číslo Pojistky (Musí mít alespoň 5 znaků)</param>
        /// <param name="pojistitel">Zadejte pojistitele typu "ClassOsoba"</param>
        /// <param name="cilovaCastka">Zadejte Cílovou Částku (Musí být vyplněná a kladná)</param>
        /// <exception cref="ArgumentException">Nebyly zadány hodnoty ve vyžádaném formátu</exception>
        public ClassPojistka(string cisloPojistky, ClassOsoba pojistitel, int cilovaCastka)
        {
            if (cisloPojistky.Length < 5) throw new ArgumentException("Číslo pojištění musí mít alespoň 5 znaků");
            if (cilovaCastka.ToString().Length <= 0) throw new ArgumentException("Částka musí být vyplněná");
            if (cilovaCastka < 0) throw new ArgumentException("Částka musí být kladná");
            if (pojistitel == null) throw new ArgumentNullException("Pojistitel nesmí být prázdný");

            _cisloPojistky = cisloPojistky;
            _pojistitel = pojistitel;
            _cilovaCastka = cilovaCastka;
        }

        public static ClassPojistka VratPojistku()
        {
            ClassOsoba pojistitel = ClassOsoba.VratOsobu("Pojistitel");

            string cisloPojistky = KontrolorVstupu.ZadavaniOdUzivatele("Číslo Pojistky", "Minimálně 5 znaků", KontrolorVstupu.KontrolaDelky5);
            int cilovaCastka = int.Parse(KontrolorVstupu.ZadavaniOdUzivatele("Cílová částka", "Musí být číslo, vyplněná a kladná", KontrolorVstupu.KontrolaNeprazdnosti, KontrolorVstupu.KontrolaCisla, KontrolorVstupu.KontrolaKladnosti));
            ClassPojistka pojistka = new ClassPojistka(cisloPojistky, pojistitel, cilovaCastka);

            return pojistka;
        }

        public static void VypisPojistky(dynamic seznamPojistek)
        {
            int index = 0;
            foreach (KeyValuePair<ClassPojistka, List<ClassOsoba>> pojistka in seznamPojistek)
            {
                Console.WriteLine("{0}. {1}", (++index), pojistka.Key);
            }
        }

        public static bool KontrolaNeprazdnostiPojistek(Dictionary<ClassPojistka, List<ClassOsoba>> seznamPojistek)
        {
            if (seznamPojistek.Count <= 0) return false;
            return true;
        }

        public static Dictionary<ClassPojistka, List<ClassOsoba>> VratPojistkuElement(int index, Dictionary<ClassPojistka, List<ClassOsoba>> seznamPojistek)
        {
            ClassPojistka klic = seznamPojistek.ElementAt(index).Key;
            List<ClassOsoba> hodnota = seznamPojistek.ElementAt(index).Value;
            Dictionary<ClassPojistka, List<ClassOsoba>> element = new Dictionary<ClassPojistka, List<ClassOsoba>>
            {
                { klic, hodnota }
            };
            return element;
        }
        public override string ToString()
        {
            return String.Format("Pojistka {0} - {1} CZK", CisloPojistky, CilovaCastka);
        }
    }
}
