using System.Windows.Forms;

namespace Edumination.WinForms.UI.Forms.TestLibrary
{
    public partial class MockTestContainerPanel : UserControl
    {
        public MockTestContainerPanel()
        {
            InitializeComponent();
        }

        // Add item manually or via API later
        public void AddItem(string title, string taken)
        {
            var item = new MockTestItemPanel
            {
                Title = title,
                Taken = taken
            };
            panelItems.Controls.Add(item);
        }
    }
}
