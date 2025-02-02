//==========================================================
// Student Number : S10267761
// Student Name : Jayden Koh Kai Xuan
// Partner Name : Kua Zi Liang
//==========================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace PRG_ASSG
{
    internal class NORMFlight : Flight
    {
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
        : base(flightNumber, origin, destination, expectedTime, status)
        {
        }

        public override double CalculateFees()
        {

            return base.CalculateFees() - 50; // normal flights $50 discount

        }

        public override string ToString()
        {
            return base.ToString(); 
        }

    }
}
