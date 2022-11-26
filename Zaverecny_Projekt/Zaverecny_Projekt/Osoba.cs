using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaverecny_Projekt
{
    public class Osoba
    {
        public string Jmeno;//{ get; private set; }
        public string Prijmeni;//{ get; private set; }
        public DateTime DatumNarozeni;//{ get; private set; }
        public string Team;//{ get; private set; }
        public string Pozice;//{ get; private set; }

        public Osoba(string jmeno, string prijmeni, DateTime datumNarozeni, string team, string pozice)
        {
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            //Vek = VypocitejVekRoky(datumNarozeni);
            DatumNarozeni = datumNarozeni;
            Team = team;
            Pozice = pozice;

        }
        public Osoba() { }
    }

    public class Hrac : Osoba
    {
        public int PocetGolu; //{ get; private set; }
        // public int PocetOdehranychMinut { get; private set; }

        public Hrac(string jmeno, string prijmeni, DateTime datumNarozeni, string team, string pozice, int pocetGolu = 0) : base(jmeno, prijmeni, datumNarozeni, team, pozice)
        {
            PocetGolu = pocetGolu;
        }
        public void ZapisGolStrelcovi()
        {
            PocetGolu += 1;
        }
        public Hrac() { }
    }

}