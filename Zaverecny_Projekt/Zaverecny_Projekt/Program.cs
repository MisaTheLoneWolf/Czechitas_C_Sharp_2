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
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initioal Load ze souboru pokud jestvuje
            // TEAMY:
            List<Team> vsechnyJestvujiciTeamy = InitialLoadTeam();

            // HRACI:
            List<Hrac> vsechniJestvujiciHraci = InitialLoadHrac();





            // Manualne zadej hrace
            bool promennaProVystupZCyklu = true;

            while (promennaProVystupZCyklu)
            {
                Console.WriteLine("Pokud chcete zadat dalšího hráče, stisknete a");
                if (Console.ReadLine() == "a")
                {
                    vsechniJestvujiciHraci.Add(zadejHrace());
                }
                else promennaProVystupZCyklu = false;
            }



            // NEJVETSI VYZVA - ZADAT ZAPAS A PRIRADIT GOLY HRACIM Z TEAMU

            promennaProVystupZCyklu = true;
            Console.WriteLine("Pokud chcete zadat vysledek zapasu, stisknete a");
            if (Console.ReadLine() == "a")
            {
                ZadejZapas();
            }
            else promennaProVystupZCyklu = false;



            // Seznam hracu dle podminek
            promennaProVystupZCyklu = true;

            while (promennaProVystupZCyklu)
            {
                Console.WriteLine("Pokud chcete vyhledat seznam hracu, stisknete a");
                if (Console.ReadLine() == "a")
                {
                    VyhledejHrace();
                }
                else promennaProVystupZCyklu = false;
            }






            Console.ReadLine();


            //METODY

            void ZadejZapas()
            {

                Console.WriteLine("Zadajte domaci team: ");
                string domaciTeam = kontrolovanyString(KontrolaExistenceTeamu(vsechnyJestvujiciTeamy, Console.ReadLine()));
                var domaciTeamDotaz = vsechnyJestvujiciTeamy.Where(r => r.Jmeno == domaciTeam);


                Console.WriteLine("Zadajte hostujici team: ");
                string hostujiciTeam = kontrolovanyString(KontrolaExistenceTeamu(vsechnyJestvujiciTeamy, Console.ReadLine()));
                var hostujiciTeamDotaz = vsechnyJestvujiciTeamy.Where(r => r.Jmeno == hostujiciTeam);

                var obaTeamyDotaz = vsechnyJestvujiciTeamy.Where(r => r.Jmeno == domaciTeam || r.Jmeno == hostujiciTeam);

                while  (domaciTeam == hostujiciTeam)
                {
                    Console.WriteLine("Hosujici team nemuze byt stejny jak domaci team!");
                    Console.WriteLine("Zadajte hostujici team: ");
                    hostujiciTeam = kontrolovanyString(KontrolaExistenceTeamu(vsechnyJestvujiciTeamy, Console.ReadLine()));
                }

                Console.WriteLine("Zadajte pocet strelenych golu teamu " + domaciTeam + "(domaci): ");
                int domaciTeamPocetGolu = kontrolovanyIntProPocetGolu(Console.ReadLine());

                Console.WriteLine("Zadajte pocet strelenych golu teamu " + hostujiciTeam + "(hoste): ");
                int hostujiciTeamPocetGolu = kontrolovanyIntProPocetGolu(Console.ReadLine());

                // doplnit body teamom
                if (domaciTeamPocetGolu == hostujiciTeamPocetGolu)
                {
                    foreach (Team t in obaTeamyDotaz)
                    {
                        t.ZapisBodyTeamu(1);
                    }
                }
                else if (domaciTeamPocetGolu > hostujiciTeamPocetGolu)
                {
                    foreach (Team t in domaciTeamDotaz)
                    {
                        t.ZapisBodyTeamu(3);
                    }
                }
                else if (domaciTeamPocetGolu < hostujiciTeamPocetGolu)
                {
                    foreach (Team t in hostujiciTeamDotaz)
                    {
                        t.ZapisBodyTeamu(3);
                    }
                }

                List<Hrac> StrelciDomaci = new List<Hrac>(domaciTeamPocetGolu);  // strelcov moze byt len tolko kolko je golov
                List<Hrac> StrelciHostia = new List<Hrac>(hostujiciTeamPocetGolu);

                // supisky pre lahsoiu kontrolu prijmeni
                Console.WriteLine(""); 
                Console.WriteLine("Supiska teamu " + domaciTeam + "(domaci):");
                foreach (var t in vsechniJestvujiciHraci.Where(r => r.Team == domaciTeam))
                {
                    Console.WriteLine(t.Jmeno + " " + t.Prijmeni);
                }
                Console.WriteLine("");
                Console.WriteLine("Supiska teamu " + hostujiciTeam + "(hoste): ");
                foreach (var t in vsechniJestvujiciHraci.Where(r => r.Team == hostujiciTeam))
                {
                    Console.WriteLine(t.Jmeno + " " + t.Prijmeni);
                }

                string prijmenihrace;

                for (int i = 1; i <= domaciTeamPocetGolu; i++)
                {
                    Console.WriteLine("Zadatjte prijmeni strelce " + i + ". golu teamu " + domaciTeam + "(domaci):");
                    prijmenihrace = kontrolovanyString(Console.ReadLine());
                    while (vyberHrace(3, domaciTeam).Where(r => r.Prijmeni == prijmenihrace).ToList().Count()==0)
                    {
                        Console.WriteLine("Hrac s prijmenim " + prijmenihrace + " v teamu " + domaciTeam + "neexistuje!") ;
                        Console.WriteLine("Zadatjte prijmeni strelce " + i + ". golu teamu " + domaciTeam + "(domaci):");
                        prijmenihrace = kontrolovanyString(Console.ReadLine());
                    }

                    StrelciDomaci = vsechniJestvujiciHraci.Where(r => r.Prijmeni == prijmenihrace && r.Team == domaciTeam).ToList();
                    StrelciDomaci[0].ZapisGolStrelcovi();
                }

                for (int i = 1; i <= hostujiciTeamPocetGolu; i++)
                {
                    Console.WriteLine("Zadatjte prijmeni strelce " + i + ". golu teamu " + hostujiciTeam + "(domaci):");
                    prijmenihrace = kontrolovanyString(Console.ReadLine());
                    while (vyberHrace(3, hostujiciTeam).Where(r => r.Prijmeni == prijmenihrace).ToList().Count() == 0)
                    {
                        Console.WriteLine("Hrac s prijmenim " + prijmenihrace + " v teamu " + hostujiciTeam + "neexistuje!");
                        Console.WriteLine("Zadatjte prijmeni strelce " + i + ". golu teamu " + hostujiciTeam + "(domaci):");
                        prijmenihrace = kontrolovanyString(Console.ReadLine());
                    }

                    StrelciHostia = vsechniJestvujiciHraci.Where(r => r.Prijmeni == prijmenihrace && r.Team == hostujiciTeam).ToList();
                    StrelciHostia[0].ZapisGolStrelcovi();
                }

            }

            void VyhledejHrace()
            {
                // Vyber hrace podle podminky
                Console.WriteLine("Seznam Hracu - vyberte si podminku:");
                Console.WriteLine("1 - vsichni hraci");
                Console.WriteLine("2 - hraci podle prijmeni");
                Console.WriteLine("3 - hraci podle tymu");
                Console.WriteLine("4 - hraci podle pozice");

                int podminka = kontrolovanyIntProFilter(Console.ReadLine());
                string kriter = "";
                if (podminka != 1)
                {
                    Console.WriteLine("Zadajte prosim vyhledavaci klic:");
                    if (podminka == 3)
                        kriter = kontrolovanyString(KontrolaExistenceTeamu(vsechnyJestvujiciTeamy, Console.ReadLine()));
                    else if (podminka == 4)
                        kriter = KontrolaExistencePozice(kontrolovanyString(Console.ReadLine()));
                    else 
                        kriter = kontrolovanyString(Console.ReadLine());
                }

                foreach (Hrac hrac in vyberHrace(podminka, kriter))
                {
                    Console.WriteLine($"Hrac: " + hrac.Jmeno + " " + hrac.Prijmeni + ", " + hrac.DatumNarozeni.Day +"."+ hrac.DatumNarozeni.Month + "." + hrac.DatumNarozeni.Year + ", " + hrac.Team + "Pocet strelenych golu: " + hrac.PocetGolu);
                }
            }


  

            List <Hrac> vyberHrace(int typKriteria, string kriterium)
            {
                List<Hrac> vybraniHraci = new List<Hrac>();

                for (int i = 0; i < vsechniJestvujiciHraci.Count; i++)
                {

                    if (typKriteria == 4) // pozice
                    {
                        if (vsechniJestvujiciHraci[i].Pozice == kriterium)
                        vybraniHraci.Add(vsechniJestvujiciHraci[i]);
                    }
                    else if (typKriteria == 3 ) // team
                    {
                        if (vsechniJestvujiciHraci[i].Team == kriterium)
                            vybraniHraci.Add(vsechniJestvujiciHraci[i]);
                    }
                    else if (typKriteria == 2 ) // Prijmeni
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

            string kontrolovanyString(string vstupnyText)
            {
                while (string.IsNullOrEmpty(vstupnyText))
                {
                    Console.WriteLine("Nezadali jse spravne vstup, prosim opakujte s vyplnenou hodnotou:");
                    vstupnyText = Console.ReadLine();
                }
                return char.ToUpper(vstupnyText[0]) + vstupnyText.Substring(1); // prvni znak velke pismeno

            }

            int kontrolovanyIntProFilter(string vstupnyIntjakString)
            {

                int vstupnyInt = 0;
                while (!int.TryParse(vstupnyIntjakString,out vstupnyInt) || (vstupnyInt < 1 && vstupnyInt > 4))
                {
                    Console.WriteLine("Nezadali jse spravne vstup, prosim opakujte s vyplnenou hodnotou 1-4:");
                    vstupnyIntjakString = Console.ReadLine();
                }
                return vstupnyInt;

            }

            int kontrolovanyIntProPocetGolu(string vstupnyIntjakString)
            {

                int vstupnyInt = 0;
                while (!int.TryParse(vstupnyIntjakString, out vstupnyInt) || (vstupnyInt < 0))
                {
                    Console.WriteLine("Nezadali jse spravne vstup pro pocet golu, prosim opakujte se spravne vyplnenou hodnotou:");
                    vstupnyIntjakString = Console.ReadLine();
                }
                return vstupnyInt;

            }


            string kontrolovanyDatum(string datumNarozeni)
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


            string KontrolaExistencePozice(string pozice)
            {
                List<string> vsechnyPozice = new List<string> { "brankar", "obrance", "utocnik" }; // hrame maly futbal takze bez zalohy

                string vyslednaPozice = "";
                foreach (string p in vsechnyPozice)
                {
                    if (p.Equals(pozice, StringComparison.OrdinalIgnoreCase))
                    {
                        vyslednaPozice = p;
                    }
                }

                if (vyslednaPozice == "")
                    Console.WriteLine("Byla zadaná nesprávna pozice");
                return vyslednaPozice;

            }


            string cesta(string coLoadujem, string nazevSouboru)
            {
                var currentDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                string projectDirectory = currentDirectory.Parent.Parent.Parent.FullName;
                string cestaKSouboru = Path.Combine(projectDirectory, coLoadujem, nazevSouboru);


                if (!Directory.Exists(Path.GetDirectoryName(cestaKSouboru)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(cestaKSouboru));
                }

                return cestaKSouboru;

            }



            List<Team> InitialLoadTeam() 
            {
                string cestaKSouboru = cesta("Teamy", "Teamy.xml");
                XmlSerializer serializerTeamy = new XmlSerializer(typeof(List<Team>));

                using (StreamReader ctecka = new StreamReader(cestaKSouboru))
                {
                    List<Team> nacteneTeamy = serializerTeamy.Deserialize(ctecka) as List<Team>;
                    return nacteneTeamy;
                }
            }

            List<Hrac> InitialLoadHrac()
            {
                string cestaKSouboru = cesta("Hraci", "Hraci.xml");
                XmlSerializer serializerHraci = new XmlSerializer(typeof(List<Hrac>));

                using (StreamReader ctecka = new StreamReader(cestaKSouboru))
                {
                    List<Hrac> nacteniHraci = serializerHraci.Deserialize(ctecka) as List<Hrac>;
                    return nacteniHraci;
                }
            }

            List<Zapas> InitialLoadZapas() // InitialLoad("Teamy", "Teamy.xml")
            {
                string cestaKSouboru = cesta("Zapasy", "Zapasy.xml");
                XmlSerializer serializerZapasy = new XmlSerializer(typeof(List<Zapas>));

                using (StreamReader ctecka = new StreamReader(cestaKSouboru))
                {
                    List<Zapas> nacteneZapasy = serializerZapasy.Deserialize(ctecka) as List<Zapas>;
                    return nacteneZapasy;
                }
            }


             int VypocitejVekRoky(DateTime datumNarozeni)
            {
                DateTime Dneska = DateTime.Today;


                int DneskaJakInt = (Dneska.Year * 1000) + (Dneska.Month * 10) + (Dneska.Day);
                int DatumNarozeniJakInt = (datumNarozeni.Year * 1000) + (datumNarozeni.Month * 10) + (datumNarozeni.Day);

                return (DneskaJakInt - DatumNarozeniJakInt) / 1000; // INT sa postara o desetatine miesta :D
            }


            Hrac zadejHrace()
            {
                // Manualne Zadat noveho Hrace
                List<Hrac> hraci = new List<Hrac>();


                Console.WriteLine("Zadajte noveho hraca jmeno");
                Console.WriteLine("Zadajte krstni jmeno: ");
                string jmenoHrace = kontrolovanyString(Console.ReadLine());


                Console.WriteLine("Zadajte prijmeni: ");
                string prijmeniHrace = kontrolovanyString(Console.ReadLine());

                Console.WriteLine("Zadajte datum narozeni ve formatu YYYYMMDD: ");
                string datumHrace = kontrolovanyDatum(Console.ReadLine());


                Console.WriteLine("Zadajte team: ");
                string teamHrace =  kontrolovanyString(KontrolaExistenceTeamu(vsechnyJestvujiciTeamy,Console.ReadLine()));

                Console.WriteLine("Zadajte pozici: ");
                string poziceHrace = KontrolaExistencePozice(kontrolovanyString(Console.ReadLine()));

                Hrac hrac1 = new Hrac(jmenoHrace, prijmeniHrace, DateTime.Parse(datumHrace, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None), teamHrace, poziceHrace);
                return hrac1;
                
            }

            string KontrolaExistenceTeamu(List<Team> vsechnyTeamy, string team)
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
                    Console.WriteLine("Byl zadán neexistující team");
                return vyslednyTeam;

            }

        }

    }
}

