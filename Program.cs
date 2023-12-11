using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;


public enum FlightStatus //статус рейсу
{
    OnTime,
    Delayed,
    Cancelled,
    Boarding,
    InFlight
}

public class Flight //клас з даними
{
    public string FlightNumber { get; set; }
    public string Airline { get; set; }
    public string Destination { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string Gate { get; set; }
    public FlightStatus Status { get; set; }
    public TimeSpan Duration { get; set; }
    public string AircraftType { get; set; }
    public string Terminal { get; set; }
}

public class FlightInformationSystem  //клас з інфою про польоти
{
    private List<Flight> flights;

    public FlightInformationSystem() //створення списку польотів
    {
        flights = new List<Flight>();
    }

    public void AddFlight(Flight flight) //додавання рейсу
    {
        flights.Add(flight);
    }

    public bool RemoveFlight(string flightNumber) //видалення рейсу
    {
        var flightToRemove = flights.FirstOrDefault(f => f.FlightNumber == flightNumber);
        if (flightToRemove != null)
        {
            flights.Remove(flightToRemove);
            return true;
        }
        return false;
    }

    public List<Flight> GetFlightsByAirline(string airline) //вивести рейси від авіакомпаній
    {
        return flights.Where(f => f.Airline == airline)
                      .OrderBy(f => f.DepartureTime)
                      .ToList();
    }

    public List<Flight> GetDelayedFlights() //вивести затримані рейси 
    {
        return flights.Where(f => f.Status == FlightStatus.Delayed)
                      .OrderBy(f => f.DepartureTime)
                      .ToList();
    }

    public List<Flight> GetFlightsByDepartureDate(DateTime departureDate) //вивести рейси за запланованим часом відправлення
    {
        return flights.Where(f => f.DepartureTime.Date == departureDate.Date)
                      .OrderBy(f => f.DepartureTime)
                      .ToList();
    }

    public List<Flight> GetFlightsByTimeRangeAndDestination(DateTime startTime, DateTime endTime, string destination) //рейси за напрямком і терміном часу
    {
        return flights.Where(f => f.DepartureTime >= startTime && f.DepartureTime <= endTime && f.Destination == destination)
                      .OrderBy(f => f.DepartureTime)
                      .ToList();
    }

    public List<Flight> GetRecentArrivals(DateTime startTime) //вивести нещодавні прибуття
    {
        return flights.Where(f => f.ArrivalTime >= startTime)
                      .OrderBy(f => f.ArrivalTime)
                      .ToList();
    }

    public List<Flight> GetArrivalsByTimeRange(DateTime startTime, DateTime endTime) //вивести за періодом часу
    {
        return flights.Where(f => f.ArrivalTime >= startTime && f.ArrivalTime <= endTime)
                      .OrderBy(f => f.ArrivalTime)
                      .ToList();
    }

    public void LoadFlightsFromJson(string jsonFilePath) //загрузка файлів з json файлу
    {
        try
        {
            string jsonData = File.ReadAllText(jsonFilePath);
            var flightsData = JsonConvert.DeserializeObject<FlightData>(jsonData); //десереалізація об'єкту
            //var settings = new JsonSerializerSettings
            //{
            //    DateFormatString = "yyyy-MM-ddTHH:mm:ss"
            //};

            //var flightsData = JsonConvert.DeserializeObject<FlightData>(jsonData, settings);

            if (flightsData != null && flightsData.Flights != null) //перевірка чи файл не пустий
            {

                flights = flightsData.Flights;
            }
            else
            {
                Console.WriteLine("Error: Invalid JSON data format.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data from JSON file: {ex.Message}");
        }
    }

    private bool ValidateJson(string jsonFilePath) //створення маски за якою має бути json файл
    {
        try
        {
            //маска json файлу
            string jsonSchema = @"{
                'type': 'object',
                'properties': {
                    'Flights': {
                        'type': 'array',
                        'items': {
                            'type': 'object',
                            'properties': {
                                'FlightNumber': {'type': 'string'},
                                'Airline': {'type': 'string'},
                                'Destination': {'type': 'string'},
                                'DepartureTime': {'type': 'string', 'format': 'date-time'},
                                'ArrivalTime': {'type': 'string', 'format': 'date-time'},
                                'Gate': {'type': 'string'},
                                'Status': {'type': 'integer', 'enum': [0, 1, 2, 3, 4]},
                                'Duration': {'type': 'string'},
                                'AircraftType': {'type': 'string'},
                                'Terminal': {'type': 'string'}
                            },
                            'required': ['FlightNumber', 'Airline', 'Destination', 'DepartureTime', 'ArrivalTime', 'Status']
                        }
                    }
                },
                'required': ['Flights']
            }";
            JsonSchema schema = JsonSchema.Parse(jsonSchema);
            string jsonData = File.ReadAllText(jsonFilePath);
            JToken.Parse(jsonData).Validate(schema);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error validating JSON file: {ex.Message}");
            return false;
        }
    }
    public string SerializeFlightsToJson() //серіалізація
    {
        var flightsData = new FlightData { Flights = flights };
        return JsonConvert.SerializeObject(flightsData, Formatting.Indented);
    }

    public class FlightData //геттер та сеттер для ліста
    {
        public List<Flight> Flights { get; set; }
    }
}

class Program
{
    static void Main()
    {
        var flightSystem = new FlightInformationSystem();
        flightSystem.LoadFlightsFromJson("flights_data.json");

        // виконання запитів та виведення результатів
        Console.WriteLine("Flights by airline (Turkish Airlines):");
        PrintFlights(flightSystem.GetFlightsByAirline("Turkish Airlines"));

        Console.WriteLine("\nDelayed flights:");
        PrintFlights(flightSystem.GetDelayedFlights());

        Console.WriteLine("\nFlights on a specific day (2023-06-12):");
        PrintFlights(flightSystem.GetFlightsByDepartureDate(new DateTime(2023, 06, 12)));

        Console.WriteLine("\nFlights in a time range to Barcelona (2023-05-1T00:00:01 to 2023-05-31T23:59:59):");
        PrintFlights(flightSystem.GetFlightsByTimeRangeAndDestination(
            DateTime.Parse("2023-05-1T00:00:01"), DateTime.Parse("2023-05-31T23:59:59"), "Barcelona"));

        Console.WriteLine("\nRecent arrivals in the last hour:");
        PrintFlights(flightSystem.GetRecentArrivals(DateTime.Now.AddHours(-1)));

        // виведення у консоль та збереження результатів у новий JSON файл
        Console.WriteLine("\nSerialized Flights Data:");
        Console.WriteLine(flightSystem.SerializeFlightsToJson());

        // приклад видалення рейсу
        var flightToRemove = flightSystem.GetFlightsByAirline("Turkish Airlines").FirstOrDefault();
        if (flightToRemove != null)
        {
            flightSystem.RemoveFlight(flightToRemove.FlightNumber);
            Console.WriteLine($"\nRemoved flight {flightToRemove.FlightNumber}");
        }
        else
        {
            Console.WriteLine("\nNo flight to remove.");
        }

        // повторний вивід зміненого списку рейсів
        Console.WriteLine("\nFlights by airline (Turkish Airlines) after removal:");
        PrintFlights(flightSystem.GetFlightsByAirline("Turkish Airlines"));
    }

    static void PrintFlights(List<Flight> flights) //принт
    {
        foreach (var flight in flights)
        {
            Console.WriteLine($"{flight.FlightNumber} - {flight.Airline} - {flight.Destination} - {flight.DepartureTime} - {flight.Status}");
        }
    }
}
