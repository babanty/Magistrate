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
    public partial class OrdersForCommunal : Form
    {
        private string nameForm; // название формы, идентичен ее названию ее класса

        #region Инициализация
        public OrdersForCommunal()
        {
            InitializeComponent();

            // добавление организаций
            FormController.SetPropertiesComboBox(ref comboBoxBank, NamePropertiesForComboBox.КомуналкаСокращенная); // Место жителства дом

            FormController.SetPropertiesComboBox(ref d1comboBox5, NamePropertiesForComboBox.МестоРождения); // Заполняем Населенный пункт, место рождения
            FormController.SetPropertiesComboBox(ref d1comboBox8, NamePropertiesForComboBox.МестоЖительстваГород); // Место жителства населенный пункт
            FormController.SetPropertiesComboBox(ref d1comboBox9, NamePropertiesForComboBox.МестоЖительстваУлица); // Место жителства улица
            FormController.SetPropertiesComboBox(ref d1comboBox10, NamePropertiesForComboBox.МестоЖительстваДом); // Место жителства дом

            FormController.SetStandartParamsInControls
                (nameForm,
                ref comboBoxLoad,               // заполнение вариантами сохранений
                ref comboBoxDateOfOrderMonth,   // заполнение месяца вынесения решения
                ref comboBoxDateOfOrderYear);   // заполнение года вынесения решения

            nameForm = this.GetType().ToString(); // Название формы
        }
        #endregion Инициализация
        

        #region Автоматическое заполнение полей

        // Автоматическое выставление судья или И.о участка
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxWhoIsJudge.Text = FormController.GetMagistrateOrDeputy(comboBoxPlotNumber.Text);
        }

        
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            // Ограничение по количеству должников
            if (numericUpDownNumDebtor.Value < 1)
            {
                MessageBox.Show("Количество должников не может быть меньше 1");
                numericUpDownNumDebtor.Value = 1;
                return;
            }
            // Ограничение по количеству должников
            if (numericUpDownNumDebtor.Value > 5)
            {
                MessageBox.Show("Максимальное количество должников 5. Если их больше, то добавлять их надо " +
                    "в ручную в сгенерированном word-е");
                numericUpDownNumDebtor.Value = 5;
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
            for (int i = 33; i < 33 + 14 * ((int)numericUpDownNumDebtor.Value - 1); i++)
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

            // название при сохранении
            textBoxClipPutName.Text = d1textBox1.Text;
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


        /// <summary> Находим полные реквизиты по комуналке </summary>
        private string FindFullDetailsCommunal()
        {
            string Requisites = FormController.GetRecMunicipal(comboBoxBank.Text); // находим полные реквизиты банка
            if (Requisites == null) // банк не опознан
            {
                MessageBox.Show("Реквизиты организации не опознаны, после герации не забудьте их вписать");
                Requisites = "";
            }

            return Requisites;
        }

        /// <summary> должника / должников </summary>
        private string DebtorOrDebtord()
        {
            string debtor = "должника";
            if (numericUpDownNumDebtor.Value > 1)
                debtor = "должников";

            return debtor;
        }

        /// <summary> солидарно или нет </summary>
        private string SeverallyOrNot()
        {
            string severally = "";
            if (numericUpDownNumDebtor.Value != 1)
                severally = "солидарно";

            return severally;
        }

        /// <summary> Заполнение второй даты, выделено отдельно т.к. иначе в word-е будут лишние точки
        /// На дату / период / с - по </summary>
        private string FillingSecondDate()
        {
            string dateTwo = ""; // дата вторая 
            if (comboBoxPeriodTwoDay.Text != "" && comboBoxPeriodTwoDay.Text != null)
            {
                dateTwo = comboBoxPeriodTwoDay.Text + "." + comboBoxPeriodTwoMonth.Text + "." + comboBoxPeriodTwoYear.Text;
                dateTwo = " по " + dateTwo + " года";
            }

            return dateTwo;
        }

        /// <summary> С кого взимать задолженность, только ФИО все вместе </summary>
        private string FromWhomCollectDebt()
        {
            string fullNames = "";
            if (numericUpDownNumDebtor.Value > 0) // добавляем по одному человеку
                fullNames += d1textBox1.Text + " " + d1textBox2.Text + " " + d1textBox3.Text;
            if (numericUpDownNumDebtor.Value > 1) // добавляем по одному человеку
                fullNames += ", " + d2textBox1.Text + " " + d2textBox2.Text + " " + d2textBox3.Text;
            if (numericUpDownNumDebtor.Value > 2) // добавляем по одному человеку
                fullNames += ", " + d3textBox1.Text + " " + d3textBox2.Text + " " + d3textBox3.Text;
            if (numericUpDownNumDebtor.Value > 3) // добавляем по одному человеку
                fullNames += ", " + d4textBox1.Text + " " + d4textBox2.Text + " " + d4textBox3.Text;
            if (numericUpDownNumDebtor.Value > 4) // добавляем по одному человеку
                fullNames += ", " + d5textBox1.Text + " " + d5textBox2.Text + " " + d5textBox3.Text;

            return fullNames;
        }
        
        /// <summary> С кого взимать задолженность, полные реквизиты все вместе </summary>
        private string FromWhomCollectDebtFullDetails()
        {
            string allDetailsDebtors = "";
            if (numericUpDownNumDebtor.Value > 0) // добавляем по одному человеку
                allDetailsDebtors += d1textBox1.Text + " " + d1textBox2.Text + " " + d1textBox3.Text
                    + ", дата рождения: " + d1comboBox1.Text + "." + d1comboBox2.Text + "." + d1comboBox3.Text + " г.р."
                    + ", место рождения: " + d1comboBox4.Text + " " + d1comboBox5.Text
                    + ", место регистрации: " + d1comboBox6.Text + ", " + d1comboBox7.Text + ", " + d1comboBox8.Text
                     + ", " + d1comboBox9.Text + ", " + d1comboBox10.Text;
            if (numericUpDownNumDebtor.Value > 1) // добавляем по одному человеку
                allDetailsDebtors += ", " + d2textBox1.Text + " " + d2textBox2.Text + " " + d2textBox3.Text
                    + ", дата рождения: " + d2comboBox1.Text + "." + d2comboBox2.Text + "." + d2comboBox3.Text + " г.р."
                    + ", место рождения: " + d2comboBox4.Text + " " + d2comboBox5.Text
                    + ", место регистрации: " + d2comboBox6.Text + ", " + d2comboBox7.Text + ", " + d2comboBox8.Text
                     + ", " + d2comboBox9.Text + ", " + d2comboBox10.Text;
            if (numericUpDownNumDebtor.Value > 2) // добавляем по одному человеку
                allDetailsDebtors += ", " + d3textBox1.Text + " " + d3textBox2.Text + " " + d3textBox3.Text
                    + ", дата рождения: " + d3comboBox1.Text + "." + d3comboBox2.Text + "." + d3comboBox3.Text + " г.р."
                    + ", место рождения: " + d3comboBox4.Text + " " + d3comboBox5.Text
                    + ", место регистрации: " + d3comboBox6.Text + ", " + d3comboBox7.Text + ", " + d3comboBox8.Text
                     + ", " + d3comboBox9.Text + ", " + d3comboBox10.Text;
            if (numericUpDownNumDebtor.Value > 3) // добавляем по одному человеку
                allDetailsDebtors += ", " + d4textBox1.Text + " " + d4textBox2.Text + " " + d4textBox3.Text
                    + ", дата рождения: " + d4comboBox1.Text + "." + d4comboBox2.Text + "." + d4comboBox3.Text + " г.р."
                    + ", место рождения: " + d4comboBox4.Text + " " + d4comboBox5.Text
                    + ", место регистрации: " + d4comboBox6.Text + ", " + d4comboBox7.Text + ", " + d4comboBox8.Text
                     + ", " + d4comboBox9.Text + ", " + d4comboBox10.Text;
            if (numericUpDownNumDebtor.Value > 4) // добавляем по одному человеку
                allDetailsDebtors += ", " + d5textBox1.Text + " " + d5textBox2.Text + " " + d5textBox3.Text
                    + ", дата рождения: " + d5comboBox1.Text + "." + d5comboBox2.Text + "." + d5comboBox3.Text + " г.р."
                    + ", место рождения: " + d5comboBox4.Text + " " + d5comboBox5.Text
                    + ", место регистрации: " + d5comboBox6.Text + ", " + d5comboBox7.Text + ", " + d5comboBox8.Text
                     + ", " + d5comboBox9.Text + ", " + d5comboBox10.Text;

            return allDetailsDebtors;
        }
        
        /// <summary> Логика конкретно данной формы </summary>
        private List<ValueControl> LogicForm(List<ValueControl> controlArrayToString)
        {
            GenerationWord.AddValueControl(ref controlArrayToString, FindFullDetailsCommunal(), "#-1");
            GenerationWord.AddValueControl(ref controlArrayToString, DebtorOrDebtord(), "#-2");
            GenerationWord.AddValueControl(ref controlArrayToString, SeverallyOrNot(), "#-3");
            GenerationWord.AddValueControl(ref controlArrayToString, FillingSecondDate(), "#-4");
            GenerationWord.AddValueControl(ref controlArrayToString, FromWhomCollectDebt(), "#-5");
            GenerationWord.AddValueControl(ref controlArrayToString, FromWhomCollectDebtFullDetails(), "#-6");


            // В сумме руб коп
            string Summ = HandlerTextControls.IntInRubAndCop(numericUpDownCredit.Value);
            if (Summ == null)
                throw new Exception("Некорректная сумма");
            GenerationWord.AddValueControl(ref controlArrayToString, Summ, "#-7"); // в ручную добавляем новый ключ


            // Пени руб коп
            string Duty = HandlerTextControls.IntInRubAndCop(numericUpDownCreditFine.Value);
            if (Duty == null)
                throw new Exception("Некорректное пени");
            GenerationWord.AddValueControl(ref controlArrayToString, Duty, "#-8"); // в ручную добавляем новый ключ

            // Всего руб коп
            decimal summToPay = numericUpDownCredit.Value + numericUpDownCreditFine.Value; // сумма к оплате
            string ToPay = HandlerTextControls.IntInRubAndCop(summToPay);
            GenerationWord.AddValueControl(ref controlArrayToString, ToPay, "#-9"); // в ручную добавляем новый ключ

            return controlArrayToString;
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
            List<ValueControl> controlArrayToString = GenerationWord.StandartListValueControl(Controls);


            try { controlArrayToString = LogicForm(controlArrayToString); }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            // Вставляем название в буфер обмена
            FormController.ClipPutNameWord(textBoxClipPutNum.Text, textBoxClipPutName.Text, this.Text);
            
            FormController.GenerateWord(controlArrayToString, "Приказ по комуналке");
        }
    }
}
