namespace DataCare.FileFormat.Leerlijnenpakket
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Forms;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using DataCare.Model;
    using DataCare.Model.Onderwijsinhoudelijk.Leerlijnen;
    using DataCare.Utilities;

    public class ImportLeerlijnenpakket
    {
        public string PakketNaam { get; private set; }

        public XDocument SourceDocument { get; private set; }

        private readonly Dictionary<string, Vakgebied> vakgebieden;

        public IEnumerable<Vakgebied> Vakgebieden
        {
            get { return this.vakgebieden.Values; }
        }

        public ImportLeerlijnenpakket()
        {
            this.vakgebieden = new Dictionary<string, Vakgebied>();
        }

        public Leerlijnenpakket GetLeerlijnenPakket(string file)
        {
            Leerlijnenpakket leerlijnenpakket = null;
            try
            {
                SourceDocument = XDocument.Load(file);
                if (SourceDocument != null)
                {
                    PakketNaam = SourceDocument.XPathSelectElement("/Leerlijnpackage/Leerlijn").Attribute("Naam").Value;
                    var vakgebiedItems = SourceDocument.XPathSelectElements("/Leerlijnpackage/Leerlijn/Vakgebieden/Vakgebied");
                    var leerlijnen = vakgebiedItems.Select(GetLeerlijn).ToList();

                    DateTime invuldatum = DateTimeExtensions.Now;

                    leerlijnenpakket = new Leerlijnenpakket(
                        Guid.NewGuid(),
                        Guid.NewGuid(),
                        PakketNaam,
                        invuldatum,
                        definitief: true,
                        leerlijnen: leerlijnen,
                        auditTrail: new AuditTrail<Leerlijnenpakket>());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Het bestand bevat geen geldig leerlijnenpakketDefinitie.");
            }

            return leerlijnenpakket;
        }

        private static int GetNiveau(XElement element)
        {
            int returnValue;

            // RJ In overleg met Ernst en Jan willem besloten dat op het moment dat er geen niveau attribuut aanwezig is, zoals bij Plancius het bijhorende niveau 0 is.
            XAttribute attributeValue = element.Attribute("Niveau");

            if (attributeValue == null || !int.TryParse(attributeValue.Value, out returnValue))
            {
                returnValue = 0;
            }

            return returnValue;
        }

        private static IEnumerable<Doel> GetHoofdDoelen(IEnumerable<XElement> element, IEnumerable<Doel> source)
        {
            if (source != null)
            {
                return
                    source.Where(e => element.Any(d => d.Attribute("Naam").Value == e.Naam && string.Equals(d.Attribute("IsHoofdDoelstelling").Value, "true", StringComparison.OrdinalIgnoreCase)))
                        .ToList();
            }

            return null;
        }

        private Leerlijn GetLeerlijn(XElement vakgebiedItem)
        {
            var deelgebiedItems = vakgebiedItem.XPathSelectElements("./Items/Item");
            var deellijnen = new List<Deellijn>();
            var deelgebieden = new List<string>();

            string vakgebiednaam = vakgebiedItem.Attribute("Naam").Value;

            foreach (var deelgebiedItem in deelgebiedItems)
            {
                Deellijn deellijn = GetDeellijn(deelgebiedItem);
                deelgebieden.Add(deellijn.Deelgebied);
                deellijnen.Add(deellijn);
            }

            Vakgebied vakgebied;
            vakgebied = this.vakgebieden.TryGetValue(vakgebiednaam, out vakgebied)
                ? new Vakgebied(vakgebiednaam, vakgebied.Deelgebieden.Union(deelgebieden), new AuditTrail<Vakgebied>(vakgebied))
                : new Vakgebied(vakgebiednaam, deelgebieden, new AuditTrail<Vakgebied>());

            this.vakgebieden[vakgebiednaam] = vakgebied;

            ////return new Leerlijn(Guid.NewGuid(), new Vakgebied(vakgebiedItem.Attribute("Naam").Value, deelgebieden.Select(d => d.Naam)), deellijnen);
            return new Leerlijn(Guid.NewGuid(), vakgebied, deellijnen, new AuditTrail<Leerlijn>());
        }

        private Deellijn GetDeellijn(XElement element)
        {
            var subitems = element.XPathSelectElements("./Subitems/Subitem").ToList();

            var niveaus = subitems.Select(GetNiveau).Distinct();
            IEnumerable<Niveautrede> tredes = niveaus.Select(
                n =>
                new Niveautrede(
                    Guid.NewGuid(),
                    n,
                    new List<Doel>(
                    subitems.Where(i => GetNiveau(i) == n)
                    .Select(d => new Doel(d.Attribute("Naam").Value, new AuditTrail<Doel>()))),
                    new AuditTrail<Niveautrede>())).ToList();

            string deelgebiednaam = element.Attribute("Naam").Value;

            var deellijn = new Deellijn(Guid.NewGuid(), deelgebiednaam, tredes, GetHoofdDoelen(subitems, tredes.SelectMany(d => d.Doelen)), new AuditTrail<Deellijn>());

            // give unique deellijn ID
            ////deellijn.DtoId = GetNextDeellijnDtoId();

            return deellijn;
        }
    }
}