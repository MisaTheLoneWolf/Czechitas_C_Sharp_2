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

            List<Team> vsechnyJestvujiciTeamy;
            List<Hrac> vsechniJestvujiciHraci;
            List<Zapas> vsechnyJestvujicyZapasy;
            // Initioal Load ze souboru pokud jestvuje
            // TEAMY:
            vsechnyJestvujiciTeamy = InitialLoadTeam();

            // HRACI:
            vsechniJestvujiciHraci = InitialLoadHrac();

            // Zapasy:
            vsechnyJestvujicyZapasy = InitialLoadZapas();

            try // jenom obecny error handling so zapisom do ErrorLog-u
            { 
                // fake chyba
                // int a = 1;
                // int b = 0;
                // int c = a/b;

                bool promennaProVystupZCyklu = true;
                bool zobrazPonukuPreUkladanie = false;

                while (promennaProVystupZCyklu)
                {
                    int vybranaVolba = SeznamVsechMoznosti(zobrazPonukuPreUkladanie);
                    vybranaVolba = SpracovaniVybraneVolby(vybranaVolba);

                    if (vybranaVolba == 1 || vybranaVolba == 2 || vybranaVolba == 9)
                        zobrazPonukuPreUkladanie = true;

                    if (vybranaVolba == 10)
                        zobrazPonukuPreUkladanie = false;

                    if (vybranaVolba == 12)
                    {
                        Ukonci();
                        break;
                    }
                }
            }
            catch (Exception vyjimka)
            {
                string coJduZapsatDoErrorLogu = "---------------------------" 
                                                + Environment.NewLine + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                                                + Environment.NewLine + vyjimka.Message 
                                                + Environment.NewLine + vyjimka.StackTrace;
                ZapisDoErrorLogu(coJduZapsatDoErrorLogu);
                Console.WriteLine("Nastala chyba, detaily jsou ulozeny do ErrorLogu, ukoncim program");
                Console.ReadLine();
            }
            finally //jeste radeji vse ulozim pokus uzivatel chce
            {
                Console.WriteLine("Pokud Chcete ulozit Hrace, Zapasy a Teamy, stisknete a");
                string vstupUzivatele = kontrolovanyString(Console.ReadLine());

                if (vstupUzivatele == "A")
                { 
                    UlozHrace();
                    UlozZapas();
                    UlozTeam();
                    Console.WriteLine("Hraci, Zapasy a Teamy byli ulozeni. Koncim program.");
                   
                };
                Ukonci();
            }

            //METODY
            int SeznamVsechMoznosti(bool jeCoUkladat)
            {
                int vyber;
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("Zde je seznam všech moznosti co muzete udelat:");
                // Zadavame = zelena
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("1.  Manulne vlozit noveho hrace" + Environment.NewLine + "2.  Manualne zadat zapas");
                Console.ResetColor();
                Console.WriteLine("3.  Vratit seznam vsech hracu " + Environment.NewLine + "4.  Vratit seznam hracu podle vybraneho teamu" + Environment.NewLine +
                    "5.  Vratit seznam hracu podle vybrane pozice" + Environment.NewLine + "6.  Vratit seznam hracu podle prijmeni" + Environment.NewLine +
                    "7.  Vratit poradi teamu dle bodu" + Environment.NewLine + "8.  Vratit nejlepsiho strelce ligy");
                // Mazeme = cervena
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("9.  Vymazat hrace");
                Console.ResetColor();
                // Nemuzeme zavolat = seda
                if (!jeCoUkladat) Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("10. Ulozit zmeny" + Environment.NewLine + "11. Ulozit zmeny a odejit");
                if (!jeCoUkladat) Console.ResetColor();
                Console.WriteLine("12. Odejit bez ulozeni zmen" + Environment.NewLine);
                Console.Write("Vyberte si cislo pro Vas pozadavek: ");

                vyber = kontrolovanyIntProFilter(Console.ReadLine(), 1, 12);

                while (!jeCoUkladat && (vyber == 10 || vyber == 11))
                {
                    Console.Write("Zatim neni co ulozit, prosim opakujte volbu: ");
                    vyber = kontrolovanyIntProFilter(Console.ReadLine(), 1, 12);
                }
                return vyber;
            }

            int SpracovaniVybraneVolby(int volba)
            {
                switch (volba)
                { case 1: // Manulne vlozit noveho hrace
                        Hrac pridanyHrac = zadejHrace();
                        vsechniJestvujiciHraci.Add(pridanyHrac);
                        Console.WriteLine("Hrac " + pridanyHrac.Jmeno + " " + pridanyHrac.Prijmeni +" byl zadan do teamu "+ pridanyHrac.Team + Environment.NewLine);
                        break;
                    case 2: //Manualne zadat zapas
                        Zapas pridanyZapas = ZadejZapas();
                        vsechnyJestvujicyZapasy.Add(pridanyZapas);
                        Console.WriteLine("Zapas mezi teamem  " + pridanyZapas.Domaci + " a teamem " + pridanyZapas.Hoste + " byl zadan" + Environment.NewLine);
                        break;
                    case 3: //Vratit seznam vsech hracu
                        VyhledejHrace(1);
                        break;
                    case 4: //Vratit seznam hracu podle vybraneho teamu
                        VyhledejHrace(3);
                        break;
                    case 5: //Vratit seznam hracu podle vybrane pozice
                        VyhledejHrace(4);
                        break;
                    case 6: //Vratit seznam hracu podle prijmeni
                        VyhledejHrace(2);
                        break;
                    case 7: //Vratit poradi teamu dle bodu
                        PoradiTeamuDleBodu();
                        break;
                    case 8: //Vratit nejlepsiho strelce ligy
                        NajlepsiStrelecLigy();
                        break;
                    case 9: //Vymazat hrace
                        Hrac vymazanyHrac = vymazHrace();
                        // vsechniJestvujiciHraci.Remove(vymazanyHrac) NEFUNGUJE ???
                        int index = vsechniJestvujiciHraci.FindIndex(a => a.Prijmeni == vymazanyHrac.Prijmeni && a.Team == vymazanyHrac.Team);
                        vsechniJestvujiciHraci.RemoveAt(index);
                        Console.WriteLine("Hrac " + vymazanyHrac.Jmeno + " " + vymazanyHrac.Prijmeni + " byl vymazan z teamu " + vymazanyHrac.Team + Environment.NewLine);
                        break;       
                    case 10: //Ulozit
                        UlozHrace();
                        UlozZapas();
                        UlozTeam();
                        break;
                    case 11: //Ulozit
                        UlozHrace();
                        UlozZapas();
                        UlozTeam();
                        volba = 12;
                        break;
                    case 12:
                        break;
                    default:
                        Console.WriteLine("Volba neexistuje, prosim opakujte se spravne vyplnenou hodnotou 1-12:");
                        break;
                }
                return volba;

            }

            void NajlepsiStrelecLigy()
            {
                var maximumGolovDotaz = vsechniJestvujiciHraci.Max(x => x.PocetGolu);

                var hraciSMaxGolamiDotaz = vsechniJestvujiciHraci.Where(x => x.PocetGolu == maximumGolovDotaz);
                foreach (var t in hraciSMaxGolamiDotaz)
                {
                    Console.WriteLine($"Hrac: " + t.Jmeno + " " + t.Prijmeni + " Team: " + t.Team + " Pocet strelenych golu: " + t.PocetGolu);
                }
            }

            Zapas ZadejZapas()
            {

                Console.WriteLine("Zadajte domaci team: ");
                string domaciTeam = kontrolovanyString(KontrolaExistenceTeamu(vsechnyJestvujiciTeamy, Console.ReadLine()));
                var domaciTeamDotaz = vsechnyJestvujiciTeamy.Where(r => r.Jmeno == domaciTeam);


                Console.WriteLine("Zadajte hostujici team: ");
                string hostujiciTeam = kontrolovanyString(KontrolaExistenceTeamu(vsechnyJestvujiciTeamy, Console.ReadLine()));
                var hostujiciTeamDotaz = vsechnyJestvujiciTeamy.Where(r => r.Jmeno == hostujiciTeam);

                var obaTeamyDotaz = vsechnyJestvujiciTeamy.Where(r => r.Jmeno == domaciTeam || r.Jmeno == hostujiciTeam);

                while (domaciTeam == hostujiciTeam)
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

                List<Hrac> StrelciDomaci = new List<Hrac>(1); 
                List<Hrac> StrelciHostia = new List<Hrac>(1);
                List<Hrac> VsichniStrelciDomaci = new List<Hrac>(domaciTeamPocetGolu);  // strelcov moze byt len tolko kolko je golov
                List<Hrac> VsichniStrelciHostia = new List<Hrac>(hostujiciTeamPocetGolu);


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
                    while (vyberHrace(3, domaciTeam).Where(r => r.Prijmeni == prijmenihrace).ToList().Count() == 0)
                    {
                        Console.WriteLine("Hrac s prijmenim " + prijmenihrace + " v teamu " + domaciTeam + "neexistuje!");
                        Console.WriteLine("Zadatjte prijmeni strelce " + i + ". golu teamu " + domaciTeam + "(domaci):");
                        prijmenihrace = kontrolovanyString(Console.ReadLine());
                    }

                    StrelciDomaci = vsechniJestvujiciHraci.Where(r => r.Prijmeni == prijmenihrace && r.Team == domaciTeam).ToList();
                    StrelciDomaci[0].ZapisGolStrelcovi();
                    VsichniStrelciDomaci.Add(new Hrac(StrelciDomaci[0].Jmeno, StrelciDomaci[0].Prijmeni, StrelciDomaci[0].DatumNarozeni, StrelciDomaci[0].Team, StrelciDomaci[0].Pozice, 1));
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
                    VsichniStrelciHostia.Add(new Hrac(StrelciHostia[0].Jmeno, StrelciHostia[0].Prijmeni, StrelciHostia[0].DatumNarozeni, StrelciHostia[0].Team, StrelciHostia[0].Pozice,1));
                }


                Zapas Zapas1 = new Zapas(domaciTeam, hostujiciTeam, domaciTeamPocetGolu, hostujiciTeamPocetGolu, VsichniStrelciDomaci, VsichniStrelciHostia);
                return Zapas1;
            }

            void VyhledejHrace(int podminka)
            {
                //Console.WriteLine("1 - vsichni hraci");
                //Console.WriteLine("2 - hraci podle prijmeni");
                //Console.WriteLine("3 - hraci podle tymu");
                //Console.WriteLine("4 - hraci podle pozice");

                string kriter = "";
                if (podminka != 1)
                {
                    Console.WriteLine("Zadajte prosim vyhledavaci klic:");
                    if (podminka == 3)
                        kriter = kontrolovanyString(KontrolaExistenceTeamu(vsechnyJestvujiciTeamy, Console.ReadLine()));
                    else if (podminka == 4)
                        kriter = KontrolaExistencePozice(kontrolovanyString(Console.ReadLine()));
                    else
                        kriter = kontrolovanyString(KontrolaExistenceHrace(vsechniJestvujiciHraci, Console.ReadLine()));
                }

                foreach (Hrac hrac in vyberHrace(podminka, kriter))
                {
                    Console.WriteLine($"Hrac: " + hrac.Jmeno + " " + hrac.Prijmeni + ", " + hrac.DatumNarozeni.Day + "." + hrac.DatumNarozeni.Month + "." + hrac.DatumNarozeni.Year + ", " + hrac.Team + " - " + hrac.Pozice + ", Pocet strelenych golu: " + hrac.PocetGolu);
                }
            }

            void PoradiTeamuDleBodu()
            {
                int poradi = 1;
                Console.WriteLine("Poradi  Nazev teamu  Pocet bodu");

                foreach (Team t in vsechnyJestvujiciTeamy.OrderByDescending(r => r.PocetBodu))
                {
                    Console.WriteLine($"{poradi}.misto: {t.Jmeno.PadRight(12)} {t.PocetBodu}");
                    poradi++;
                }
            }

            List<Hrac> vyberHrace(int typKriteria, string kriterium)
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
                string teamHrace = kontrolovanyString(KontrolaExistenceTeamu(vsechnyJestvujiciTeamy, Console.ReadLine()));

                Console.WriteLine("Zadajte pozici: ");
                string poziceHrace = KontrolaExistencePozice(kontrolovanyString(Console.ReadLine()));

                Hrac hrac1 = new Hrac(jmenoHrace, prijmeniHrace, DateTime.Parse(datumHrace, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None), teamHrace, poziceHrace);
                return hrac1;
            }


            Hrac vymazHrace()
            {
                // Manualne Zadat noveho Hrace
   
                Console.WriteLine("Zadajte team z ktereho chcete hrace vymazat");
                string teamHrace = kontrolovanyString(KontrolaExistenceTeamu(vsechnyJestvujiciTeamy, Console.ReadLine()));
                Console.WriteLine("");
                Console.WriteLine("Supiska teamu " + teamHrace + ":");
                foreach (var t in vsechniJestvujiciHraci.Where(r => r.Team == teamHrace))
                {
                    Console.WriteLine(t.Jmeno + " " + t.Prijmeni);
                }

                Console.WriteLine("Zadajte prijmeni hrac, ktereho chcete vymazat (z teamu " +teamHrace+ ")");
                string prijmeniHrace = kontrolovanyString(KontrolaExistenceHrace(vsechniJestvujiciHraci, Console.ReadLine()));

                var HracNaVymaz = vsechniJestvujiciHraci.Where(r => r.Prijmeni == prijmeniHrace && r.Team == teamHrace).ToList();
                Hrac hracBudeVymazan = new Hrac(HracNaVymaz[0].Jmeno, HracNaVymaz[0].Prijmeni, HracNaVymaz[0].DatumNarozeni, HracNaVymaz[0].Team, HracNaVymaz[0].Pozice, HracNaVymaz[0].PocetGolu);
                return hracBudeVymazan;
            }

            string kontrolovanyString(string vstupnyText)
            {
                while (string.IsNullOrEmpty(vstupnyText))
                {
                    Console.WriteLine("Nezadali jse spravne vstup, prosim opakujte s vyplnenou hodnotou:");
                    vstupnyText = Console.ReadLine();
                }
                return char.ToUpper(vstupnyText[0]) + vstupnyText.Substring(1).ToLower(CultureInfo.InvariantCulture); // prvni znak velke pismeno
            }

            int kontrolovanyIntProFilter(string vstupnyIntjakString, int minimalneMozneCislo, int MaximalneMozneCislo)
            {

                int vstupnyInt = 0;
                while (!int.TryParse(vstupnyIntjakString, out vstupnyInt) || (vstupnyInt < minimalneMozneCislo && vstupnyInt > MaximalneMozneCislo))
                {
                    Console.WriteLine("Nezadali jse spravne vstup, prosim opakujte se spravne vyplnenou hodnotou " + minimalneMozneCislo + "-" + MaximalneMozneCislo + ":");
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
                    Console.WriteLine("Byl zadan neexistujici team");
                return vyslednyTeam;
            }

            string KontrolaExistenceHrace(List<Hrac> vsechniHraci, string hrac)
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
                    Console.WriteLine("Byla zadana nespravna pozice");
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


                if (!File.Exists(cestaKSouboru))
                {
                    using (File.Create(cestaKSouboru)) ;
                }

                return cestaKSouboru;
            }

            List<Team> InitialLoadTeam()
            {
                string cestaKSouboru = cesta("Teamy", "Teamy.xml");

                if (VelkostSouboru(cestaKSouboru) == 0)
                {
                    return new List<Team>(); // vraciam prazdny list
                }
                else
                {
                    XmlSerializer serializerTeamy = new XmlSerializer(typeof(List<Team>));
                    using (StreamReader ctecka = new StreamReader(cestaKSouboru))
                    {
                        List<Team> nacteneTeamy = serializerTeamy.Deserialize(ctecka) as List<Team>;
                        return nacteneTeamy;
                    }
                }
            }

            List<Hrac> InitialLoadHrac()
            {
                string cestaKSouboru = cesta("Hraci", "Hraci.xml");

                if (VelkostSouboru(cestaKSouboru) == 0)
                {
                    return new List<Hrac>(); // vraciam prazdny list
                }
                else
                {                
                    XmlSerializer serializerHraci = new XmlSerializer(typeof(List<Hrac>));
                    using (StreamReader ctecka = new StreamReader(cestaKSouboru))
                    {
                        List<Hrac> nacteniHraci = serializerHraci.Deserialize(ctecka) as List<Hrac>;
                        return nacteniHraci;
                    }
                }
            }

            List<Zapas> InitialLoadZapas()
            {
                string cestaKSouboru = cesta("Zapasy", "Zapasy.xml");

                if (VelkostSouboru(cestaKSouboru) == 0)
                {
                    return new List<Zapas>(); // vraciam prazdny list
                }
                else
                {
                    XmlSerializer serializerZapasy = new XmlSerializer(typeof(List<Zapas>));
                    using (StreamReader ctecka = new StreamReader(cestaKSouboru))
                    {
                        List<Zapas> nacteneZapasy = serializerZapasy.Deserialize(ctecka) as List<Zapas>;
                        return nacteneZapasy;
                    }
                }
            }

            void UlozTeam()
            {
                string cestaKSouboru = cesta("Teamy", "Teamy.xml");
                XmlSerializer serializerHraci = new XmlSerializer(typeof(List<Team>));

                using (StreamWriter writer = new StreamWriter(cestaKSouboru))
                {
                    serializerHraci.Serialize(writer, vsechnyJestvujiciTeamy);
                }
            }

            void UlozHrace()
            {
                string cestaKSouboru = cesta("Hraci", "Hraci.xml");
                XmlSerializer serializerHraci = new XmlSerializer(typeof(List<Hrac>));

                using (StreamWriter writer = new StreamWriter(cestaKSouboru))
                {
                    serializerHraci.Serialize(writer, vsechniJestvujiciHraci);
                }
            }

            void UlozZapas()
            {
                string cestaKSouboru = cesta("Zapasy", "Zapasy.xml");
                XmlSerializer serializerHraci = new XmlSerializer(typeof(List<Zapas>));

                using (StreamWriter writer = new StreamWriter(cestaKSouboru))
                {
                    serializerHraci.Serialize(writer, vsechnyJestvujicyZapasy);
                }
            }

            long VelkostSouboru(string cestaKSouboru)
            {
                    return new FileInfo(cestaKSouboru).Length;
            }

            void ZapisDoErrorLogu(string novyZapis)
            {
                string cestaKSouboru = cesta("Log", "ErrorLog.xml");
                StreamWriter writer = new StreamWriter(cestaKSouboru, append: true);
                writer.WriteLine(novyZapis);
                writer.Flush();     //v tuto chvili se zapise na disk
                writer.Close();     //uzavreni souboru

            }

            void Ukonci()
            {
                Console.WriteLine("Koncim program, naschledanou!");
                Console.ReadLine();
            }
        }
    }



}

