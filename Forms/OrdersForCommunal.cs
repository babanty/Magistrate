using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Magistrate.Forms
{
    public partial class OrdersForCommunal : Form
    {
        public OrdersForCommunal()
        {
            InitializeComponent();

            // добавление организаций
            Db.SetPropertiesComboBox(ref comboBox7, "komynalka"); // Место жителства дом
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GenerationWord.StandartListValueControl(Controls);


            // Заполнение второй даты, выделено отдельно т.к. иначе в word-е будут лишние точки
            // На дату / период / с - по
            string dateTwo = ""; // дата вторая 
            if (comboBox25.Text != "" && comboBox25.Text != null)
            {
                dateTwo = comboBox25.Text + "." + comboBox24.Text + "." + comboBox23.Text;
                dateTwo = " по " + dateTwo + " года";
            }
            GenerationWord.AddValueControl(ref controlArrayToString, dateTwo, "#-1"); // в ручную добавляем новый ключ


            // Сгенерировать ворд
            GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "Приказ по комуналке", controlArrayToString);
        }

        // Автоматическое выставление судья или И.о участка
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == PropertiesMyApp.GetPropertiesValue(TypeProperties.PlaceNum))
            {
                comboBox5.Text = "Мировой судья";
            }
            else
            {
                comboBox5.Text = "И.о. мирового судьи";
            }
        }

        
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            // Ограничение по количеству должников
            if (numericUpDown3.Value < 1)
            {
                MessageBox.Show("Количество должников не может быть меньше 1");
                numericUpDown3.Value = 1;
            }
            // Ограничение по количеству должников
            if (numericUpDown3.Value > 5)
            {
                MessageBox.Show("Максимальное количество должников 5. Если их больше, то добавлять их надо " +
                    "в ручную в сгенерированном word-е");
                numericUpDown3.Value = 5;
            }



        }

        /// <summary>
        /// Возвращает контрол по его таб индексу
        /// </summary>
        /// <param name="controls">коллекция контролов</param>
        /// <param name="tabIndex">номер таб индекса</param>
        /// <returns></returns>
        private Control GetControlAtTabIndex(Control.ControlCollection controls, int tabIndex)
        {
            foreach(Control control in controls)
            {
                if (control.TabIndex == tabIndex)
                    return control;
            }

            return null;
        }

        // Фамилия автоперенос
        private void d1textBox1_TextChanged(object sender, EventArgs e)
        {
            d2textBox1.Text = d1textBox1.Text;
            d3textBox1.Text = d1textBox1.Text;
            d4textBox1.Text = d1textBox1.Text;
            d5textBox1.Text = d1textBox1.Text;
        }

        // Имя автоперенос
        private void d1textBox2_TextChanged(object sender, EventArgs e)
        {
            d2textBox2.Text = d1textBox2.Text;
            d3textBox2.Text = d1textBox2.Text;
            d4textBox2.Text = d1textBox2.Text;
            d5textBox2.Text = d1textBox2.Text;
        }

        // Отчество автоперенос
        private void d1textBox3_TextChanged(object sender, EventArgs e)
        {
            d2textBox3.Text = d1textBox3.Text;
            d3textBox3.Text = d1textBox3.Text;
            d4textBox3.Text = d1textBox3.Text;
            d5textBox3.Text = d1textBox3.Text;
        }

        // Место рождения страна автоперенос
        private void d1comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            d2comboBox4.Text = d1comboBox4.Text;
            d3comboBox4.Text = d1comboBox4.Text;
            d4comboBox4.Text = d1comboBox4.Text;
            d5comboBox4.Text = d1comboBox4.Text;
        }

        // Место рождения населенный пункт автоперенос
        private void d1comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            d2comboBox5.Text = d1comboBox5.Text;
            d3comboBox5.Text = d1comboBox5.Text;
            d4comboBox5.Text = d1comboBox5.Text;
            d5comboBox5.Text = d1comboBox5.Text;
        }

        // Зарегестрирован страна автоперенос
        private void d1comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            d2comboBox6.Text = d1comboBox6.Text;
            d3comboBox6.Text = d1comboBox6.Text;
            d4comboBox6.Text = d1comboBox6.Text;
            d5comboBox6.Text = d1comboBox6.Text;
        }

        // Зарегестрирован область автоперенос
        private void d1comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            d2comboBox7.Text = d1comboBox7.Text;
            d3comboBox7.Text = d1comboBox7.Text;
            d4comboBox7.Text = d1comboBox7.Text;
            d5comboBox7.Text = d1comboBox7.Text;
        }

        // Зарегестрирован город автоперенос
        private void d1comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            d2comboBox8.Text = d1comboBox8.Text;
            d3comboBox8.Text = d1comboBox8.Text;
            d4comboBox8.Text = d1comboBox8.Text;
            d5comboBox8.Text = d1comboBox8.Text;
        }

        // Зарегестрирован улица автоперенос
        private void d1comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            d2comboBox9.Text = d1comboBox9.Text;
            d3comboBox9.Text = d1comboBox9.Text;
            d4comboBox9.Text = d1comboBox9.Text;
            d5comboBox9.Text = d1comboBox9.Text;
        }

        // Зарегестрирован дом автоперенос
        private void d1comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            d2comboBox10.Text = d1comboBox10.Text;
            d3comboBox10.Text = d1comboBox10.Text;
            d4comboBox10.Text = d1comboBox10.Text;
            d5comboBox10.Text = d1comboBox10.Text;
        }
    }
}
