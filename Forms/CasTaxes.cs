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
    public partial class CasTaxes : Form
    {
        public CasTaxes()
        {
            InitializeComponent();

            Db.SetPropertiesComboBox(ref comboBox10, "mesto rogdeniya"); // Заполняем Населенный пункт, место рождения
            Db.SetPropertiesComboBox(ref comboBox13, "mesto jitelstva gorod"); // Место жителства населенный пункт
            Db.SetPropertiesComboBox(ref comboBox14, "mesto jitelstva ylitsa"); // Место жителства улица
            Db.SetPropertiesComboBox(ref comboBox15, "mesto jitelstva dom"); // Место жителства дом

            SaveLoadForm.SetVariantsSaveInComboBox(nameForm, ref comboBoxLoad);// заполнение вариантами сохранений

            // Автозаполнение 
            // Заполнение даты вынесения решения текущими датами
            DateTime dateTimeNow = DateTime.Now;
            string month = GenerationWord.MonthInString(dateTimeNow.Month); // месяц
            string year = dateTimeNow.Year.ToString(); // год
            comboBox3.Text = month;
            comboBox4.Text = year;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GenerationWord.StandartListValueControl(Controls);


            // Если ИНН не равен 12 символам
            if (textBox7.Text != null && textBox7.Text.Length != 12) 
            {
                MessageBox.Show("ИНН должен состоять из 12 символов");
                return;
            }


            // Если не чекнули ни один налог
            if(checkBox1.Checked == false && checkBox2.Checked == false && checkBox3.Checked == false)
            {
                MessageBox.Show("Не поставили галку не над одиним налогом");
                return;
            }


            // Обработка налогов, составление строки
            string taxesString = "";
            if (checkBox1.Checked) // Транспортный налог
            {
                taxesString += checkBox1.Text; // указываем налог
                taxesString += " за " + comboBox26.Text + " год"; // добавляем год
                taxesString += " в сумме " + GenerationWord.IntInRubAndCop(numericUpDown1.Value); // сумму налога переводим в 0 руб. 0 коп
                taxesString += ", пени в сумме " + GenerationWord.IntInRubAndCop(numericUpDown2.Value) + ", "; // сумму пени переводим в 0 руб. 0 коп
            }
            if (checkBox2.Checked) // Транспортный налог
            {
                taxesString += checkBox2.Text; // указываем налог
                taxesString += " за " + comboBox17.Text + " год"; // добавляем год
                taxesString += " в сумме " + GenerationWord.IntInRubAndCop(numericUpDown3.Value); // сумму налога переводим в 0 руб. 0 коп
                taxesString += ", пени в сумме " + GenerationWord.IntInRubAndCop(numericUpDown4.Value) + ", "; // сумму пени переводим в 0 руб. 0 коп
            }
            if (checkBox3.Checked) // Транспортный налог
            {
                taxesString += checkBox3.Text; // указываем налог
                taxesString += " за " + comboBox18.Text + " год"; // добавляем год
                taxesString += " в сумме " + GenerationWord.IntInRubAndCop(numericUpDown5.Value); // сумму налога переводим в 0 руб. 0 коп
                taxesString += ", пени в сумме " + GenerationWord.IntInRubAndCop(numericUpDown6.Value) + ", "; // сумму пени переводим в 0 руб. 0 коп
            }
            GenerationWord.AddValueControl(ref controlArrayToString, taxesString, "#-1"); // в ручную добавляем новый ключ


            // Подсчет суммы всех налогов
            decimal taxesSum = 0m; // Сумма всех налогов
            try
            {
                if (checkBox1.Checked)
                    taxesSum += numericUpDown1.Value + numericUpDown2.Value;
                if (checkBox2.Checked)
                    taxesSum += numericUpDown3.Value + numericUpDown4.Value;
                if (checkBox3.Checked)
                    taxesSum += numericUpDown5.Value + numericUpDown6.Value;
            }
            catch
            {
                MessageBox.Show("Все поля с цифрами задолженности и пени должны быть заполнены хотя бы нулем");
                return;
            }
            string taxesSumToString = GenerationWord.IntInRubAndCop(taxesSum); // сумму налога переводим в 0 руб. 0 коп
            taxesSumToString = "всего " + taxesSumToString;
            GenerationWord.AddValueControl(ref controlArrayToString, taxesSumToString, "#-2"); // в ручную добавляем новый ключ


            //Считаем гос.пошлину
            decimal duty = 0m;
            string dutyToString = "";
            if(taxesSum <= 20000)
            {
                duty = taxesSum / 100 * 2;
                if (duty < 200)
                    duty = 200;
            }
            else if(taxesSum > 20000 && taxesSum <= 100000)
            {
                duty = (800 + (taxesSum - 20000) / 100 * 3) / 2;
            }
            else if (taxesSum > 100000 && taxesSum <= 200000)
            {
                duty = (3200 + (taxesSum - 100000) / 100 * 2) / 2;
            }
            else if (taxesSum > 200000 && taxesSum <= 1000000)
            {
                duty = (5200 + (taxesSum - 200000) / 100) / 2;
            }
            else if (taxesSum > 1000000)
            {
                duty = (13200 + (taxesSum - 1000000) / 100 / 2) / 2;
                if (duty > 60000)
                    duty = 60000;
            }
            else
            {
                MessageBox.Show("Ну удалось посчитать госпошлину из-за некорректных данных");
                return;
            }
            dutyToString = GenerationWord.IntInRubAndCop(Math.Round(duty, 2)); // сумму пошлины переводим в 0 руб. 0 коп
            GenerationWord.AddValueControl(ref controlArrayToString, dutyToString, "#-3"); // в ручную добавляем новый ключ


            // Вставляем название в буфер обмена
            Clipboard.SetText(textBoxClipPutNum.Text + "  " + textBoxClipPutName.Text + "  " + this.Text);


            // Сгенерировать ворд
            GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "Взыскание налога", controlArrayToString);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox6.Text = textBox1.Text;

            // название при сохранении
            textBoxClipPutName.Text = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox5.Text = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox4.Text = textBox3.Text;
        }

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

        #region Сохранение
        // Сохранить заполненные поля
        string nameForm = "CasTaxes";
        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GenerationWord.StandartListValueControl(Controls);

            string nameSave = nameForm + "$" + textBoxForSave.Text; // имя сохранения
            SaveLoadForm.SaveForm(nameSave, controlArrayToString); // сохранить

            // перезаполняем варианты сохранений
            comboBoxLoad.Items.Clear(); // стираем текущие варианты
            SaveLoadForm.SetVariantsSaveInComboBox(nameForm, ref comboBoxLoad);// заполнение вариантами сохранений
        }
        // Загрузить сохраненные поля
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            var controls = SaveLoadForm.LoadForm(nameForm, comboBoxLoad.Text, Controls); // получаем заполненные сейвом контролы

            // Переносим текст массива заполненных контролов в контролы этой формы. Иначе ни как, т.к. Controls {get;}
            for (int i = 0; i < controls.Count; i++)
            {
                Controls[i].Text = controls[i].Text;
            }
        }
        // Удалить сохранение
        private void buttonDeleteSave_Click(object sender, EventArgs e)
        {
            SaveLoadForm.DeleteSave(comboBoxLoad.Text, nameForm); // Удаляем сохранение

            // перезаполняем варианты сохранений
            comboBoxLoad.Items.Clear(); // стираем текущие варианты
            comboBoxLoad.Text = ""; // стираем текущие варианты
            SaveLoadForm.SetVariantsSaveInComboBox(nameForm, ref comboBoxLoad);// заполнение вариантами сохранений
        }
        #endregion Сохранение

    }
}
