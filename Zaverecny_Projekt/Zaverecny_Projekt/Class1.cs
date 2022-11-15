﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaverecny_Projekt
{

//Databaze Zapasu futbalove ligy
//- kazdy hrac ma vlastnosti - jmeno, team, pozice, vek, pocet golu, pocet odehranych minut
//- kazdy team ma vlastnosti - mesto, ... TBD
//Uzivatel muze :
//- zadat noveho hrace
//- vypsat hrace podle teamu nebo podle pozice
//- vyhledat hrace
//- smazet team(hraci bez teamu bud taky vymazani nebo budu "bez teamu")
//- zadat zapas - trida Zapas(sestava, vysledek, kto dal goly)
//Kral strelcu za ligu, za teamy
//Kral odehranych minut(edited)



    public class Osoba 
    {
        public string Jmeno { get; private set; }
        public string Prijmeni { get; private set; }
        public DateTime DatumNarozeni { get; private set; }

        public string Team { get; private set; }


        public Osoba(string jmeno, string prijmeni, DateTime datumNarozeni, string team)
        {
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            DatumNarozeni = datumNarozeni;
            Team = team;    
        }


        public int VypocitejVekRoky()
        {
            DateTime Dneska = DateTime.Today;


            int DneskaJakInt = (Dneska.Year * 1000) + (Dneska.Month * 10) + (Dneska.Day);
            int DatumNarozeniJakInt = (DatumNarozeni.Year * 1000) + (DatumNarozeni.Month * 10) + (DatumNarozeni.Day);

            return (DneskaJakInt - DatumNarozeniJakInt) / 1000; // INT sa postara o desetatine miesta :D
        }

    }

    public class Hrac : Osoba
    {
        public string Pozice { get; private set; }

        public int PocetGolu { get; private set; }
        public int PocetOdehranychMinut { get; private set; }



        public Hrac(string jmeno, string prijmeni, DateTime datumNarozeni, string team, string pozice, int pocetGolu = 0 , int pocetOdehranychMinut = 0 ) : base (jmeno, prijmeni, datumNarozeni, team)
        {

            Pozice = KontrolaExistencePozice();

        }


        public string KontrolaExistencePozice()
        {
            string[] vsechnyPozice = new string[] { "brankar", "obrance", "zaloznik", "utocnik" };

            return "brankar";  // dodelat kontrolu
        }










    }



    public class Trener : Osoba
    {
        public Trener(string jmeno, string prijmeni, DateTime datumNarozeni, string team) : base(jmeno, prijmeni, datumNarozeni, team)
        {

        }

    }


    internal class Team
    {




    }



}