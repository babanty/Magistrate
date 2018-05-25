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
        // Конструктор формы
        public Magistrate()
        {
            InitializeComponent();
        }


        // Настройки
        private void button6_Click(object sender, EventArgs e)
        {
            PropertiesApp form2 = new PropertiesApp(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }


        // Приказы по банкам и фин.орг.
        private void button5_Click(object sender, EventArgs e)
        {
            CreditOrder form2 = new CreditOrder(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }


        // КАС Налоги
        private void button4_Click_2(object sender, EventArgs e)
        {
            CasTaxes form2 = new CasTaxes(); // Создание соовтественующего окна
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


        // Отмена приказа по КАС
        private void button9_Click(object sender, EventArgs e)
        {
            CancellationOrderKAS form2 = new CancellationOrderKAS(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }


        // Приказы по комуналке
        private void button10_Click(object sender, EventArgs e)
        {
            OrdersForCommunal form2 = new OrdersForCommunal(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }


        // СТ. 12.15 (сплошная линия)
        private void button11_Click(object sender, EventArgs e)
        {
            Article1215 form2 = new Article1215(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }


        // СТ. 20.25 (неуплата штрафа)
        private void button12_Click(object sender, EventArgs e)
        {
            Article2025 form2 = new Article2025(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }


        // Возврат госпошлины
        private void button13_Click(object sender, EventArgs e)
        {
            ReturnDuty form2 = new ReturnDuty(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }


        // По подсудности
        private void button14_Click(object sender, EventArgs e)
        {
            ByJurisdiction form2 = new ByJurisdiction(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }
    }
}
