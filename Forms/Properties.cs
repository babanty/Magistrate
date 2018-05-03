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
    public partial class PropertiesApp : Form
    {

        // Конструктор с инициализацией формы
        public PropertiesApp()
        {
            InitializeComponent();

            // Заполняем строку с полями названием полей
            try
            {
                comboBox1.Items.AddRange(Db.GetColumnsName());
            }
            catch
            {
                MessageBox.Show("Проблема при открытии окна настроек, вероятно нет файла базы данных db.ini");
            }

            // заполняем значением поле с основным участком
            comboBox2.Text = PropertiesMyApp.GetPropertiesValue(TypeProperties.PlaceNum, "228");
        }

        // Удалить
        private void button1_Click(object sender, EventArgs e)
        {
            Db.DeleteValueInColumn(listBox1.Text, comboBox1.Text);
        }

        // Добавить
        private void button2_Click(object sender, EventArgs e)
        {
            Db.SetValueInColumn(textBox1.Text, comboBox1.Text);
            comboBox1.Text = "";
            comboBox1_SelectedIndexChanged(null, null);
        }

        // Сохранить
        private void button3_Click(object sender, EventArgs e)
        {
            PropertiesMyApp.SetPropertiesValue(comboBox2.Text, TypeProperties.PlaceNum); // Сохранения основного участка судьи
        }

        /// <summary>
        /// Заполняем лист бокс вариантами внутри полей
        /// </summary>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Заполняем вариантами внутри полей лист бокс
            string nameColumn = comboBox1.Text; // название поля для ввода с вариантами
            List<string> array = Db.GetColumn(nameColumn); // возвращает список вариантов записанных в поле для ввода
            listBox1.Items.AddRange(array.ToArray()); // закидываем варианты в листбокс
        }
    }
}
