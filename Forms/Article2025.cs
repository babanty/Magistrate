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
    public partial class Article2025 : Form
    {
        private string nameForm; // название формы, идентичен ее названию ее класса

        #region Инициализация

        public Article2025()
        {
            InitializeComponent();

            // Заполнение полей ввода вариантами
            FormController.SetPropertiesComboBox(ref comboBoxPlaceOfBirth, NamePropertiesForComboBox.PlaceBirth); // Заполняем Населенный пункт, место рождения
            FormController.SetPropertiesComboBox(ref comboBoxResidenceCity, NamePropertiesForComboBox.PlaceResidenceCity); // Место жителства населенный пункт
            FormController.SetPropertiesComboBox(ref comboBoxResidenceStreet, NamePropertiesForComboBox.PlaceResidenceStreet); // Место жителства улица
            FormController.SetPropertiesComboBox(ref comboBoxResidenceHouse, NamePropertiesForComboBox.PlaceResidenceHouse); // Место жителства дом
            comboBoxRecipientGIBDD.Items.AddRange(Db.GetAllShortRequisitesGAI().ToArray()); // Получатель бабулесов, ГАИ, заполняем варианты
            // Save
            SaveLoadForm.SetVariantsSaveInComboBox(this.Name, ref comboBoxLoad);// заполнение вариантами сохранений

            FormController.SetStandartParamsInControls
                (nameForm,
                ref comboBoxLoad,               // заполнение вариантами сохранений
                ref comboBoxDateOfOrderMonth,   // заполнение месяца вынесения решения
                ref comboBoxDateOfOrderYear);   // заполнение года вынесения решения

            nameForm = this.GetType().ToString(); // Название формы
        }

        #endregion Инициализация


        #region Автоматическое заполнение полей
        // Выбранный участок, автоматически подставляет кто судья
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxWhoIsJudge.Text = FormController.GetMagistrateOrDeputy(comboBoxPlotNumber.Text);
        }

        // автоматически переносит фамилию на другие поля ввода
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBoxFullNameSurNameIvanova.Text = textBoxFullNameSurNameIvanov.Text;
            textBoxFullNameSurNameIvanovu.Text = textBoxFullNameSurNameIvanov.Text;

            // название при сохранении
            textBoxClipPutName.Text = textBoxFullNameSurNameIvanov.Text;
        }

        // Автоматически переносит полное ФИО на другие поля
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBoxFullNameSurNameIvanovoy.Text = textBoxFullNameSurNameIvanova.Text;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBoxFullNameNameMarii.Text = textBoxFullNameNameIvana.Text;
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBoxFullNamePatronymicIvanovni.Text = textBoxFullNamePatronymicIvanovicha.Text;
        }

        // Автоматическое изменение УИН в лейбле
        private void comboBoxGAI_SelectedIndexChanged(object sender, EventArgs e)
        {
            string yin = ""; // Статичная часть УИНа
            Db.GetRequisitesGAI(comboBoxRecipientGIBDD.Text, out string requisitesGAI, out yin); // Заполняем переменные с реквизитами

            // меняем текст в лэйбле
            if (yin == null)
            {
                labelYIN.Text = "УИН получателя, получатель не опознан";
            }
            else
            {
                labelYIN.Text = "УИН получателя, продолжить: " + yin;
            }
        }

        #endregion  автоматическое заполнение полей


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

        // добавить организацию
        private void buttonAddRequisites_Click(object sender, EventArgs e)
        {
            NewGAI form2 = new NewGAI(); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }
        // Обновить реквизиты
        private void buttonUpdateRequisites_Click(object sender, EventArgs e)
        {
            comboBoxRecipientGIBDD.Items.Clear();
            FormController.SetPropertiesComboBox(ref comboBoxRecipientGIBDD, NamePropertiesForComboBox.DetailsTrafficPolice); // Место жителства дом
        }
        #endregion приватные методы


        // СГЕНЕРИРОВАТЬ WORD
        private void button1_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GeneratorWord.StandartListValueControl(Controls);


            //Явился или не явился
            string resultAppeared = FormController.AppearedOrNotExplanation(checkBoxAppearedOrNot.Checked, radioButtonSexWoomen.Checked);
            GeneratorWord.AddValueControl(ref controlArrayToString, resultAppeared, "#-1"); // в ручную добавляем новый ключ

            // Делаем инициалы ФИО
            string initials = FormController.GetInitials(textBoxFullNameNameIvana.Text, textBoxFullNamePatronymicIvanovicha.Text);
            GeneratorWord.AddValueControl(ref controlArrayToString, initials, "#-2"); // в ручную добавляем новый ключ

            // Получатель и УИН
            string requisitesGAI = FormController.GenerateFullRequisitesGAI(comboBoxRecipientGIBDD.Text, textBoxYINgIBDD.Text); // полные реквзииты ГАИ
            GeneratorWord.AddValueControl(ref controlArrayToString, requisitesGAI, "#-3"); // в ручную добавляем новый ключ

            // Сумма штрафа умножить на два
            try
            {
                int sumFine = Convert.ToInt32(comboBoxFine.Text) * 2; // сумма штрафа
                GeneratorWord.AddValueControl(ref controlArrayToString, sumFine.ToString(), "#-4"); // в ручную добавляем новый ключ
            }catch
            {
                MessageBox.Show("Некорректно введена сумма штрафа");
                return;
            }

            // Вставляем название в буфер обмена
            FormController.ClipPutNameWord(textBoxClipPutNum.Text, textBoxClipPutName.Text, this.Text);


            FormController.GenerateWord(controlArrayToString, radioButtonSexMen.Checked, "ст 20.25 Муж", "ст 20.25 Жен");

        }
    }
}
