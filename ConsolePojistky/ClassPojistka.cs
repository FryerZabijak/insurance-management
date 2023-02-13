using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePojistky
{
    [Serializable]
    sealed internal class ClassPojistka
    {
        string _cisloPojistky;
        ClassPracovnik _zpracovatel;
        int _cilovaCastka;
        public string CisloPojistky
        {
            get { return _cisloPojistky; }
        }
        public ClassPracovnik Zpracovatel
        {
            get { return _zpracovatel; }
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
        public ClassPojistka(string cisloPojistky, ClassPracovnik zpracovatel, int cilovaCastka)
        {
            if (cisloPojistky.Length < 5) throw new ArgumentException("Číslo pojištění musí mít alespoň 5 znaků");
            if (cilovaCastka.ToString().Length <= 0) throw new ArgumentException("Částka musí být vyplněná");
            if (cilovaCastka < 0) throw new ArgumentException("Částka musí být kladná");
            if (zpracovatel == null) throw new ArgumentNullException("Pojistitel nesmí být prázdný");

            _cisloPojistky = cisloPojistky;
            _zpracovatel = zpracovatel;
            _cilovaCastka = cilovaCastka;
        }

        public static List<ClassPracovnik> ZiskejVsechnyPracovniky(Dictionary<ClassPojistka, List<ClassOsoba>> seznamPojistek)
        {
            List<ClassPracovnik> pracovnici = new List<ClassPracovnik>();
            foreach (KeyValuePair<ClassPojistka, List<ClassOsoba>> p in seznamPojistek)
            {
                if (pracovnici.Contains(p.Key.Zpracovatel)) continue;
                pracovnici.Add(p.Key.Zpracovatel);
            }
            return pracovnici;
        }

        public static void VypisList<T>(List<T> list, bool indexace=true, int pocatecniCislo=1)
        {
            pocatecniCislo -= 1;
            foreach(T item in list)
            {
                if(indexace) { Console.Write(++pocatecniCislo+". "); }
                Console.Write(item+"\n");
            }
        }

        public static ClassPojistka VratPojistku(Dictionary<ClassPojistka, List<ClassOsoba>> seznamPojistek)
        {
            Console.WriteLine("Jakého pracovníka chcete k této pojistce přiřadit?");
            Console.WriteLine("1. Existujícího pracovníka");
            Console.WriteLine("2. Vytvořit nového pracovníka");
            int volba = 0;

            do
            {
                volba = int.Parse(KontrolorVstupu.ZadavaniOdUzivatele("Volba přiřazení pracovníka", "Nesmí být prázdné, číslo", KontrolorVstupu.KontrolaNeprazdnosti, KontrolorVstupu.KontrolaCisla, KontrolorVstupu.KontrolaKladnosti).ToString().Substring(0, 1));
            } while (!KontrolorVstupu.ZkontrolujPlatnostIndexu(volba, 2+1) || volba==0);

            ClassPracovnik pracovnik = new ClassPracovnik("random", "borec");

            if (volba == 1) {
                Console.WriteLine("Vybral jste možnost vybrat existujícího pracovníka.");
                List<ClassPracovnik> pracovnici = ZiskejVsechnyPracovniky(seznamPojistek);

                if (pracovnici.Count <= 0)
                {
                    Console.WriteLine("Ale v systému nejsou žádní pracovníci, takže vytvoříte nového");
                    volba = 2;
                }
                else
                {
                    VypisList(pracovnici);

                    int index = 0;
                    do
                    {
                        index = int.Parse(KontrolorVstupu.ZadavaniOdUzivatele("Číslo Pracovníka", "Neprázdné, číslo", KontrolorVstupu.KontrolaNeprazdnosti, KontrolorVstupu.KontrolaCisla))-1;
                    } while (!KontrolorVstupu.ZkontrolujPlatnostIndexu(index, pracovnici.Count));

                    pracovnik = pracovnici[index];
                }
            }
            if (volba == 2)
            {
                Console.WriteLine("Vytváříte nového pracovníka.");
                pracovnik = ClassPracovnik.VratInstanci("Pojistitel");
            }

            string cisloPojistky = KontrolorVstupu.ZadavaniOdUzivatele("Číslo Pojistky", "Minimálně 5 znaků", KontrolorVstupu.KontrolaDelky5);
            int cilovaCastka = int.Parse(KontrolorVstupu.ZadavaniOdUzivatele("Cílová částka", "Musí být číslo, vyplněná a kladná", KontrolorVstupu.KontrolaNeprazdnosti, KontrolorVstupu.KontrolaCisla, KontrolorVstupu.KontrolaKladnosti));
            ClassPojistka pojistka = new ClassPojistka(cisloPojistky, pracovnik, cilovaCastka);

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
