using GBX.NET;
using GBX.NET.Managers;
using System.Globalization;

namespace ClassInfoFromId
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void TextBoxClassId_TextChanged(object sender, EventArgs e)
        {
            if (!uint.TryParse(TextBoxClassId.Text, out var classId))
            {
                LabelClassName.Text = "Invalid class id";
                return;
            }

            try
            {
                LabelClassName.Text = ClassManager.GetName(classId & 0xFFFFF000, all: true);
            }
            catch
            {
                LabelClassName.Text = "";
            }

            if (LabelClassName.Text != "")
            {
                return;
            }

            if (!uint.TryParse(TextBoxClassId.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out classId))
            {
                LabelClassName.Text = "Invalid class id";
                return;
            }

            try
            {
                LabelClassName.Text = ClassManager.GetName(classId & 0xFFFFF000, all: true);
            }
            catch
            {
                LabelClassName.Text = "";
            }

            if (LabelClassName.Text != "")
            {
                return;
            }

            LabelClassName.Text = "Invalid class id";
        }

        private void ButtonCopyName_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(LabelClassName.Text);
        }
    }
}
