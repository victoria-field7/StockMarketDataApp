using Project_Windows_Forms_App;             //Namespac for the Windows Forms App that has the Candlestick class
using System;                                //System namespace for basic system functions
using System.Collections.Generic;            //Namespace for importing LIST and other generic collection classe
using System.ComponentModel;                 //Namespace for handling component and control events
using System.Linq;                           //Namespace for LINQ queries (Language Integrated Query)
using System.Text;                           //Namespace for text processing and encoding
using System.Threading.Tasks;                //Namespace responsible for programming and parallelism
using System.Windows.Forms;                  //Namespace for importing LIST and other generic collection classes



//Student Name: Viktoriia Solomennikova (U99858124)
//Class: Software System Development C#
/*Project description: This Windows desktop form application is designed to analyze stock market data using candlestick charts.
The main functionality avaliable to the user includes:
          - loading stock data from a CSV file,
          - identifying peaks and valleys in the data,
          - validating them based on margin levels, and
          - determining waves (up and down) based on the identified peaks and valleys.
This apllication display the results of the data handling in a chart for visual analysis.
*/


// Custom namespace for this project
namespace Project_2
{
    //Class PeakValleyIdentifier: reposonsible for identifying peaks (local maxima) and valleys (local minima) based on the candlestick data:
    internal class PeakValleyIdentifier
    {
        public Dictionary<int, List<PeakValley>> marginPeaksAndValleys = new Dictionary<int, List<PeakValley>>();  //dictionary to store the peaks and valleys for each margin level (1-8). The key is the margin value, and the value is a list of PeakValley objects.

        //Function: to determine the peaks and valleys for a default margin 1
        public List<PeakValley> findPeaksAndValleys(BindingList<Candlestick> candlesticks) //pass binding list a parameter
        {
            List<PeakValley> peaksAndValleys = new List<PeakValley>();                     //list to store the peaks and valley identified from candlestick data
            if (candlesticks == null)                                                      //handle the case when the candlestick data wasn't loaded properly 
            {
                MessageBox.Show("Candlesticks is null — data failed to load.");            //show user a message that data failed to load
                return null;                                                               //exit early to avoid the NullReferenceException    
            }
            else if (candlesticks.Count == 0)                                              //else: if the candlestick list is empty (no data to process)
            {
                MessageBox.Show("Candlesticks loaded, but it's empty.");                   //show user a message that the no candlestick data was loaded (empty lis)
                return null;                                                               //exit early to avoid processing an empty list
            }

            //Loop to iterate through the list of candlesticks
            for (int i = 1; i < candlesticks.Count - 1; i++)
            {
                decimal currentPeakPrice = candlesticks[i].High;                            //get the current peak's price from the candlestick list( (high price)
                decimal currentValleyPrice = candlesticks[i].Low;                           //get the current valley's price from the candlestick list (low price)

                bool isPeak = currentPeakPrice >= candlesticks[i - 1].High && currentPeakPrice >= candlesticks[i + 1].High;            //check if the current price is a peak: compare high prices for previous and next candlestick
                bool isValley = currentValleyPrice <= candlesticks[i - 1].Low && currentValleyPrice <= candlesticks[i + 1].Low; ;      //check if the current price is a valley: compare low prices for previous and next candlestick

                if (isPeak)                                                              //check for local maxima (if the current price is a peak)
                {
                    peaksAndValleys.Add(new PeakValley(i, candlesticks[i].Date, currentPeakPrice, true, false, i - 1, i + 1)); //if it's a peak, add to the list 
                }
                if (isValley)                                                            //check for local minima (if the current price is a valley)
                {
                    peaksAndValleys.Add(new PeakValley(i, candlesticks[i].Date, currentValleyPrice, false, true, i - 1, i + 1));  //if it's a valley, add to the list
                }
            }
            return peaksAndValleys;                                                       //return the generated list of peaks and valleys
        }


        // Function: create a dictionary that contains a list of peaks and valleys for each margin level (1-8)
        public Dictionary<int, List<PeakValley>> findPeaksAndValleysByMargin(BindingList<Candlestick> candlesticks)
        {
            
            if (marginPeaksAndValleys == null)                                            //check if the dictionary is not initialized yet        
            {
                marginPeaksAndValleys = new Dictionary<int, List<PeakValley>>();          //if it's null, initialize it to store peaks and valleys
            }
            marginPeaksAndValleys.Clear();                                                //clear the dictionary to start fresh for each function call

            // Find peaks and valleys for the margin of 1 (default margin) 
            var allPeaksAndValleys = findPeaksAndValleys(candlesticks);                   //function call to find peak and valleys (margin 1)

            marginPeaksAndValleys[1] = allPeaksAndValleys;                                //add the list of peaks and valleys to the dictionary with key 1
            
            Console.WriteLine($"Margin: {1}, Total Peaks and Valleys: {allPeaksAndValleys.Count}");      //debug statement: print the count of peaks and valleys for the current margin

            // Validate peaks and valleys for each margin level from 2 to 8 (inclusive); then, populate the dictionary
            for (int margin = 2; margin <= 8; margin++)       //loop through margin from 2 to 8
            {
                // Function call to validate the peaks and valleys based on the current margin level
                List<PeakValley> validatedPeaksAndValleys = validatePeaksAndValleysByMargin(allPeaksAndValleys, margin);

                // For the current margin level, add list (validated) to the dictionary 
                marginPeaksAndValleys[margin] = validatedPeaksAndValleys;

                // Debuging statement: print the number of peaks and valleys for the currrent margin
                Console.WriteLine($"Margin: {margin}, Total Peaks and Valleys: {validatedPeaksAndValleys.Count}");
            }
            //return the dictionary for all margin levels 1-8
            return marginPeaksAndValleys;
        }


        // Function: Based on the specified margin, validate the peaks and valleys to ensure they meet the margin constraints
        public List<PeakValley> validatePeaksAndValleysByMargin(List<PeakValley> sortedPeaksAndValleys, int desiredMargin)
        {
            if (sortedPeaksAndValleys == null || sortedPeaksAndValleys.Count == 0)          //check if the list of peaks and valleys is empty (null)
            {
                return new List<PeakValley>();                                              //if empty: don't process it and return an empty list
            }

            List<PeakValley> validatedPeaksAndValleys = new List<PeakValley>();             //list to store the validated peaks and valleys based on the selected margin 

            // this line sorts the peaks and valleys by index to process them in order
            sortedPeaksAndValleys = sortedPeaksAndValleys.OrderBy(pv => pv.Index).ToList();

            PeakValley previousPeakValley = null;                                           //variable to keep track of last peak/valley to stay within the margin

            foreach (var current in sortedPeaksAndValleys)                                  //in the sorted list, loop through each peak/valley
            {
                bool isValidPeak = current.Peak;                                            // check if the current peak/valley is a peak (T/F)
                bool isValidValley = current.Valley;                                        // check if the current peak/valley is a valley (T/F)

                // check marging versus the last added peak/valley to ensure it's not too close
                if (previousPeakValley != null && Math.Abs(current.Index - previousPeakValley.Index) < desiredMargin)
                {
                    continue; //skip this peak/valley because it's too close to last one
                }

                // check that no nearby located peak/valley violates the margin margin reqirement
                for (int j = 1; j <= desiredMargin; j++)
                {
                    if (current.Index - j >= 0)                                             //check if the index stays within bounds for the left side
                    {
                        if (sortedPeaksAndValleys.Any(p => p.Index == current.Index - j && p.Value >= current.Value))    //check if there is a peak in the left margin that is > than the current peak's value
                        {
                            isValidPeak = false;                                            //if yes, then it's not a valid peak (too close to another peak)
                        }
                        if (sortedPeaksAndValleys.Any(p => p.Index == current.Index - j && p.Value <= current.Value))    //check if there is a valley in the left margin that is < than the current valley's value
                        {
                            isValidValley = false;                                          //if yes, it's not a valid valley (too close to another valley)
                        }
                    }

                    //check if index is within the bounds for the right side
                    if (current.Index + j < sortedPeaksAndValleys.Count)
                    {
                        if (sortedPeaksAndValleys.Any(p => p.Index == current.Index + j && p.Value >= current.Value))   //check if there is a peak in the right margin that is > than the current peak's value
                        {
                            isValidPeak = false;                                      //if yes, then it's not a valid peak (too close to another peak)
                        }
                        if (sortedPeaksAndValleys.Any(p => p.Index == current.Index + j && p.Value <= current.Value))   //check: is there a valley in the right margin that is < than the current valley's value
                        {
                            isValidValley = false;                                    //if yes, then it's not a valid valley (too close to another valley)
                        }
                    }
                }

                // Check if the peak/valley is valid based on the margin constraints
                if ((isValidPeak && current.Peak) || (isValidValley && current.Valley))
                {
                    validatedPeaksAndValleys.Add(current);                                      //if it's valid, add it to the validated list of peaks and valleys
                    previousPeakValley = current;                                               //update the last peak/valley to the current one (for the next iteration)
                }
            }

            return validatedPeaksAndValleys;                                                    //return list of validate peaks and valleys for specific margin
        }

        // Function: based on identified peaks and valley, this function FindWaves will determine the waves
        public Dictionary<string, List<Wave>> IdentifyWaves(List<PeakValley> peaksAndValleys)
        {
            var waves = new Dictionary<string, List<Wave>>();                         //create a dictionary for storing up and down waves (key string)
            waves["Up"] = new List<Wave>();                                           //initialize list for up waves
            waves["Down"] = new List<Wave>();                                         //initialize list for down waves

            for (int i = 0; i < peaksAndValleys.Count - 1; i++)                       //loop through the list of valleys and peaks to identify our waves
            {
                var current = peaksAndValleys[i];                                     //access the current peak/valley in the list
                var next = peaksAndValleys[i + 1];                                    //access the next peak/valley in the list

                // Down wave is defined as Peak to Valley:
                if (current.Peak && next.Valley)
                {
                    waves["Down"].Add(new Wave(                                       //create a new wave object for "Down" wave:
                        startIndex: current.Index,                                    //start index of the current peak 
                        endIndex: next.Index,                                         //end index of the next valley                                         
                        startDate: current.Date,                                      //starting date of the current peak
                        endDate: next.Date,                                           //ending date of the next valley
                        startValue: current.Value,                                    //starting value of the current peak
                        endValue: next.Value                                          //ending value of the next valley
                    ));
                }
                // Up wave is defined as Valley to Peak:
                else if (current.Valley && next.Peak)
                {
                    waves["Up"].Add(new Wave(                                         //create a new wave object for "Up" wave:
                        startIndex: current.Index,                                    //start index of the current valley
                        endIndex: next.Index,                                         //end index of the next peak
                        startDate: current.Date,                                      //starting date of the current valley
                        endDate: next.Date,                                           //ending date of the next peak
                        startValue: current.Value,                                    //starting value of the current valley
                        endValue: next.Value                                          //ending value of the next peak
                    ));
                }
            }

            return waves;                                                             //return dictionary with all up and down waves
        }
    }

}

   

