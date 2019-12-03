﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

//Question 3 - Must contain 3rd party library
using LumenWorks.Framework.IO.Csv; //URL: https://www.nuget.org/packages/LumenWorksCsvReader/

namespace AT3
{
    public partial class CSVReader : Form
    {

        public CSVReader()
        {
            InitializeComponent();
        }


        //Open CSV file path button
        private void BtnDisplay_Click(object sender, EventArgs e)
        {
            lstOutput.Items.Clear(); //Clear listbox when displaying new file

            //Try and Catch Method is used to catch if user is display a CSV File without selecting one.
            try
            {
                using (CsvReader csv =
                new CsvReader(new StreamReader(tbPath.Text), true)) //Display result based on the file path provided at textBox
                {
                    int fieldCount = csv.FieldCount; //Count number of items

                    string[] headers = csv.GetFieldHeaders(); //First item on CSV will be the header

                    while (csv.ReadNextRecord())
                    {
                        for (int i = 0; i < fieldCount; i++)

                            lstOutput.Items.Add(string.Format("{0} = {1};", //Add items into listbox as header = information
                                          headers[i], csv[i]));
                    }
                    toolStripStatusLabel1.Text = ("Added");

                }
            }
            catch
            {
                MessageBox.Show("Please open a CSV File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Open CSV file Button
        private void BtnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog(); //Creating a new instance of a OpenFileDialog Class

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) //Launch new File Dialog to select a directory
            {
                string filepath = ofd.FileName; //filepath = path directory
                tbPath.Text = filepath; //FileName property represents the file name selected in the open file dialog.
            }

        }

        //Question 3 - Must contains searching techiques
        private int BinarySearch(string j)
        {

            //get numofitems added to list
            int numItems = lstOutput.Items.Count;

            int lowerBound = 0;
            int upperBound = (numItems) - 1;
            int mid;

            while (true)
            {
                //Get the middle element of the array because value is compared with it
                mid = (lowerBound + upperBound) / 2;
                int i = lstOutput.Items[mid].ToString().CompareTo(j);
                if (i == 0)
                {
                    return mid; //if it match, return the middle element
                }
                else if (lowerBound > upperBound) //compare whether lowerBound is more than upperBound, if yes, return -1
                {
                    return -1;
                }
                else
                {
                    if (i < 0) //if int i is less than 0, lowerBound = middle element plus one else uppperBound = middle element minus one
                    {
                        lowerBound = mid + 1;
                    }
                    else
                    {
                        upperBound = mid - 1;
                    }
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {

            //Searched listBox based on user input
            int result = BinarySearch(tbSearch.Text);

            if (result == -1) //if result is equal to -1, toolstripstatuslabel will displayed "Not Found"
            {
                toolStripStatusLabel1.Text = ("Not Found");
            }
            else //else, toolstripstatuslabel will displayed "Found"
            {
                //Value will be selected from ListBox
                lstOutput.SelectedIndex = result;
                toolStripStatusLabel1.Text = ("Found");
            }
        }

        //Question 3 - Must contains sorting algorithms
        public void BubbleSort(List<string> list)
        {
            //this count the number of items added in listBox
            int numItems = lstOutput.Items.Count;
            string temp;

            for (int i = 1; i < numItems; i++)
            {
                for (int j = 0; j < (numItems - i); j++)
                {
                    if (list[j].CompareTo(list[j + 1]) > 0)
                    {
                        temp = list[j + 1];
                        list[j + 1] = list[j];
                        list[j] = temp;

                    }
                }
            }
        }
        private void BtnSort_Click(object sender, EventArgs e)
        {
            //Question 3 - Must contain dynamic data structures
            List<string> csv = new List<string>();

            //This method pull lstOutput data into object I
            foreach (object i in lstOutput.Items)
            {
                csv.Add(i.ToString()); //add listBox item in list
            }

            BubbleSort(csv); //Sort List
            lstOutput.Items.Clear(); //Reset ListBox

            foreach (object i in csv) //pull sorted items from csv list and save into object I
            {
                lstOutput.Items.Add(i); //Add list item in listBox
            }
            toolStripStatusLabel1.Text = ("Sorted");
        }

        //When listBox is clicked twice, listBox will be reset
        private void LstOutput_DoubleClick(object sender, EventArgs e)
        {
            lstOutput.Items.Clear();
        }

    }

}

