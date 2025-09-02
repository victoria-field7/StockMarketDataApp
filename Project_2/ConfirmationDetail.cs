using System;                                     //Namespace that contains basic classes and functions
using System.Collections.Generic;                 //Namespace that contains generic collections
using System.Linq;                                //Namespace for LINQ (Language Integrated Query) functionality
using System.Text;                                //Namespace for string manipulation
using System.Threading.Tasks;                     //Namespace for asynchronous programming

//Custome namespace for the project:
namespace Project_2
{
    //Public class to hold confirmation details for a specific price type:
    public class ConfirmationDetail       
    {
        public int Index { get; set; }                                       //Index of the confirmation detail
        public string PriceType { get; set; }                                //Price types: High, Low, Open, Close
        public double Price { get; set; }                                    //Price of the confirmation detail
        public double FibLevel { get; set; }                                 //Fibonacci level associated with the price
        public double DeltaPrice => Price - FibLevel;                        //Find data price as difference between price and fib level
        public double PercentOfRange { get; set; }                           //Jump in the price percentage from the top of the range
        public double PercentFibError => DeltaPrice / (FibLevel == 0 ? 1 : FibLevel);                  //Percentage error of the Fibonacci level
        public double RawConfirmations { get; set; }                         //Raw confirmations for the price type
        public string Support { get; set; }                                  //Support level connected to the price
        public string Resistance { get; set; }                               //Resistance level connected to the price
        public string SupportOrResistance { get; internal set; }             //Support or resistance level
    }
}
