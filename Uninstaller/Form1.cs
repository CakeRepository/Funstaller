using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uninstaller
{
    public partial class Form1 : Form
    {
        UninstallProgram uPrograms;
        public Form1()
        {
            InitializeComponent();
        }
        private void displayApps()
        {
            dataGV1.Rows.Clear();
            uPrograms = new UninstallProgram();
            var uninstallList = uPrograms.getApplication();

            foreach (var tmp in uninstallList)
            {
                var index = dataGV1.Rows.Add();
                dataGV1.Rows[index].Cells["ProductName"].Value = tmp.Item1;
                dataGV1.Rows[index].Cells["UninstallString"].Value = tmp.Item2;
            }
        }
        private void dataGV1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            createDatatable();
            displayApps();
        }

        private void createDatatable()
        {
            dataGV1.RowHeadersVisible = false;

            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.Name = "X";
            checkColumn.HeaderText = "Check";
            checkColumn.Width = 50;
            checkColumn.ReadOnly = false;
            checkColumn.FillWeight = 10;
            dataGV1.Columns.Add(checkColumn);
            dataGV1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            DataGridViewTextBoxColumn productNameColumn = new DataGridViewTextBoxColumn();
            productNameColumn.Name = "ProductName";
            productNameColumn.HeaderText = "Product Name";
            productNameColumn.Width = 175;
            productNameColumn.ReadOnly = true;
            productNameColumn.ToolTipText = "Product Name";
            dataGV1.Columns.Add(productNameColumn);

            DataGridViewTextBoxColumn uninstallStringColumn = new DataGridViewTextBoxColumn();
            uninstallStringColumn.Name = "UninstallString";
            uninstallStringColumn.HeaderText = "Uninstall String";
            uninstallStringColumn.Width = 275;
            uninstallStringColumn.ReadOnly = true;
            dataGV1.Columns.Add(uninstallStringColumn);


        }

        private void uninstallButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGV1.Rows)
            {
                string uninstallString = "";
                try
                {
                    uninstallString = row.Cells[2].Value.ToString();
                }
                catch (Exception error)
                {
                    Console.WriteLine(error);
                }
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    if (uninstallString.Contains("Msiexec") || uninstallString.Contains("MsiExec") || uninstallString.Contains("msiexec"))
                    {
                        string uninstall = " /X " + uninstallString.Substring(14);

                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = "MsiExec.exe";
                        startInfo.Arguments = uninstall;
                        startInfo.RedirectStandardOutput = true;
                        startInfo.RedirectStandardError = true;
                        startInfo.UseShellExecute = false;
                        startInfo.CreateNoWindow = true;

                        Process processTemp = new Process();
                        processTemp.StartInfo = startInfo;
                        processTemp.EnableRaisingEvents = true;
                        try
                        {
                            processTemp.Start();
                            processTemp.WaitForExit();
                        }
                        catch (Exception error)
                        {
                            Console.WriteLine(error);
                        }
                    }
                    else if (uninstallString == "")
                    {
                        //Last Variable here
                    }
                    else
                    {

                        string uniString = row.Cells[2].Value.ToString();
                        int index = uniString.IndexOf(".exe");
                        string uninstallProgram = uniString.Substring(0, index + 4);

                        if (uninstallProgram.Contains('"'))
                        {
                            string arguments = uniString.Substring(index + 5);
                            var process = Process.Start(uninstallProgram + "\"", arguments);
                            process.WaitForExit();
                        }
                        else
                        {

                            string firstP = uninstallProgram.Insert(0, "\"");
                            string secondP = firstP.Insert(firstP.Length, "\"");
                            var process = Process.Start(secondP);
                            process.WaitForExit();
                        }

                    }
                }

            }

        }
    }
}

