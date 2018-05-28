using Magistrate.FormLogic;
using Magistrate.PropertiesForm;
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
    public partial class CreditOrder : Form
    {
        private string nameForm; // название формы, идентичен ее названию ее класса

        #region Инициализация
        public CreditOrder()
        {
            InitializeComponent();

            FormController.SetPropertiesComboBox(ref comboBoxBank, NamePropertiesForComboBox.БанкСокращенный); // Заполняем банки
            FormController.SetPropertiesComboBox(ref comboBoxPlaceOfBirth, NamePropertiesForComboBox.МестоРождения); // Заполняем Населенный пункт, место рождения
            FormController.SetPropertiesComboBox(ref comboBoxResidenceCity, NamePropertiesForComboBox.МестоЖительстваГород); // Место жителства населенный пункт
            FormController.SetPropertiesComboBox(ref comboBoxResidenceStreet, NamePropertiesForComboBox.МестоЖительстваУлица); // Место жителства улица
            FormController.SetPropertiesComboBox(ref comboBoxResidenceHouse, NamePropertiesForComboBox.МестоЖительстваДом); // Место жителства дом

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
            // название при сохранении
            textBoxClipPutName.Text = textBoxFullNameSurNameIvanovoy.Text;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxWhoIsJudge.Text = FormController.GetMagistrateOrDeputy(comboBoxPlotNumber.Text);
        }
        #endregion Автоматическое заполнение полей


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

        #endregion Приватные методы


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


        // СГЕНЕРИРОВАТЬ WORD
        private void button1_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GeneratorWord.StandartListValueControl(Controls);


            // Заполняем полные реквизиты банка
            string bankRequisites = FormController.GetBank(comboBoxBank.Text); // находим полные реквизиты банка
            if (bankRequisites == null) // банк не опознан
            {
                MessageBox.Show("Реквизиты банка не опознаны, после герации не забудьте их вписать");
                bankRequisites = "";
            }
            GeneratorWord.AddValueControl(ref controlArrayToString, bankRequisites, "#-1"); // в ручную добавляем новый ключ


            // Заполнение второй даты, выделено отдельно т.к. иначе в word-е будут лишние точки
            string dateTwo = ""; // дата вторая 
            if (comboBoxPeriodTwoDay.Text != "" && comboBoxPeriodTwoDay.Text != null)
            {
                dateTwo = comboBoxPeriodTwoDay.Text + "." + comboBoxPeriodTwoMonth.Text + "." + comboBoxPeriodTwoYear.Text;
                dateTwo = " по " + dateTwo + " года";
            }
            GeneratorWord.AddValueControl(ref controlArrayToString, dateTwo, "#-2"); // в ручную добавляем новый ключ


            // Изменение суммы задолженности по принципу 0 руб. 0 коп.
            string Debt = HandlerTextControls.IntInRubAndCop(numericUpDownCredit.Value);
            if (Debt == null)
                return;
            GeneratorWord.AddValueControl(ref controlArrayToString, Debt, "#-3"); // в ручную добавляем новый ключ


            // Изменение суммы государственной пошлины по принципу 0 руб. 0 коп.
            string Duty = HandlerTextControls.IntInRubAndCop(numericUpDownCreditFine.Value);
            if (Duty == null)
                return;
            GeneratorWord.AddValueControl(ref controlArrayToString, Duty, "#-4"); // в ручную добавляем новый ключ


            // Суммирование взыскиваемой суммы
            decimal summToPay = numericUpDownCredit.Value + numericUpDownCreditFine.Value; // сумма к оплате
            string ToPay = HandlerTextControls.IntInRubAndCop(summToPay);
            GeneratorWord.AddValueControl(ref controlArrayToString, ToPay, "#-5"); // в ручную добавляем новый ключ


            // Вставляем название в буфер обмена
            FormController.ClipPutNameWord(textBoxClipPutNum.Text, textBoxClipPutName.Text, this.Text);


            FormController.GenerateWord(controlArrayToString, "Приказ по банку или фин орг");
        }

        // добавить организацию
        private void buttonAddRequisites_Click(object sender, EventArgs e)
        {
            NewRequisites form2 = new NewRequisites(TypeRequisites.Bank); // Создание соовтественующего окна
            form2.Show(); // показать окно
        }
        // Обновить реквизиты
        private void buttonUpdateRequisites_Click(object sender, EventArgs e)
        {
            comboBoxBank.Items.Clear();
            FormController.SetPropertiesComboBox(ref comboBoxBank, NamePropertiesForComboBox.БанкСокращенный); // Место жителства дом
        }
    }
}

