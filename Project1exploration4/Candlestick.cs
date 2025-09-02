//Namespaces used in the project:
using System;                     //System namespace for basic system functions
using System.Collections.Generic; //Namespace to import List and other generic collection classes
using System.Globalization;       //Namespace for formatting dates, numbers, etc.
using System.Linq;                //Namespace for LINQ queries (Language Integrated Query)
using System.Text;                //Namespace for text processing and encoding
using System.Threading.Tasks;     //Namespace for asynchronous programming and parallelism

//Custom namespace for this project
namespace Project_Windows_Forms_App
{   
    //Created a public class Candlestick that represents candlesticks for displaying stock data for a specific user's preferrences 
    public class Candlestick
    {
        // Properties that store the data for each candlestick object: Date, Open, High, Low, Close, and Volume. 'get' and 'set' are accessors for the properties
        public DateTime Date { get; set; }  // Date property of candlestick
        public decimal Open { get; set; }   // Opening price property of candlestick
        public decimal High { get; set; }   // Highest price property of candlestick
        public decimal Low { get; set; }    // Lowest price property of candlestick
        public decimal Close { get; set; }  // Closing price property of candlestick
        public long Volume { get; set; }    // Trading volume property of candlestick

        // Default Constructor
        public Candlestick()
        {
        }

        // Public constructor 'Candlestick': initializes the properties of the candlestick object with the given values
        public Candlestick(DateTime date, decimal open, decimal high, decimal low, decimal close, long volume)
        {
            Date = date; //initialize the Date property with the date value
            Open = open; //initialize the Open property with the open price value
            High = high; //initialize the High property with the high price value
            Low = low; //   initialize the Low property with the low price value
            Close = close; //initialize the Close property with the close price value
            Volume = volume; //initialize the Volume property with the volume value
        }

        // Public constructor 'Candlestick': initializes the properties of the candlestick object using the data from a parsed .csv file
        public Candlestick(string data)
        {
            // Split method splits the data into values using comma and double quotes and removes empty entries that are redundant
            var values = data.Split(new char[] { ',', '\"' }, StringSplitOptions.RemoveEmptyEntries);

            // Handles the case when the data is not in the proper format (more or less than these 6 values: Date, Open, High, Low, Close, and Volume)
            if (values.Length != 6)
            {
                // Throw an exception with message for invalid formats 
                throw new ArgumentException("Data format is invalid. 6 values are expected.");
            }

            // TryParseExact method parses the data into the precise DateTime format (decided to handle separately for debugging purposes)
            if (!DateTime.TryParseExact(values[0], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                // Throw an exception with message for invalid data format
                throw new ArgumentException("Invalid date format.");
            }

            // TryParse method parses the data for the properties Open, High, Low, Close, and Volume with a proper format
            if (!decimal.TryParse(values[1], out decimal open) ||   // parsing data for Open property
                !decimal.TryParse(values[2], out decimal high) ||   // parsing data for High property
                !decimal.TryParse(values[3], out decimal low) ||    // parsing data for Low property
                !decimal.TryParse(values[4], out decimal close) ||  // parsing data for Close property
                !long.TryParse(values[5], out long volume))         // parsing data for Volume property
            {
                // Throw an exception for any numeric format that was not parsed properly
                throw new ArgumentException("Invalid numeric format.");
            }

            // Initializing the properties of each candlestick object with the parsed data (Round method rounds the prices to 2 decimal places )
            Date = date;                   // Date property initialized with the parsed date
            Open = Math.Round(open, 2);    // Open property initialized with the parsed open price value
            High = Math.Round(high, 2);    // High property initialized with the parsed high price value
            Low = Math.Round(low, 2);      // Low property initialized with the parsed low price value
            Close = Math.Round(close, 2);  // Close property initialized with the parsed close price value
            Volume = volume;               // Volume property initialized with the parsed volume value
        }

        // ToString method obtains and returns a string representation of the Candlestick object
        public override string ToString()
        {
            // Return the candlestick data in a formatted string (for displaying stock data):
            return $"Date: {Date:yyyy-MM-dd}, Open: {Open}, High: {High}, Low: {Low}, Close: {Close}, Volume: {Volume}";
        }
    }
}


