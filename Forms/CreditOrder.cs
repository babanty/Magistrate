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
        public CreditOrder()
        {
            InitializeComponent();

            Db.SetPropertiesComboBox(ref comboBox17, NamePropertiesForComboBox.БанкСокращенный); // Заполняем банки
            Db.SetPropertiesComboBox(ref comboBox10, NamePropertiesForComboBox.МестоРождения); // Заполняем Населенный пункт, место рождения
            Db.SetPropertiesComboBox(ref comboBox13, NamePropertiesForComboBox.МестоЖительстваГород); // Место жителства населенный пункт
            Db.SetPropertiesComboBox(ref comboBox14, NamePropertiesForComboBox.МестоЖительстваУлица); // Место жителства улица
            Db.SetPropertiesComboBox(ref comboBox15, NamePropertiesForComboBox.МестоЖительстваДом); // Место жителства дом

            SaveLoadForm.SetVariantsSaveInComboBox(nameForm, ref comboBoxLoad);// заполнение вариантами сохранений

            // Автозаполнение 
            // Заполнение даты вынесения решения текущими датами
            DateTime dateTimeNow = DateTime.Now;
            string month = HandlerTextControls.MonthInString(dateTimeNow.Month); // месяц
            string year = dateTimeNow.Year.ToString(); // год
            comboBox3.Text = month;
            comboBox4.Text = year;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GenerationWord.StandartListValueControl(Controls);


            // Заполняем полные реквизиты банка
            string bankRequisites = GetBank(comboBox17.Text); // находим полные реквизиты банка
            if(bankRequisites == null) // банк не опознан
            {
                MessageBox.Show("Реквизиты банка не опознаны, после герации не забудьте их вписать");
                bankRequisites = "";
            }
            GenerationWord.AddValueControl(ref controlArrayToString, bankRequisites, "#-1"); // в ручную добавляем новый ключ


            // Заполнение второй даты, выделено отдельно т.к. иначе в word-е будут лишние точки
            string dateTwo = ""; // дата вторая 
            if (comboBox25.Text != "" && comboBox25.Text != null)
            {
                dateTwo = comboBox25.Text + "." + comboBox24.Text + "." + comboBox23.Text;
                dateTwo = " по " + dateTwo + " года";
            }
            GenerationWord.AddValueControl(ref controlArrayToString, dateTwo, "#-2"); // в ручную добавляем новый ключ


            // Изменение суммы задолженности по принципу 0 руб. 0 коп.
            string Debt = HandlerTextControls.IntInRubAndCop(textBox5.Text);
            if (Debt == null)
                return;
            GenerationWord.AddValueControl(ref controlArrayToString, Debt, "#-3"); // в ручную добавляем новый ключ


            // Изменение суммы государственной пошлины по принципу 0 руб. 0 коп.
            string Duty = HandlerTextControls.IntInRubAndCop(textBox6.Text);
            if (Duty == null)
                return;
            GenerationWord.AddValueControl(ref controlArrayToString, Duty, "#-4"); // в ручную добавляем новый ключ
            

            // Суммирование взыскиваемой суммы
            double summToPay = double.Parse(textBox5.Text) + double.Parse(textBox6.Text); // сумма к оплате
            string ToPay = HandlerTextControls.IntInRubAndCop(summToPay);
            GenerationWord.AddValueControl(ref controlArrayToString, ToPay, "#-5"); // в ручную добавляем новый ключ


            // Вставляем название в буфер обмена
            Clipboard.SetText(textBoxClipPutNum.Text + "  " + textBoxClipPutName.Text + "  " + this.Text);


            // Сгенерировать ворд
            GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "Приказ по банку или фин орг", controlArrayToString);
        }

        /// <summary>
        /// Возвращает полные реквизиты банка по названию или null
        /// </summary>
        /// <param name="Name">Название банка</param>
        private string GetBank(string Name)
        { // потом сделать динамчным вытгиванием из БД 
            int nameLength = Name.Length; // Количество символов в названии банка
            string result = null;


            // Проверяем сходится ли название с полными реквизитами
            List<string> allBanks = Db.GetColumn("bank complete");
            if (allBanks == null)
                return null;

            foreach(string bank in allBanks)
            {
                if (bank.Substring(0, nameLength) == Name) // первые слова в полных реквизитах соотвествуют 
                    return bank;
            }

            // Возвращаем результат null
            return result;


        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // название при сохранении
            textBoxClipPutName.Text = textBox1.Text;
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
        string nameForm = "CreditOrder";
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

