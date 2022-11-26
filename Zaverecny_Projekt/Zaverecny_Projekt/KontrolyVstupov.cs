using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

namespace Zaverecny_Projekt
{
    public class KontrolyVstupov
    {

        public string kontrolovanyString(string vstupnyText)
        {
            while (string.IsNullOrEmpty(vstupnyText) || string.IsNullOrWhiteSpace(vstupnyText))
            {
                Console.WriteLine("Nezadali jse spravne vstup, prosim opakujte s vyplnenou hodnotou:");
                vstupnyText = Console.ReadLine();
            }
            return char.ToUpper(vstupnyText[0]) + vstupnyText.Substring(1).ToLower(CultureInfo.InvariantCulture); // prvni znak velke pismeno
        }

        public int kontrolovanyIntProFilter(string vstupnyIntjakString, int minimalneMozneCislo, int MaximalneMozneCislo)
        {

            int vstupnyInt = 0;
            while (!int.TryParse(vstupnyIntjakString, out vstupnyInt) || (vstupnyInt < minimalneMozneCislo && vstupnyInt > MaximalneMozneCislo))
            {
                Console.WriteLine("Nezadali jse spravne vstup, prosim opakujte se spravne vyplnenou hodnotou " + minimalneMozneCislo + "-" + MaximalneMozneCislo + ":");
                vstupnyIntjakString = Console.ReadLine();
            }
            return vstupnyInt;
        }

        public int kontrolovanyIntProPocetGolu(string vstupnyIntjakString)
        {

            int vstupnyInt = 0;
            while (!int.TryParse(vstupnyIntjakString, out vstupnyInt) || (vstupnyInt < 0))
            {
                Console.WriteLine("Nezadali jse spravne vstup pro pocet golu, prosim opakujte se spravne vyplnenou hodnotou:");
                vstupnyIntjakString = Console.ReadLine();
            }
            return vstupnyInt;
        }

        public string kontrolovanyDatum(string datumNarozeni)
        {
            // kontrola spravnosti zadaneho datumu narozeni

            // spravny pocet znaku
            while (datumNarozeni.Length != 8)
            {
                Console.WriteLine("Nezadali jse spravny datum narozeni ve formatu YYYYMMDD, prosim opakujte se spravne vyplnenou hodnotou: ");
                datumNarozeni = Console.ReadLine();
            }
            int rokNarozeni = 0;
            while (!int.TryParse(datumNarozeni.Substring(0, 4), out rokNarozeni) || (rokNarozeni < 1900 && rokNarozeni > DateTime.Now.Year))
            {
                Console.WriteLine("Nezadali jse spravny datum narozeni ve formatu YYYYMMDD, prosim opakujte se spravne vyplnenou hodnotou: ");
                datumNarozeni = Console.ReadLine();
            }

            int mesicNarozeni = 0;
            while (!int.TryParse(datumNarozeni.Substring(4, 2), out mesicNarozeni) || (mesicNarozeni > 12 && mesicNarozeni <= 0))
            {
                Console.WriteLine("Nezadali jse spravny datum narozeni ve formatu YYYYMMDD, prosim opakujte se spravne vyplnenou hodnotou: ");
                datumNarozeni = Console.ReadLine();
            }

            int denNarozeni = 0;
            while (!int.TryParse(datumNarozeni.Substring(4, 2), out denNarozeni) || (denNarozeni > 31 && denNarozeni <= 0))
            {
                Console.WriteLine("Nezadali jse spravny datum narozeni ve formatu YYYYMMDD, prosim opakujte se spravne vyplnenou hodnotou: ");
                datumNarozeni = Console.ReadLine();
            }

            string datum = denNarozeni + "/" + mesicNarozeni + "/" + rokNarozeni;
            DateTime datumNarozeniJakDatum;

            while (!DateTime.TryParse(datum, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out datumNarozeniJakDatum))
            {
                Console.WriteLine("Nezadali jse spravny datum narozeni ve formatu YYYYMMDD, prosim opakujte se spravne vyplnenou hodnotou: ");
                datumNarozeni = Console.ReadLine();
            }
            // nemuze byt buducnost
            while (datumNarozeniJakDatum > DateTime.Now)
            {
                Console.WriteLine("Nezadali jse spravny datum narozeni ve formatu YYYYMMDD, prosim opakujte se spravne vyplnenou hodnotou: ");
                datumNarozeni = Console.ReadLine();
            }
            return datum;
        }

        public string KontrolaExistenceTeamu(List<Team> vsechnyTeamy, string team)
        {

            string vyslednyTeam = "";
            foreach (Team p in vsechnyTeamy)
            {
                if (p.Jmeno.Equals(team, StringComparison.OrdinalIgnoreCase))
                {
                    vyslednyTeam = p.Jmeno;
                }
            }

            if (vyslednyTeam == "")
                Console.WriteLine("Byl zadan neexistujici team");
            return vyslednyTeam;
        }

        public string KontrolaExistenceHrace(List<Hrac> vsechniHraci, string hrac)
        {

            string vyslednyHrac = "";
            foreach (Hrac p in vsechniHraci)
            {
                if (p.Prijmeni.Equals(hrac, StringComparison.OrdinalIgnoreCase))
                {
                    vyslednyHrac = p.Prijmeni;
                }
            }

            if (vyslednyHrac == "")
                Console.WriteLine("Byl zadan neexistujici hrac");
            return vyslednyHrac;
        }

        public string KontrolaExistencePozice(string pozice)
        {
            List<string> vsechnyPozice = new List<string> { "Brankar", "Obrance", "Utocnik" }; // hrame maly futbal takze bez zalohy

            string vyslednaPozice = "";
            foreach (string p in vsechnyPozice)
            {
                if (p.Equals(pozice, StringComparison.OrdinalIgnoreCase))
                {
                    vyslednaPozice = p;
                }
            }

            if (vyslednaPozice == "")
                Console.WriteLine("Byla zadana nespravna pozice");
            return vyslednaPozice;
        }
    }
}
