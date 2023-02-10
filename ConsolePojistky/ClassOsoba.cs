using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePojistky
{
    internal class ClassOsoba
    {
        string jmeno;
        string prijmeni;
        public string CeleJmeno { get { return String.Format("{0} {1}",jmeno,prijmeni); } }

        public ClassOsoba(string jmeno, string prijmeni)
        {
            this.jmeno= jmeno;
            this.prijmeni= prijmeni;
        }
    }
}
