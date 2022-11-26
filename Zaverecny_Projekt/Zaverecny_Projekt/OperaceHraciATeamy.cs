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
    public class OperaceHraciATeamy
    {
        public void PoradiTeamuDleBodu(List<Team> vsechnyJestvujiciTeamy)
        {
            int poradi = 1;
            Console.WriteLine("Poradi   Nazev teamu  Pocet bodu");
            int pocetBoduMaxDotaz = vsechnyJestvujiciTeamy.Max(r => r.PocetBodu);


            List<Team> zbyleTeamy = vsechnyJestvujiciTeamy;

            int MinulyTeamPocetMaxDotaz = 0;

            foreach (Team t in vsechnyJestvujiciTeamy.OrderByDescending(r => r.PocetBodu))
            {
                if (MinulyTeamPocetMaxDotaz == pocetBoduMaxDotaz && poradi != 1)
                {
                    Console.WriteLine($"         {t.Jmeno.PadRight(12)} {t.PocetBodu}");
                }
                else
                {
                    Console.WriteLine($"{poradi}.misto: {t.Jmeno.PadRight(12)} {t.PocetBodu}");
                }
                MinulyTeamPocetMaxDotaz = zbyleTeamy.Max(r => r.PocetBodu);
                zbyleTeamy = zbyleTeamy.Where(p => p.Jmeno != t.Jmeno).ToList();
                if (zbyleTeamy.Any()) // na prazdnom liste pada co catchu
                {
                    pocetBoduMaxDotaz = zbyleTeamy.Max(r => r.PocetBodu);
                }

                poradi++;

            }
        }

        public void NajlepsiStrelecLigyATeamu(List<Hrac> vsechniJestvujiciHraci, List<Team> vsechnyJestvujiciTeamy)
        {
            // Celkovo najlepsi strelec
            var maximumGolovDotaz = vsechniJestvujiciHraci.Max(x => x.PocetGolu);
            var hraciSMaxGolamiDotaz = vsechniJestvujiciHraci.Where(x => x.PocetGolu == maximumGolovDotaz);

            Console.WriteLine("Nejlepsi strelec nebo strelci ligy:");
            foreach (var t in hraciSMaxGolamiDotaz)
            {
                Console.WriteLine($"Team: " + t.Team);
                Console.WriteLine($"      " + (t.Jmeno + " " + t.Prijmeni).PadRight(20) + " Goly: " + t.PocetGolu);
            }

            // najlepsi strelci po teamoch
            var maximumGolovTeamDotaz = vsechniJestvujiciHraci.Max(x => x.PocetGolu);
            var hraciSMaxGolamiTeamDotaz = vsechniJestvujiciHraci.Where(x => x.PocetGolu == maximumGolovDotaz);
            Console.WriteLine(Environment.NewLine + "Nejlepsi strelec nebo strelci podle teamu:");

            foreach (Team m in vsechnyJestvujiciTeamy)
            {
                maximumGolovTeamDotaz = vsechniJestvujiciHraci.Where(y => y.Team == m.Jmeno).Max(x => x.PocetGolu);
                hraciSMaxGolamiTeamDotaz = vsechniJestvujiciHraci.Where(y => y.Team == m.Jmeno && y.PocetGolu == maximumGolovTeamDotaz);
                Console.WriteLine($"Team: " + m.Jmeno);

                foreach (var t in hraciSMaxGolamiTeamDotaz)
                {
                    Console.WriteLine($"      " + (t.Jmeno + " " + t.Prijmeni).PadRight(20) + " Goly: " + t.PocetGolu);
                }
            }
        }

        public void VyhledejHrace(int podminka, List<Hrac> vsechniJestvujiciHraci, List<Team> vsechnyJestvujiciTeamy)
        {
            //Console.WriteLine("1 - vsichni hraci");
            //Console.WriteLine("2 - hraci podle prijmeni");
            //Console.WriteLine("3 - hraci podle tymu");
            //Console.WriteLine("4 - hraci podle pozice");
            var kontrolyVstupov = new KontrolyVstupov();

            string klic = "";
            string aktualneTeamy = "";

            if (podminka == 2)
            {
                klic = " - prijmeni hrace:";
            }
            else if (podminka == 3)
            {
                foreach (Team t in vsechnyJestvujiciTeamy)
                {
                    aktualneTeamy = string.Concat(aktualneTeamy, t.Jmeno, ", ");
                }
                aktualneTeamy = aktualneTeamy.Substring(0, aktualneTeamy.Length - 2);
                klic = " - team hracu: (vyber z " + aktualneTeamy + ")";

            }
            else if (podminka == 4 )
            {
                klic = " - pozice hracu: (vyber z Brankar, Obrance, Utocnik)";
            }
            else
            {
                klic = ":";
            }

            string kriter = "";
            if (podminka != 1)
            {
                Console.WriteLine("Zadajte prosim vyhledavaci klic" + klic + ")");
                if (podminka == 3)
                    kriter = kontrolyVstupov.kontrolovanyString(kontrolyVstupov.KontrolaExistenceTeamu(vsechnyJestvujiciTeamy, Console.ReadLine()));
                else if (podminka == 4)
                    kriter = kontrolyVstupov.kontrolovanyString(kontrolyVstupov.KontrolaExistencePozice(Console.ReadLine()));
                else
                    kriter = kontrolyVstupov.kontrolovanyString(kontrolyVstupov.KontrolaExistenceHrace(vsechniJestvujiciHraci, Console.ReadLine()));
            }

            foreach (Hrac hrac in vyberHrace(podminka, kriter, vsechniJestvujiciHraci))
            {
                Console.WriteLine($"Hrac: " + (hrac.Jmeno + " " + hrac.Prijmeni).PadRight(20) + (hrac.DatumNarozeni.Day + "." + hrac.DatumNarozeni.Month + "." + hrac.DatumNarozeni.Year).PadRight(12) + hrac.Team.PadRight(12) + hrac.Pozice.PadRight(10) + " Goly: " + hrac.PocetGolu);
            }
        }


        public List<Hrac> vyberHrace(int typKriteria, string kriterium, List<Hrac> vsechniJestvujiciHraci)
        {
            List<Hrac> vybraniHraci = new List<Hrac>();

            for (int i = 0; i < vsechniJestvujiciHraci.Count; i++)
            {
              
                if (typKriteria == 4) // pozice
                {
                    if (vsechniJestvujiciHraci[i].Pozice == kriterium)
                        vybraniHraci.Add(vsechniJestvujiciHraci[i]);
                }
                else if (typKriteria == 3) // team
                {
                    if (vsechniJestvujiciHraci[i].Team == kriterium)
                        vybraniHraci.Add(vsechniJestvujiciHraci[i]);
                }
                else if (typKriteria == 2) // Prijmeni
                {
                    if (vsechniJestvujiciHraci[i].Prijmeni == kriterium)
                        vybraniHraci.Add(vsechniJestvujiciHraci[i]);
                }
                else if (typKriteria == 1) // vsichni hraci
                {
                    vybraniHraci.Add(vsechniJestvujiciHraci[i]);
                }
            }

            return vybraniHraci;
        }

        public Hrac zadejHrace(List<Team> vsechnyJestvujiciTeamy)
        {
            // Manualne Zadat noveho Hrace
            var kontrolyVstupov = new KontrolyVstupov();
            List<Hrac> hraci = new List<Hrac>();


            Console.WriteLine("Zadajte noveho hraca");
            Console.WriteLine("Zadajte krstni jmeno: ");
            string jmenoHrace = kontrolyVstupov.kontrolovanyString(Console.ReadLine());


            Console.WriteLine("Zadajte prijmeni: ");
            string prijmeniHrace = kontrolyVstupov.kontrolovanyString(Console.ReadLine());

            Console.WriteLine("Zadajte datum narozeni ve formatu YYYYMMDD: ");
            string datumHrace = kontrolyVstupov.kontrolovanyDatum(Console.ReadLine());


            string aktualneTeamy ="";
            foreach (Team t in vsechnyJestvujiciTeamy)
            {
                aktualneTeamy = string.Concat(aktualneTeamy, t.Jmeno, ", ");
            }
            aktualneTeamy = aktualneTeamy.Substring(0, aktualneTeamy.Length - 2);

            Console.WriteLine("Zadajte team: (vyber z " + aktualneTeamy + ")");
            string teamHrace = kontrolyVstupov.kontrolovanyString(kontrolyVstupov.KontrolaExistenceTeamu(vsechnyJestvujiciTeamy, Console.ReadLine()));

            Console.WriteLine("Zadajte pozici: (vyber z Brankar, Obrance, Utocnik)");
            string poziceHrace = kontrolyVstupov.kontrolovanyString(kontrolyVstupov.KontrolaExistencePozice(Console.ReadLine()));

            Hrac hrac1 = new Hrac(jmenoHrace, prijmeniHrace, DateTime.Parse(datumHrace, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None), teamHrace, poziceHrace);
            return hrac1;
        }


        public Hrac vymazHrace(List<Hrac> vsechniJestvujiciHraci, List<Team> vsechnyJestvujiciTeamy)
        {
            // Manualne Zadat noveho Hrace
            var kontrolyVstupov = new KontrolyVstupov();

            string aktualneTeamy = "";
            foreach (Team t in vsechnyJestvujiciTeamy)
            {
                aktualneTeamy = string.Concat(aktualneTeamy, t.Jmeno, ", ");
            }
            aktualneTeamy = aktualneTeamy.Substring(0, aktualneTeamy.Length - 2);

            Console.WriteLine("Zadajte team z ktereho chcete hrace vymazat: (vyber z " + aktualneTeamy + ")"); ;
            string teamHrace = kontrolyVstupov.kontrolovanyString(kontrolyVstupov.KontrolaExistenceTeamu(vsechnyJestvujiciTeamy, Console.ReadLine()));
            Console.WriteLine("");
            Console.WriteLine("Supiska teamu " + teamHrace + ":");
            foreach (var t in vsechniJestvujiciHraci.Where(r => r.Team == teamHrace))
            {
                Console.WriteLine(t.Jmeno + " " + t.Prijmeni);
            }

            Console.WriteLine("Zadajte prijmeni hrac, ktereho chcete vymazat (z teamu " + teamHrace + ")");
            string prijmeniHrace = kontrolyVstupov.kontrolovanyString(kontrolyVstupov.KontrolaExistenceHrace(vsechniJestvujiciHraci, Console.ReadLine()));

            var HracNaVymaz = vsechniJestvujiciHraci.Where(r => r.Prijmeni == prijmeniHrace && r.Team == teamHrace).ToList();
            Hrac hracBudeVymazan = new Hrac(HracNaVymaz[0].Jmeno, HracNaVymaz[0].Prijmeni, HracNaVymaz[0].DatumNarozeni, HracNaVymaz[0].Team, HracNaVymaz[0].Pozice, HracNaVymaz[0].PocetGolu);
            return hracBudeVymazan;
        }
    }
}
