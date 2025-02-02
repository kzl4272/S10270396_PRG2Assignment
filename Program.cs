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
            // skips header
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

                // parses the data from the CSV file
                string flightNumber = values[0];
                string origin = values[1];
                string destination = values[2];
                DateTime expectedTime = Convert.ToDateTime(values[3]);
                string checkforsrc = values[4];
                string src = "";
                if (checkforsrc != "")
                { src = checkforsrc; }
                string status = "Scheduled";

                // creates a Flight object using the data
                Flight flight = new Flight(flightNumber, origin, destination, expectedTime, status);

                // adds the flight to the dictionary with FlightNumber as the key
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


//LoadFlights();

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
//LoadAirlines();




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
//LoadBoardingGates();

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
//listBoardingGates();




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
/* FEATURE 6 (Jayden) */
void CreateFlight()
{
    while (true)
    {
        Console.Write("Enter Flight Number: ");
        string flightnum = Console.ReadLine()?.ToUpper();
        Console.Write("Enter Origin: ");
        string origin = Console.ReadLine();
        Console.Write("Enter Destination: ");
        string destination = Console.ReadLine();
        DateTime departureArrivalTime;
        while (true)
        {
            Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
            string dateTimeInput = Console.ReadLine();

            if (DateTime.TryParseExact(dateTimeInput, "d/M/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out departureArrivalTime))
                break;
            else
                Console.WriteLine("Invalid date format. Please enter in dd/mm/yyyy hh:mm format: ");
        }
        Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
        string specialRequestCode = Console.ReadLine();
        // checks the src
        string[] src = { "CFFT", "DDJB", "LWTT", "None" };
        while (!src.Contains(specialRequestCode))
        {
            Console.Write("Invalid SRC. Enter one of (CFFT/DDJB/LWTT/None): ");
            specialRequestCode = Console.ReadLine();
        }
        Flight newFlight = new Flight(flightnum, origin, destination, departureArrivalTime, specialRequestCode);

        // adds flight to the flights dictionary 
        flightsDictionary[flightnum] = newFlight;

        Console.WriteLine($"Flight {flightnum} has been added!");

        Console.WriteLine("Would you like to add another flight? (Y/N)");
        string response = Console.ReadLine().Trim().ToUpper();

        if (response == "N")
            break;
    }

}
//CreateFlight();
//listflights();

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
//DisplayAirlineFlights();



/* FEATURE 8 (Zi Liang) */

void ModifyFlightDetails()
{
    Console.WriteLine("\n=============================================");
    Console.WriteLine("Modify Flight Details");
    Console.WriteLine("=============================================");

    // 1. List all airlines
    Console.WriteLine("\n=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15}{1,-25}", "Airline Code", "Airline Name");
    foreach (var airline in airlineDictionary.Values)
    {
        Console.WriteLine($"{airline.Code,-15}{airline.Name,-25}");
    }

    // 2. Get airline code input
    Console.Write("\nEnter Airline Code: ");
    string airlineCode = Console.ReadLine()?.Trim().ToUpper();

    // Validate airline
    if (!airlineDictionary.TryGetValue(airlineCode, out Airline selectedAirline))
    {
        Console.WriteLine("Invalid Airline Code!");
        return;
    }

    // 3. Get flights for the airline
    var airlineFlights = flightsDictionary.Values
                        .Where(f => f.FlightNumber.StartsWith(airlineCode))
                        .ToList();

    // 4. Display flights
    Console.WriteLine("\n=============================================");
    Console.WriteLine($"List of Flights for {selectedAirline.Name}");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15}{1,-20}{2,-20}",
        "Flight Number", "Origin", "Destination");
    foreach (var flight in airlineFlights)
    {
        Console.WriteLine($"{flight.FlightNumber,-15}" +
                          $"{flight.Origin,-20}" +
                          $"{flight.Destination,-20}");
    }

    // 5. Get flight number input
    Console.Write("\nEnter Flight Number: ");
    string flightNumber = Console.ReadLine()?.Trim().ToUpper();

    // Find flight
    var selectedFlight = airlineFlights.FirstOrDefault(f =>
        f.FlightNumber.Equals(flightNumber, StringComparison.OrdinalIgnoreCase));

    if (selectedFlight == null)
    {
        Console.WriteLine("Flight not found!");
        return;
    }

    // 6. Choose action
    Console.WriteLine("\n1. Modify Flight\n2. Delete Flight");
    Console.Write("Choose an option: ");
    string action = Console.ReadLine();

    switch (action)
    {
        case "1": // Modify
            Console.WriteLine("\n1. Modify Basic Information\n2. Modify Status\n3. Modify SRC\n4. Modify Boarding Gate");
            Console.Write("Choose an option: ");
            string modifyChoice = Console.ReadLine();

            switch (modifyChoice)
            {
                case "1": // Basic Info
                    Console.Write("Enter new Origin: ");
                    string newOrigin = Console.ReadLine();
                    Console.Write("Enter new Destination: ");
                    string newDestination = Console.ReadLine();
                    Console.Write("Enter new Time (dd/MM/yyyy HH:mm): ");

                    if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newTime))
                    {
                        selectedFlight.Origin = newOrigin;
                        selectedFlight.Destination = newDestination;
                        selectedFlight.ExpectedTime = newTime;
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format!");
                        return;
                    }
                    break;

                case "2": // Status
                    Console.Write("Enter new Status: ");
                    selectedFlight.Status = Console.ReadLine();
                    break;

                case "3": // Special Request Code
                    Console.Write("Enter new SRC (leave blank to remove): ");
                    string? src = Console.ReadLine();
                    flightsSRC[selectedFlight.FlightNumber] = string.IsNullOrWhiteSpace(src) ? "None" : src;
                    break;

                case "4": // Boarding Gate
                    Console.Write("Enter new Boarding Gate: ");
                    string newBoardingGate = Console.ReadLine();
                    if (boardingGateDictionary.TryGetValue(newBoardingGate, out BoardingGate newGate))
                    {
                        if (newGate.Flight != null)
                        {
                            Console.WriteLine($"Error: Boarding Gate {newBoardingGate} is already assigned to Flight {newGate.Flight.FlightNumber}.");
                        }
                        else
                        {
                            // Unassign current gate if any
                            foreach (var gate in boardingGateDictionary.Values)
                            {
                                if (gate.Flight == selectedFlight)
                                {
                                    gate.Flight = null;
                                    break;
                                }
                            }
                            // Assign new gate
                            newGate.Flight = selectedFlight;
                            Console.WriteLine($"Boarding Gate {newBoardingGate} assigned to Flight {selectedFlight.FlightNumber}.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Boarding Gate!");
                    }
                    break;

                default:
                    Console.WriteLine("Invalid option!");
                    return;
            }
            Console.WriteLine("\nFlight updated successfully!");
            break;

        case "2": // Delete
            Console.Write("Confirm deletion (Y/N): ");
            string confirm = Console.ReadLine()?.ToUpper();
            if (confirm == "Y")
            {
                flightsDictionary.Remove(selectedFlight.FlightNumber);
                Console.WriteLine("Flight deleted!");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
            break;

        default:
            Console.WriteLine("Invalid option!");
            return;
    }

    // 7. Display updated details (if modified)
    if (action == "1")
    {
        Console.WriteLine("\n=============================================");
        Console.WriteLine("Updated Flight Details");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Flight Number:",-25} {selectedFlight.FlightNumber}");
        Console.WriteLine($"{"Airline:",-25} {selectedAirline.Name}");
        Console.WriteLine($"{"Origin:",-25} {selectedFlight.Origin}");
        Console.WriteLine($"{"Destination:",-25} {selectedFlight.Destination}");
        Console.WriteLine($"{"Departure/Arrival:",-25} {selectedFlight.ExpectedTime:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"{"Status:",-25} {selectedFlight.Status}");
        Console.WriteLine($"{"Special Request Code:",-25} {flightsSRC[selectedFlight.FlightNumber] ?? "None"}");
        Console.WriteLine($"{"Boarding Gate:",-25} {boardingGateDictionary.Values.FirstOrDefault(g => g.Flight == selectedFlight)?.GateName ?? "Not Assigned"}");
    }
}
//ModifyFlightDetails();


/* FEATURE 9 (Jayden) */

void DisplaySortedFlights()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15}{1,-20}{2,-20}{3,-20}{4,-25}\n{5,-15}{6,-15}",
        "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time", "Status", "Boarding Gate");

    // sorts the flights by ExpectedTime
    var sortedFlights = flightsDictionary.Values.OrderBy(flight => flight.ExpectedTime).ToList();

    foreach (Flight flight in sortedFlights)
    {
        // finds the airline name
        string airlineName = "";
        foreach (Airline airline in airlineDictionary.Values)
        {
            //matches flightnumber to code
            if (flight.FlightNumber.Contains(airline.Code))
            {
                airlineName = airline.Name;
                break;
            }
        }

        // Find the boarding gate (if assigned)
        string boardingGate = "Unassigned";
        foreach (BoardingGate gate in boardingGateDictionary.Values)
        {
            if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
            {
                boardingGate = gate.GateName;
                break;
            }
        }

        // Ensure date formatting is correct
        string formattedDate = flight.ExpectedTime.ToString("dd/M/yyyy h:mm:ss tt");

        // Ensure status is correctly displayed
        string flightStatus = flight.Status ?? "Scheduled";

        // displays the flight details
        Console.WriteLine("{0,-15}{1,-20}{2,-20}{3,-20}{4,-25}\n{5,-15}{6,-15}",
            flight.FlightNumber,
            airlineName,
            flight.Origin,
            flight.Destination,
            formattedDate,
            flightStatus,
            boardingGate);
    }
}
//DisplaySortedFlights();

//loading (Jayden)
void LoadAll()
{
    Console.WriteLine("Loading Airlines...");
    LoadAirlines();
    Console.WriteLine("8 Airlines Loaded!");
    Console.WriteLine("Loading Boarding Gates...");
    LoadBoardingGates();
    Console.WriteLine("66 Boarding Gates Loaded!");
    Console.WriteLine("Loading Flights...");
    LoadFlights();
    Console.WriteLine("30 Flights Loaded!");

}
LoadAll();

