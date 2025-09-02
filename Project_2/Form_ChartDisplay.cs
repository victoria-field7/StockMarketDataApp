using Project_2;                                      //This namespace is used to access the classes and method in Project 2 namespace
using Project_Windows_Forms_App;                      //This namespace is used to access the classes and methods in the Project Windows Forms App namespace 


using System;                                         //namespace to access basic system functions
using System.Collections.Generic;                     //Namespace for importing LIST and other generic collection classes//namespace 
using System.ComponentModel;                          //namespace to handle component and control events
using System.Data;                                    //namespace for databases
using System.Diagnostics;                             //namespace for debugging and diagnostics
using System.Diagnostics.Eventing.Reader;
using System.Drawing;                                 //namespace for drawing and graphics

using System.IO;                                      //namespace for file input/output operations
using System.Linq;                                    //namespace for LINQ queries
using System.Net;
using System.Reflection.Emit;
using System.Text;                                    //namespace for text processing 
using System.Threading.Tasks;                         //namespace for asynchronous programming and parallelism
using System.Windows.Forms;                           //namespace for UI of Windows Forms Apps
using System.Windows.Forms.DataVisualization.Charting;//namespace for charting in Windows Forms Apps

//Custom namespace for the project
namespace Project_2
{
    //Public class: displays the chart and handles user interactions in the Window Desktop form:
    public partial class Form_ChartDisplay : Form 
    {
        StockReader stockReader;                                                   //Instantiate the StockReader class to read the stock data from the file
        String filePath;                                                           //The file path for stock data
        public List<Candlestick> candlesticks;                                     //List of candlesticks to store the data read from the file
        public List<Candlestick> filteredCandlesticksByDate;                       //List of filtered candlesticks to store the filtered data by date range
        PeakValleyIdentifier peakValleyIdentifier = new PeakValleyIdentifier();    //Instantiate the PeakValleyIdentifier class to identify the peaks and valleys
        private RectangleAnnotation selectedRectangle;                             //Rectangle annotation
        private LineAnnotation diagonalLineAnnotation;                             //Line annotation 
        private bool isDraggingActive = false;                                     //Set isDragging to false by default                                       
        private int startIndex = -1;                                               //Set startIndex to -1 by default
        private int endIndex = -1;                                                 //Set endIndex to -1 by default
        private bool isSimulationRunning = false;                                  //Set isSimulating to false by default (to change it later in the simulation)
        private RectangleAnnotation currentDragRectangle;                          //Rectangle annotation for dragging
        private const int SNAP_TOLERANCE_PIXELS = 300;                             //Predefined threshold for snapping
        private Wave currentlySelectedWave;                                        //Selected wave for simulation instantiated from the Wave class
        private bool IsWaveRubberbanded = false;                                   //Set waveIsRubberbanded to false by default
        private Point simulationStartPoint;                                        //Set simulation starting point
        private Point simulationEndPoint;                                          //Set simulation ending point
        private double minimumPriceForSimulation;                                  //Define the simulation minimum price
        private double maximumPriceForSimulation;                                  //Define the simulation maximum price
        private double simulationStepSize;                                         //Define the simulation step size
        private double currentSimulatedPrice;                                      //Define the current simulation price
        private bool isReversingSimulation = false;                                //Set isSimulationReversing to false by default
        private Point rubberBandOrigin;                                            //Set the rubber band start point


        //Dictionary: to store the peaks and valleys 
        public Form_ChartDisplay()                                                 //default constructor for Form_ChartDisplay class
        {
            InitializeComponent();                                                 //initialize the components of the form created
            chart_OHLCV.MouseDown += chart_OHLCV_MouseDown;                        //Add event handler for mouse down event
            chart_OHLCV.MouseMove += chart_OHLCV_MouseMove;                        //Add event handler for mouse move event
            chart_OHLCV.MouseUp += chart_OHLCV_MouseUp;                            //Add event handler for mouse up event

        }

        //Constructor: initialize the form with a filename and date range:
        public Form_ChartDisplay(String filename, DateTime startDate, DateTime endDate)
        {
            InitializeComponent();                                              //initialize the components of the form created

            hScrollBar_CandlestickMargin.Minimum = 1;                           // minimum margin
            hScrollBar_CandlestickMargin.Maximum = 20;                          // maximum margin
            hScrollBar_CandlestickMargin.SmallChange = 1;                       // small change for scrollbar equals to 1
            hScrollBar_CandlestickMargin.LargeChange = 1;                       // large change for scrollbar equals to 1
            hScrollBar_CandlestickMargin.Value = 1;                             // set a default value for a scrollbar to 1

            // Event handler: Upwaves combobox
            comboBox_UpWaves.SelectedIndexChanged += (sender, e) =>
            {
                if (comboBox_UpWaves.SelectedItem != null)                             //verify if the selected item in the UpWaves combobox is not null    
                    DrawWaveOnChart((Wave)comboBox_UpWaves.SelectedItem);              //is not null, call the DrawWave method and pass the current wave
            };

            //Event handler: Downwaves combobox
            comboBox_DownWaves.SelectedIndexChanged += (sender, e) =>                   //SelectIndexChanged event for DownWaves combobox
            {
                if (comboBox_DownWaves.SelectedItem != null)                            //verify if the selected item in the DownWaves combobox is not null
                    DrawWaveOnChart((Wave)comboBox_DownWaves.SelectedItem);                    //if it is not null, call the DrawWave method and pass the current wave
            };

            chart_OHLCV.Titles.Add(Path.GetFileNameWithoutExtension(filename));         //add the title to the chart as filename without extension

            // Create new object of StockReader class to read file:
            stockReader = new StockReader();
            filePath = filename;                                                        //set filePath property to filename passed
            dateTimePicker_StartDate.Value = startDate;                                 //set the start date of the dateTimePicker to the startDate passed
            dateTimePicker_EndDate.Value = endDate;                                     //set the end date of the dateTimePicker to the endDate passed

            //Set Event handlers for the chart rubberbanding and simulation:
            chart_OHLCV.MouseDown += chart_OHLCV_MouseDown;                             //Add event handler for mouse down event
            chart_OHLCV.MouseMove += chart_OHLCV_MouseMove;                             //Add event handler for mouse move event
            chart_OHLCV.MouseUp += chart_OHLCV_MouseUp;                                 //Add event handler for mouse up event

            loadAndDisplayData();                                                       //Method to load and display the stock data on the chart
            Show();                                                                     //Show the form after loading data
        }

        //Function: loads the candlestick data from the specified file and displays it
        public void loadAndDisplayData()
        {
            Text = filePath;                                                            //set form's title to filepath
            candlesticks = loadTicker(filePath);                                        //load the ticker data from the specified file and store it
            displayStockChart();                                                        //method to display the stock on the chart
        }

        // Function: reads the candlestick information from the file
        private List<Candlestick> loadTicker(string filename)                           //load the data
        {
            List<Candlestick> listOfCandlesticks = stockReader.readCandlesticksFromFile(filename);  //method to read candlesticks from the file
            Candlestick firstCandlestick = listOfCandlesticks[0];                       //get the first candlestick to compare dates
            Candlestick secondCandlestick = listOfCandlesticks[1];                      //get the second candlestick to compare dates
            if (firstCandlestick.Date > secondCandlestick.Date)                         //Check: is date of first is greater than date of second?
            {
                listOfCandlesticks.Reverse();                                           //if yes, reverse the list for chronological order
            }
            return (listOfCandlesticks);                                                //return the list of candlesticks
        }

        // Function: displays the stock on the chart based on filtered candlesticks by date range:
        public void displayStockChart()
        {
            ////call the function to filter candlesticks by date range:
            filteredCandlesticksByDate = FilterCandlesticksByDateRange(candlesticks, dateTimePicker_StartDate.Value, dateTimePicker_EndDate.Value);  
            normalizeStockChart(filteredCandlesticksByDate);                            //function call to normalize chart
            chart_OHLCV.DataSource = filteredCandlesticksByDate;                        //set the data source of the chart to filtered candlesticks 
            chart_OHLCV.DataBind();                                                     //bind data to the chart to be able to display it
            chart_OHLCV.Show();                                                         //show chart after binding
            InitializePeaksAndValleys();                                                //function call to initialize peaks and valleys on the chart
        }

        // Function: initializes the peaks and valleys on the chart based on filtered data:
        private void InitializePeaksAndValleys()
        {
            // Clear previous annotations on the chart
            if (filteredCandlesticksByDate != null && filteredCandlesticksByDate.Count > 0)                  //check if there are filtered candlesticks to work with
            {
                var bindingList = new BindingList<Candlestick>(filteredCandlesticksByDate);                  //create a binding list of filtered candlesticks

                // Find all peaks and valleys for the current filtered data:
                peakValleyIdentifier.marginPeaksAndValleys =
                peakValleyIdentifier.findPeaksAndValleysByMargin(bindingList);                               //note: use specified margin

                int currentMargin = hScrollBar_CandlestickMargin.Value;                                      //initialize variable to store the default margin from scrollbar
                InitializeUpAndDownWaves(peakValleyIdentifier.marginPeaksAndValleys[currentMargin]);         //initialize the waves based on the picked margin
                AddAnnotationsPeaksAndValleys(peakValleyIdentifier.marginPeaksAndValleys[currentMargin]);    //annotate peaks and valleys on chart
            }
        }

        // Function: normalize the chart by setting the Y-axis based on min and max values of data:
        private void normalizeStockChart(List<Candlestick> candlesticks)
        {

            decimal maxHighValue = candlesticks.Max(c => c.High);                 // find the max high value from the candlestick list
            decimal minLowValue = candlesticks.Min(c => c.Low);                   // find the min low value from the candlestick list

            // Use padding for proper visualization on the chart:
            decimal paddingPercentage = 0.02m;                                    //calculate padding percentage
            decimal maxY = maxHighValue * (1 + paddingPercentage);                //calculate the max Y value 
            decimal minY = minLowValue * (1 - paddingPercentage);                 //calculate the min Y value

            chart_OHLCV.ChartAreas[0].AxisY.Minimum = (double)minY;               //set the min Y value
            chart_OHLCV.ChartAreas[0].AxisY.Maximum = (double)maxY;               //set the max Y value
        }

        //Function to handle the click event to refresh the chart based on newly selected dates:
        private void button_Refresh_Click(object sender, EventArgs e)
        {

            try                                                                                 //'try' handles any exceptions that may occur during execution
            {
                // Check if we have data to work with before refreshing:
                if (candlesticks == null || candlesticks.Count == 0)
                {
                    MessageBox.Show("Data must be loaded from a file before continuing.");      //if there are no candlesticks, show this message to user
                    return;                                                                     //exit the function if no candlesticks
                }

                // Get the starting and ending dated from the date time pickers:
                DateTime startingDateTime = dateTimePicker_StartDate.Value;                     //get starting date
                DateTime endingDateTime = dateTimePicker_EndDate.Value;                         //get ending date


                if (startingDateTime > endingDateTime)                                          // Then, check the date range for validity
                {
                    MessageBox.Show("Ensure that the end date is later than the start date.");  //display this message if the end date is before the start date
                    return;                                                                     //return from function
                }

                // Clear any previous annotions before refreshing chart
                chart_OHLCV.Annotations.Clear();

                // Call function to filred candlesticks and display based on new date range
                displayStockChart();

                // Update the combo boxes to reflect new waves:
                comboBox_UpWaves.DataSource = null;                                            //clear data for UpWaves combobox
                comboBox_DownWaves.DataSource = null;                                          //clear data for DownWaves combobox

                // Set new margin for scroll bar (to 1) to ensure chart diplays correctly after refreshing:
                hScrollBar_CandlestickMargin.Value = 1;

                //Redefine peaks and valley based on the currently selected margin:
                int currentMargin = hScrollBar_CandlestickMargin.Value;                                        //get margin from the scrollbar
                var bindingList = new BindingList<Candlestick>(filteredCandlesticksByDate);                    //create a new binding list for filtered candlesticks
                var peaksAndValleys = peakValleyIdentifier.findPeaksAndValleys(bindingList);                   //find peaks and valleys based on the binding list
                peakValleyIdentifier.marginPeaksAndValleys = peakValleyIdentifier.findPeaksAndValleysByMargin(bindingList);  //find peaks/valleys by margin

                InitializeUpAndDownWaves(peaksAndValleys);                                                     //initialize the waves again based on new margin

                // Redraw the annotations for peaks and valleys based on the current margin:
                if (peakValleyIdentifier.marginPeaksAndValleys.ContainsKey(currentMargin))                     //check if the current margin exists in the dictionary
                {
                    AddAnnotationsPeaksAndValleys(peakValleyIdentifier.marginPeaksAndValleys[currentMargin]);  //annotate peaks and valleys
                }
            }
            catch (Exception ex)                                                                               //catch any exceptions that may come up
            {
                MessageBox.Show($"Error refreshing data: {ex.Message}");                                       //display the message to user
            }
        }

        //Function: Filter candlestocks by date range    
        private List<Candlestick> FilterCandlesticksByDateRange(List<Candlestick> candlesticks, DateTime startDate, DateTime endDate)// Filter the candlesticks by date
        {
            //call LINQ to filter candlesticks based on the date range from the user:
            List<Candlestick> filteredCandlesticksByDate = candlesticks.Where(instanceCandlestick => instanceCandlestick.Date >= startDate && instanceCandlestick.Date <= endDate).ToList();

            if (filteredCandlesticksByDate.Count == 0)                                                          //debug message: if there are no candlesticks:
            {
                MessageBox.Show("No data in the stock for the selected date range.");                           //then, display this message to user
            }

            return filteredCandlesticksByDate;                                                                  //return the filtered list of candlesticks 
        }

        // Function: annotates peaks and valleys on the chart:
        private void AddAnnotationsPeaksAndValleys(List<PeakValley> peaksAndValleys)
        {
            chart_OHLCV.Annotations.Clear();                                                                    //clear all previous annotations
            foreach (var peakValley in peaksAndValleys)                                                         //loop through each peak and valley
            {
                if (peakValley.Peak)                                                                            //if it is a peak
                {
                    addTextAnnotationOnChart(peakValley.Index, peakValley.Value, Color.Red, "P");               //then, add annotation for this peak in the chart
                }
                if (peakValley.Valley)                                                                          //if it is a valley
                {
                    addTextAnnotationOnChart(peakValley.Index, peakValley.Value, Color.Green, "V");             //then, add annotation for this valley in the chart
                }
            }
        }

        // Function: adds a text annotation fora peaks and valleys on the chart:
        private void addTextAnnotationOnChart(int index, decimal value, Color color, string label)
        {
            var textAnnotation = new TextAnnotation                                                 // create new object for text annotations
            {
                Text = label,                                                                       // set the text to label
                X = index,                                                                          // set X position for annotation on the chart
                Y = (double)value + 2,                                                              // set Y position for annotation on the chart
                AnchorDataPoint = chart_OHLCV.Series[0].Points[index],                              // connect annotation to the data point on the chart
                Font = new Font("Arial", 10, FontStyle.Bold),                                       // set the font for text annotations
                ForeColor = color,                                                                  // set color for text
                BackColor = Color.Transparent,                                                      // set background color for text annotation
                Alignment = ContentAlignment.MiddleCenter,                                          // set alignment for text
            };

            chart_OHLCV.Annotations.Add(textAnnotation);                                            // add text annotation to a annotation's collection on the chart
            chart_OHLCV.Invalidate();                                                               // make the chart to redraw itself (with annotations)
        }

        // Function: handles the scroll event for margin scrollbar:
        private void hScrollBar_CandlestickMargin_Scroll(object sender, ScrollEventArgs e)
        {
            if (filteredCandlesticksByDate == null || filteredCandlesticksByDate.Count == 0)        // check if there candlesticks to work with
                return;                                                                             // if no, exit function
            try                                                                                     // 'try' block to handle the exceptions that may occur
            {
                int margin = hScrollBar_CandlestickMargin.Value;                                    // get current margin from scroll bar
                label_selectMargin.Text = $"Margin: {hScrollBar_CandlestickMargin.Value}";          // update the lables to display the value of margin
                var peaksAndValleys = peakValleyIdentifier.marginPeaksAndValleys[margin];           // get peaks and valleys based on current margin
                InitializeUpAndDownWaves(peaksAndValleys);                                          // update waves

                AddAnnotationsPeaksAndValleys(peaksAndValleys);                                     // redrew the annotations
                if (comboBox_UpWaves.SelectedItem != null)                                          // check if there is a item selected in combobox Upwave
                    DrawWaveOnChart((Wave)comboBox_UpWaves.SelectedItem);                                  // if yes, draw wave
                if (comboBox_DownWaves.SelectedItem != null)                                        // check if there is a item selected in combobox Downwave
                    DrawWaveOnChart((Wave)comboBox_DownWaves.SelectedItem);                                // if yes, draw wave
            }
            catch (KeyNotFoundException)                                                            // catch block to handle any exceptions
            {
                label_selectMargin.Text = "Invalid margin";                                         // when the margin too high there is no any waves to display 
            }
        }

        // Dictionary to store waves:
        private Dictionary<string, List<Wave>> waves;

        // Function: initialize waves based on identified peaks and valleys
        private void InitializeUpAndDownWaves(List<PeakValley> peaksAndValleys)                //initialize up and down waves
        {
            // STEP 1. Set the dictionary to store waves:
            var currentUpWave = comboBox_UpWaves.SelectedItem;                                // store the selected item from the UpWaves combobox
            var currentDownWave = comboBox_DownWaves.SelectedItem;                            // store the selected item from the DownWaves combobox

            // STEP 2. Obtain the new waves based on peaks and valleys:
            waves = peakValleyIdentifier.IdentifyWaves(peaksAndValleys);

            // STEP 3. Update comboboxes
            comboBox_UpWaves.DataSource = null;                                                //clear the data source for Upwaves combobox
            comboBox_UpWaves.DataSource = waves["Up"];                                         //set the data source for UpWaves combobox to up waves dictionary
            comboBox_UpWaves.DisplayMember = "ToString";                                       //set the display member for UpWaves combobox to the ToString method
            comboBox_DownWaves.DataSource = null;                                              //clear the data source for Downwaves combobox
            comboBox_DownWaves.DataSource = waves["Down"];                                     //set the data source for DownWaves combobox to down waves dictionary
            comboBox_DownWaves.DisplayMember = "ToString";                                     //set the display member for DownWaves combobox to the ToString method

            if (currentUpWave != null && waves["Up"].Contains(currentUpWave))                  //restore selections if exist
                comboBox_UpWaves.SelectedItem = currentUpWave;                                 //is not null or exists, set combobox to previously selected UpWave
            else                                                                               //else:
                comboBox_UpWaves.SelectedIndex = -1;                                           //set the selected index of the UpWaves combobox to -1

            if (currentDownWave != null && waves["Down"].Contains(currentDownWave))            //check if selected DownWave is not null and exists in dictionary Down waves
                comboBox_DownWaves.SelectedItem = currentDownWave;                             //is it's okay, then set the combobox for DownWaves to the previously selected DownWave
            else                                                                               //else:
                comboBox_DownWaves.SelectedIndex = -1;                                         //set the selected index of the DownWaves combobox to -1
        }

        // Function: Draw the selected wave on the chart with specified properties and annotations:
        private void DrawWaveOnChart(Wave wave)                                                //DrawWave definition
        {
            try                                                                                //Debug message to catch the exceptions:
            {
                if (wave == null || chart_OHLCV.ChartAreas.Count == 0)                         //If the wave is null or there are no chart areas
                {
                    throw new ArgumentNullException("Wave or chart area has not been initialized.");          //throw an exception message
                }

                currentlySelectedWave = wave;                                                           //Set the selected wave to the current wave
                IsWaveRubberbanded = false;                                                    //Preset the waveIsRubberbanded to false
                currentSimulatedPrice = (double)wave.StartValue;                              //Set the current simulation price to the start value of the wave

                // Step 1. Compute simulation range:
                double waveHeight = Math.Abs((double)(wave.EndValue - wave.StartValue));       //double variable to store the height of the wave
                minimumPriceForSimulation = Math.Min((double)wave.StartValue, (double)wave.EndValue) - (waveHeight * 0.10);           //start value ,end value - wave height
                maximumPriceForSimulation = Math.Max((double)wave.StartValue, (double)wave.EndValue) + (waveHeight * 0.10);           //end value + wave height
                simulationStepSize = waveHeight * 0.02;                                                                        //wave height multiplied by 0.02

                // Step 2. Set the chart area and color:
                ChartArea chartArea = chart_OHLCV.ChartAreas[0];                               //Get the chart area from the chart
                Color waveAnnotationColor = wave.IsUpWave ? Color.Green : Color.Red;                     //Set the color of the wave based on the type of wave (up or down)

                // Step 3. Calculate positions on the chart:
                double startX = wave.StartIndex;                                               //Start index of the wave
                double endX = wave.EndIndex;                                                   //End index of the wave
                double startY = (double)wave.StartValue;                                       //Start value of the wave
                double endY = (double)wave.EndValue;                                           //End value of the wave

                // Step 4. Create a specofic number for the wave ID:
                string waveId = $"wave_{startX}_{endX}";                                       //Based on start and end index of the wave

                // Step 5. Create and manage rectangle annotations:
                var waveRectangleAnnotation = new RectangleAnnotation                                   //Define rectangle annotation
                {
                    AxisX = chartArea.AxisX,                                                   //Chart area X axis
                    AxisY = chartArea.AxisY,                                                   //Chart area Y axis
                    X = startX,                                                                //Start X coordinate
                    Y = Math.Max(startY, endY),                                                //Start Y coordinate
                    Width = endX - startX,                                                     //Width of the rectangle
                    Height = -Math.Abs(endY - startY),                                         //Height of the rectangle
                    LineColor = waveAnnotationColor,                                                     //Color of the rectangle
                    LineWidth = 2,                                                             //Line width
                    BackColor = Color.FromArgb(30, waveAnnotationColor),                                 //Transparent color
                    IsSizeAlwaysRelative = false,                                              //Set isSizeAlwaysRelative to false
                    ClipToChartArea = chartArea.Name,                                          //Chart area name
                    Name = $"{waveId}_Rectangle"                                               //Name of the rectangle associated with the wave ID
                };

                // Step 6. Create and manage line annotations:
                var waveLineAnnotation = new LineAnnotation                                        //Define line annotation
                {
                    AxisX = chartArea.AxisX,                                                   //Chart area X axis
                    AxisY = chartArea.AxisY,                                                   //Chart area Y axis
                    X = startX,                                                                //Start X coordinate
                    Y = startY,                                                                //Start Y coordinate
                    Width = endX - startX,                                                     //Width of the line
                    Height = endY - startY,                                                    //Height of the line
                    LineColor = waveAnnotationColor,                                                     //Color of the line
                    LineWidth = 2,                                                             //Line width
                    StartCap = LineAnchorCapStyle.None,                                        //Start cap style
                    EndCap = LineAnchorCapStyle.Arrow,                                         //End cap style
                    IsSizeAlwaysRelative = false,                                              //Set isSizeAlwaysRelative to false
                    ClipToChartArea = chartArea.Name,                                          //Chart area name
                    Name = $"{waveId}_Line"                                                    //Name of the line associated with the wave ID
                };

                // Step 7. Clear existing Wave annotations:
                RemoveExistingWaveAnnotations(waveId);

                // Step 8. Clear any previous Fibonacci annotations:
                RemovePreviousFibonacciAnnotations();

                // Step 9. Add new line and rectangle annotations to the chart:
                chart_OHLCV.Annotations.Add(waveRectangleAnnotation);                                  //Add rectangle annotation to the chart
                chart_OHLCV.Annotations.Add(waveLineAnnotation);                                  //Add line annotation to the chart

                // Step 10. For Fibonacci lines, create and set the ChooseWave object:
                var chooseWave = new ChooseWave();                                            //Create a new ChooseWave object
                chooseWave.SetWaveParameters(wave.StartIndex, wave.EndIndex, startY, endY);             //Set the wave properties

                // Step 11. Calculate and draw Fibonacci levels fpr this existing wave:
                var fibLevels = chooseWave.CalculateFibonacciLevels(startY, endY);            //Calculate Fibonacci levels

                // Step 12. Draw Fibonacci lines and labels on the Chart:
                foreach (var (level, label) in fibLevels)                                     //loop through each level and label in the Fibonacci levels
                {
                    var fibLine = new HorizontalLineAnnotation                                //Define horizontal line annotation
                    {
                        AxisX = chartArea.AxisX,                                              //Chart area X axis
                        AxisY = chartArea.AxisY,                                              //Chart area Y axis
                        X = chooseWave.Left,                                                  //Left coordinate of the wave
                        Y = level,                                                            //Level of the Fibonacci line
                        Width = chooseWave.Width,                                             //Width of the wave
                        LineColor = Color.Blue,                                               //Color of the line
                        LineDashStyle = ChartDashStyle.Solid,                                 //Line dash style
                        LineWidth = 1,                                                        //Line width
                        IsSizeAlwaysRelative = false,                                         //Set isSizeAlwaysRelative to false
                        ClipToChartArea = chartArea.Name,                                     //Chart area name
                        Name = $"fib_{label}_{wave.StartIndex}"                               //Name of the Fibonacci line associated with the wave ID
                    };

                    chart_OHLCV.Annotations.Add(fibLine);                                     //Add Fibonacci line to the chart

                    // Step 13. Create and add labels for Fibonacci levels:
                    var fibLabel = new TextAnnotation                                         //Define text annotation
                    {
                        Text = $"{label} — {level:F2}",                                       //Text to display for the Fibonacci level
                        AxisX = chartArea.AxisX,                                              //Chart area X axis
                        AxisY = chartArea.AxisY,                                              //Chart area Y axis
                        X = chooseWave.Right + 1,                                             //Right coordinate of the wave
                        Y = level,                                                            //Level of the Fibonacci line
                        Font = new Font("Arial", 8, FontStyle.Bold),                          //Font for the label
                        ForeColor = Color.Purple,                                             //Color of the label
                        Alignment = ContentAlignment.MiddleLeft,                              //Alignment for the label
                        IsSizeAlwaysRelative = false,                                         //Set isSizeAlwaysRelative to false
                        ClipToChartArea = chartArea.Name,                                     //Chart area name
                        Name = $"fib_label_{label}_{wave.StartIndex}"                         //Name of the label associated with the wave ID
                    };
                    // Add the label to the chart:
                    chart_OHLCV.Annotations.Add(fibLabel);
                }

                // Step 14. Compute and display confirmations on the chart:
                var bindingList = new BindingList<Candlestick>(filteredCandlesticksByDate);   //Create a new binding list of filtered candlesticks
                double fibLevelMargin = chooseWave.Height * 0.01;                             //Set fibMargin to 1% of the wave height
                var waveConfirmations = chooseWave.FindWaveConfirmations(wave.StartIndex, wave.EndIndex, bindingList, fibLevels, fibLevelMargin);  //Find confirmations for the wave

                // Step 15. Draw confirmation dots on the chart:
                foreach (var (x, y) in waveConfirmations)                                     //Loop through each confirmation
                {
                    var dot = new TextAnnotation                                              //Define text annotation
                    {
                        AxisX = chartArea.AxisX,                                              //Chart area X axis
                        AxisY = chartArea.AxisY,                                              //Chart area Y axis
                        AnchorDataPoint = chart_OHLCV.Series[0].Points[(int)x],               //Anchor to the data point
                        Y = y + 0.5,                                                          //Y position of the dot
                        Text = "•",                                                           //Dot symbol for confirmation dots
                        ForeColor = Color.Blue,                                               //Color for confirmation dots
                        Font = new Font("Arial", 14, FontStyle.Bold),                         //Font for confirmation dots
                        ClipToChartArea = chartArea.Name,                                     //Chart area name
                        Alignment = ContentAlignment.MiddleCenter,                            //Alignment for confirmation dots
                        SmartLabelStyle = { Enabled = false },                                //Disable smart label style
                        IsSizeAlwaysRelative = false,                                         //Set isSizeAlwaysRelative to false
                        Tag = "confirm"                                                       //Tag for confirmation dots
                    };
                    chart_OHLCV.Annotations.Add(dot);                                         //Add confirmation dot to the chart
                }

                // Step 16. Update confirmation count and set it to the label (number changes when we run the simulation):
                if (label_ConfirmationCount != null)
                {
                    label_ConfirmationCount.Text = $"Confirmations: {waveConfirmations.Count}";   //Update the label with the confirmation count
                }
                // Step 17. Invalidate the chart to refresh and display the new annotations:
                chart_OHLCV.Invalidate();
            }
            catch (ArgumentNullException ex)                                                  //Handle exception for null arguments
            {
                Console.WriteLine($"We were not able to draw a wave successfully: {ex.Message}");                    //Debug message          
                MessageBox.Show("Unable to draw wave: Wave data is invalid or the chart is not initialized.",        //Display error message
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);                                //error message (continued)
            }
            catch (InvalidOperationException ex)                                                                     //Handle exception for invalid operations
            {
                Console.WriteLine($"Chart operation failed: {ex.Message}");                                          //Debug message
            }
            catch (Exception ex)                                                                                     //Handle any other exceptions
            {
                Console.WriteLine($"Unknown error happened when drawing wave: {ex.Message}");                        //Debug message                     
                Debug.WriteLine(ex.ToString());                                                                      //Display error message
            }
        }


        // Function: Clear any existing Fibonacci annotations on the chart:
        private void RemoveExistingWaveAnnotations(string waveId)                              //Clear existing wave annotations
        {
            try
            {
                var annotationsToRemove = chart_OHLCV.Annotations                             //Get and remove any already existing wave annotations
                    .Where(a => a.Name != null && a.Name.StartsWith(waveId))                  //Where the name of the annotation starts with the wave ID and is not null
                    .ToList();                                                                //Then, convert it to a list

                foreach (var annotation in annotationsToRemove)                               //Loop: through each annotation
                {
                    chart_OHLCV.Annotations.Remove(annotation);                               //And remove it from the chart if found
                }
            }
            catch (Exception ex)                                                              //Try to catch any exception that may occur
            {
                Console.WriteLine($"Error: clearing annotations was not successfull: {ex.Message}"); //Error message
            }
        }

        // Function: Clear any existing Fibonacci annotations on the chart:
        private void chart_OHLCV_MouseDown(object sender, MouseEventArgs e)                   //Function definition for Mouse down event (for rubberbanding)
        {
            //Step 1. clear all previous annotations:
            RemovePreviousFibonacciAnnotations();                                             //Clear any existing Fibonacci annotations

            //Step 2. Clear any previous rubberbanding or rectangle annotations:
            var prevRubberbanding = chart_OHLCV.Annotations                                           //Get all previous rubberbanding annotations
                .Where(a => a.Name != null &&                                                 //Where the name of annotation starts from the following: 
                    (a.Name.StartsWith("rubberband_rect") ||                                  //Rubberband rectangle
                     a.Name.StartsWith("rubberband_diag")))                                   //Rubberband diagonal
                .ToList();                                                                    //Add then to the list

            //Step 3. For the list of all rubberbanding and annotations that that we created, remove them from the list
            foreach (var ann in prevRubberbanding)                                                    //Loop: through each annotation
                chart_OHLCV.Annotations.Remove(ann);                                          //then remove it from the chart

            //Step 4. Get the nearest peak or valley to the mouse position:
            int nearestSnappedIndex = findClosestPeakOrValley(e.X, e.Y);

            //Step 5. If a peak or valley is found, start dragging:
            if (nearestSnappedIndex != -1)                                                          //If snappedIndex is not -1 (default value), then start dragging
            {
                isDraggingActive = true;                                                           //Set isDragging to true
                startIndex = nearestSnappedIndex;                                                   //Set the start index to the snapped index
                decimal startingPrice;                                                          //Get the start price based on the type of peak or valley
                if (isPeak(startIndex))                                                      //If it is a peak,
                    startingPrice = filteredCandlesticksByDate[startIndex].High;                //Get the High price and set it as start price
                else if (isValley(startIndex))                                               //If it is a valley,
                    startingPrice = filteredCandlesticksByDate[startIndex].Low;                 //Get the Low price and set it as start price
                else                                                                         //else:
                    startingPrice = filteredCandlesticksByDate[startIndex].Close;               //Get the Close price and set it as start price

                var chartArea = chart_OHLCV.ChartAreas[0];                                   //Get the chart area
                double startY = (double)startingPrice;                                          //Get the start Y value based on the start price

                //Step 6. Create a new rectangle annotations:
                selectedRectangle = new RectangleAnnotation                                   //Create a new rectangle annotation
                {
                    AxisX = chartArea.AxisX,                                                 //Get chart area X and set it to Axis X for rectangle
                    AxisY = chartArea.AxisY,                                                 //Get chart area Y and set it to Axis Y for rectangle
                    X = startIndex,                                                          //Set the start index for rectangle
                    Y = startY,                                                              //Set the start Y value for rectangle
                    LineColor = Color.Red,                                                   //Set the line color for rectangle
                    BackColor = Color.FromArgb(40, Color.Red),                               //Set the background color for rectangle
                    LineWidth = 2,                                                           //Set the line width for rectangle
                    ClipToChartArea = chartArea.Name,                                        //Set the chart area name for rectangle
                    IsSizeAlwaysRelative = false                                             //Set isSizeAlwaysRelative to false
                };

                //Step 7. Create a new diagonal line annotation:
                diagonalLineAnnotation = new LineAnnotation                                             //Create a new diagonal line annotation
                {
                    AxisX = chartArea.AxisX,                                                 //Get chart area X and set it to Axis X for diagonal line
                    AxisY = chartArea.AxisY,                                                 //Get chart area Y and set it to Axis Y for diagonal line
                    LineColor = Color.Red,                                                   //Set the line color for diagonal line
                    LineWidth = 2,                                                           //Set the line width for diagonal line
                    IsSizeAlwaysRelative = false,                                            //Set isSizeAlwaysRelative to false
                    ClipToChartArea = chartArea.Name                                         //Set the chart area name for diagonal line
                };

                //Step 8. Set the name for rectangle and diagonal line annotations:
                chart_OHLCV.Annotations.Add(selectedRectangle);                               //Add rectangle annotation to the chart
                chart_OHLCV.Annotations.Add(diagonalLineAnnotation);                                    //Add diagonal line annotation to the chart
            }
        }

        // Function: Clear any previous Fibonacci annotations on the chart:
        private void ClearPreviousAnnotations()                                              //Clear previous annotations
        {
            var annotationsToRemove = chart_OHLCV.Annotations                                //Get all previous annotations
                .Where(a => a.Name != null &&                                                //Where the name of annotation starts from the following:
                       (a.Name.StartsWith("rubberband_rect") ||                              //Rubberband rectangle
                        a.Name.StartsWith("rubberband_diag")))                               //Rubberband diagonal
                .ToList();                                                                   //Add them to the list

            foreach (var ann in annotationsToRemove)                                         //Loop through each annotation
                chart_OHLCV.Annotations.Remove(ann);                                         //And remove them from the chart

            chart_OHLCV.Invalidate();                                                        //Refresh the chart
        }

        // Function: Get the nearest peak or valley based on mouse position (to know where to start drawing rectangle):
        private int findClosestPeakOrValley(int mouseX, int mouseY)                             //Get the nearest peak or valley: function definition
        {
            double nearestDistance = double.MaxValue;                                        //Initialize the closest distance to maximum value
            int nearestSnappedIndex = -1;                                                           //Initialize the closest index to -1 (default value)

            //Get the index of the candlestick based on the mouse position:
            for (int i = 0; i < filteredCandlesticksByDate.Count; i++)
            {
                //Check if the index is a peak or valley:
                if (!isPeak(i) && !isValley(i))
                    continue; //skip if not a peak or valley

                var currentCandlestick = filteredCandlesticksByDate[i];                                  //Get the candlestick at the current index
                double yVal = isPeak(i) ? (double)currentCandlestick.High : (double)currentCandlestick.Low;          //If it is a peak, get the High value; if it is a valley, get the Low value
                double xPixel = chart_OHLCV.ChartAreas[0].AxisX.ValueToPixelPosition(i);     //Get the X pixel position based on the index
                double yPixel = chart_OHLCV.ChartAreas[0].AxisY.ValueToPixelPosition(yVal);  //Get the Y pixel position based on the Y value

                //Calculate the distance between the mouse position and the candlestick position:
                double distance = Math.Sqrt(Math.Pow(mouseX - xPixel, 2) + Math.Pow(mouseY - yPixel, 2));

                //Debug message to display the index, pixel positions, and mouse position:
                Console.WriteLine($"Index: {i}, xPixel: {xPixel}, yPixel: {yPixel}, mouseX: {mouseX}, mouseY: {mouseY}");

                //Check if the distance is less than the threshold and less than the closest distance:
                if (distance < SNAP_TOLERANCE_PIXELS && distance < nearestDistance)
                {
                    nearestDistance = distance;                                              //Update the closest distance
                    nearestSnappedIndex = i;                                                        //Update the closest index
                }

            }
            //Debug message to display the closest index:
            Console.WriteLine($"We currently snapped on index {nearestSnappedIndex}");
            return nearestSnappedIndex;                                                             //return the closest index
        }

        // Function: Check if the index is a peak or valley:
        private bool isPeak(int index)                                                       //Check if the index is a peak
        {
            return peakValleyIdentifier.marginPeaksAndValleys.Values                         //return the list of peaks and valleys
                .Any(list => list.Any(pv => pv.Index == index && pv.Peak));                  //contains the index and is a peak
        }

        // Function: Check if the index is a valley:
        private bool isValley(int index)                                                     //Check if the index is a valley
        {
            return peakValleyIdentifier.marginPeaksAndValleys.Values                         //Return the list of peaks and valleys
                .Any(list => list.Any(pv => pv.Index == index && pv.Valley));                //Contains the index and is a valley
        }

        List<(double, double)> listConfirmation = new List<(double, double)>();              //Degfine list of confirmations for rubberbanding
        double startY;                                                                       //Define start Y value (price) for rubberbanding
        double endY;                                                                         //Define end Y value (price) for rubberbanding

        // Function: Mouse Move event handler for rubberbanding:
        private void chart_OHLCV_MouseMove(object sender, MouseEventArgs e)                  //Mouse move event handler
        {
            if (!isDraggingActive || selectedRectangle == null) return;                            //Check if dragging is in progress and rectangle is not null

            double mouseXVal = chart_OHLCV.ChartAreas[0].AxisX.PixelPositionToValue(e.X);   //Get the mouse X value
            double mouseYVal = chart_OHLCV.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);   //Get the mouse Y value
            int approxIndex = (int)Math.Round(mouseXVal);                                   //Get the approximate index based on the mouse X value
            if (approxIndex < 0 || approxIndex >= filteredCandlesticksByDate.Count) return; //Check if the index is within the valid range

            endIndex = approxIndex;                                                         //Set the end index to the approximate index
            var startPoint = chart_OHLCV.Series[0].Points[startIndex];                      //Get the start point based on the start index
            startY = isPeak(startIndex) ? startPoint.YValues[0] : startPoint.YValues[1];    //Get the start Y value based on the type of peak or valley
            endY = mouseYVal;                                                               //Set the end Y value to the mouse Y value

            // Step 1. Calculate the rectangle and diagonal line positions:
            double rectX = startIndex;                                                     //Start X value for rectangle as start index
            double rectY = Math.Min(startY, endY);                                         //Start Y value for rectangle as minimum of start and end Y values
            double width = endIndex - startIndex;                                          //Width of the rectangle as difference between end and start index
            double height = Math.Abs(endY - startY);                                       //Height of the rectangle as absolute difference between end and start Y values

            // Step 2. Set these values to the rectangle:
            selectedRectangle.X = rectX;                                                    //Set the X value for rectangle as start index
            selectedRectangle.Y = rectY;                                                    //Set the Y value for rectangle as minimum of start and end Y values
            selectedRectangle.Width = width;                                                //Set the width of the rectangle as difference between end and start index
            selectedRectangle.Height = height;                                              //Set the height of the rectangle as absolute difference between end and start Y values

            // Step 3. Set these values to the diagonal line:
            diagonalLineAnnotation.X = startIndex;                                          //Start X value for diagonal line as start index
            diagonalLineAnnotation.Y = startY;                                              //Start Y value for diagonal line as start Y value
            diagonalLineAnnotation.Width = width;                                           //Width of the diagonal line as difference between end and start index
            diagonalLineAnnotation.Height = endY - startY;                                  //Set the height of the diagonal line as difference between end and start Y values

            // Step 4. Remove all old Fibonacci lines confirmations:
            var toRemove = chart_OHLCV.Annotations                                         //Get all previous Fibonacci lines and confirmations
                .Where(a => a.Name != null &&                                              //where the name of annotation starts from the following:
                    (a.Name.StartsWith("fib_") || a.Name.StartsWith("fib_label_") || a.Name.StartsWith("confirm_")))      //get from Fibonacci lines and confirmations
                .ToList();                                                                 //And add them to the list
            //Step 5. Loop through each annotation and remove it from the chart:
            foreach (var ann in toRemove)                                                  //Then, loop through each annotation                            
                chart_OHLCV.Annotations.Remove(ann);                                       //And remove it from the chart

            // Step 6. Create a new wave object based on the rubber-banded rectangle:
            decimal startPrice = (decimal)startY;                                          //Get the start price based on the start Y value
            decimal endPrice = (decimal)mouseYVal;                                         //Get the end price based on the mouse Y value
            var wave = new ChooseWave();                                                   //Create a new ChooseWave object
            wave.SetWaveParameters(startIndex, endIndex, (double)startPrice, (double)endPrice);      //Set the wave properties
            
            //Step 7. Set chart area:
            var chartArea = chart_OHLCV.ChartAreas[0];

            // Step 8. Draw Fibonacci lines dynamically in the rubber-banded rectangle:
            var fibonacciLevels = wave.CalculateFibonacciLevels((double)startPrice, (double)endPrice).OrderByDescending(f => f.level).ToList();  //Calculate Fibonacci levels

            // Step 9. Draw Fibonacci lines on the chart:
            foreach (var (level, label) in fibonacciLevels)                                       //Loop through each level and label in the Fibonacci levels
            {
                var fibLine = new HorizontalLineAnnotation                                  //Define horizontal line annotation
                {
                    AxisX = chartArea.AxisX,                                                // Chart area X axis
                    AxisY = chartArea.AxisY,                                                // Chart area Y axis
                    X = wave.Left,                                                          // Left coordinate of the wave
                    Y = level,                                                              // Level of the Fibonacci line
                    Width = wave.Width,                                                     // Width of the wave
                    LineColor = Color.Blue,                                                 // Color of the line
                    LineDashStyle = ChartDashStyle.Solid,                                   // Line dash style
                    LineWidth = 1,                                                          // Line width
                    IsSizeAlwaysRelative = false,                                           // Set isSizeAlwaysRelative to false
                    ClipToChartArea = chartArea.Name,                                       // Chart area name
                    Name = $"fib_{label}_{startIndex}"                                      // Name of the Fibonacci line associated with the wave ID
                };
                chart_OHLCV.Annotations.Add(fibLine);                                       // Add Fibonacci line to the chart

                // Step 10. Create and add text annotations for Fibonacci levels:
                var fibLabel = new TextAnnotation                                           //Define text annotation    
                {
                    Text = $"{label} — {level:F2}",                                         //Set text for a Fibonacci level
                    AxisX = chartArea.AxisX,                                                // Chart area X axis
                    AxisY = chartArea.AxisY,                                                // Chart area Y axis
                    X = wave.Right + 1,                                                     // Right coordinate of the wave
                    Y = level,                                                              // Level of the Fibonacci line
                    Font = new Font("Arial", 8, FontStyle.Bold),                            // Font for the label
                    ForeColor = Color.Purple,                                               // Color of the label
                    Alignment = ContentAlignment.MiddleLeft,                                // Alignment for the label
                    IsSizeAlwaysRelative = false,                                           // Set isSizeAlwaysRelative to false
                    ClipToChartArea = chartArea.Name,                                       // Chart area name
                    Name = $"fib_label_{label}_{startIndex}"                                // Name of the label associated with the wave ID
                };
                //Step 11. Add the label to the chart:
                chart_OHLCV.Annotations.Add(fibLabel);
            }

            // Step 12. Find confirmations for the rubber-banded wave:
            var bindingList = new BindingList<Candlestick>(filteredCandlesticksByDate);     //Create a new binding list of filtered candlesticks
            double fibMargin = wave.Height * 0.01;                                          //Set fibMargin to 1% of the wave height
            var confirmations = wave.FindWaveConfirmations(startIndex, endIndex, bindingList, fibonacciLevels, fibMargin);    //Find confirmations for the wave

           // Use the confirmations method:
            var detailedConfirmations = wave.GetConfirmationDetails(startIndex, endIndex, bindingList, fibonacciLevels, fibMargin);

            // Step 13. Remove all previous confirmations from the chart first:
            var oldConfirmations = chart_OHLCV.Annotations                                  //Define old confirmations
                .Where(a => a.Tag != null && a.Tag.ToString() == "confirm")                 //Where the tag of annotation is not null and equals to "confirm"
                .ToList();                                                                  //Add them to the list
            
            foreach (var ann in oldConfirmations)                                           //Loop through each annotation                  
                chart_OHLCV.Annotations.Remove(ann);                                        //And remove old confirmations from the chart

            // Step 14. Draw updated confirmation dots using offset
            double axisRange = chart_OHLCV.ChartAreas["ChartArea_OHLC"].AxisY.Maximum       //Take the maximum value of the Y axis
                             - chart_OHLCV.ChartAreas["ChartArea_OHLC"].AxisY.Minimum;      //Take the minimum value of the Y axis
            double offset = axisRange * 0.01;                                               //Calculate offset

            // Step 15. Draw confirmation dots on the chart:
            foreach (var (x, y) in confirmations)                                           //Loop through each confirmation
            {
                var dot = new TextAnnotation                                                //Define text annotation for dots
                {
                    AxisX = chart_OHLCV.ChartAreas["ChartArea_OHLC"].AxisX,                 //Set the X axis for the chart area
                    AxisY = chart_OHLCV.ChartAreas["ChartArea_OHLC"].AxisY,                 //Set the Y axis for the chart area
                    AnchorDataPoint = chart_OHLCV.Series[0].Points[(int)x],                 // Anchor to the data point
                    Y = y + 0.5,                                                            //Y position of the dot
                    Text = "•",                                                             //Dot symbol for confirmation dots
                    ForeColor = Color.Blue,                                                 //Color for confirmation dots
                    Font = new Font("Arial", 14, FontStyle.Bold),                           //Font for confirmation dots
                    ClipToChartArea = "ChartArea_OHLC",                                     //Set the chart area name
                    Alignment = ContentAlignment.MiddleCenter,                              //Alignment for confirmation dots
                    SmartLabelStyle = { Enabled = false },                                  //Disable smart label style
                    IsSizeAlwaysRelative = false,                                           //Set isSizeAlwaysRelative to false
                    Tag = "confirm"                                                         //Tag for confirmation dots
                };

                // Step 16. Add confirmation dot to the chart:
                chart_OHLCV.Annotations.Add(dot);
            }

            // Step 17. Update confirmation count and set it to the label (number changes when we run the simulation):
            if (label_ConfirmationCount != null)                                           //Check if the label for confirmation count is not null
            {
                label_ConfirmationCount.Text = $"Confirmations: {confirmations.Count}";    //Update the label with the confirmation count
            }

        }


        // Function: Clear any existing Fibonacci annotations on the chart
        private void RemovePreviousFibonacciAnnotations()                                          //Clear any existing Fibonacci annotations
        {
            var annotationsToRemove = chart_OHLCV.Annotations                             //Get all previous annotations
                .Where(a => a.Name != null &&                                             //Where the name of annotation starts from the following:
                    (a.Name.StartsWith("fib_") ||                                         //Fibonacci lines
                     a.Name.StartsWith("confirm_") ||                                     //Confirmation dots
                     a.Name.StartsWith("rubberband_rect") ||                              //Rubberband rectangle
                     a.Name.StartsWith("rubberband_diag")))                               //Rubberband diagonal
                .ToList();

            //Loop through each annotation and remove it from the chart:
            foreach (var annotation in annotationsToRemove)
            {
                chart_OHLCV.Annotations.Remove(annotation);                              //Remove annotation from the chart
            }

            chart_OHLCV.Invalidate();                                                    //Refresh the chart
        }

        // Function: Mouse Up event handler for rubberbanding
        private void chart_OHLCV_MouseUp(object sender, MouseEventArgs e)                //Mouse Up event handler
        {
            if (!isDraggingActive || selectedRectangle == null)                                 //Check if dragging is in progress and rectangle is not null
                return;                                                                  //Exit the function if not dragging

            isDraggingActive = false;                                                          //Set isDragging to false

            //Step 1. Check if the end index is valid:
            if (startIndex == -1 || endIndex == -1 || startIndex == endIndex)            //If start and end index are not valid
            {
                MessageBox.Show("This wave is invalid! Please, try another wave.");      //Display error message
                return;                                                                  //Exit the function
            }

            //Step 2. Check if the start and end index are valid:
            if (endIndex < startIndex)
            {
                int temp = startIndex;                                                   //Swap start and end index
                startIndex = endIndex;                                                   //Set start index to end index
                endIndex = temp;                                                         //Set end index to start index
                double tempY = startY;                                                   //Swap start and end Y value
                startY = endY;                                                           //Set start Y value to end Y value
                endY = tempY;                                                            //Set end Y value to start Y value
            }

            // Step 3. Set status of wave to rubberbanded (True):
            IsWaveRubberbanded = true;

            // Step 4. Create a selected wave object based on rubber-banding
            currentlySelectedWave = new Wave(                                                     //Create a new wave object
                startIndex,                                                              //Start index
                endIndex,                                                                //End index
                filteredCandlesticksByDate[startIndex].Date,                             //Date of the candlestick
                filteredCandlesticksByDate[endIndex].Date,                               //Date of the candlestick
                (decimal)startY,                                                         //Start Y value (price)
                (decimal)endY);                                                          //End Y value (price)


            // Step 5. Set the parameters for our simulation:
            double waveHeight = Math.Abs(endY - startY);                                 //Set wave height to absolute difference between start and end Y values
            minimumPriceForSimulation = Math.Min(startY, endY) - (waveHeight * 0.10);           //Set minimum price to 10% below minimum
            maximumPriceForSimulation = Math.Max(startY, endY) + (waveHeight * 0.10);           //Set maximum price to 10% above maximum
            simulationStepSize = waveHeight * 0.02;                                      //Set step size to 2% of wave height
            currentSimulatedPrice = startY;                                             //Set current simulation price to start Y value

            // Step 6. Store rubber band start point for simulation
            rubberBandOrigin = new Point(                                                 //Rubber band start point
                (int)chart_OHLCV.ChartAreas[0].AxisX.ValueToPixelPosition(startIndex),   //where X is the start index
                (int)chart_OHLCV.ChartAreas[0].AxisY.ValueToPixelPosition(startY)        //and Y is the start Y value
            );

            // Step 7. Change the name for annotations to avoid duplicates:
            if (selectedRectangle != null)                                                //Set the name for rectangle annotation
                selectedRectangle.Name = $"rubberband_rect_{startIndex}";                 //to this name

            // Step 8. Set the name for diagonal line annotation:
            if (diagonalLineAnnotation != null)                                                     //Set the name for diagonal line annotation    
                diagonalLineAnnotation.Name = $"rubberband_diag_{startIndex}";                      //to this name

            selectedRectangle = null;                                                     //Clear the current rectangle
            diagonalLineAnnotation = null;                                                          //Clear the diagonal line
        }

        // Function: Update the simulation end point based on the current price
        private void UpgradeSimulationEndingPoint(double price)                              //Update the simulation end point
        {
            if (currentlySelectedWave == null) return;                                            //Check if selected wave is null, then exit the function

            var chartArea = chart_OHLCV.ChartAreas[0];                                   //Set the chart area

            // Step 1. Set the end Y value based on the price 
            if (IsWaveRubberbanded)
            {
                endY = price;                                                            //Set the end Y value to the price
            }
            else                                                                         //Else:
            {
                currentlySelectedWave.EndValue = (decimal)price;                                  //For each wave from the dropdown list, set the end value to the price
            }

            // Step 3. Clear any existing Fibonacci annotations:
            RemovePreviousFibonacciAnnotations();

            // Step 4. Remove any previous simulation annotations:
            var oldSimulationAnnotations = chart_OHLCV.Annotations                         //Find all previous simulation annotations
                .Where(a => a.Name == "sim_rect" || a.Name == "sim_diag")                //where the name of annotation is either "sim_rect" or "sim_diag"
                .ToList();                                                               //And add them to the list
            foreach (var ann in oldSimulationAnnotations)                                  //Loop through each and 
                chart_OHLCV.Annotations.Remove(ann);                                     //Remove them


            // Step 5. Create a wave for drawing and set its parameters
            var wave = new ChooseWave();                                                 //Create a wave
            wave.SetWaveParameters(currentlySelectedWave.StartIndex, currentlySelectedWave.EndIndex,                 //Set the parameters of the wave
                        (double)currentlySelectedWave.StartValue, price);                         //Start value and price

            // Step 6. Draw rectangle annotation:
            var rectAnnotation = new RectangleAnnotation                                 //Create new rectangle annotation
            {
                AxisX = chartArea.AxisX,                                                 //Set the chart area X axis           
                AxisY = chartArea.AxisY,                                                 //Set the chart area Y axis
                X = currentlySelectedWave.StartIndex,                                             //Set the start index for rectangle
                Y = Math.Max((double)currentlySelectedWave.StartValue, price),                    //Set the Y value for rectangle
                Width = currentlySelectedWave.EndIndex - currentlySelectedWave.StartIndex,                 //Set the width of the rectangle
                Height = -Math.Abs(price - (double)currentlySelectedWave.StartValue),             //Set the height of the rectangle
                LineColor = Color.Red,                                                   //Set the line color for rectangle
                BackColor = Color.FromArgb(40, Color.Red),                               //Set the background color for rectangle
                LineWidth = 2,                                                           //Set the line width for rectangle
                IsSizeAlwaysRelative = false,                                            //Set isSizeAlwaysRelative to false
                Name = "sim_rect"};                                                      //Set the name for rectangle annotation
            

            chart_OHLCV.Annotations.Add(rectAnnotation);                                 //Add new rectangle annotation to the annotations of the chart

            // Step 7. Draw diagonal line annotation:
            var lineAnnotation = new LineAnnotation                                      //Create new diagonal line annotation
            {
                AxisX = chartArea.AxisX,                                                 //Set the chart area X axis
                AxisY = chartArea.AxisY,                                                 //Set the chart area Y axis
                X = currentlySelectedWave.StartIndex,                                             //Set the start index for diagonal line
                Y = (double)currentlySelectedWave.StartValue,                                     //Set the start Y value for diagonal line
                Width = currentlySelectedWave.EndIndex - currentlySelectedWave.StartIndex,                 //Set the width of the diagonal line
                Height = price - (double)currentlySelectedWave.StartValue,                        //Set the height of the diagonal line
                LineColor = Color.Red,                                                   //Set the line color for diagonal line
                LineWidth = 2,                                                           //Set the line width for diagonal line
                IsSizeAlwaysRelative = false,                                            //Set isSizeAlwaysRelative to false
                Name = "sim_diag"                                                        //Set the name for diagonal line annotation
            };
            chart_OHLCV.Annotations.Add(lineAnnotation);                                 //Add new diagonal line annotation to the annotations of the chart

            // Step 8. Calculate the Fibonacci levels for the currently selected wave:
            var chartBindingList = new BindingList<Candlestick>(filteredCandlesticksByDate);          //Create a new binding list of filtered candlesticks
            var fibLevels = wave.CalculateFibonacciLevels((double)currentlySelectedWave.StartValue, price);    //Calculate Fibonacci levels

            // Step 9. Draw Fibonacci lines on the chart:
            foreach (var (level, label) in fibLevels)                                    //Loop through each level and label in the Fibonacci levels
            {
                var fibLine = new HorizontalLineAnnotation                               //Create new horizontal line annotation
                {
                    AxisX = chartArea.AxisX,                                             // Set the chart area X axis
                    AxisY = chartArea.AxisY,                                             // Set the chart area Y axis
                    X = wave.Left,                                                       // Left coordinate of the wave
                    Y = level,                                                           // Level of the Fibonacci line
                    Width = wave.Width,                                                  // Width of the wave
                    LineColor = Color.Blue,                                              // Color of the line
                    LineDashStyle = ChartDashStyle.Solid,                                // Line dash style
                    LineWidth = 1,                                                       // Line width
                    IsSizeAlwaysRelative = false,                                        // Set isSizeAlwaysRelative to false
                    ClipToChartArea = chartArea.Name,                                    // Chart area name
                    Name = $"fib_{label}_{currentlySelectedWave.StartIndex}"                      // Name of the Fibonacci line associated with the wave ID
                };
                chart_OHLCV.Annotations.Add(fibLine);                                    // Add Fibonacci line to the chart

                // Step 10. Create and add text annotations for Fibonacci levels:
                var fibLabel = new TextAnnotation                                        //Create new text annotation
                {
                    Text = $"{label} — {level:F2}",                                      // Set text for a Fibonacci level
                    AxisX = chartArea.AxisX,                                             // Set the chart area X axis
                    AxisY = chartArea.AxisY,                                             // Set the chart area Y axis
                    X = wave.Right + 1,                                                  // Right coordinate of the wave
                    Y = level,                                                           // Level of the Fibonacci line
                    Font = new Font("Arial", 8, FontStyle.Bold),                         // Font for the label
                    ForeColor = Color.Purple,                                            // Color of the label
                    Alignment = ContentAlignment.MiddleLeft,                             // Alignment for the label
                    IsSizeAlwaysRelative = false,                                        // Set isSizeAlwaysRelative to false
                    ClipToChartArea = chartArea.Name,                                    // Chart area name
                    Name = $"fib_label_{label}_{currentlySelectedWave.StartIndex}"       // Name of the label associated with the wave ID
                };
                chart_OHLCV.Annotations.Add(fibLabel);                                   // Add the label to the chart
            }

            // Step 11. Find confirmations for the currently selected wave:
            double fibMargin = wave.Height * 0.01;                                       //Set fibMargin to 1% of the wave height
            var confirmations = wave.FindWaveConfirmations(                              //Findconfirmations function call and set to 'confirmations'
                currentlySelectedWave.StartIndex,                                        //Start index
                currentlySelectedWave.EndIndex,                                          //End index
                chartBindingList,                                                        //Binding list of filtered candlesticks
                fibLevels,                                                               //Fibonacci levels
                fibMargin                                                                //Margin for confirmations
            );

            // Step 12. Draw the dots for confirmations for a currently selected wave:
            foreach (var (x, y) in confirmations)                                        //Loop through each confirmation
            {
                var dot = new TextAnnotation                                             //Create a text annotation for each dot
                {
                    AxisX = chartArea.AxisX,                                             //Set the chart area X axis
                    AxisY = chartArea.AxisY,                                             //Set the chart area Y axis
                    AnchorDataPoint = chart_OHLCV.Series[0].Points[(int)x],              //Anchor to the data point
                    Y = y + 0.5,                                                         //Y position of the dot
                    Text = "•",                                                          //Dot symbol for confirmation dots
                    ForeColor = Color.Blue,                                              //Color for confirmation dots
                    Font = new Font("Arial", 14, FontStyle.Bold),                        //Font for confirmation dots
                    ClipToChartArea = chartArea.Name,                                    //Set the chart area name
                    Alignment = ContentAlignment.MiddleCenter,                           //Alignment for confirmation dots
                    SmartLabelStyle = { Enabled = false },                               //Disable smart label style
                    IsSizeAlwaysRelative = false,                                        //Set isSizeAlwaysRelative to false
                    Tag = "confirm"                                                      //Tag for confirmation dots
                };
                chart_OHLCV.Annotations.Add(dot);                                        //Add dot text annotations to the other annotations on the chart
            }

            // Step 13. Update confirmation count and set it to the label (number changes when we run the simulation):
            if (label_ConfirmationCount != null)                                         //If confirmation count is not null
            {
                label_ConfirmationCount.Text = $"Confirmations: {confirmations.Count}";  //Update the label with the confirmation count
            }

            chart_OHLCV.Invalidate();                                                    //Refresh the chart
        }

        // Function: Start the simulation automatically
        private void LaunchSimulation()                                                   //Start the simulation
        {
            if (currentlySelectedWave == null)                                                    //If selected wave is null (no wave was selected)
            {
                MessageBox.Show("Please select any wave on the chart.");                 //Display user message
                return;                                                                  //Exit the function
            }
            // Step 1. When simulation is running, disable the increase/decrease buttons to prevent adjasting the simulation from the user
            button_Plus.Enabled = false;                                                 //Plus button is disabled during simulation
            button_Minus.Enabled = false;                                                //Minus button is disabled during simulation

            // Step 2. Toggle the button text. When simulation is running, the button text is set to "Stop"
            button_Simulate.Text = "Stop";                                               //Set to "Stop" when simulation is running

            // Step 3. Then, reverse the dirction of simulation 
            isReversingSimulation = false;                                               //reset flag variable to false

            // Step 4. Start the timer to tick
            Timer_Simulate.Start();
            //Step 5. Start the Simulation
            isSimulationRunning = true;
        }

        // Function: Stop the simulation
        private void TerminateSimulation()                                                      //Function definition for StopSimulation
        {
            // Step 1. Stop the timer
            Timer_Simulate.Stop();                                                              //Stop the timer
            isSimulationRunning = false;                                                        //Set isSimulating to false (to stop simulation)

            // Step 2. Enable Plus and Minus buttons to allow user to adjust the simulation
            button_Plus.Enabled = true;                                                  //Set Plus button to enabled
            button_Minus.Enabled = true;                                                 //Set Minus button to enabled

            // Step 3. Upgrade the button text 
            button_Simulate.Text = "Simulate";
        }

        // Function: Timer tick event handler for simulation
        private void Timer_Simulate_Tick(object sender, EventArgs e)                     //Function definition
        {
            if (currentlySelectedWave == null)                                           //If selected wave is null
            {
                TerminateSimulation();                                                   //Stop the simulation
                return;                                                                  //Exit the function
            }

            // Step 1. Update the current simulation price based on the direction of the simulation
            if (isReversingSimulation)                                                   //If simulation is reversing
                currentSimulatedPrice -= simulationStepSize;                            //Decrease the current simulation price by step size
            else                                                                         //Else:
                currentSimulatedPrice += simulationStepSize;                            //Increase the current simulation price by step size

            // Step 2. Check if the current simulation price is within the min and max price range: to reverse of the simulation
            if (!isReversingSimulation && currentSimulatedPrice >= maximumPriceForSimulation)
            {
                isReversingSimulation = true;                                           // Reverse direction of simulation
            }
            else if (isReversingSimulation && currentSimulatedPrice <= minimumPriceForSimulation)    //Else: if current simulation price is less than or equal to min price
            {
                isReversingSimulation = false;                                          // Reverse direction of simulation again
            }

            // Step 3. Update the simulation end point based on the current price and redraw simulation:
            UpgradeSimulationEndingPoint(currentSimulatedPrice);
        }

        // Function: Button click event handler for simulation
        private void button_Simulate_Click(object sender, EventArgs e)                  // Button click event handler
        {
            if (!isSimulationRunning)                                                   // If simulation is not running,
            {
                LaunchSimulation();                                                     // then start the simulation.
            }
            else                                                                        // If simulation is running,
            {
                TerminateSimulation();                                                  // then stop the simulation.
            }
        }

        // Function: Button click event handler for increasing and decreasing the simulation price
        private void button_Plus_Click(object sender, EventArgs e)                      //Button click event handler for "Plus" to increase the price
        {
            if (currentlySelectedWave == null) return;                                  //Check if selected wave is null, then exit the function

            //Increase the current price by step size:
            currentSimulatedPrice += simulationStepSize;

            //If current simulation price is greater than max price, set it to max price:
            if (currentSimulatedPrice > maximumPriceForSimulation)
            {
                currentSimulatedPrice = maximumPriceForSimulation;                      //Set current simulation price to max price
            }

            // Update the end points and redraw the simulation:
            UpgradeSimulationEndingPoint(currentSimulatedPrice);
        }

        // Function: Button click event handler for decreasing the simulation price
        private void button_Minus_Click(object sender, EventArgs e)                     //Button click event handler for "Minus" to decrease the price
        {
            if (currentlySelectedWave == null) return;                                  //If no wave is selected, exit the function

            // Now, decrease the current price by step size:
            currentSimulatedPrice -= simulationStepSize;

            // Check if current simulation price is less than min price, set it to min price:
            if (currentSimulatedPrice < minimumPriceForSimulation)
            {
                currentSimulatedPrice = minimumPriceForSimulation;                     //Set current simulation price to min price
            }

            // Update the end points and redraw the simulation:
            UpgradeSimulationEndingPoint(currentSimulatedPrice);
        }
    }

}


       











