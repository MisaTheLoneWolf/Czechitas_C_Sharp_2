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

            var nacteniAZapis = new NacteniAZapis();
            var kontrolyVstupov = new KontrolyVstupov();
            var hraciATeamy = new OperaceHraciATeamy();
            var zadaniZapasu = new ZadaniZapasu();    

            List<Team> vsechnyJestvujiciTeamy;
            List<Hrac> vsechniJestvujiciHraci;
            List<Zapas> vsechnyJestvujicyZapasy;

            // Initioal Load ze souboru pokud jestvuje
            // TEAMY:
            vsechnyJestvujiciTeamy = nacteniAZapis.InitialLoadTeam();

            // HRACI:
            vsechniJestvujiciHraci = nacteniAZapis.InitialLoadHrac();

            // ZAPASY:
            vsechnyJestvujicyZapasy = nacteniAZapis.InitialLoadZapas();

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
                    vybranaVolba = SpracovaniVybraneVolby(vybranaVolba, vsechnyJestvujiciTeamy, vsechniJestvujiciHraci, vsechnyJestvujicyZapasy);

                    if (vybranaVolba == 1 || vybranaVolba == 2 || vybranaVolba == 9)
                        zobrazPonukuPreUkladanie = true;

                    if (vybranaVolba == 10)
                        zobrazPonukuPreUkladanie = false;

                    if (vybranaVolba == 12)
                    {
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
                nacteniAZapis.ZapisDoErrorLogu(coJduZapsatDoErrorLogu);
                Console.WriteLine("Nastala chyba, detaily jsou ulozeny do ErrorLogu, ukoncim program");
                Console.ReadLine();
                
                Console.WriteLine("Pokud Chcete ulozit Hrace, Zapasy a Teamy, stisknete a");
                string vstupUzivatele = kontrolyVstupov.kontrolovanyString(Console.ReadLine());

                if (vstupUzivatele == "A")
                {
                    nacteniAZapis.UlozHraceZapasTeam(vsechnyJestvujiciTeamy, vsechniJestvujiciHraci, vsechnyJestvujicyZapasy);
                    Console.WriteLine("Hraci, Zapasy a Teamy byli ulozeni. Koncim program.");
                };
            }
            finally //jeste radeji vse ulozim pokus uzivatel chce
            {

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
                    "7.  Vratit poradi teamu dle bodu" + Environment.NewLine + "8.  Vratit nejlepsiho strelce ligy a jednotlivych teamu");
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

                vyber = kontrolyVstupov.kontrolovanyIntProFilter(Console.ReadLine(), 1, 12);

                while (!jeCoUkladat && (vyber == 10 || vyber == 11))
                {
                    Console.Write("Zatim neni co ulozit, prosim opakujte volbu: ");
                    vyber = kontrolyVstupov.kontrolovanyIntProFilter(Console.ReadLine(), 1, 12);
                }
                return vyber;
            }

            int SpracovaniVybraneVolby(int volba, List< Team > vsechnyTeamy, List<Hrac> vsechniHraci, List< Zapas > vsechnyZapasy)
            {



                switch (volba)
                { case 1: // Manulne vlozit noveho hrace
                        Hrac pridanyHrac = hraciATeamy.zadejHrace(vsechnyTeamy);
                        vsechniHraci.Add(pridanyHrac);
                        Console.WriteLine("Hrac " + pridanyHrac.Jmeno + " " + pridanyHrac.Prijmeni +" byl zadan do teamu "+ pridanyHrac.Team + Environment.NewLine);
                        break;
                    case 2: //Manualne zadat zapas
                        Zapas pridanyZapas = zadaniZapasu.ZadejZapas(vsechnyTeamy, vsechniHraci);
                        vsechnyZapasy.Add(pridanyZapas);
                        Console.WriteLine("Zapas mezi teamem  " + pridanyZapas.Domaci + " a teamem " + pridanyZapas.Hoste + " byl zadan" + Environment.NewLine);
                        break;
                    case 3: //Vratit seznam vsech hracu
                        hraciATeamy.VyhledejHrace(1, vsechniHraci, vsechnyTeamy);
                        break;
                    case 4: //Vratit seznam hracu podle vybraneho teamu
                        hraciATeamy.VyhledejHrace(3, vsechniHraci, vsechnyTeamy);
                        break;
                    case 5: //Vratit seznam hracu podle vybrane pozice
                        hraciATeamy.VyhledejHrace(4, vsechniHraci, vsechnyTeamy);
                        break;
                    case 6: //Vratit seznam hracu podle prijmeni
                        hraciATeamy.VyhledejHrace(2, vsechniHraci, vsechnyTeamy);
                        break;
                    case 7: //Vratit poradi teamu dle bodu
                        hraciATeamy.PoradiTeamuDleBodu(vsechnyTeamy);
                        break;
                    case 8: //Vratit nejlepsiho strelce ligy + teamu
                        hraciATeamy.NajlepsiStrelecLigyATeamu(vsechniHraci, vsechnyTeamy);
                        break;
                    case 9: //Vymazat hrace
                        Hrac vymazanyHrac = hraciATeamy.vymazHrace(vsechniHraci, vsechnyTeamy);
                        // vsechniHraci.Remove(vymazanyHrac) NEFUNGUJE ???
                        int index = vsechniHraci.FindIndex(a => a.Prijmeni == vymazanyHrac.Prijmeni && a.Team == vymazanyHrac.Team);
                        vsechniHraci.RemoveAt(index);
                        Console.WriteLine("Hrac " + vymazanyHrac.Jmeno + " " + vymazanyHrac.Prijmeni + " byl vymazan z teamu " + vymazanyHrac.Team + Environment.NewLine);
                        break;       
                    case 10: //Ulozit
                        nacteniAZapis.UlozHraceZapasTeam(vsechnyTeamy, vsechniHraci, vsechnyZapasy);
                        break;
                    case 11: //Ulozit
                        nacteniAZapis.UlozHraceZapasTeam(vsechnyTeamy, vsechniHraci, vsechnyZapasy);
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

            void Ukonci()
            {
                Console.WriteLine("Koncim program, naschledanou!");
                Console.ReadLine();
            }
        }
    }



}

