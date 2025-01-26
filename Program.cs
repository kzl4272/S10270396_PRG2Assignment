using PRG_ASSG;


/* FEATURE 1 (ZI LIANG) */

List<Airline> airlinesList = new List<Airline>();
void Loadingairlines()
{
    using (StreamReader airlinesFile = new StreamReader("airlines.csv"))
    {
        string? airlines = airlinesFile.ReadLine();
        while ((airlines = airlinesFile.ReadLine()) != null)
        {
            Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
            string[] airlinesDetails = airlines.Split(',');
            string name = airlinesDetails[0];
            string code = airlinesDetails[1];
            
        }
    }
}

/* FEATURE 2 (JAYDEN) */



/* FEATURE 3 (ZI LIANG) */