using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_ASSG
{
    internal class LWTTFlight : Flight
    {
		private double requestFee;

		public double RequestFee
		{
			get { return requestFee; }
			set { requestFee = value; }
		}

        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee)
        : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = requestFee;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
