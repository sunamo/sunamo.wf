namespace F.WF
{
    // Nemuze byt genericky protoze bych to musel dat jak tu tak do Designer
    public partial class CHLBForm : Form
    {


        public CHLBForm(string label, params object[] items)
        {
            InitializeComponent();


            label1.Text = label;
            checkedListBox1.Items.AddRange(items);
        }

        public int CheckedCount
        {
            get
            {
                return checkedListBox1.CheckedIndices.Count;
            }
        }
    }
}
