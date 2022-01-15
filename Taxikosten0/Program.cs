using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxikosten0
{
    class Program
    {
        static void Main(string[] args)
        {
            //Constante waarde
            double perKm = 1;
            double perMinDag = .25;
            double perMinNacht = .45;
            double weekendToeslag = 1.015;

            //Afstand bepalen
            Console.Write("Afstand rit, in km: ");
            string rawAfstand = Console.ReadLine();
            double ritAfstand = 0;
            while (!double.TryParse(rawAfstand, out ritAfstand))
            {
                Console.Write("\n" +
                    "Dit is geen geldige invoer.\n" +
                    "Typ alstublieft een getal: ");
                rawAfstand = Console.ReadLine();
            }

            //Begin- en eindtijd bepalen
            Console.Write("Begintijd rit, in \"uu:mm\": ");
            string rawBegin = Console.ReadLine();
            DateTime beginTijd = DateTime.Now;
            while(!DateTime.TryParse(rawBegin, out beginTijd))
            {
                Console.Write("\n" +
                    "Dit is geen geldige invoer.\n" +
                    "Typ de begintijd in de vorm \"uu:mm\": ");
                rawBegin = Console.ReadLine();
            }

            Console.Write("Eindtijd rit, in \"uu:mm\": ");
            string rawEind = Console.ReadLine();
            DateTime eindTijd = DateTime.Now;
            while (!DateTime.TryParse(rawEind, out eindTijd))
            {
                Console.Write("\n" +
                    "Dit is geen geldige invoer.\n" +
                    "Typ de begintijd in de vorm \"uu:mm\": ");
                rawEind = Console.ReadLine();
            }
            
            //Dag/nacht bepalen
            double uurBegin = beginTijd.Hour;
            double uurEind = eindTijd.Hour;
            double ritDuurDag = 0;
            double ritDuurNacht = 0;
            if ((uurBegin >= 0) && (uurEind < 8))
            {
                //Nachttarief
                TimeSpan ritTijd = eindTijd.Subtract(beginTijd);
                ritDuurNacht = ritTijd.TotalMinutes;
            }
            else if ((uurBegin >= 8) && (uurEind < 18))
            {
                //Dagtarief
                TimeSpan ritTijd = eindTijd.Subtract(beginTijd);
                ritDuurDag = ritTijd.TotalMinutes;
            }
            else if ((uurBegin >= 18) && (uurEind < 24))
            {
                //Nachttarief
                TimeSpan ritTijd = eindTijd.Subtract(beginTijd);
                ritDuurNacht = ritTijd.TotalMinutes;
            }
            else if ((uurBegin >= 0) && (uurEind < 18))
            {
                //Nacht + dag
                DateTime middenTijd = DateTime.Parse("8:00");
                TimeSpan ritTijd = middenTijd.Subtract(beginTijd);
                ritDuurNacht = ritTijd.TotalMinutes;
                ritTijd = eindTijd.Subtract(middenTijd);
                ritDuurDag = ritTijd.TotalMinutes;
            }
            else if ((uurBegin >= 8) && (uurEind < 24))
            {
                //Dag + Nacht
                DateTime middenTijd = DateTime.Parse("18:00");
                TimeSpan ritTijd = middenTijd.Subtract(beginTijd);
                ritDuurDag = ritTijd.TotalMinutes;
                ritTijd = eindTijd.Subtract(middenTijd);
                ritDuurNacht = ritTijd.TotalMinutes;
            }
            else if ((uurBegin >= 0) && (uurEind < 24))
            {
                //Nacht + Dag + Nacht
                DateTime middenTijd1 = DateTime.Parse("8:00");
                DateTime middenTijd2 = DateTime.Parse("18:00");
                TimeSpan ritTijd1 = middenTijd1.Subtract(beginTijd);
                double ritDuurNacht1 = ritTijd1.TotalMinutes;
                TimeSpan ritTijd2 = eindTijd.Subtract(middenTijd2);
                double ritDuurNacht2 = ritTijd2.TotalMinutes;
                ritDuurNacht = ritDuurNacht1 + ritDuurNacht2;
                TimeSpan ritTijd3 = middenTijd2.Subtract(middenTijd1);
                ritDuurDag = ritTijd3.TotalMinutes;
            }

            //Rit in weekend?
            Console.Write("Begon de rit in het weekend? y/n: ");
            string weekend = Console.ReadLine();

            //Prijs berekening
            double prijsAfstand = ritAfstand * perKm;
            double prijsTijd = ritDuurDag * perMinDag + ritDuurNacht * perMinNacht;
            double totaalPrijs = prijsAfstand + prijsTijd;
            while (true)
            {
                if (weekend == "n")
                {
                    break;
                }
                else if (weekend == "y")
                {
                    totaalPrijs = totaalPrijs * weekendToeslag;
                    break;
                }
                else
                {
                    Console.Write("\n" +
                    "Dit is geen geldige invoer.\n" +
                    "Kies y of n: ");
                    weekend = Console.ReadLine();
                }
            }
            //Prijs weergeven
            Console.WriteLine("\nDe totaalprijs is: {0:N} euro.",totaalPrijs);
            Console.Write("Druk op enter om af te sluiten.");
            Console.ReadLine();
        }
    }
}
