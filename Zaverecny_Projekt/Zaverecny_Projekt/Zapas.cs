using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaverecny_Projekt
{
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
