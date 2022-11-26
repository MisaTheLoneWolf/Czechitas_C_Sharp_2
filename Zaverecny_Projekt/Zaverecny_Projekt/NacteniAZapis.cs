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

    public class NacteniAZapis
    {
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
                using (File.Create(cestaKSouboru));
            }

            return cestaKSouboru;
        }
        public List<Team> InitialLoadTeam()
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

        public List<Hrac> InitialLoadHrac()
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

        public List<Zapas> InitialLoadZapas()
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

        public void UlozHraceZapasTeam(List<Team> vsechnyJestvujiciTeamy, List<Hrac> vsechniJestvujiciHraci, List<Zapas> vsechnyJestvujicyZapasy)
        {
            UlozHrace(vsechniJestvujiciHraci);
            UlozZapas(vsechnyJestvujicyZapasy);
            UlozTeam(vsechnyJestvujiciTeamy);
        }


        private void UlozTeam(List<Team> vsechnyJestvujiciTeamy)
        {
            string cestaKSouboru = cesta("Teamy", "Teamy.xml");
            XmlSerializer serializerHraci = new XmlSerializer(typeof(List<Team>));

            using (StreamWriter writer = new StreamWriter(cestaKSouboru))
            {
                serializerHraci.Serialize(writer, vsechnyJestvujiciTeamy);
            }
        }

        private void UlozHrace(List<Hrac> vsechniJestvujiciHraci)
        {
            string cestaKSouboru = cesta("Hraci", "Hraci.xml");
            XmlSerializer serializerHraci = new XmlSerializer(typeof(List<Hrac>));

            using (StreamWriter writer = new StreamWriter(cestaKSouboru))
            {
                serializerHraci.Serialize(writer, vsechniJestvujiciHraci);
            }
        }

        private void UlozZapas(List<Zapas> vsechnyJestvujicyZapasy)
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

        public void ZapisDoErrorLogu(string novyZapis)
        {
            string cestaKSouboru = cesta("Log", "ErrorLog.xml");
            StreamWriter writer = new StreamWriter(cestaKSouboru, append: true);
            writer.WriteLine(novyZapis);
            writer.Flush();     //v tuto chvili se zapise na disk
            writer.Close();     //uzavreni souboru

        }


    }
}
