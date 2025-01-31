//==========================================================
// Student Number : S10267761
// Student Name : Jayden Koh Kai Xuan
// Partner Name : Kua Zi Liang
//==========================================================
using System.Xml.Schema;

namespace PRG_ASSG
{
    internal class Flight : IComparable<Flight>
    {
        private string flightNumber;

        public string FlightNumber
        {
            get { return flightNumber; }
            set { flightNumber = value; }
        }



        private string origin;

        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        private string destination;

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        private DateTime expectedTime;

        public DateTime ExpectedTime
        {
            get { return expectedTime; }
            set { expectedTime = value; }
        }

        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
        }

        public virtual double CalculateFees()
        {
            double total = 0;
            total += 300;
            return total;
        }

        public int CompareTo(Flight other)
        {
            if (other == null) return 1;
            return ExpectedTime.CompareTo(other.ExpectedTime);
        }

        public override string ToString()
        {
            return $"{FlightNumber} {Origin} {Destination} {ExpectedTime:dd/MM/yyyy hh:mm tt}";
        }


    }
}
