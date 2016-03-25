using System;
using System.Media;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Plugin.Utils;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class TextDialog : Form
    {
        private readonly Func<string, bool> checker;
        private readonly string defaultVal;

        public TextDialog(string title,
                          string dialog,
                          string defaultVal,
                          Func<string, bool> checker,
                          string okText = "OK",
                          string cancelText = "CANCEL")
        {
            InitializeComponent();

            Text = title;
            label_input.Text = dialog;

            button_ok.Text = okText;
            button_cancel.Text = cancelText;

            this.checker = checker;
            this.defaultVal = defaultVal;
            textBox_input.Text = defaultVal;
            textBox_input.KeyPress += TextBoxInputOnKeyPress;
            VisibleChanged += OnVisibleChanged;
            FormClosing += OnFormClosing;
        }

        public string Input => textBox_input.Text;

        private void TextBoxInputOnKeyPress(object sender, KeyPressEventArgs keyPressEventArgs)
        {
            if (keyPressEventArgs.KeyChar != '\n' || ValidateInput())
                return;
            keyPressEventArgs.Handled = true;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                Debugger.WriteLine("Validation failed");
                return;
            }
            Debugger.WriteLine("Validation OK!");
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            Debugger.WriteLine(LogLevel.Info, $"Closing prompt because: {EnumHelper.GetName(e.CloseReason)}");
            if (DialogResult != DialogResult.OK)
                DialogResult = DialogResult.Cancel;
            e.Cancel = false;
        }

        private bool ValidateInput()
        {
            if (checker(textBox_input.Text))
                return true;
            textBox_input.Text = defaultVal;
            textBox_input.Select(0, defaultVal.Length);
            SystemSounds.Hand.Play();
            return false;
        }

        private void OnVisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                textBox_input.Focus();
        }
    }
}