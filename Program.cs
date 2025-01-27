// See https://aka.ms/new-console-template for more information
using PRG_ASSG;
using System.Reflection.PortableExecutable;







/* FEATURE 2 (JAYDEN) */
Dictionary<string, Flight> flightsDictionary = new Dictionary<string, Flight>();
void LoadFlights()
{
    string path = "flights.csv";
    try
    {
        using (StreamReader reader = new StreamReader(path))
        {
            // skip header
            reader.ReadLine();

            // reads CSV file
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                //
                if (values.Length != 5)
                {
                    Console.WriteLine($"Skipping invalid line: {line}");
                    continue;
                }

                // Parse the data from the CSV file
                string flightNumber = values[0];
                string origin = values[1];
                string destination = values[2];
                DateTime expectedTime = Convert.ToDateTime(values[3]);
                string status = "on time";

                // Create a Flight object using the parsed data
                Flight flight = new Flight(flightNumber, origin, destination, expectedTime, status);

                // Add the flight to the dictionary with FlightNumber as the key
                flightsDictionary[flight.FlightNumber] = flight;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error reading this file : " + ex.Message);
    }
}


LoadFlights();

/* FEATURE 1 (ZI LIANG) */

List<Airline> airlinesList = new List<Airline>();
void LoadAirlines()
{
    using (StreamReader airlinesFile = new StreamReader("airlines.csv"))
    {
        string? airlines = airlinesFile.ReadLine();
        while ((airlines = airlinesFile.ReadLine()) != null)
        {
            Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
            string[] airlinesItem = airlines.Split(',');
            string name = airlinesItem[0];
            string code = airlinesItem[1];
            foreach (Flight flight in flightsDictionary.Values)
            {
                flights[flight.FlightNumber] = flight;
                
            }
            
        }
    }
}


Dictionary<string, BoardingGate> boardingGateDictionary = new Dictionary<string, BoardingGate>();
void LoadBoardingGates()
{
    using (StreamReader boardingGatesFile = new StreamReader("boardinggates.csv"))
    {
        string? boardingGate = boardingGatesFile.ReadLine();
        while ((boardingGate = boardingGatesFile.ReadLine()) != null)
        {
            string[] boardingGateItem = boardingGate.Split(",");
            string gateName = boardingGateItem[0];
            bool supportCFFT = Convert.ToBoolean(boardingGateItem[1]);
            bool supportDDJB = Convert.ToBoolean(boardingGateItem[2]);
            bool supportLWTT = Convert.ToBoolean(boardingGateItem[3]);
            BoardingGate withoutFlight = new BoardingGate(gateName, supportCFFT, supportDDJB, supportLWTT);
            boardingGateDictionary[gateName] = withoutFlight;
        }
    }
}

/* FEATURE 3 (Jayden) */


void listflights()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    foreach (var flight in flightsDictionary.Values)
    {
        Console.WriteLine(flight);
    }
}

listflights();


