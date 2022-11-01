using AngleSharp;
using AspxToCode.Parser;

namespace AspxToCode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            var source = txtSource.Text;

            var code = Html.GetCode(source);
        }
    }
}