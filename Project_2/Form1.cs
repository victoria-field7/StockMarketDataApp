using System;                       //System namespace for basic system functions
using System.Collections.Generic;   //Namespace to import List and other generic collection classes
using System.ComponentModel;        //Namespace for handling component and control events
using System.Data;                  //Namespace for database operations
using System.Drawing;               //Namespace for drawing and graphics
using System.Linq;                  //Namespace for LINQ queries (Language Integrated Query)
using System.Text;                  //Namespace for text processing and encoding
using System.Threading.Tasks;       //Namespace for asynchronous programming and parallelism
using System.Windows.Forms;         //Namespace for UI components of Windowns Forms Apps
using System.Windows.Forms.VisualStyles;  //Namespace for visual styles in Windows Forms

//Custom generated namespace for the project:
namespace Project_2
{
    //Form: load the stock data from csv file and display in in a chart
    public partial class Form1: Form
    {
        public Form1()                                              //Constructor for the class which initializes the components of the form
        {
            InitializeComponent();                                  //Methos to autocreate the components of the form via designer
        }

        //Function: open the file dialog to load the stock data from when file when user click the button "Load Stock"
        private void button_LoadStock_Click(object sender, EventArgs e) //Click the button
        {
            openFileDialog_LoadTicker.ShowDialog();                //Show the user open file dialog to select the stock data file (.csv)
        }

        //Event handler: when the user select the file and clicks "OK", this method will be called to process the selected file or files
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //Access all filenames and loop, create ChartDisplay for each
            foreach (var filename in openFileDialog_LoadTicker.FileNames) 
            {
                //Create a new object from the StockReader class to read the candlestick data from the selected file:
                Form_ChartDisplay f = new Form_ChartDisplay(filename, dateTimePicker_StartDate.Value, dateTimePicker_EndDate.Value);
               
            }
         
        }
    }
}
