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

        private void button4_Click(object sender, EventArgs e)
        {
            Article1226p1_NotAppeared form2 = new Article1226p1_NotAppeared(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }

        private void button4_Click_1(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            CreditOrder form2 = new CreditOrder(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            CasTaxes form2 = new CasTaxes(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }

        // Настройки
        private void button6_Click(object sender, EventArgs e)
        {
            PropertiesApp form2 = new PropertiesApp(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }

        // 15.33 СЗВ-М
        private void button7_Click(object sender, EventArgs e)
        {
            Article1533 form2 = new Article1533(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }

        // Отмена приказа по ГПК
        private void button8_Click(object sender, EventArgs e)
        {
            CancellationOrderGPK form2 = new CancellationOrderGPK(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }
    }
}
