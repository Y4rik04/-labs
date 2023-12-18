using System;
using System.Collections.Generic;
using System.IO;

// головний клас додатка
public class TableReservationApp
{
    static void Main(string[] args)
    {
        ReservationManager manager = new ReservationManager();
        manager.AddRestaurant("A", 10);
        manager.AddRestaurant("B", 5);

        Console.WriteLine(manager.BookTable("A", new DateTime(2023, 12, 25), 3)); // True
        Console.WriteLine(manager.BookTable("A", new DateTime(2023, 12, 25), 3)); // False
    }
}

// клас менеджера бронювання
public class ReservationManager
{
    private List<Restaurant> restaurants;
    public List<Restaurant> Restaurants 
    {
        get { return restaurants; }
    }
    //додав цю властивість до класу ReservationManager для того, щоб  тест міг звертатися до неї
    public ReservationManager()
    {
        restaurants = new List<Restaurant>();
    }

    // метод для додавання ресторану
    public void AddRestaurant(string name, int tableCount)
    {
        try
        {
            Restaurant restaurant = new Restaurant(name, tableCount);
            restaurants.Add(restaurant);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
        
    }

    // метод для бронювання столика
    public bool BookTable(string restaurantName, DateTime date, int tableNumber)
    {
        foreach (var restaurant in restaurants)
        {
            if (restaurant.Name == restaurantName)
            {
                if (tableNumber < 0 || tableNumber >= restaurant.Tables.Length)
                {
                    throw new Exception("Invalid table number");
                }

                return restaurant.Tables[tableNumber].Book(date);
            }
        }

        throw new Exception("Restaurant not found");
    }
    public void SortRestaurantsByAvailabilityForUsersMethod(DateTime dt)
    {
        try
        {
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 0; i < restaurants.Count - 1; i++)
                {
                    int avTc = CountAvailableTablesForRestaurantClassAndDateTimeMethod(restaurants[i], dt); // доступні столики зараз
                    int avTn = CountAvailableTablesForRestaurantClassAndDateTimeMethod(restaurants[i + 1], dt); // досупні столики пізніше

                    if (avTc < avTn)
                    {
                        // міняємо місцями
                        var temp = restaurants[i];
                        restaurants[i] = restaurants[i + 1];
                        restaurants[i + 1] = temp;
                        swapped = true;
                    }
                }
            } while (swapped);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    // обрахунок доступних столиків в закладі
    public int CountAvailableTablesForRestaurantClassAndDateTimeMethod(Restaurant r, DateTime dt)
    {
        try
        {
            int count = 0;
            foreach (var t in r.Tables)
            {
                if (!t.IsBooked(dt))
                {
                    count++;
                }
            }
            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return 0;
        }
    }
}

// клас ресторану
public class Restaurant
{
    public string Name { get; set; }
    public RestaurantTable[] Tables { get; set; }

    public Restaurant(string name, int tableCount)
    {
        Name = name;
        Tables = new RestaurantTable[tableCount];
        for (int i = 0; i < tableCount; i++)
        {
            Tables[i] = new RestaurantTable();
        }
    }
}


// клас столика
public class RestaurantTable
{
    private List<DateTime> bookedDates;

    public RestaurantTable()
    {
        bookedDates = new List<DateTime>();
    }

    // метод для бронювання столика
    public bool Book(DateTime date)
    {
        try
        {
            if (bookedDates.Contains(date))
            {
                return false;
            }

            bookedDates.Add(date);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return false;
        }
    }

    // метод для перевірки, чи столик заброньований на певну дату
    public bool IsBooked(DateTime date)
    {
        return bookedDates.Contains(date);
    }
}

