using Magistrate.FormLogic;
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
        private string nameForm; // название формы, идентичен ее названию ее класса

        #region Инициализация

        public CasTaxes()
        {
            InitializeComponent();

            FormController.SetPropertiesComboBox(ref comboBoxPlaceOfBirth, NamePropertiesForComboBox.PlaceBirth); // Заполняем Населенный пункт, место рождения
            FormController.SetPropertiesComboBox(ref comboBoxResidenceCity, NamePropertiesForComboBox.PlaceResidenceCity); // Место жителства населенный пункт
            FormController.SetPropertiesComboBox(ref comboBoxResidenceStreet, NamePropertiesForComboBox.PlaceResidenceStreet); // Место жителства улица
            FormController.SetPropertiesComboBox(ref comboBoxResidenceHouse, NamePropertiesForComboBox.PlaceResidenceHouse); // Место жителства дом

            FormController.SetStandartParamsInControls
                (nameForm,
                ref comboBoxLoad,               // заполнение вариантами сохранений
                ref comboBoxDateOfOrderMonth,   // заполнение месяца вынесения решения
                ref comboBoxDateOfOrderYear);   // заполнение года вынесения решения

            nameForm = this.GetType().ToString(); // Название формы
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
            comboBoxWhoIsJudge.Text = FormController.GetMagistrateOrDeputy(comboBoxPlotNumber.Text);
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
        private void buttonSave_Click(object sender, EventArgs e)
        {
            FormController.SaveForm(Controls, nameForm, textBoxForSave.Text, ref comboBoxLoad);
        }

        // Загрузить сохраненные поля
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            // получаем заполненные сейвом контролы
            var controls = FormController.GetControlsLoadForm(Controls, nameForm, ref comboBoxLoad);

            UpdateControls(controls);
        }

        // Удалить сохранение
        private void buttonDeleteSave_Click(object sender, EventArgs e)
        {
            FormController.DeleteSave(ref comboBoxLoad, nameForm);
        }

        #endregion Сохранение


        #region Приватные методы

        /// <summary>Обновить контролы на форме</summary>
        /// <param name="controls">контролы, которыми будут заменяться контролы на форме</param>
        private void UpdateControls(Control.ControlCollection controls)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                Controls[i].Text = controls[i].Text;
            }
        }


        // Обработка налогов, составление строки
        private string TaxHandlerStringPreparation()
        {
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

            return taxesString;

        }


        // Подсчет суммы всех налогов
        private string SumAllTaxes(out decimal taxesSum)
        {
            taxesSum = 0m; // Сумма всех налогов
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
                throw new Exception("Все поля с цифрами задолженности и пени должны быть заполнены хотя бы нулем");
            }

            string taxesSumToString = HandlerTextControls.IntInRubAndCop(taxesSum); // сумму налога переводим в 0 руб. 0 коп

            taxesSumToString = "всего " + taxesSumToString;

            return taxesSumToString;
        }


        // Считаем госпошлину, где taxesSum - сумма всех прочих налогов
        private string CountStateFee(decimal taxesSum)
        {
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
                throw new Exception("Все поля с цифрами задолженности и пени должны быть заполнены хотя бы нулем");
            }
            dutyToString = HandlerTextControls.IntInRubAndCop(Math.Round(duty, 2)); // сумму пошлины переводим в 0 руб. 0 коп

            return dutyToString;
        }
        #endregion Приватные методы

        // СГЕНЕРИРОВАТЬ WORD
        private void button1_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GeneratorWord.StandartListValueControl(Controls);


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
            string taxesString = TaxHandlerStringPreparation();
            GeneratorWord.AddValueControl(ref controlArrayToString, taxesString, "#-1"); // в ручную добавляем новый ключ


            // Подсчет суммы всех налогов
            decimal taxesSum; // Сумма всех налогов
            try
            {
                string taxesSumToString = SumAllTaxes(out taxesSum);
                GeneratorWord.AddValueControl(ref controlArrayToString, taxesSumToString, "#-2"); // в ручную добавляем новый ключ
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


            //Считаем гос.пошлину
            try
            {
                string dutyToString = CountStateFee(taxesSum);
                GeneratorWord.AddValueControl(ref controlArrayToString, dutyToString, "#-3"); // в ручную добавляем новый ключ
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


            // Вставляем название в буфер обмена
            FormController.ClipPutNameWord(textBoxClipPutNum.Text, textBoxClipPutName.Text, this.Text);


            FormController.GenerateWord(controlArrayToString, "Взыскание налога");
        }
    }
}
