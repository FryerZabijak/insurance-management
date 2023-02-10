using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Markup;
using System.Reflection;

namespace ConsolePojistky
{
    internal class Program
    {
        static Dictionary<ClassPojistka, List<ClassOsoba>> seznamPojistek = new Dictionary<ClassPojistka, List<ClassOsoba>>();
        delegate bool KontrolaZadavani(string line);
        static void Main(string[] args)
        {
            bool chod_programu = true;
            while (chod_programu)
            {
                Console.Clear();
                NapisMenu();
                char step = Console.ReadKey(false).KeyChar;
                Console.Clear();
                switch (step)
                {
                    case '1':
                        {
                            ClassPojistka pojistka = VratPojistku();
                            ClassOsoba pojisteny = VratOsobu("Pojištěný");
                            seznamPojistek.Add(pojistka, new List<ClassOsoba> { pojisteny });
                        }
                        break;
                    case '2':
                        {
                            if (!KontrolaPrazdnostiPojistek(seznamPojistek)) break;

                            VypisPojistky(seznamPojistek);
                            int index = int.Parse(ZadavaniOdUzivatele(KontrolaCisla, "Pořadové Číslo", "číslo"))-1;
                            Dictionary<ClassPojistka, List<ClassOsoba>> pojistka = VratPojistkuElement(index, seznamPojistek);

                            foreach (KeyValuePair<ClassPojistka, List<ClassOsoba>> p in pojistka)
                            {
                                ClassPojistka klic = p.Key;
                                List<ClassOsoba>  hodnota = p.Value;

                                if (seznamPojistek.ContainsKey(klic))
                                {
                                    List<ClassOsoba> h = seznamPojistek[klic];
                                    hodnota.Add(VratOsobu("Nový Pojištěněc"));
                                    seznamPojistek[klic] = hodnota;
                                }
                                Console.WriteLine("\nNový pojištěnec byl úspěšně přidán do Pojistky " +klic);
                            }

                            Pokracovani();
                        }
                        break;
                    case '3':
                        {
                            if (!KontrolaPrazdnostiPojistek(seznamPojistek)) break;

                            VypisPojistky(seznamPojistek);
                            Pokracovani();
                        }
                        break;
                    case '4':
                        {
                            if (!KontrolaPrazdnostiPojistek(seznamPojistek)) break;

                            VypisPojistky(seznamPojistek);
                            int index = int.Parse(ZadavaniOdUzivatele(KontrolaCisla, "Pořadové Číslo", "číslo"))-1;
                            Dictionary<ClassPojistka, List<ClassOsoba>> pojistka = VratPojistkuElement(index, seznamPojistek);
                            foreach(KeyValuePair<ClassPojistka, List<ClassOsoba>> p in pojistka)
                            {
                                int poradi = 0;
                                foreach(ClassOsoba osoba in p.Value)
                                {
                                    Console.WriteLine(++poradi+". "+osoba.ToString());
                                }
                            }
                            Pokracovani();
                        }
                        break;
                    case 'U': case 'u':
                        Uloz(seznamPojistek);
                        break;
                    case 'N': case 'n':
                        seznamPojistek = Nacti();
                        break;
                    case 'K': case 'k':
                        chod_programu = false;
                        break;
                }

                Console.WriteLine("Děkuji za použití aplikace");
                Pokracovani();
            }
        }

        static void VypisSoubory(string cesta)
        {
            string[] files = Directory.GetFiles(cesta);
            foreach (string file in files)
            {
                if (file.Split('.')[1] == "dat")
                {
                    string[] podslozky = file.Split('\\');
                    Console.WriteLine(podslozky[podslozky.Length - 1]);
                }
            }
        }

        static bool KontrolaPrazdnostiPojistek(Dictionary<ClassPojistka, List<ClassOsoba>> seznamPojistek)
        {
            if (seznamPojistek.Count<=0)
            {
                Console.WriteLine("Nejsou vytvořené žádné pojistky");
                Pokracovani();
                return false;
            }
            return true;
        }

        static void Pokracovani()
        {
            Console.WriteLine("\nStiskněte Enter pro pokračování...");
            Console.ReadLine();
        }

        static void Uloz(Dictionary<ClassPojistka, List<ClassOsoba>> seznam)
        {
            string nazev_souboru = null;
            Console.WriteLine("1. Vybrat existující soubor");
            Console.WriteLine("2. Vytvořit nový soubor");
            int volba = int.Parse(ZadavaniOdUzivatele(KontrolaNeprazdnosti, "Odpověď", "Musí být neprázdný, číslo", KontrolaCisla));
            
            switch (volba)
            {
                case 1:
                    {
                        List<string> finalNazvy = VypisSoubory(Directory.GetCurrentDirectory(), pripona: "dat");
                        if (finalNazvy.Count == 0)
                        {
                            Console.WriteLine("Nejsou vytvořeny žádné soubory");
                            Pokracovani();
                            return;
                        }
                        int index_souboru = int.Parse(ZadavaniOdUzivatele(KontrolaNeprazdnosti, "Číslo názvu souboru pro uložení", "Nesmí být prázdný, číslo", KontrolaCisla))-1;
                        if (!ZkontrolujDelku(index_souboru, finalNazvy.Count - 1))
                        {
                            Console.WriteLine("Neplatná volba");
                            Pokracovani();
                            return;
                        }

                        nazev_souboru = finalNazvy[index_souboru];
                    }
                    break;
                case 2:
                    {
                        nazev_souboru = ZadavaniOdUzivatele(KontrolaNeprazdnosti, "Název souboru pro uložení", "Nesmí být prázdný");
                    }
                    break;
                default:
                    Console.WriteLine("Neplatná volba");
                    Pokracovani();
                    return;
            }

            FileStream fs = new FileStream(nazev_souboru+".dat", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, seznam);
            fs.Close();

            Console.WriteLine("Data byla úspěšně uložena do souboru {0}.dat",nazev_souboru);
            Pokracovani();
        }

        static bool ZkontrolujDelku(int input, int delka)
        {
            return input <= delka;
        }

        static Dictionary<ClassPojistka, List<ClassOsoba>> Nacti()
        {
            List<string> finalNazvy = VypisSoubory(Directory.GetCurrentDirectory(),pripona:"dat");

            int index_souboru = 0;
            do
            {
                index_souboru = int.Parse(ZadavaniOdUzivatele(KontrolaNeprazdnosti, "Číslo souboru pro načtení", "Nesmí být prázdný, musí být číslo")) - 1;
            } while (!ZkontrolujDelku(index_souboru, finalNazvy.Count - 1) || index_souboru<0);

            string nazev_souboru = finalNazvy[index_souboru];
            FileStream fs = new FileStream(nazev_souboru + ".dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            Dictionary<ClassPojistka, List<ClassOsoba>> seznam = (Dictionary <ClassPojistka, List <ClassOsoba>>)bf.Deserialize(fs);
            fs.Close();

            Console.WriteLine("Data byla úspěšně načtena ze souboru {0}.dat", nazev_souboru);
            Pokracovani();
            return seznam;
        }

        static List<string> VypisSoubory(string cesta, string pripona="")
        {
            string[] soubory = Directory.GetFiles(cesta);
            List<string> finalNazvy = new List<string>();
            int index = 0;
            foreach (string soubor in soubory)
            {
                if (soubor.Split('.')[1] == pripona || pripona=="")
                {
                    string[] podslozky = soubor.Split('\\');
                    finalNazvy.Add(podslozky[podslozky.Length - 1].Split('.')[0]);
                    Console.WriteLine((++index) + ". " + podslozky[podslozky.Length - 1]);
                }
            }
            return finalNazvy;
        }

        static ClassPojistka VratPojistku()
        {
            ClassOsoba pojistitel = VratOsobu("Pojistitel");

            string cisloPojistky = ZadavaniOdUzivatele(KontrolaDelky5,"Číslo Pojistky", "Minimálně 5 znaků");
            int cilovaCastka = int.Parse(ZadavaniOdUzivatele(KontrolaNeprazdnosti, "Cílová částka", "Musí být číslo, vyplněná a kladná", KontrolaCisla, KontrolaKladnosti));
            ClassPojistka pojistka = new ClassPojistka(cisloPojistky, pojistitel, cilovaCastka);

            return pojistka;
        }

        static ClassOsoba VratOsobu(string kdo="")
        {
            string jmeno = ZadavaniOdUzivatele(KontrolaNeprazdnosti,kdo+" Jméno", "Nesmí být prázdné");
            string prijemni= ZadavaniOdUzivatele(KontrolaNeprazdnosti, kdo+" Příjmení", "Nesmí být prázdné");
            ClassOsoba osoba = new ClassOsoba(jmeno, prijemni);
            return osoba;
        }

        static void VypisPojistky(dynamic seznamPojistek)
        {
            int index = 0;
            foreach (KeyValuePair<ClassPojistka,List<ClassOsoba>> pojistka in seznamPojistek)
            {
                Console.WriteLine("{0}. {1}", (++index), pojistka.Key);
            }
        }

        static Dictionary<ClassPojistka, List<ClassOsoba>> VratPojistkuElement(int index, Dictionary<ClassPojistka, List<ClassOsoba>> seznamPojistek)
        {
            ClassPojistka klic = seznamPojistek.ElementAt(index).Key;
            List <ClassOsoba> hodnota = seznamPojistek.ElementAt(index).Value;
            Dictionary<ClassPojistka, List<ClassOsoba>> element = new Dictionary<ClassPojistka, List<ClassOsoba>>();
            element.Add(klic, hodnota);
            return element;
        }

        static dynamic ZadavaniOdUzivatele(KontrolaZadavani kontrola, string nazev = "", string podminky = "", params KontrolaZadavani[] kontroly)
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

                KontrolaZadavani[] vsechny_kontroly = new KontrolaZadavani[1 + (kontroly.Length)];
                vsechny_kontroly[0] = kontrola;

                if (kontroly.Length > 0)
                {
                    for (int i = 1; i < vsechny_kontroly.Length; i++)
                        vsechny_kontroly[i] = kontroly[i - 1];
                }

                valid = true;
                for (int i = 0; i < vsechny_kontroly.Length; i++)
                {
                    if (!vsechny_kontroly[i](vstup))
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

        static bool KontrolaCisla(string line)
        {
            if (int.TryParse(line, out int n)) return true;
            return false;
        }

        static bool KontrolaKladnosti(string line)
        {
            if (int.Parse(line) > 0) return true;
            return false;
        }

        static bool KontrolaDelky5(string line)
        {
            if(line.Length<5) return false;
            return true;
        }

        static bool KontrolaNeprazdnosti(string line)
        {
            if (line == null || line=="") return false;

            return true;
        }

        static void NapisMenu()
        {
            Console.WriteLine("Vítej v aplikaci pro správu databáze pojistek");
            Console.WriteLine("Vytvořil: Pepa Mráz");
            Console.WriteLine("https://github.com/FryerZabijak/ConsolePojistky (Pro zobrazení CTRL + Levé tlačítko myši)\n");
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
