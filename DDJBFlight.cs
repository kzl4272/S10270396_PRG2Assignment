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

namespace PRG_ASSG
{
    internal class DDJBFlight : Flight
    {
        private double requestFee;

        public double RequestFee
        {
            get { return requestFee; }
            set { requestFee = value; }
        }

        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee)
        : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = 300;
        }

        public override double CalculateFees()
        {
            return base.CalculateFees() + RequestFee; // adds request fee
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
