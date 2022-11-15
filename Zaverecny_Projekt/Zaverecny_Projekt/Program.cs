using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaverecny_Projekt
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //- zadat noveho hrace
            Hrac Brankar1 = new Hrac("Tomas", "Skala", new DateTime(2000, 1, 1), "Acka", "neco");
            Hrac Brankar2 = new Hrac("Jan", "Kamen", new DateTime(2001, 10, 10), "Becka", "neco");
            Hrac Brankar3 = new Hrac("Petr", "Sutr", new DateTime(2002, 12, 12), "Cecka", "neco");

            List<Hrac> hraci = new List<Hrac>() { Brankar1, Brankar2, Brankar3 };


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


            List<Hrac> vyber = vyberHrace("pozice", "brankar");
            foreach (Hrac hrac in vyber)
            {
                Console.WriteLine($"Hrac: " + hrac.Jmeno + " " + hrac.Prijmeni + ", " + hrac.Vek + ", " + hrac.Team);


            }

            Console.ReadLine();



            //- vyhledat hrace

        }
    }
}
