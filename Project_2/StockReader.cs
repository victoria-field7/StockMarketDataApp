//Namespaces used in the project:
using System;                     //System namespace for basic system functions
using System.Collections.Generic; //Namespace to import List and other generic collection classes
using System.IO;                  //Namespace to support file input/output operations
using System.Linq;                //Namespace for LINQ queries (Language Integrated Query)
using System.Text;                //Namespace for text processing and encoding
using System.Threading.Tasks;     //Namespace for asynchronous programming and parallelism

//Custom namespace for this project:
namespace Project_2
{
    // Public class 'StockReader' for reading the stock data from a .csv file
    public class StockReader
    {
        //FUNCTION: readCandlestickFromFile reads the candlesticks' data from from .csv file line by line, creating a new candlestick object and adding it to the list
        public List<Candlestick> readCandlesticksFromFile(string filePath)    //file path is specified to redirect to the selected file
        {
            var candlesticks = new List<Candlestick>();                       //List of candlesticks to store the data which was extracted from .csv file
            using (var reader = new StreamReader(filePath))                   //This line opens the specified file for reading
            {
                var header = reader.ReadLine();                               //Reads the header and skips it (do not need to be parsed)
                while (!reader.EndOfStream)                                   //Reads the file until the end of the file - also called 'end of stream'
                {
                    //ReadLine method reads a single line of text from an input stream
                    candlesticks.Add(new Candlestick(reader.ReadLine()));     //calls the constructor of Candlestick class to add created candlestick object to the list
                }
            }
            return candlesticks;   //Returns the list of candlesticks
        }
    }
}
