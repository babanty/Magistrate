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

        private void button3_Click(object sender, EventArgs e)
        {
            Article1281notAppear form2 = new Article1281notAppear(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Article1226p1_appeared form2 = new Article1226p1_appeared(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }
    }
}
