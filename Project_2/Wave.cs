using System;                             //System namespace for basic system functions
using System.Collections.Generic;         //Namespace to import List
using System.Linq;                        //Namespace for LINQ queries
using System.Text;                        //Namespace for text processing
using System.Threading.Tasks;             //Namespace for asynchronous programming and parallelism

// Custom generated namespace for the project:
namespace Project_2
{
    //Class: Wave to represent a wave in the stock market data
    public class Wave               
    {
        public int StartIndex { get; set; }                           //property to store the start index of the wave
        public int EndIndex { get; set; }                             //property to store the end index of the wave
        public DateTime StartDate { get; set; }                       //property to store the start date of the wave
        public DateTime EndDate { get; set; }                         //property to store the end date of the wave
        public decimal StartValue { get; set; }                       //property to store the start value of the wave
        public decimal EndValue { get; set; }                         //property to store the end value of the wave
        public bool IsUpWave { get; set; }                            //property to store if the wave is an upward wave 

        public double Left => StartIndex;                             //property to get the left bound of the wave 
        public double Right => EndIndex;                              //property to get the right bound of the wave
        public double Bottom => Math.Min((double)StartValue, (double)EndValue);           //property to get the bottom value of the wave
        public double Top => Math.Max((double)StartValue, (double)EndValue);              //property to get the top value of the wave
        public double Width => EndIndex - StartIndex;                                     //property to get the width of the wave
        public double Height => Top - Bottom;                                             //property to get the height of the wave

        //Constructor for the waves: initalize the properties of the wave objects:
        public Wave(int startIndex, int endIndex, DateTime startDate, DateTime endDate, decimal startValue, decimal endValue)
        {
            StartIndex = startIndex;                                 //set the start index of the wave
            EndIndex = endIndex;                                     //set the end index of the wave
            StartDate = startDate;                                   //set the start date of the wave
            EndDate = endDate;                                       //set the end date of the wave
            StartValue = startValue;                                 //set the start value of the wave
            EndValue = endValue;                                     //set the end value of the wave
            IsUpWave = startValue < endValue;                        //determine if the wave is an upward wave
        }

        //Method: override ToString method to provide string representation of wave object
        public override string ToString()                            //overide the method
        {
            return $"{StartDate:MM/dd/yyyy} - {EndDate:MM/dd/yyyy}"; //return formatted string 
        }
    }
}
