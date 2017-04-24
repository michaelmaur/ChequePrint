using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


/* 
Add Trapping 
Input the centavo in 1 blank
Confirm the process for centavos
Setup printer task!


https://pastebin.com/zNSxW4VN
https://msdn.microsoft.com/en-us/library/6he9hz8c(v=vs.110).aspx
*/


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void menuClear_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtAmount.Clear();
            txtStrAmount.Clear();

        }
        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                //condition

                if (String.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Please enter the Name the box Provided", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtName.Focus();
                }
                //CHECK FOR SPECIAL CHARACTERS
                else if (checkSpecialChar(txtName.Text))
                {
                    MessageBox.Show("Please input without any special characters", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtName.Focus();
                }
                else if (checkNumber(txtName.Text))
                {
                    MessageBox.Show("Please do not include numbers in the name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtName.Focus();
                }
                else
                    txtAmount.Focus();

            }
        }

        public Boolean checkSpecialChar(string check)
        {

            Boolean flag = false;

            // convert each character of the string value in the textbox to ASCII
            byte[] ASCII = Encoding.ASCII.GetBytes(check);
            //foreach loop to check each value 
            foreach (byte b in ASCII)
            {
                //condition to check and filter for special characters entered
                if (b < 31 || (b > 32 && b < 48) || (b > 75 && b < 65) || (b > 90 && b < 97) || b > 122)
                    flag = true;
            }

            return flag;
        }

        public Boolean checkNumber(string check)
        {

            Boolean flag = false;

            // convert each character of the string value in the textbox to ASCII
            byte[] ASCII = Encoding.ASCII.GetBytes(check);
            //foreach loop to check each value 
            foreach (byte b in ASCII)
            {
                //condition to check and filter for special characters entered
                if (b > 48 && b < 58)
                    flag = true;
            }

            return flag;
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

                if (!isNumeric(txtAmount.Text))
                {
                    MessageBox.Show("Please input a numeric value", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtAmount.Focus();
                    txtAmount.Clear();
                }
                //condition
                
                else
                {

                    double temp = double.Parse(txtAmount.Text);


                    if (temp > 100000000 || temp < 0)
                    {
                        MessageBox.Show("Invalid amount", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtAmount.Clear();
                        txtAmount.Focus();
                    }
                    else
                    {
                        string amount = "";
                        amount = NumberToWords(Convert.ToInt32(Math.Truncate(temp)));
                        int result = (int)((temp - (int)temp) * 100);
                        txtStrAmount.Text = amount + " and " + result + "/100" + " pesos only";
                    }
                }
               
            }

        }
        private Boolean isNumeric(string text)
        {
            Boolean flag = false;
            double num;

            flag =  Double.TryParse(text, out num);

            return flag;
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {


            PrintDialog printDlg = new PrintDialog();
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = "Print Document";
            printDlg.Document = printDoc;
            printDlg.AllowSelection = true;
            printDlg.AllowSomePages = true;
            //Call ShowDialog
            if (printDlg.ShowDialog() == DialogResult.OK)
                printDoc.Print();


        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();

            PrintDocument printDocument = new PrintDocument();

            
            printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 250, 110);

            printDialog.Document = printDocument; //add the document to the dialog box...        

            printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateCheque); //add an event handler that will do the printing

            //on a till you will not want to ask the user where to print but this is fine for the test envoironment.

            DialogResult result = printDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                printDocument.Print();

            }
        }
        private void CreateCheque(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            string name = txtName.Text;
            string amount = txtAmount.Text;
            string stramount = txtStrAmount.Text;
            string date = dateTimeCalender.Value.ToString("MM/dd/yyyy");

            Graphics graphic = e.Graphics;

            Font font = new Font("Courier New", 12); 

            float fontHeight = font.GetHeight();


            int startX = 10;
            int startY = 10;
            int offset = 40;

            graphic.DrawString(date, new Font("Courier New", 18), new SolidBrush(Color.Black), startX, startY);
            offset += 15;
            graphic.DrawString(name, font, new SolidBrush(Color.Black), startX, startY + offset);
            startX += 450;

            graphic.DrawString(amount, font, new SolidBrush(Color.Black), startX, startY + offset);
            offset += 15;
            graphic.DrawString(stramount, font, new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + (int)fontHeight; //make the spacing consistent
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 5; //make the spacing consistent
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtAmount.Clear();
            txtStrAmount.Clear();
        }
    }
}
