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
    public partial class ByJurisdiction : Form
    {
        #region Инициализация
        public ByJurisdiction()
        {
            InitializeComponent();

            // Заполнение полей ввода вариантами
            Db.SetPropertiesComboBox(ref comboBoxResidenceCity, NamePropertiesForComboBox.МестоЖительстваГород); // Место жителства населенный пункт
            Db.SetPropertiesComboBox(ref comboBoxResidenceStreet, NamePropertiesForComboBox.МестоЖительстваУлица); // Место жителства улица
            Db.SetPropertiesComboBox(ref comboBoxResidenceHouse, NamePropertiesForComboBox.МестоЖительстваДом); // Место жителства дом
            // Save
            SaveLoadForm.SetVariantsSaveInComboBox(this.Name, ref comboBoxLoad);// заполнение вариантами сохранений

            // Автозаполнение 
            // Заполнение даты вынесения решения текущими датами
            DateTime dateTimeNow = DateTime.Now;
            string month = HandlerTextControls.MonthInString(dateTimeNow.Month); // месяц
            string year = dateTimeNow.Year.ToString(); // год
            comboBoxDateOfOrderMonth.Text = month;
            comboBoxDateOfOrderYear.Text = year;
        }
        #endregion Инициализация

        #region Автозаполнение форм

        // Выбранный участок, автоматически подставляет кто судья
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

        // автоматически переносит фамилию на другие поля ввода
        private void textBox2FIO_TextChanged(object sender, EventArgs e)
        {
            textBoxFullNameSurNameIvanova.Text = textBoxFullNameSurNameIvanovu.Text;

            // название при сохранении
            textBoxClipPutName.Text = textBoxFullNameSurNameIvanovu.Text;
            textBoxForSave.Text = textBoxFullNameSurNameIvanovu.Text;
        }

        #endregion Автозаполнение форм

        #region Сохранение
        // Сохранить заполненные поля
        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GenerationWord.StandartListValueControl(Controls);

            string nameSave = this.Name + "$" + textBoxForSave.Text; // имя сохранения
            SaveLoadForm.SaveForm(nameSave, controlArrayToString); // сохранить

            // перезаполняем варианты сохранений
            comboBoxLoad.Items.Clear(); // стираем текущие варианты
            SaveLoadForm.SetVariantsSaveInComboBox(this.Name, ref comboBoxLoad);// заполнение вариантами сохранений
        }
        // Загрузить сохраненные поля
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            var controls = SaveLoadForm.LoadForm(this.Name, comboBoxLoad.Text, Controls); // получаем заполненные сейвом контролы

            // Переносим текст массива заполненных контролов в контролы этой формы. Иначе ни как, т.к. Controls {get;}
            for (int i = 0; i < controls.Count; i++)
            {
                Controls[i].Text = controls[i].Text;
            }
        }
        // Удалить сохранение
        private void buttonDeleteSave_Click(object sender, EventArgs e)
        {
            SaveLoadForm.DeleteSave(comboBoxLoad.Text, this.Name); // Удаляем сохранение

            // перезаполняем варианты сохранений
            comboBoxLoad.Items.Clear(); // стираем текущие варианты
            comboBoxLoad.Text = ""; // стираем текущие варианты
            SaveLoadForm.SetVariantsSaveInComboBox(this.Name, ref comboBoxLoad);// заполнение вариантами сохранений
        }

        #endregion Сохранение

        // СГЕНЕРИРОВАТЬ WORD
        private void button1_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GenerationWord.StandartListValueControl(Controls);
            

            // Делаем инициалы ФИО
            string initials = "";
            if (textBoxFullNameNameIvana.Text.Length > 2 && textBoxFullNamePatronymicIvanovicha.Text.Length > 2) // Если правильно заполнили имя и отчество
                initials = textBoxFullNameNameIvana.Text.Remove(1) + "." + textBoxFullNamePatronymicIvanovicha.Text.Remove(1) + ".";
            GenerationWord.AddValueControl(ref controlArrayToString, initials, "#-1"); // в ручную добавляем новый ключ


            // Вставляем название в буфер обмена
            string numCase = ""; // номер дела
            if (textBoxClipPutNum.Text != null && textBoxClipPutNum.Text != "")
                numCase = textBoxClipPutNum.Text + "  ";
            Clipboard.SetText(numCase + textBoxClipPutName.Text + "  " + this.Text);


            // Сгенерировать ворд
            GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "Постановление по подсудности", controlArrayToString);

        }
    }
}
