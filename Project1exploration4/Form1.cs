using System;                        //System namespace for basic system functions
using System.Collections.Generic;    //Namespace to import List and other generic collection classes
using System.ComponentModel;         //Namespace for handling component and control events
using System.Data;                   //Namespace for database operations
using System.Drawing;                //Namespace for drawing and graphics
using System.IO.IsolatedStorage;     //Namespace for creating isolated storage
using System.Linq;                   //Namespace for LINQ queries (Language Integrated Query)
using System.Text;                   //Namespace for text processing and encoding
using System.Threading.Tasks;        //Namespace for asynchronous programming and parallelism
using System.Windows.Forms;          //Namespace for UI components of Windowns Forms Apps

/*
Student name: Viktoriia Solomennikova, U99858124
Class: Software System Development - C# Programming
Project Description: This project is aimed to design a Windows Forms application that reads stock data from a .csv file,
processes it, and display it in a Chart and a DataGridView. The user can specify the stock symbol, 
the starting and ending date of the interest, and a period for each candlestick (Daily, Weekly, or Monthly).
 */


//Custom namespace for this project
namespace Project_Windows_Forms_App
{
    //This Form uploads the data from a file, processes it, and displays it in the Chart and DataGridView
    //Public class Form_StockDisplay inherited from the Form class
    public partial class Form_StockDisplay: Form
    {
        List<Candlestick> candlesticks;  //Create list of candlesticks' data
        StockReader stockReader;         //Instantiate a StockReader class to read stock data

        //Public form Form_StockDisplay:
        public Form_StockDisplay()
        {
            InitializeComponent(); //Initialize the form components
        }

        //'Click' event handler for the button 'LoadTicker'
        private void button_LoadTicker_Click(object sender, EventArgs e)
        {
            openFileDialog_LoadTicker.ShowDialog(); //show the dialog box to choose file from computer's file system
        }

        //Event handler for selecting the file (click 'OK') in the Open File Dialog
        private void openFileDialog_LoadTicker_FileOk(object sender, CancelEventArgs e)
        {
            Text = openFileDialog_LoadTicker.FileName;  //Save the file name in the form's title

            List<Candlestick> listOfCandlesticks = loadTicker(openFileDialog_LoadTicker.FileName); //Get the candlestick data from selected file
            candlesticks = listOfCandlesticks;  //stored the list of candlestick in the variable
            listOfCandlesticks = filterCandlesticks(candlesticks, dateTimePicker_StartDate.Value, dateTimePicker_EndDate.Value); //call filtering function
            displayChart(listOfCandlesticks);   //call the function to display candlesticks in the chart
        }

        //Helper function: 'loadTicker' obtains the candlesticks from the .csv file using ticker
        private List<Candlestick> loadTicker(string filepath)
        {
            StockReader reader = new StockReader(); //Instantiate a class StockReader to read the data
            List<Candlestick> processedListOfCandlesticks = reader.readCandlesticksFromFile(filepath); //Read the candlesticks from the file
            processedListOfCandlesticks.Reverse();  //To maintain the dates in ascending order, reverse the list
            return processedListOfCandlesticks;     //Return the list of processed candlesticks
        }

        //Helper function: 'filterCandlesticks' filters the candeltsticks by date: to display the candlesticks within a user's specified date range
        //This function takes the list of unfiltered candlesticks, start date, and end date as parameters.
        private List<Candlestick> filterCandlesticks(List<Candlestick> candlesticks, DateTime startDate, DateTime endDate)
        {
            //Note: LINQ query was used to filter the list of candlesticks by date and return filtered candlesticks
            //'Where' method (LINQ query) is used to filter the chandlesticks by the date range specified by the user.
            //In the parantheses, there is a filtering condition passed to Where method such that:
            //instanceCandlestick is an instance of the Candlestick class with its 'Date' property
            //startDate && endDate and date range specified by the user selecting only the chandlesticks within this range.
            //ToList() method converts the sequence of candlesticks into a List of filtered Candlesticks:
            List<Candlestick> filteredCandlesticksByDate = candlesticks.Where(instanceCandlestick => instanceCandlestick.Date >= startDate && instanceCandlestick.Date <= endDate).ToList();

            return filteredCandlesticksByDate; // Return the filtered Candlesticks to the caller function

        }

        //Helper function: 'bindCandlestickData' binds the data to display the filtered chandlesticks on the Chart and on the DataGridView
        private void bindCandlestickData(List<Candlestick> candlesticks)
        {
            candlestickBindingSource.DataSource = candlesticks; //this line binds the chandlesticks' data to candlestickBindingSource
        }

        //Helper function: 'displayChart' displays the filtered candlesticks in the Chart given the list of candlesticks as a parameter
        private void displayChart(List<Candlestick> candlesticks)
        {
            bindCandlestickData(candlesticks); //bind candlesticks to the Chart using the helper function
            normalizeChart(candlesticks);      //normalize the chart for better visual representation
            chart_OHLCV.DataSource = candlesticks; //set the Chart's data source to the list of candlesticks
            chart_OHLCV.DataBind();            //refresh the chart to display updated data
        }

        //Function: displays the filtered candlesticks in the DataGridView given the list of candlesticks as parameter
        private void displayDataGridView(List<Candlestick> candlesticks)
        {
            bindCandlestickData(candlesticks); //bind candlesticks to the DataGridView using the helper function
            dataGridView_Grid.DataSource = null; //clear prior data binding from DataGridView
            dataGridView_Grid.DataSource = candlesticks; //bind new chandlestick data to the DataGridView
        }

        //Helper function: 'normalizeChart' normalized the Chart adjasting the Y-axis scale, calculating the candlesticks'
        //high and low prices, and adding padding for better visual representation of the data
        private void normalizeChart(List<Candlestick> candlesticks)
        {
            decimal maxHigh = candlesticks.Max(c => c.High); //find the max 'High' price value from list of candlesticks
            decimal minLow = candlesticks.Min(c => c.Low);   //find the min 'Low' price value from list of candlesticks

            decimal paddingPercentage = 0.02m; //padding 2%
            decimal maxY = maxHigh * (1 + paddingPercentage); //compute max Y-axis value with new padding
            decimal minY = minLow * (1 - paddingPercentage);  //compute min Y-axis value with new padding

            chart_OHLCV.ChartAreas[0].AxisY.Minimum = (double)minY; //set min value for Y-axis on the Chart
            chart_OHLCV.ChartAreas[0].AxisY.Maximum = (double)maxY; //set max value for Y-axis on the Chart
        }

    }
}
