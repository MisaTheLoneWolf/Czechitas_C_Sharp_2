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

            var currentDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string projectDirectory = currentDirectory.Parent.Parent.Parent.FullName;

            string cestaKSouboru = Path.Combine(projectDirectory,"Teamy","Teamy.xml");

            if (!Directory.Exists(Path.GetDirectoryName(cestaKSouboru)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(cestaKSouboru));
            }

            Team teamA = new Team("Amateri", "As", "5.liga");
            Team teamB = new Team("Borci", "Brno", "5.liga");



            XmlSerializer serializer = new XmlSerializer(typeof(Team));

            using (StreamWriter writer = new StreamWriter(cestaKSouboru))
            {
                serializer.Serialize(writer, teamA);
                serializer.Serialize(writer, teamB);
            }








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
            string teamHrace = kontrolovanyString(Console.ReadLine());

            Console.WriteLine("Zadajte pozici: ");
            string poziceHrace = KontrolaExistencePozice(kontrolovanyString(Console.ReadLine()));


            zadejNovehoHrace(jmenoHrace, prijmeniHrace, DateTime.Parse(datumHrace, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None), teamHrace, poziceHrace);

            
           
            Hrac Brankar2 = new Hrac("Jan", "Kamen", new DateTime(2001, 10, 10), "Becka", "brankar");
            Hrac Brankar3 = new Hrac("Petr", "Sutr", new DateTime(2002, 12, 12), "Cecka", "brankar");
            hraci.Add(Brankar2);
            hraci.Add(Brankar3);


            List<Hrac> vyber = vyberHrace("pozice", "brankar");
            foreach (Hrac hrac in vyber)
            {
                Console.WriteLine($"Hrac: " + hrac.Jmeno + " " + hrac.Prijmeni + ", " + hrac.Vek + ", " + hrac.Team);


            }

            Console.ReadLine();

















  


            //- zadat noveho hrace
            //Hrac Brankar1 = new Hrac("Tomas", "Skala", new DateTime(2000, 1, 1), "Acka", "neco");
   









            //List<Hrac> hraci = new List<Hrac>() { Brankar1, Brankar2, Brankar3 };


            //- vypsat hrace podle teamu
            //- vypsat hrace podle pozice  
            List<Hrac> vyberHrace(string typKriteria, string kriterium)
            {

                List<Hrac> vybraniHraci = new List<Hrac>();



                for (int i = 0; i < hraci.Count; i++)
                {

                    if (typKriteria == "pozice" && hraci[i].Pozice == kriterium)
                    {
                        vybraniHraci.Add(hraci[i]);
                    }
                    else if (typKriteria == "team" && hraci[i].Team == kriterium)
                    {
                        vybraniHraci.Add(hraci[i]);
                    }
                    else if (typKriteria != "pozice" && typKriteria != "team")
                    {
                        Console.WriteLine("nebolo zadane spravne kriterium");
                        break;
                    }

                }

                return vybraniHraci;


            }

            // list vsetkych brankarov Meno Prijmeni, vek, team


            //List<Hrac> vyber = vyberHrace("pozice", "brankar");
            //foreach (Hrac hrac in vyber)
            //{
            //    Console.WriteLine($"Hrac: " + hrac.Jmeno + " " + hrac.Prijmeni + ", " + hrac.Vek + ", " + hrac.Team);


            //}

            //Console.ReadLine();



            //- vyhledat hrace



            // METODY
            string kontrolovanyString(string vstupnyText)
            {
                while (string.IsNullOrEmpty(vstupnyText))
                {
                    Console.WriteLine("Nezadali jse spravne vstup, prosim opakujte s vyplnenou hodnotou:");
                    vstupnyText = Console.ReadLine();
                }
                return vstupnyText;
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
                List<string> vsechnyPozice = new List<string> { "brankar", "obrance", "zaloznik", "utocnik" };

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


            void zadejNovehoHrace(string jmeno, string prijmeni, DateTime datumNarozeni, string team, string pozice)
            {

                Hrac hrac1 = new Hrac(jmeno, prijmeni, datumNarozeni, team, pozice);
                hraci.Add(hrac1);
            }
        }
    }
}
