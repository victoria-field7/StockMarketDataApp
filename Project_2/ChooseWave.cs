using Project_Windows_Forms_App;                           //Namespac for the Windows Forms App that has the Candlestick class
using System;                                              //System namespace for basic system functions
using System.Collections.Generic;                          //Namespace for importing LIST and other generic collection classe
using System.ComponentModel;                               //Namespace for handling component and control events
using System.Linq;                                         //Namespace for LINQ queries (Language Integrated Query)
using System.Text;                                         //Namespace for text processing and encoding
using System.Threading.Tasks;                              //Namespace responsible for programming and parallelism
using System.Windows.Forms.DataVisualization.Charting;     //Namespace to enable chart controls and data visualization features

//Custom namespace for this project
namespace Project_2                                       
{
    public class ChooseWave                                                              //Public class that represents a wave  in the stock market chart
    {
        public int StartIndex { get; set; }                                              //Property that stores the starting index of the wave
        public int EndIndex { get; set; }                                                //Property that stores the ending index of the wave
        public decimal StartValue { get; set; }                                          //Property that stores the starting value of the wave
        public decimal EndValue { get; set; }                                            //Property that stores the ending value of the wave
        public bool IsUpWave => StartValue < EndValue;

        public Dictionary<double, decimal> FibonacciLevels { get; set; }                 //Property that stores the Fibonacci levels of the wave
        public List<int> ConfirmationIndices { get; set; }                               //Property that stores the confirmation indices of the wave

        public double Left => StartIndex;                                                //Property that stores the left index of the wave
        public double Right => EndIndex;                                                 //Property that stores the right index of the wave
        public double Width => EndIndex - StartIndex;                                    //Property that stores the width of the wave
        public double Height => Math.Abs((double)(EndValue - StartValue));               //Property that stores the height of the wave
        public double Top => (double)Math.Max(StartValue, EndValue);                     //Property that stores the top index of the wave
        public double Bottom => (double)Math.Min(StartValue, EndValue);                  //Bottom index of the wave
        private List<(double level, string label)> fibonacciLevels;                      //Fibonacci levels of the wave
        private List<(double x, double y)> confirmations;                                //Number of confirmations of the wave
        private double startPrice;                                                       //Starting price of the wave
        private double endPrice;                                                         //Ending price of the wave

        // Set Fibonacci percentages: 0%, 23.6%, 38.2%, 50%, 61.8%, 76.4%, and 100%:
        private readonly double[] fibonacciPercents = new double[] { 0.0, 0.236, 0.382, 0.5, 0.618, 0.764, 1.00 };     //percentages

        // Constructor that initializes the Fibonacci levels and confirmations
        public ChooseWave()
        {
            fibonacciLevels = new List<(double level, string label)>();                 //List of Fibonacci levels of the wave 
            confirmations = new List<(double x, double y)>();                           //List of confirmations of the wave
        }

        // Method to set the wave parameters: start index, end index, starting price, and ending price
        public void SetWaveParameters(int startIndex, int endIndex, double startY, double endY)
        {
            StartIndex = startIndex;                                                    //Set starting index of the wave
            EndIndex = endIndex;                                                        //Set ending index of the wave
            startPrice = startY;                                                        //Set starting price of the wave
            endPrice = endY;                                                            //Set ending price of the wave
            StartValue = (decimal)startY;                                               //Set starting value of the wave
            EndValue = (decimal)endY;                                                   //Set ending value of the wave

            // Calculate Fibonacci levels based on the starting and ending prices (function defined below)
            fibonacciLevels = CalculateFibonacciLevels(startY, endY);
            //FibonacciLevels = fibonacciLevels.ToDictionary(f => f.level, f => (decimal)f.level); // optional

            confirmations = new List<(double x, double y)>();                           //Redefine the old confirmations
        }

        //Function: calculates the Fibonacci levels based on the starting and ending prices
        public List<(double level, string label)> CalculateFibonacciLevels(double start, double end)
        {
            double height = Math.Abs(end - start);                                      //Calculate the height of the wave
            double minimum = Math.Min(end, start);                                      //Calculate the minimum price of the wave
            double maximum = Math.Max(end, start);                                      //Calculate the maximum price of the wave

            // Check if the wave is upward or downward and calculate the Fibonacci levels accordingly
            if (end > start)                                                            //For upwave: if the ending price is greater than the starting price
            {
                return fibonacciPercents                                                //Then return the Fibonacci percentages
                    .Select(p => (minimum + (height * p), $"{p * 100:F1}%"))            //Select the Fibonacci levels
                    .ToList();                                                          //And add them to the list
            }
            else                                                                        //For downwave: if the ending price is less than the starting price
            {
                return fibonacciPercents                                                //Then return the Fibonacci percentages
                    .Select(p => (maximum - (height * p), $"{p * 100:F1}%"))            //Select the Fibonacci levels
                    .ToList();                                                          //And add them to the list
            }
        }

        //Function: finds confirmations based on the Fibonacci levels and the candlestick data
        public List<(double x, double y)> FindWaveConfirmations(
            int startIndex,                                                             //Starting index of the wave
            int endIndex,                                                               //Ending index of the wave
            BindingList<Candlestick> candles,                                           //List of candlestick data
            List<(double Level, string Label)> fibLevels,                               //List of Fibonacci levels
            double fibMargin)                                                           //Margin of error for the Fibonacci levels
        {
            var confirmations = new List<(double, double)>();                           //List of confirmations
            var seen = new HashSet<string>();                                           //Set to keep track of already seen confirmations

            for (int i = startIndex; i <= endIndex && i < candles.Count; i++)           //Loop through the candlestick data
            {
                var candle = candles[i];                                                //Get the current candlestick data

                //Check the candlestick prices against the Fibonacci levels: opening, closing, high, and low prices
                double[] prices = {(double)candle.Open, (double)candle.Close, (double)candle.High, (double)candle.Low};

                // Loop through the Fibonacci levels and check if the candlestick prices are within the margin of error:
                foreach (var fib in fibLevels)
                {
                    foreach (var price in prices)                                       //Check each price against the Fibonacci levels
                    {
                        if (Math.Abs(price - fib.Level) <= fibMargin)                   //Check if the price is within the margin of error
                        {
                            string key = $"{i}_{price:F2}";                             //Create a unique key for the confirmation
                            if (!seen.Contains(key))                                    //Check if the confirmation has already been seen
                            {
                                confirmations.Add((i, price));                          //Add the confirmation to the list
                                seen.Add(key);                                          //Mark the confirmation as seen
                                Console.WriteLine($"CONFIRM: Candlestick {i} touched {price:F2}, close to Fib {fib.Level:F2}");    //Showing the confirmation in the console
                            }
                        }
                    }
                }
            }

            return confirmations;                                                       //Return the list of confirmations
        }

        // Function: gets the confirmation details based on the Fibonacci levels and the stock market data of the candlestick
        public List<ConfirmationDetail> GetConfirmationDetails(
        int startIndex,                                                                 //Starting index of the wave
        int endIndex,                                                                   //Ending index of the wave
        BindingList<Candlestick> candles,                                               //List of candlestick data
        List<(double level, string label)> fibLevels, double margin){                    //List of Fibonacci levels and margin of error                                      
            List<ConfirmationDetail> details = new List<ConfirmationDetail>();          //List to store the confirmation details

            for (int i = startIndex; i <= endIndex; i++)                                //Loop through the candlesticks
            {                               
                var candle = candles[i];                                                //Get the current candlestick data
                var priceTypes = new (string Name, double Value)[]                      //Array of price types
                {("High", (double)candle.High), ("Low", (double)candle.Low), ("Open", (double)candle.Open), ("Close", (double)candle.Close)};   //array of price types
                // Loop through the Fibonacci levels and check if the candlestick prices are within the margin of error:
                foreach (var fib in fibLevels)
                {
                    // Loop through the price types and check if the price is within the margin of error:
                    foreach (var pt in priceTypes)
                    {
                        double error = Math.Abs(pt.Value - fib.level);                  //Calculate the error between the price and the Fibonacci level
                        if (error <= margin)                                            //If the error is within the marging, then:
                        {
                            details.Add(new ConfirmationDetail                          //Create a new confirmation detail object
                            {
                                Index = i,                                              //assign the index of the candlestick
                                PriceType = pt.Name,                                    //assign the type of price (high, low, open, close)
                                Price = pt.Value,                                       //assign the value of the price
                                FibLevel = fib.level,                                   //assign the Fibonacci level
                                SupportOrResistance = (pt.Value >= fib.level) ? "Resistance" : "Support" //assign the support or resistance level
                            });
                        }
                    }
                }
            }

            return details;                                                             //Return the list of confirmation details
        }

    }
}




