// See https://aka.ms/new-console-template for more information
using PRG_ASSG;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Xml.Linq;







/* FEATURE 2 (JAYDEN) */
Dictionary<string, Flight> flightsDictionary = new Dictionary<string, Flight>();
Dictionary<string, string> flightsSRC = new Dictionary<string, string>();
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
                string checkforsrc = values[4];
                string src = "";
                if (checkforsrc != "")
                { src = checkforsrc; }
                string status = "on time";

                // Creates a Flight object using the data
                Flight flight = new Flight(flightNumber, origin, destination, expectedTime, status);

                // Adds the flight to the dictionary with FlightNumber as the key
                flightsDictionary[flight.FlightNumber] = flight;
                flightsSRC[flightNumber] = src;
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
Dictionary<string, Airline> airlineDictionary = new Dictionary<string, Airline>();
void LoadAirlines()
{
    using (StreamReader airlinesFile = new StreamReader("airlines.csv"))
    {
        string? airlines = airlinesFile.ReadLine();
        while ((airlines = airlinesFile.ReadLine()) != null)
        {
            string[] airlinesItem = airlines.Split(',');
            string name = airlinesItem[0];
            string code = airlinesItem[1];
            Airline airline = new Airline(name, code);
            airlineDictionary[code] = airline;
        }
    }
}
LoadAirlines();




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
            bool supportDDJB = Convert.ToBoolean(boardingGateItem[1]);
            bool supportCFFT = Convert.ToBoolean(boardingGateItem[2]);
            bool supportLWTT = Convert.ToBoolean(boardingGateItem[3]);
            BoardingGate withoutFlight = new BoardingGate(gateName, supportCFFT, supportDDJB, supportLWTT);
            boardingGateDictionary[gateName] = withoutFlight;
        }
    }
}
LoadBoardingGates();

/* FEATURE 3 (Jayden) */


void listflights()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15}{1,-19}{2,-20}{3,-18}{4,-24}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");
    foreach (Flight flight in flightsDictionary.Values)
    {
        foreach (Airline airline in airlineDictionary.Values)
        {
            if (flight.FlightNumber.Contains(airline.Code))
            {
                Console.WriteLine($"{flight.FlightNumber,-15}{airline.Name,-19}{flight.Origin,-20}{flight.Destination,-18}{flight.ExpectedTime,-24}");
            }
        }
    }
}

//listflights();

/* FEATURE 4 (Zi Liang) */

void listBoardingGates()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15}{1,-10}{2,-10}{3,-10}{4,-15}", "Gate Name", "DDJB", "CFFT", "LWTT", "Assigned Flight");

    foreach (BoardingGate gate in boardingGateDictionary.Values)
    {
        //check for any assigned gate
        string assignedFlight = "None";

        foreach (var flight in flightsDictionary.Values)
        {
            if (gate.Flight == null)
            {
                assignedFlight = "None";
                break;
            }
            else assignedFlight = gate.Flight.FlightNumber;
        }

        Console.WriteLine(
            $"{gate.GateName,-15}" +
            $"{gate.SupportsDDJB,-10}" +
            $"{gate.SupportsCFFT,-10}" +
            $"{gate.SupportsLWTT,-10}" +
            $"{assignedFlight,-15}"
        );
    }
}
listBoardingGates();




/* FEATURE 5 (Jayden) */


void AssignBoardingGate()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Assign a Boarding Gate to a Flight");
    Console.WriteLine("=============================================");

    Console.Write("Enter Flight Number: ");
    string flightnum = Console.ReadLine()?.Trim();

    // checks for flightnumber
    if (!flightsDictionary.ContainsKey(flightnum))
    {
        Console.WriteLine("Error: Flight not found.");
        return;
    }

    Console.Write("Enter Boarding Gate Name: ");
    string bgname = Console.ReadLine()?.Trim();

    // checks for boarding gate 
    if (!boardingGateDictionary.ContainsKey(bgname))
    {
        Console.WriteLine("Error: Boarding gate not found.");
        return;
    }

    Flight flight = flightsDictionary[flightnum];
    BoardingGate gate = boardingGateDictionary[bgname];
    if (gate.Flight != null)
    {
        Console.WriteLine($"Error: Boarding Gate {bgname} is already assigned to Flight {gate.Flight.FlightNumber}.");
        return;
    }
    // displays flight details
    Console.WriteLine($"Flight Number: {flight.FlightNumber}");
    Console.WriteLine($"Origin: {flight.Origin}");
    Console.WriteLine($"Destination: {flight.Destination}");
    Console.WriteLine($"Expected Time: {flight.ExpectedTime:dd/M/yyyy h:mm:ss tt}");
    if (!flightsSRC.TryGetValue(flightnum, out string src) || string.IsNullOrWhiteSpace(src))
    {
        src = "None";
    }
    Console.WriteLine($"Special Request Code: {src}");


    // displays boarding gate details
    Console.WriteLine($"Boarding Gate Name: {bgname}");
    Console.WriteLine($"Supports DDJB: {gate.SupportsDDJB}");
    Console.WriteLine($"Supports CFFT: {gate.SupportsCFFT}");
    Console.WriteLine($"Supports LWTT: {gate.SupportsLWTT}");

    // Assign the boarding gate
    gate.Flight = flight;

    Console.Write("Would you like to update the status of the flight? (Y/N): ");
    string choice = Console.ReadLine()?.Trim().ToUpper();

    if (choice == "Y")
    {
        Console.WriteLine("1. Delayed");
        Console.WriteLine("2. Boarding");
        Console.WriteLine("3. On Time");
        Console.Write("Please select the new status of the flight: ");

        string statusChoice = Console.ReadLine()?.Trim();
        switch (statusChoice)
        {
            case "1":
                flight.Status = "Delayed";
                break;
            case "2":
                flight.Status = "Boarding";
                break;
            case "3":
                flight.Status = "On Time";
                break;
            default:
                Console.WriteLine("Invalid choice. Keeping original status.");
                break;
        }
    }

    Console.WriteLine($"Flight {flight.FlightNumber} has been assigned to Boarding Gate {bgname}!");
}
//AssignBoardingGate();

//listBoardingGates();

/* FEATURE 7 (Zi Liang) */


void DisplayAirlineFlights()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Airline Name",-20}{"Airline Code"}");

    foreach (var airline in airlineDictionary.Values)
    {
        Console.WriteLine($"{airline.Code,-20}{airline.Name}");
    }

    Console.WriteLine("\nEnter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    Airline selectedAirline = airlineDictionary[airlineCode];

    foreach (Flight flight in flightsDictionary.Values)
    {
        if (flight.FlightNumber.Contains(airlineCode))
        {
            selectedAirline.AddFlight(flight);
        }
    }

    

    Console.WriteLine("\n=============================================");
    Console.WriteLine($"List of Flights for {selectedAirline.Name}");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Flight Number",-20}{"Origin",-20}{"Destination",-20}");

    foreach (var flight in selectedAirline.Flights.Values)
    {
        Console.WriteLine($"{flight.FlightNumber,-20}{flight.Origin,-20}{flight.Destination,-20}");
    }

    Console.WriteLine("\nEnter Flight Number: ");
    string selectflightNumber = Console.ReadLine().ToUpper();
    Flight selectedFlight = flightsDictionary[selectflightNumber];
    Console.WriteLine("\n=============================================");
    Console.WriteLine("Flight Details");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Flight Number",-20}: {selectedFlight.FlightNumber}");
    string selectedairlinename = "";
    foreach (Airline airline in airlineDictionary.Values)
    {
        if (selectedFlight.FlightNumber.Contains(airline.Code))
        {
            selectedairlinename = airline.Name;
        }
    }

    Console.WriteLine($"{"Airline Name",-20}: {selectedairlinename}");
    Console.WriteLine($"{"Origin",-20}: {selectedFlight.Origin}");
    Console.WriteLine($"{"Destination",-20}: {selectedFlight.Destination}");
    Console.WriteLine($"{"Expected Time",-20}: {selectedFlight.ExpectedTime:dd/MM/yyyy hh:mm tt}");

    if (flightsSRC.ContainsKey(selectflightNumber))
    {
        Console.WriteLine($"{"Special Request Code",-20}: {flightsSRC[selectflightNumber]}");
    }
    else
    {
        Console.WriteLine($"{"Special Request Code",-20}: None");
    }

    string selectedGate = "";

    foreach (BoardingGate gate in boardingGateDictionary.Values)
    {
        if (gate.Flight != null)
        {
            selectedGate = gate.GateName;
        }

        else
        {
            selectedGate = "None";
        }
    }

    Console.WriteLine($"{"Boarding Gate",-20}: {selectedGate}");

}
DisplayAirlineFlights();


/* FEATURE 8 (Zi Liang) */


void DisplayAirlineFlights()
{
    // List all airlines
    Console.WriteLine("\n=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15}{1,-25}", "Airline Code", "Airline Name");
    foreach (var airline in airlineDictionary.Values)
    {
        Console.WriteLine($"{airline.Code,-15}{airline.Name,-25}");
    }

    // Get airline code input
    Console.Write("\nEnter Airline Code: ");
    string airlineCode = Console.ReadLine()?.Trim().ToUpper();

    // Validate airline
    if (!airlineDictionary.TryGetValue(airlineCode, out Airline selectedAirline))
    {
        Console.WriteLine("Invalid Airline Code!");
        return;
    }

    // Get flights for the selected airline
    var airlineFlights = flightsDictionary.Values;

    // Display flights (simplified list)
    Console.WriteLine("\n=============================================");
    Console.WriteLine($"List of Flights for {selectedAirline.Name}");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15}{1,-25}{2,-20}{3,-25}",
        "Flight Number", "Origin", "Destination", "Departure/Arrival Time");

    foreach (var flight in airlineFlights)
    {
        Console.WriteLine(
            $"{flight.FlightNumber,-15}" +
            $"{flight.Origin,-25}" +
            $"{flight.Destination,-20}" +
            $"{flight.ExpectedTime:dd/MM/yyyy hh:mm tt}"
        );
    }

    // Get flight number input
    Console.Write("\nEnter Flight Number: ");
    string flightNumber = Console.ReadLine()?.Trim().ToUpper();

    // Find the flight
    var selectedFlight = airlineFlights
                        .FirstOrDefault(f => f.FlightNumber.Equals(flightNumber, StringComparison.OrdinalIgnoreCase));

    if (selectedFlight == null)
    {
        Console.WriteLine("Invalid Flight Number!");
        return;
    }

    // Display full flight details
    Console.WriteLine("\n=============================================");
    Console.WriteLine("Flight Details");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Flight Number:",-25} {selectedFlight.FlightNumber}");
    Console.WriteLine($"{"Airline:",-25} {selectedAirline.Name}");
    Console.WriteLine($"{"Origin:",-25} {selectedFlight.Origin}");
    Console.WriteLine($"{"Destination:",-25} {selectedFlight.Destination}");
    Console.WriteLine($"{"Departure/Arrival Time:",-25} {selectedFlight.ExpectedTime:dd/MM/yyyy hh:mm tt}");
    Console.WriteLine($"{"Status:",-25} {selectedFlight.Status}");

    // Special Request Code (from Flight class property)
    Console.WriteLine($"{"Special Request Code:",-25} {selectedFlight.SpecialRequestCode ?? "None"}");

    // Boarding Gate (search via boarding gates)
    var assignedGate = boardingGateDictionary.Values
        .FirstOrDefault(g => g.Flight?.FlightNumber == selectedFlight.FlightNumber);
    Console.WriteLine($"{"Boarding Gate:",-25} {assignedGate?.GateName ?? "Not Assigned"}");
}
DisplayAirlineFlights();



