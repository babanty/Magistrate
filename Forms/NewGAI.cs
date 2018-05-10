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
    public partial class NewGAI : Form
    {
        public const string NameColumn = "Details of GAI"; // название поля для ввода с вариантами

        public NewGAI()
        {
            InitializeComponent();
            RestartVariants();
        }

        // добавить
        private void button2_Click(object sender, EventArgs e)
        {
            string text = "";
            text += textBox2.Text + "$";
            text += textBox1.Text + "$";
            text += textBox3.Text;


            Db.SetValueInColumn(text, NameColumn);
            
            // обнуляем поля для ввода
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            RestartVariants(); // Перезаполняем лист с вариантами

            //"вася/петя/саша".Split('/');
            //string[] {"вася", "петя", "саша"}
        }



        // удалить
        private void button1_Click(object sender, EventArgs e)
        {
            Db.DeleteValueInColumn(listBox1.Text, NameColumn);
            RestartVariants(); // Перезаполняем лист с вариантами
        }

        // Перезаполняем лист с вариантами
        private void RestartVariants()
        {
            // Заполняем вариантами внутри полей лист бокс
            List<string> array = Db.GetColumn(NameColumn); // возвращает список вариантов записанных в поле для ввода
            listBox1.Items.Clear(); // обнуляет лист бокс
            listBox1.Items.AddRange(Db.GetAllShortRequisitesGAI().ToArray()); // закидываем варианты в листбокс
        }

    }
}
