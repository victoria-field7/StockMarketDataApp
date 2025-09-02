using System;                           //System namespace for basic system functions
using System.Collections.Generic;       //Namespace to import List and generic collection classes
using System.Linq;                      //Namespace for LINQ queries
using System.Text;                      //Namespace for text 
using System.Threading.Tasks;           //Namespace for  parallelism

// Custom generated namespace for the project:
namespace Project_2
{
    //New class: PeakValley to store the peaks and valleys of the stock market data:
    class PeakValley  //Name of the class
    {
        //Properties of PeakValley class:
        public bool Valley { get; set; } = true;                  //bool: if this point is a valley (true by default)
        public bool Peak { get; set; } = false;                   //bool: if this point is a peak (false by default)
        public int Index { get; set; }                            //index of the point in the stock data
        public DateTime Date { get; set; }                        //date of the stock data point
        public decimal Value { get; set; }                        //price of the stock data point
        public int leftMargin { get; set; }                       //left margin for peak/valley
        public int rightMargin { get; set; }                      //right margin for peak/valley

        //Constructor class PeakValley to initialize the properties of the objects:
        public PeakValley(int index, DateTime date, decimal value, bool isPeak, bool isValley, int ln, int rn) //all parameters are listed in (...)
        {
            Index = index;                                        //set the index of the peak/valley
            Date = date;                                          //set the date of the peak/valley
            Value = value;                                        //set the value of the peak/valley
            Peak = isPeak;                                        //set if the peak/valley is a peak (true/false)
            Valley = isValley;                                    //set if the peak/valley is a valley (true/false)
            leftMargin = ln;                                      //set the left margin for the peak/valley
            rightMargin = rn;                                     //set the right margin for the peak/valley
        }
    }     
}
