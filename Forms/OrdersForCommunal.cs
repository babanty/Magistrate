﻿using System;
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

            // Находим полные реквизиты по комуналке
            string Requisites = GetRec(comboBox7.Text); // находим полные реквизиты банка
            if (Requisites == null) // банк не опознан
            {
                MessageBox.Show("Реквизиты организации не опознаны, после герации не забудьте их вписать");
                Requisites = "";
            }
            GenerationWord.AddValueControl(ref controlArrayToString, Requisites, "#-1"); // в ручную добавляем новый ключ


            // должника / должников
            string debtor = "";
            if(numericUpDown3.Value == 1)
            {
                debtor = "должника";
            }
            else
            {
                debtor = "должников";
            }
            GenerationWord.AddValueControl(ref controlArrayToString, debtor, "#-2"); // в ручную добавляем новый ключ


            // солидарно или нет
            string severally = "";
            if (numericUpDown3.Value != 1)
            {
                severally = "солидарно";
            }
            GenerationWord.AddValueControl(ref controlArrayToString, severally, "#-2"); // в ручную добавляем новый ключ


            // Заполнение второй даты, выделено отдельно т.к. иначе в word-е будут лишние точки
            // На дату / период / с - по
            string dateTwo = ""; // дата вторая 
            if (comboBox25.Text != "" && comboBox25.Text != null)
            {
                dateTwo = comboBox25.Text + "." + comboBox24.Text + "." + comboBox23.Text;
                dateTwo = " по " + dateTwo + " года";
            }
            GenerationWord.AddValueControl(ref controlArrayToString, dateTwo, "#-4"); // в ручную добавляем новый ключ

            // С кого взимать задолженность, только ФИО все вместе
            string fullNames = "";
            if (numericUpDown3.Value > 0) // добавляем по одному человеку
                fullNames += d1textBox1 + " " + d1textBox2 + " " + d1textBox3;
            if (numericUpDown3.Value > 1) // добавляем по одному человеку
                fullNames += ", " + d2textBox1 + " " + d2textBox2 + " " + d2textBox3;
            if (numericUpDown3.Value > 2) // добавляем по одному человеку
                fullNames += ", " + d3textBox1 + " " + d3textBox2 + " " + d3textBox3;
            if (numericUpDown3.Value > 3) // добавляем по одному человеку
                fullNames += ", " + d4textBox1 + " " + d4textBox2 + " " + d4textBox3;
            if (numericUpDown3.Value > 4) // добавляем по одному человеку
                fullNames += ", " + d5textBox1 + " " + d5textBox2 + " " + d5textBox3;
            GenerationWord.AddValueControl(ref controlArrayToString, fullNames, "#-5"); // в ручную добавляем новый ключ


            // С кого взимать задолженность, полные реквизиты все вместе
            string allDetailsDebtors = "";
            if (numericUpDown3.Value > 0) // добавляем по одному человеку
                fullNames += d1textBox1 + " " + d1textBox2 + " " + d1textBox3
                    + ", дата рождения: " + d1comboBox1 + "." + d1comboBox2 + "." + d1comboBox3 + "г.р."
                    + ", место рождения: " + d1comboBox4 + " " + d1comboBox5
                    + ", место регистрации: " + d1comboBox6 + ", " + d1comboBox7 + ", " + d1comboBox8
                     + ", " + d1comboBox9 + ", " + d1comboBox10;
            if (numericUpDown3.Value > 1) // добавляем по одному человеку
                fullNames += ", " + d2textBox1 + " " + d2textBox2 + " " + d2textBox3
                    + ", дата рождения: " + d2comboBox1 + "." + d2comboBox2 + "." + d2comboBox3 + "г.р."
                    + ", место рождения: " + d2comboBox4 + " " + d2comboBox5
                    + ", место регистрации: " + d2comboBox6 + ", " + d2comboBox7 + ", " + d2comboBox8
                     + ", " + d2comboBox9 + ", " + d2comboBox10;
            if (numericUpDown3.Value > 2) // добавляем по одному человеку
                fullNames += ", " + d3textBox1 + " " + d3textBox2 + " " + d3textBox3
                    + ", дата рождения: " + d3comboBox1 + "." + d3comboBox2 + "." + d3comboBox3 + "г.р."
                    + ", место рождения: " + d3comboBox4 + " " + d3comboBox5
                    + ", место регистрации: " + d3comboBox6 + ", " + d3comboBox7 + ", " + d3comboBox8
                     + ", " + d3comboBox9 + ", " + d3comboBox10;
            if (numericUpDown3.Value > 3) // добавляем по одному человеку
                fullNames += ", " + d4textBox1 + " " + d4textBox2 + " " + d4textBox3
                    + ", дата рождения: " + d4comboBox1 + "." + d4comboBox2 + "." + d4comboBox3 + "г.р."
                    + ", место рождения: " + d4comboBox4 + " " + d4comboBox5
                    + ", место регистрации: " + d4comboBox6 + ", " + d4comboBox7 + ", " + d4comboBox8
                     + ", " + d4comboBox9 + ", " + d4comboBox10;
            if (numericUpDown3.Value > 4) // добавляем по одному человеку
                fullNames += ", " + d5textBox1 + " " + d5textBox2 + " " + d5textBox3
                    + ", дата рождения: " + d5comboBox1 + "." + d5comboBox2 + "." + d5comboBox3 + "г.р."
                    + ", место рождения: " + d5comboBox4 + " " + d5comboBox5
                    + ", место регистрации: " + d5comboBox6 + ", " + d5comboBox7 + ", " + d5comboBox8
                     + ", " + d5comboBox9 + ", " + d5comboBox10;
            GenerationWord.AddValueControl(ref controlArrayToString, allDetailsDebtors, "#-6"); // в ручную добавляем новый ключ


            // В сумме руб коп
#-7

            // Пени руб коп
#-8

            // Всего руб коп
#-9

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
                return;
            }
            // Ограничение по количеству должников
            if (numericUpDown3.Value > 5)
            {
                MessageBox.Show("Максимальное количество должников 5. Если их больше, то добавлять их надо " +
                    "в ручную в сгенерированном word-е");
                numericUpDown3.Value = 5;
                return;
            }

            // делаем заблоченными всех людей
            for (int i = 33; i < 89; i++)
            {
                Control getControl = GetControlAtTabIndex(Controls, i);
                if (getControl != null)
                    getControl.Enabled = false;
            }

            // делаем разблоченными необходимых людей
            for (int i = 33; i < 33 + 14 * ((int)numericUpDown3.Value - 1); i++)
            {
                Control getControl = GetControlAtTabIndex(Controls, i);
                if (getControl != null)
                    getControl.Enabled = true;
            }

        }

        #region автопереносы
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
        #endregion

        /// <summary>
        /// Возвращает контрол по его таб индексу
        /// </summary>
        /// <param name="controls">коллекция контролов</param>
        /// <param name="tabIndex">номер таб индекса</param>
        /// <returns></returns>
        private Control GetControlAtTabIndex(Control.ControlCollection controls, int tabIndex)
        {
            foreach (Control control in controls)
            {
                if (control.TabIndex == tabIndex)
                    return control;
            }

            return null;
        }

        /// <summary>
        /// Возвращает полные реквизиты организации по названию или null
        /// </summary>
        /// <param name="Name">Название организации</param>
        private string GetRec(string Name)
        { // потом сделать динамчным вытгиванием из БД 
            int nameLength = Name.Length; // Количество символов в названии банка
            string result = null;


            // Проверяем сходится ли название с полными реквизитами
            List<string> allRec = Db.GetColumn("komynalka complete");
            if (allRec == null)
                return null;

            foreach (string bank in allRec)
            {
                if (bank.Substring(0, nameLength) == Name) // первые слова в полных реквизитах соотвествуют 
                    return bank;
            }

            // Возвращаем результат null
            return result;
        }

    }
}