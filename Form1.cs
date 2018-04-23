using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Magistrate.Forms;

namespace Magistrate
{
    public partial class Magistrate : Form
    {
        public Magistrate()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Article1281appeared form2 = new Article1281appeared(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
