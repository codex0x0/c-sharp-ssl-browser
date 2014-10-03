using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace testSSLPrj
{
    public partial class Form1 : Form
    {


        lib_http.HTTP httpClient = new lib_http.HTTP(false);

        public Form1()
        {
            InitializeComponent();


           
        }

        private void btnSSLVisit_Click(object sender, EventArgs e)
        {
            bool writeResults = false;
            bool msgBoxResults = false;
            String defaultUA = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:7.0.1) Gecko/20100101 Firefox/7.0.12011-10-16 20:23:00";
            String defaultURi = Convert.ToString(txtboxURL.Text);

            if (chkBoxWriteResults.Checked)
            {
                writeResults = true;
            }
            if (chkMsgBox.Checked)
            {
                msgBoxResults = true;
            }


            String strResults = httpClient.sslGet(defaultURi, "/", "www.meetme.com", defaultUA);

            if (writeResults)
            {
                

                File.WriteAllText(Application.StartupPath + "strResults.htm", strResults);

            }
            if (msgBoxResults)
            {

                MessageBox.Show(strResults);
            }



        }

        private void btnRegHTTP_Click(object sender, EventArgs e)
        {
            bool writeResults = false;
            bool msgBoxResults = false;
            String defaultUA = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:7.0.1) Gecko/20100101 Firefox/7.0.12011-10-16 20:23:00";
            String defaultURi = Convert.ToString(txtboxURL.Text);

            if (chkBoxWriteResults.Checked)
            {
                writeResults = true;
            }
            if (chkMsgBox.Checked)
            {
                msgBoxResults = true;
            }


            String strResults = httpClient.Get(defaultURi, "/", "www.meetme.com", defaultUA);

            if (writeResults)
            {


                File.WriteAllText(Application.StartupPath + "strResults.htm", strResults);

            }
            if (msgBoxResults)
            {

                MessageBox.Show(strResults);
            }
        }
    }
}
