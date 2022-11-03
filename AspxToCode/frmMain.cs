using AngleSharp;
using AspxToCode.Parser;

namespace AspxToCode
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            var source = txtSource.Text;

            var code = Html.ParseHtml(source);

            txtDest.Text = string.Join(Environment.NewLine, code);
        }
    }
}