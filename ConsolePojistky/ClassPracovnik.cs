using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePojistky
{
    [Serializable]
    internal class ClassPracovnik : ClassOsoba
    {
        public DateTime DatumNastupu { get; set; }
        public ClassPracovnik(string jmeno, string prijmeni, DateTime datumNastupu) : base(jmeno, prijmeni)
        {
            DatumNastupu= datumNastupu;
        }

        public ClassPracovnik(string jmeno, string prijmeni) : base(jmeno, prijmeni)
        {
            DatumNastupu = DateTime.Today;
        }

        public static ClassPracovnik VratInstanci(DateTime datumNastupu, string kdo="Pracovník")
        {
            string jmeno = KontrolorVstupu.ZadavaniOdUzivatele(kdo + " Jméno", "Nesmí být prázdné", KontrolorVstupu.KontrolaNeprazdnosti);
            string prijemni = KontrolorVstupu.ZadavaniOdUzivatele(kdo + " Příjmení", "Nesmí být prázdné", KontrolorVstupu.KontrolaNeprazdnosti);
            ClassPracovnik osoba = new ClassPracovnik(jmeno, prijemni, datumNastupu);
            return osoba;
        }

        public static new ClassPracovnik VratInstanci(string kdo = "Pracovník")
        {
            string jmeno = KontrolorVstupu.ZadavaniOdUzivatele(kdo + " Jméno", "Nesmí být prázdné", KontrolorVstupu.KontrolaNeprazdnosti);
            string prijemni = KontrolorVstupu.ZadavaniOdUzivatele(kdo + " Příjmení", "Nesmí být prázdné", KontrolorVstupu.KontrolaNeprazdnosti);
            ClassPracovnik osoba = new ClassPracovnik(jmeno, prijemni);
            return osoba;
        }

        public override string ToString()
        {
            return String.Format("Pracovník {0}, Datum nástupu: {1}",CeleJmeno,DatumNastupu.ToShortDateString());
        }
    }
}
