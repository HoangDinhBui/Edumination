using Edumination.WinForms.UI.Admin.TestManager;
using IELTS.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.UI.Admin.TestManager
{
    public partial class AddSectionButtonControl : UserControl
    {
        private int TestPaperId;
        public int GetTestPaperId() { return TestPaperId; }
        public void SetTestPaperId( int id) { TestPaperId = id; }
        private  TestManagerControl _testManagerControl;

        private AddReadingSectionControl _addReadingSectionControl;
        public AddReadingSectionControl AddReadingSectionControl
        {
            get => _addReadingSectionControl;
            set => _addReadingSectionControl = value;
        }
        public TestManagerControl TestManagerControl
        {
            get => _testManagerControl;
            set => _testManagerControl = value;
        } 

        private string _pdfFilePath;

        public string PdfFilePath
        {
            get { return _pdfFilePath; }
            set { _pdfFilePath = value; }
        }

        private string _pdfFileName;
         
        public string PdfFileName
        {
            get { return _pdfFileName; }
            set { _pdfFileName = value; }
        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        private string  _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public AddSectionButtonControl()
        {
            InitializeComponent();
        }

        public AddSectionButtonControl(TestManagerControl testManagerControl)
        {
            InitializeComponent();
            _testManagerControl = testManagerControl;
        }

        public AddSectionButtonControl(string pdfFilePath, string pdfFileName, string title, string description)
        {
            InitializeComponent();
            _addReadingSectionControl = new AddReadingSectionControl();
            _pdfFilePath = pdfFilePath;
            _pdfFileName = pdfFileName;
            _title = title;
            _description = description;
        }
        private void btnAddReadingSection_Click(object sender, EventArgs e)
        {
            AddTestSectionForm addTestSectionForm = new AddTestSectionForm(TestPaperId);
            addTestSectionForm.ShowDialog();
        }
    }
}
