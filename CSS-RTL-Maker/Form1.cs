using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSSRTLMaker
{
    public partial class Form1 : Form
    {
        string targetpath;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                string filename = Path.GetFileNameWithoutExtension(textBox1.Text);
                targetpath = Path.GetDirectoryName(openFileDialog1.FileName) + "\\"+filename+"-rtl.css";
                button2.Enabled = true;
            }
            else
            {
                textBox1.Clear();
                button2.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            convertToRTL(textBox1.Text);
        }

        private void convertToRTL(string Filename)
        {
            List<string> fileLines = File.ReadAllLines(Filename).ToList();

           
            for (int i = 0; i < fileLines.Count; i++)
            {
                string ln = fileLines[i];
                if (endswithsemi(ln))
                {
                    if (ContainsWord(ln, "right"))
                    {
                        fileLines[i] = ln.Replace("right", "left");
                    }
                    else if (ContainsWord(ln, "left"))
                    {
                        fileLines[i] = ln.Replace("left", "right");
                    }

                    if (ContainsWord(ln, "translatex"))
                    {
                        if (ContainsWord(ln.Substring(ln.IndexOf("(")), "-"))
                        {
                            //string x= ln.Substring(0, ln.IndexOf("(")) + (ln.Substring(ln.IndexOf("(")).Replace("-", ""));
                            fileLines[i] = ln.Substring(0,ln.IndexOf("("))+(ln.Substring(ln.IndexOf("(")).Replace("-", ""));
                        }
                        else
                        {
                            fileLines[i] = ln.Replace("(", "(-");
                        }
                    }
                }
            }

         

            Saveconverted(targetpath, fileLines);

        }



        private void Saveconverted(string targetpath, List<string> fileLines)
        {
            try
            {
                File.WriteAllLines(targetpath, fileLines);
                MessageBox.Show("Export Converted File Succedded");
                string dir = Path.GetDirectoryName(targetpath);
                Process.Start(dir);
            }
            catch (Exception e)
            {
                MessageBox.Show("error : " + e.Message);
            }

        }


        private bool ContainsWord(string ln, string word)
        {
            return ln.ToLower().Contains(word);
        }

        private bool endswithsemi(string ln)
        {


            return ln.Length > 0 ? ln.ElementAt(ln.Length - 1).Equals(';') : false;

        }
    }
}
