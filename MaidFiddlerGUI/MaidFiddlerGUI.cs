using System;
using System.Drawing;
using System.Windows.Forms;

namespace MaidFiddlerGUI
{
    public partial class MaidFiddlerGUI : Form
    {
        public MaidFiddlerGUI()
        {
            InitializeComponent();

            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.DrawItem += listBox1_DrawItem;
            listBox1.BeginUpdate();
            for (int i = 0; i < 20; i++)
            {
                listBox1.Items.Add(i.ToString());
            }
            listBox1.EndUpdate();
        }

        private void Form1_Load(object sender, EventArgs e) {}

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            e.Graphics.FillRectangle(Brushes.BlueViolet, e.Bounds.X, e.Bounds.Y, e.Bounds.Height, e.Bounds.Height);
            e.Graphics.DrawString(
            (string) listBox1.Items[e.Index],
            e.Font,
            Brushes.Black,
            e.Bounds,
            StringFormat.GenericDefault);

            e.DrawFocusRectangle();
        }
    }
}