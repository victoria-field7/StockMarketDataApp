using System;                             //System namespace for basic system functions
using System.Collections.Generic;         //Namespace to import List and other generic collection classes
using System.Linq;                        //Namespace for LINQ queries (Language Integrated Query)
using System.Threading.Tasks;             //Namespace for asynchronous programming and parallelism
using System.Windows.Forms;               //Namespace for UI components of Windows Forms Apps

//Custom namespace for this project
namespace Project_2
{
    //Main class for application which contains the entry point for the program
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()                                           //main method: entry point to the program
        {
            Application.EnableVisualStyles();                         //Enable visual styles for the application to make it look modern and consistent with the OS theme  
            Application.SetCompatibleTextRenderingDefault(false);     //Set the default text rendering to be compatible with the current OS settings
            Application.Run(new Form1());                             //Run the main form of the application which will be displayed as the main window of the app
        }
    }
}
