using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaverecny_Projekt
{

    //Databaze Zapasu futbalove ligy
    //- kazdy hrac ma vlastnosti - jmeno, team, pozice, vek, pocet golu
    //- kazdy team ma vlastnosti - mesto, ... TBD
    //Uzivatel muze :
    //- zadat noveho hrace
    //- vypsat hrace podle teamu nebo podle pozice
    //- vyhledat hrace
    //- smazet team(hraci bez teamu bud taky vymazani nebo budu "bez teamu")
    //- zadat zapas - trida Zapas(sestava, vysledek, kto dal goly)
    //Kral strelcu za ligu, za teamy




    public class Osoba
    {
        public string Jmeno;//{ get; private set; }
        public string Prijmeni;//{ get; private set; }
        public DateTime DatumNarozeni;//{ get; private set; }
        //public int Vek;//{ get; private set; }
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


    public class Team
    {
        public string Jmeno;

        public string Mesto;

        public string Soutez ;

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

    public class Zapas
    {
        public string Domaci;

        public string Hoste;

        public int GolyDomaciPocet;
        public int GolyHostePocet;

        public List<Hrac> GolyDomaciStrelci;
        public List<Hrac> GolyHosteStrelci;


        public Zapas(string domaci, string hoste, int golyDomaciPocet, int golyHostePocet, List<Hrac> golyDomaciStrelci, List<Hrac> golyHosteStrelci)
        {
            Domaci = domaci; // musi byt z teamu 
            Hoste = hoste;
            GolyDomaciPocet = golyDomaciPocet;
            GolyHostePocet = golyHostePocet;
            GolyDomaciStrelci = golyDomaciStrelci;
            GolyHosteStrelci = golyHosteStrelci;
        }


        public Zapas() { }
    }

}