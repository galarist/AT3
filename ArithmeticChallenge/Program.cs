/*
 *      Student Number: 451381461
 *      Name:           Mitchell Stone
 *      Date:           14/09/2018
 *      Purpose:        The entry point for tha applicaton
 *      Known Bugs:     nill
 */

using System;
using System.Windows.Forms;

namespace ArithmeticChallenge
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Instructor());
        }
    }
}
