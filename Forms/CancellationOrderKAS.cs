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
    public partial class CancellationOrderKAS : Form
    {

        #region Инициализация

        public CancellationOrderKAS()
        {
            InitializeComponent();

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPlotNumber.Text == PropertiesMyApp.GetPropertiesValue(TypeProperties.PlaceNum))
            {
                comboBoxWhoIsJudge.Text = "Мировой судья";
                comboBoxSomeIsJudge.Text = "мировым судьей";
            }
            else
            {
                comboBoxWhoIsJudge.Text = "И.о. мирового судьи";
                comboBoxSomeIsJudge.Text = "и.о. мирового судьи";
            }
        }


        // название при сохранении
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBoxClipPutName.Text = textBoxFullNameSurNameIvanova.Text;
        }

        #endregion Автоматическое заполнение полей


        #region Сохранение

        // Сохранить заполненные поля
        string nameForm = "CancellationOrderKAS";
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


            // Вставляем название в буфер обмена
            string numCase = ""; // номер дела
            if (textBoxClipPutNum.Text != null && textBoxClipPutNum.Text != "")
                numCase = textBoxClipPutNum.Text + "  ";
            Clipboard.SetText(numCase + textBoxClipPutName.Text + "  " + this.Text);


            // Если с восстановлением, то один шаблон, если нет, то другой
            if (radioButtonNotRecovery.Checked)
            {
                // Сгенерировать ворд
                GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "Отмена приказа без восстановления КАС", controlArrayToString);
            }
            else if (radioButtonRecovery.Checked)
            {
                // Сгенерировать ворд
                GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "Отмена приказа с восстановлением КАС", controlArrayToString);
            }
            else
            {
                MessageBox.Show("Не выбрано с восстановлением или без");
            }
        }

    }
}
