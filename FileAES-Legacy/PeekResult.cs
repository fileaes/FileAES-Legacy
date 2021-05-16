using System.Windows.Forms;

namespace FileAES
{
    public partial class PeekResult : Form
    {
        public PeekResult(string peekText)
        {
            InitializeComponent();

            resultTextBox.Text = peekText;
            resultTextBox.SelectionStart = resultTextBox.Text.Length;
            resultTextBox.ScrollToCaret();
        }
    }
}
