using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaverecny_Projekt
{
    public class ZadaniZapasu
    {
            
        
        public Zapas ZadejZapas(List<Team> vsechnyJestvujiciTeamy, List<Hrac> vsechniJestvujiciHraci)
        {
            var kontrolyVstupov = new KontrolyVstupov();
            var hraciATeamy = new OperaceHraciATeamy();

            string aktualneTeamy = "";
            foreach (Team t in vsechnyJestvujiciTeamy)
            {
                aktualneTeamy = string.Concat(aktualneTeamy, t.Jmeno, ", ");
            }
            aktualneTeamy = aktualneTeamy.Substring(0, aktualneTeamy.Length - 2);

            Console.WriteLine("Zadajte domaci team: (vyber z " + aktualneTeamy + ")");
            string domaciTeam = kontrolyVstupov.kontrolovanyString(kontrolyVstupov.KontrolaExistenceTeamu(vsechnyJestvujiciTeamy, Console.ReadLine()));
            var domaciTeamDotaz = vsechnyJestvujiciTeamy.Where(r => r.Jmeno == domaciTeam);

            aktualneTeamy = "";
            foreach (Team t in vsechnyJestvujiciTeamy)
            {
                if (t.Jmeno != domaciTeam)
                aktualneTeamy = string.Concat(aktualneTeamy, t.Jmeno, ", ");
            }
            aktualneTeamy = aktualneTeamy.Substring(0, aktualneTeamy.Length - 2);


            Console.WriteLine("Zadajte hostujici team: (vyber z " + aktualneTeamy + ")");
            string hostujiciTeam = kontrolyVstupov.kontrolovanyString(kontrolyVstupov.KontrolaExistenceTeamu(vsechnyJestvujiciTeamy, Console.ReadLine()));
            var hostujiciTeamDotaz = vsechnyJestvujiciTeamy.Where(r => r.Jmeno == hostujiciTeam);

            var obaTeamyDotaz = vsechnyJestvujiciTeamy.Where(r => r.Jmeno == domaciTeam || r.Jmeno == hostujiciTeam);

            while (domaciTeam == hostujiciTeam)
            {
                Console.WriteLine("Hosujici team nemuze byt stejny jak domaci team!");
                Console.WriteLine("Zadajte hostujici team: ");
                hostujiciTeam = kontrolyVstupov.kontrolovanyString(kontrolyVstupov.KontrolaExistenceTeamu(vsechnyJestvujiciTeamy, Console.ReadLine()));
            }

            Console.WriteLine("Zadajte pocet strelenych golu teamu " + domaciTeam + "(domaci): ");
            int domaciTeamPocetGolu = kontrolyVstupov.kontrolovanyIntProPocetGolu(Console.ReadLine());

            Console.WriteLine("Zadajte pocet strelenych golu teamu " + hostujiciTeam + "(hoste): ");
            int hostujiciTeamPocetGolu = kontrolyVstupov.kontrolovanyIntProPocetGolu(Console.ReadLine());

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
                prijmenihrace = kontrolyVstupov.kontrolovanyString(Console.ReadLine());
                while (hraciATeamy.vyberHrace(3, domaciTeam, vsechniJestvujiciHraci).Where(r => r.Prijmeni == prijmenihrace).ToList().Count() == 0)
                {
                    Console.WriteLine("Hrac s prijmenim " + prijmenihrace + " v teamu " + domaciTeam + "neexistuje!");
                    Console.WriteLine("Zadatjte prijmeni strelce " + i + ". golu teamu " + domaciTeam + "(domaci):");
                    prijmenihrace = kontrolyVstupov.kontrolovanyString(Console.ReadLine());
                }

                StrelciDomaci = vsechniJestvujiciHraci.Where(r => r.Prijmeni == prijmenihrace && r.Team == domaciTeam).ToList();
                StrelciDomaci[0].ZapisGolStrelcovi();
                VsichniStrelciDomaci.Add(new Hrac(StrelciDomaci[0].Jmeno, StrelciDomaci[0].Prijmeni, StrelciDomaci[0].DatumNarozeni, StrelciDomaci[0].Team, StrelciDomaci[0].Pozice, 1));
            }

            for (int i = 1; i <= hostujiciTeamPocetGolu; i++)
            {
                Console.WriteLine("Zadatjte prijmeni strelce " + i + ". golu teamu " + hostujiciTeam + "(domaci):");
                prijmenihrace = kontrolyVstupov.kontrolovanyString(Console.ReadLine());
                while (hraciATeamy.vyberHrace(3, hostujiciTeam, vsechniJestvujiciHraci).Where(r => r.Prijmeni == prijmenihrace).ToList().Count() == 0)
                {
                    Console.WriteLine("Hrac s prijmenim " + prijmenihrace + " v teamu " + hostujiciTeam + "neexistuje!");
                    Console.WriteLine("Zadatjte prijmeni strelce " + i + ". golu teamu " + hostujiciTeam + "(domaci):");
                    prijmenihrace = kontrolyVstupov.kontrolovanyString(Console.ReadLine());
                }

                StrelciHostia = vsechniJestvujiciHraci.Where(r => r.Prijmeni == prijmenihrace && r.Team == hostujiciTeam).ToList();
                StrelciHostia[0].ZapisGolStrelcovi();
                VsichniStrelciHostia.Add(new Hrac(StrelciHostia[0].Jmeno, StrelciHostia[0].Prijmeni, StrelciHostia[0].DatumNarozeni, StrelciHostia[0].Team, StrelciHostia[0].Pozice, 1));
            }


            Zapas Zapas1 = new Zapas(domaciTeam, hostujiciTeam, domaciTeamPocetGolu, hostujiciTeamPocetGolu, VsichniStrelciDomaci, VsichniStrelciHostia);
            return Zapas1;
        }
    }
}
