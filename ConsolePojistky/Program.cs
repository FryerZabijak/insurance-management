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

        //Začátek 2. Cvičení
        static Dictionary<ClassPojistka, List<ClassOsoba>> seznamPojistek = new Dictionary<ClassPojistka, List<ClassOsoba>>();

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
                            ClassPojistka pojistka = ClassPojistka.VratPojistku(seznamPojistek);
                            ClassOsoba pojisteny = ClassOsoba.VratInstanci("Pojištěný");
                            seznamPojistek.Add(pojistka, new List<ClassOsoba> { pojisteny });

                            Pokracovani();
                        }
                        break;
                    case '2':
                        {
                            if (UpozorneniNaNevytvorenePojistky()) break;

                            ClassPojistka.VypisPojistky(seznamPojistek);

                            int index = 0;
                            do
                            {
                                index = int.Parse(KontrolorVstupu.ZadavaniOdUzivatele("Pořadové Číslo", "číslo", KontrolorVstupu.KontrolaCisla)) - 1;
                            } while (!KontrolorVstupu.ZkontrolujPlatnostIndexu(index, seznamPojistek.Count));
                            
                            Dictionary <ClassPojistka, List<ClassOsoba>> pojistka = ClassPojistka.VratPojistkuElement(index, seznamPojistek);

                            foreach (KeyValuePair<ClassPojistka, List<ClassOsoba>> p in pojistka)
                            {
                                ClassPojistka klic = p.Key;
                                List<ClassOsoba>  hodnota = p.Value;

                                if (seznamPojistek.ContainsKey(klic))
                                {
                                    List<ClassOsoba> h = seznamPojistek[klic];
                                    hodnota.Add(ClassOsoba.VratInstanci("Nový Pojištěněc"));
                                    seznamPojistek[klic] = hodnota;
                                }
                                Console.WriteLine("\nNový pojištěnec byl úspěšně přidán do Pojistky " +klic);
                            }

                            Pokracovani();
                        }
                        break;
                    case '3':
                        {
                            if (UpozorneniNaNevytvorenePojistky()) break;


                            ClassPojistka.VypisPojistky(seznamPojistek);
                            Pokracovani();
                        }
                        break;
                    case '4':
                        {
                            if (UpozorneniNaNevytvorenePojistky()) break;

                            ClassPojistka.VypisPojistky(seznamPojistek);

                            int index = 0;
                            do {
                                index = int.Parse(KontrolorVstupu.ZadavaniOdUzivatele("Pořadové Číslo", "číslo", KontrolorVstupu.KontrolaCisla)) - 1;
                            } while (!KontrolorVstupu.ZkontrolujPlatnostIndexu(index, seznamPojistek.Count));

                                Dictionary <ClassPojistka, List<ClassOsoba>> pojistka = ClassPojistka.VratPojistkuElement(index, seznamPojistek);
                            Console.WriteLine("Pojistitel: {0}\n" +
                                "Číslo Pojistky: {1}\n" +
                                "Cílová Čáska: {2} CZK", 
                                pojistka.ElementAt(0).Key.Zpracovatel, pojistka.ElementAt(0).Key.CisloPojistky,pojistka.ElementAt(0).Key.CilovaCastka);
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
                    case '5':
                        {
                            if (UpozorneniNaNevytvorenePojistky()) break;

                            List<ClassPracovnik> pracovnici = ClassPojistka.ZiskejVsechnyPracovniky(seznamPojistek);
                            ClassPojistka.VypisList(pracovnici);

                            int index = 0;
                            do
                            {
                                index = int.Parse(KontrolorVstupu.ZadavaniOdUzivatele("Číslo Pracovníka", "Neprázdné, číslo", KontrolorVstupu.KontrolaNeprazdnosti, KontrolorVstupu.KontrolaCisla, KontrolorVstupu.KontrolaKladnosti))-1;
                            } while (!KontrolorVstupu.ZkontrolujPlatnostIndexu(index, pracovnici.Count));

                            ClassPracovnik finalPracovnik = pracovnici[index];

                            foreach(KeyValuePair<ClassPojistka, List<ClassOsoba>> pojistka in seznamPojistek)
                            {
                                if (pojistka.Key.Zpracovatel == finalPracovnik)
                                {
                                    Console.WriteLine(pojistka.Key);
                                }
                            }
                            Pokracovani();
                        }
                        break;
                    case 'U': case 'u':
                        Uloz(seznamPojistek);
                        break;
                    case 'N': case 'n':
                        var nactenePojistky = Nacti();
                        if (nactenePojistky.Count > 0) seznamPojistek = nactenePojistky;
                        break;
                    case 'K': case 'k':
                        chod_programu = false;
                        break;
                }
            }
            Console.WriteLine("Děkuji za použití aplikace");
            Pokracovani();
        }



        public static void Pokracovani()
        {
            Console.WriteLine("\nStiskněte Enter pro pokračování...");
            Console.ReadLine();
        }

        static bool UpozorneniNaNevytvorenePojistky()
        {
            if (!ClassPojistka.KontrolaNeprazdnostiPojistek(seznamPojistek))
            {
                Console.WriteLine("Nejsou vytvořeny žádné pojistky");
                Pokracovani();
                return true;
            }

            return false;
        }

        static void Uloz(Dictionary<ClassPojistka, List<ClassOsoba>> seznam)
        {
            string nazev_souboru = null;
            Console.WriteLine("1. Vybrat existující soubor");
            Console.WriteLine("2. Vytvořit nový soubor");
            int volba = int.Parse(KontrolorVstupu.ZadavaniOdUzivatele("Odpověď", "Musí být neprázdný, číslo", KontrolorVstupu.KontrolaNeprazdnosti, KontrolorVstupu.KontrolaCisla));
            
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
                        int index_souboru = 0;
                        do
                        {
                            index_souboru = int.Parse(KontrolorVstupu.ZadavaniOdUzivatele("Číslo názvu souboru pro uložení", "Nesmí být prázdný, číslo", KontrolorVstupu.KontrolaNeprazdnosti, KontrolorVstupu.KontrolaCisla)) - 1;
                        } while (!KontrolorVstupu.ZkontrolujPlatnostIndexu(index_souboru, finalNazvy.Count));

                        nazev_souboru = finalNazvy[index_souboru];
                    }
                    break;
                case 2:
                    {
                        nazev_souboru = KontrolorVstupu.ZadavaniOdUzivatele("Název souboru pro uložení", "Nesmí být prázdný, Dodržujte prosím správné pojmenování souborů ve svém operačním systému", KontrolorVstupu.KontrolaNeprazdnosti);
                    }
                    break;
                default:
                    Console.WriteLine("Neplatná volba");
                    Pokracovani();
                    return;
            }
            char odpoved = 'n';
            if (SouborExistuje(nazev_souboru + ".dat"))
            {
                Console.Write("Soubor s tímto názvem již existuje, přejete si jej přepsat? (a/n)\n> ");
                odpoved = Console.ReadKey(false).KeyChar;
            }
            if (odpoved == 'a')
            {

                FileStream fs = new FileStream(nazev_souboru + ".dat", FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, seznam);
                fs.Close();

                Console.WriteLine("Data byla úspěšně uložena do souboru {0}.dat", nazev_souboru);
            }
            else
            {
                Console.WriteLine("\nSoubor nebyl uložen.");
            }
            Pokracovani();
        }

        public static bool SouborExistuje(string nazevSouboru)
        {
            string cesta = Path.Combine(Directory.GetCurrentDirectory(), nazevSouboru);
            return File.Exists(cesta);
        }

        static Dictionary<ClassPojistka, List<ClassOsoba>> Nacti()
        {
            List<string> finalNazvy = VypisSoubory(Directory.GetCurrentDirectory(),pripona:"dat");
            if(finalNazvy.Count == 0)
            {
                Console.WriteLine("Nejsou zde žádné soubory k načtení.");
                Pokracovani();
                return new Dictionary<ClassPojistka, List<ClassOsoba>>();
            }

            int index_souboru = 0;
            do
            {
                index_souboru = int.Parse(KontrolorVstupu.ZadavaniOdUzivatele("Číslo souboru pro načtení", "Nesmí být prázdný, musí být číslo", KontrolorVstupu.KontrolaNeprazdnosti)) - 1;
            } while (!KontrolorVstupu.ZkontrolujDelku(index_souboru, finalNazvy.Count) || index_souboru<0);

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

        static void NapisMenu()
        {
            Console.WriteLine("Vítej v aplikaci pro správu databáze pojistek");
            Console.WriteLine("Vytvořil: Pepa Mráz");
            Console.WriteLine("https://github.com/FryerZabijak/ConsolePojistky (Pro zobrazení CTRL + Levé tlačítko myši)\n");
            Console.WriteLine("1..Přidání pojistky");
            Console.WriteLine("2..Přidání pojištěné osoby k pojistce");
            Console.WriteLine("3..Výpis všech pojistek");
            Console.WriteLine("4..Výpis údajů konkrétního pojistky vč. pojištěných osob");
            Console.WriteLine("5..Výpis všech pojistek uzavřených konkrétním pracovníkem");
            Console.WriteLine("U..Uložení do souboru");
            Console.WriteLine("N..Načtení ze souboru");
            Console.WriteLine("K..Konec");
            Console.Write("\nVolba: ");
        }
    }
}
