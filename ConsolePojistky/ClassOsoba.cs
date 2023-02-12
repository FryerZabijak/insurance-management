using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePojistky
{
    [Serializable]
    internal class ClassOsoba
    {
        string jmeno;
        string prijmeni;
        public string CeleJmeno { get { return String.Format("{0} {1}",jmeno,prijmeni); } }

        public ClassOsoba(string jmeno, string prijmeni)
        {
            this.jmeno = jmeno;
            this.prijmeni = prijmeni;
        }

        public static ClassOsoba VratInstanci(string kdo = "")
        {
            string jmeno = KontrolorVstupu.ZadavaniOdUzivatele(kdo + " Jméno", "Nesmí být prázdné", KontrolorVstupu.KontrolaNeprazdnosti);
            string prijemni = KontrolorVstupu.ZadavaniOdUzivatele(kdo + " Příjmení", "Nesmí být prázdné", KontrolorVstupu.KontrolaNeprazdnosti);
            ClassOsoba osoba = new ClassOsoba(jmeno, prijemni);
            return osoba;
        }

        public override string ToString()
        {
            return CeleJmeno;
        }
    }
}
