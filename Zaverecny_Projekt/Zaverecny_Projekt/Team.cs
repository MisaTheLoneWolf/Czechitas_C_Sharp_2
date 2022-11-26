using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaverecny_Projekt
{
    public class Team
    {
        public string Jmeno;
        public string Mesto;
        public string Soutez;
        public int PocetOdohratychZapasov;
        public int PocetBodu;

        public Team(string jmeno, string mesto, string soutez, int pocetOdohratychZapasov, int pocetBodu)
        {
            Jmeno = jmeno;
            Mesto = mesto;
            Soutez = soutez;
            PocetOdohratychZapasov = pocetOdohratychZapasov;
            PocetBodu = pocetBodu;
        }
        public void ZapisBodyTeamu(int body)
        {
            PocetBodu += body;
        }
        public Team() { }
    }
}
