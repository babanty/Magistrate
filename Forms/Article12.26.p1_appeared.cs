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
    public partial class Article1226p1_appeared : Form
    {
        public Article1226p1_appeared()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GenerationWord.StandartListValueControl(Controls);

            // На форме есть связанные чекбоксы, забиваю их значения в одну строку
            string checkBoxValue = "";
            foreach (Control control in Controls)
            { // Если чекбокс с галкой и его индекс от 38 до 45
                if(control is CheckBox && (control as CheckBox).Checked && 
                    (control as CheckBox).TabIndex > 38 && (control as CheckBox).TabIndex < 45)
                {
                    checkBoxValue += control.Text + ", "; // Взять его текст и добавить к строке с запятой и пробелом
                }
            }
            if(checkBoxValue.Length > 3)
                checkBoxValue = checkBoxValue.Remove(checkBoxValue.Length - 2); // Удалем последние два лишних символа ", "
            GenerationWord.AddValueControl(ref controlArrayToString, checkBoxValue, "#38"); // в ручную добавляем новый ключ

            // Делаем инициалы ФИО
            string initials = "";
            if (textBox2.Text.Length > 2 && textBox3.Text.Length > 2) // Если правильно заполнили имя и отчество
                initials = textBox2.Text.Remove(1) + "." + textBox3.Text.Remove(1) + ".";
            GenerationWord.AddValueControl(ref controlArrayToString, initials, "#-01"); // в ручную добавляем новый ключ



            // Сгенерировать ворд
            GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "ст.12.26 ч.1 явился", controlArrayToString);
        }
    }
}
