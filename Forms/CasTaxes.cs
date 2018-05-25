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

        #region Инициализация

        public CasTaxes()
        {
            InitializeComponent();

            Db.SetPropertiesComboBox(ref comboBoxPlaceOfBirth, NamePropertiesForComboBox.МестоРождения); // Заполняем Населенный пункт, место рождения
            Db.SetPropertiesComboBox(ref comboBoxResidenceCity, NamePropertiesForComboBox.МестоЖительстваГород); // Место жителства населенный пункт
            Db.SetPropertiesComboBox(ref comboBoxResidenceStreet, NamePropertiesForComboBox.МестоЖительстваУлица); // Место жителства улица
            Db.SetPropertiesComboBox(ref comboBoxResidenceHouse, NamePropertiesForComboBox.МестоЖительстваДом); // Место жителства дом

            SaveLoadForm.SetVariantsSaveInComboBox(nameForm, ref comboBoxLoad);// заполнение вариантами сохранений

            // Автозаполнение 
            // Заполнение даты вынесения решения текущими датами
            DateTime dateTimeNow = DateTime.Now;
            string month = HandlerTextControls.MonthInString(dateTimeNow.Month); // месяц
            string year = dateTimeNow.Year.ToString(); // год
            comboBoxDateOfOrderMonth.Text = month;
            comboBoxDateOfOrderYear.Text = year;
        }
        #endregion Инициализация


        #region Автоматическое заполнение полей

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBoxFullNameSurNameIvanovu.Text = textBoxFullNameSurNameIvanovoy.Text;

            // название при сохранении
            textBoxClipPutName.Text = textBoxFullNameSurNameIvanovoy.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBoxFullNameNameIvanu.Text = textBoxFullNameNameMarii.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBoxFullNamePatronymicIvanovichu.Text = textBoxFullNamePatronymicIvanovni.Text;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPlotNumber.Text == PropertiesMyApp.GetPropertiesValue(TypeProperties.PlaceNum))
            {
                comboBoxWhoIsJudge.Text = "Мировой судья";
            }
            else
            {
                comboBoxWhoIsJudge.Text = "И.о. мирового судьи";
            }
        }

        #region Разблокировка возможности писать сумму задолженности и пени
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTransportTax.Checked)
            {
                comboBoxTransportTaxYear.Enabled = true;
                numericUpDownTransportTax.Enabled = true;
                numericUpDownTransportTaxFine.Enabled = true;
            }
            else
            {
                comboBoxTransportTaxYear.Enabled = false;
                numericUpDownTransportTax.Enabled = false;
                numericUpDownTransportTaxFine.Enabled = false;
            }

        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLandTax.Checked)
            {
                comboBoxLandTaxYear.Enabled = true;
                numericUpDownLandTax.Enabled = true;
                numericUpDownLandTaxFine.Enabled = true;
            }
            else
            {
                comboBoxLandTaxYear.Enabled = false;
                numericUpDownLandTax.Enabled = false;
                numericUpDownLandTaxFine.Enabled = false;
            }
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPropertyTax.Checked)
            {
                comboBoxPropertyTaxYear.Enabled = true;
                numericUpDownPropertyTax.Enabled = true;
                numericUpDownPropertyTaxFine.Enabled = true;
            }
            else
            {
                comboBoxPropertyTaxYear.Enabled = false;
                numericUpDownPropertyTax.Enabled = false;
                numericUpDownPropertyTaxFine.Enabled = false;
            }
        }
        #endregion Разблокировка возможности писать сумму задолженности и пени
        
        #endregion Автоматическое заполнение полей


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

        
        // СГЕНЕРИРОВАТЬ WORD
        private void button1_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GenerationWord.StandartListValueControl(Controls);


            // Если ИНН не равен 12 символам
            if (textBoxINN.Text != null && textBoxINN.Text.Length != 12)
            {
                MessageBox.Show("ИНН должен состоять из 12 символов");
                return;
            }


            // Если не чекнули ни один налог
            if (checkBoxTransportTax.Checked == false && checkBoxLandTax.Checked == false && checkBoxPropertyTax.Checked == false)
            {
                MessageBox.Show("Не поставили галку не над одиним налогом");
                return;
            }


            // Обработка налогов, составление строки
            string taxesString = "";
            if (checkBoxTransportTax.Checked) // Транспортный налог
            {
                taxesString += checkBoxTransportTax.Text; // указываем налог
                taxesString += " за " + comboBoxTransportTaxYear.Text + " год"; // добавляем год
                taxesString += " в сумме " + HandlerTextControls.IntInRubAndCop(numericUpDownTransportTax.Value); // сумму налога переводим в 0 руб. 0 коп
                taxesString += ", пени в сумме " + HandlerTextControls.IntInRubAndCop(numericUpDownTransportTaxFine.Value) + ", "; // сумму пени переводим в 0 руб. 0 коп
            }
            if (checkBoxLandTax.Checked) // Транспортный налог
            {
                taxesString += checkBoxLandTax.Text; // указываем налог
                taxesString += " за " + comboBoxLandTaxYear.Text + " год"; // добавляем год
                taxesString += " в сумме " + HandlerTextControls.IntInRubAndCop(numericUpDownLandTax.Value); // сумму налога переводим в 0 руб. 0 коп
                taxesString += ", пени в сумме " + HandlerTextControls.IntInRubAndCop(numericUpDownLandTaxFine.Value) + ", "; // сумму пени переводим в 0 руб. 0 коп
            }
            if (checkBoxPropertyTax.Checked) // Транспортный налог
            {
                taxesString += checkBoxPropertyTax.Text; // указываем налог
                taxesString += " за " + comboBoxPropertyTaxYear.Text + " год"; // добавляем год
                taxesString += " в сумме " + HandlerTextControls.IntInRubAndCop(numericUpDownPropertyTax.Value); // сумму налога переводим в 0 руб. 0 коп
                taxesString += ", пени в сумме " + HandlerTextControls.IntInRubAndCop(numericUpDownPropertyTaxFine.Value) + ", "; // сумму пени переводим в 0 руб. 0 коп
            }
            GenerationWord.AddValueControl(ref controlArrayToString, taxesString, "#-1"); // в ручную добавляем новый ключ


            // Подсчет суммы всех налогов
            decimal taxesSum = 0m; // Сумма всех налогов
            try
            {
                if (checkBoxTransportTax.Checked)
                    taxesSum += numericUpDownTransportTax.Value + numericUpDownTransportTaxFine.Value;
                if (checkBoxLandTax.Checked)
                    taxesSum += numericUpDownLandTax.Value + numericUpDownLandTaxFine.Value;
                if (checkBoxPropertyTax.Checked)
                    taxesSum += numericUpDownPropertyTax.Value + numericUpDownPropertyTaxFine.Value;
            }
            catch
            {
                MessageBox.Show("Все поля с цифрами задолженности и пени должны быть заполнены хотя бы нулем");
                return;
            }
            string taxesSumToString = HandlerTextControls.IntInRubAndCop(taxesSum); // сумму налога переводим в 0 руб. 0 коп
            taxesSumToString = "всего " + taxesSumToString;
            GenerationWord.AddValueControl(ref controlArrayToString, taxesSumToString, "#-2"); // в ручную добавляем новый ключ


            //Считаем гос.пошлину
            decimal duty = 0m;
            string dutyToString = "";
            if (taxesSum <= 20000)
            {
                duty = taxesSum / 100 * 2;
                if (duty < 200)
                    duty = 200;
            }
            else if (taxesSum > 20000 && taxesSum <= 100000)
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
            dutyToString = HandlerTextControls.IntInRubAndCop(Math.Round(duty, 2)); // сумму пошлины переводим в 0 руб. 0 коп
            GenerationWord.AddValueControl(ref controlArrayToString, dutyToString, "#-3"); // в ручную добавляем новый ключ


            // Вставляем название в буфер обмена
            string numCase = ""; // номер дела
            if (textBoxClipPutNum.Text != null && textBoxClipPutNum.Text != "")
                numCase = textBoxClipPutNum.Text + "  ";
            Clipboard.SetText(numCase + textBoxClipPutName.Text + "  " + this.Text);


            // Сгенерировать ворд
            GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "Взыскание налога", controlArrayToString);
        }
    }
}
