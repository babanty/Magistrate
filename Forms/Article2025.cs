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
        #region Инициализация
        public Article2025()
        {
            InitializeComponent();

            // Заполнение полей ввода вариантами
            Db.SetPropertiesComboBox(ref comboBoxPlaceOfBirth, NamePropertiesForComboBox.МестоРождения); // Заполняем Населенный пункт, место рождения
            Db.SetPropertiesComboBox(ref comboBoxResidenceCity, NamePropertiesForComboBox.МестоЖительстваГород); // Место жителства населенный пункт
            Db.SetPropertiesComboBox(ref comboBoxResidenceStreet, NamePropertiesForComboBox.МестоЖительстваУлица); // Место жителства улица
            Db.SetPropertiesComboBox(ref comboBoxResidenceHouse, NamePropertiesForComboBox.МестоЖительстваДом); // Место жителства дом
            comboBoxRecipientGIBDD.Items.AddRange(Db.GetAllShortRequisitesGAI().ToArray()); // Получатель бабулесов, ГАИ, заполняем варианты
            // Save
            SaveLoadForm.SetVariantsSaveInComboBox(this.Name, ref comboBoxLoad);// заполнение вариантами сохранений

            // Автозаполнение 
            // Заполнение даты вынесения решения текущими датами
            DateTime dateTimeNow = DateTime.Now;
            string month = HandlerTextControls.MonthInString(dateTimeNow.Month); // месяц
            string year = dateTimeNow.Year.ToString(); // год
            comboBoxDateOfOrderMonth.Text = month;
            comboBoxDateOfOrderYear.Text = year;
            // автоматом выставление года, когда было вынесено постановление
            comboBoxDateCrimeYear.Text = (dateTimeNow.Year - 1).ToString();
        }
        #endregion Инициализация


        #region Автоматическое заполнение полей
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


        #region Приватные методы
        /// <summary>
        /// метод возвращающий формулировку в случае если явился или не явился или null если не правильные параметры
        /// </summary>
        /// <param name="appeared">Если явился, то true</param>
        /// <param name="ItIsWoomen">Если женщина, то true</param>
        /// <returns>метод возвращающий формулировку в случае если явился или не явился или null</returns>
        private string AppearedOrNot(bool appeared, bool ItIsWoomen)
        {
            if (appeared && ItIsWoomen)
                return "явилась, вину признала";
            if (appeared == false && ItIsWoomen)
                return "не явилась, извещена надлежащим образом о времени и месте судебного " +
                    "заседания. Учитывая, что имеются данные о ее надлежащем извещении о месте и " +
                    "времени рассмотрения дела и от нее не поступило ходатайство об отложении " +
                    "рассмотрения дела, суд на основании ч.2 ст.25.1 КоАП РФ считает возможным " +
                    "рассмотреть дело в ее отсутствие";

            if (appeared && ItIsWoomen == false)
                return "явился, вину признал";
            if (appeared == false && ItIsWoomen == false)
                return "не явился, извещен надлежащим образом о времени и месте судебного " +
                    "заседания. Учитывая, что имеются данные о его надлежащем извещении о месте и " +
                    "времени рассмотрения дела и от него не поступило ходатайство об отложении " +
                    "рассмотрения дела, суд на основании ч.2 ст.25.1 КоАП РФ считает возможным " +
                    "рассмотреть дело в его отсутствие";

            return null;
        }

        #endregion приватные методы


        // СГЕНЕРИРОВАТЬ WORD
        private void button1_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GenerationWord.StandartListValueControl(Controls);


            //Явился или не явился
            string resultAppeared = AppearedOrNot(checkBoxAppearedOrNot.Checked, radioButtonSexWoomen.Checked);
            GenerationWord.AddValueControl(ref controlArrayToString, resultAppeared, "#-1"); // в ручную добавляем новый ключ

            // Делаем инициалы ФИО
            string initials = "";
            if (textBoxFullNameNameIvana.Text.Length > 2 && textBoxFullNamePatronymicIvanovicha.Text.Length > 2) // Если правильно заполнили имя и отчество
                initials = textBoxFullNameNameIvana.Text.Remove(1) + "." + textBoxFullNamePatronymicIvanovicha.Text.Remove(1) + ".";
            GenerationWord.AddValueControl(ref controlArrayToString, initials, "#-2"); // в ручную добавляем новый ключ

            // Получатель и УИН
            string requisitesGAI = ""; // полные реквзииты ГАИ
            Db.GetRequisitesGAI(comboBoxRecipientGIBDD.Text, out requisitesGAI, out string standartYIN); // Заполняем переменные с реквизитами
            requisitesGAI += textBoxYINgIBDD.Text; // Добавляем оставшийся УИН
            GenerationWord.AddValueControl(ref controlArrayToString, requisitesGAI, "#-3"); // в ручную добавляем новый ключ

            // Сумма штрафа умножить на два
            try
            {
                int sumFine = Convert.ToInt32(comboBoxFine.Text) * 2; // сумма штрафа
                GenerationWord.AddValueControl(ref controlArrayToString, sumFine.ToString(), "#-4"); // в ручную добавляем новый ключ
            }catch
            {
                MessageBox.Show("Некорректно введена сумма штрафа");
                return;
            }

            // Вставляем название в буфер обмена
            string numCase = ""; // номер дела
            if (textBoxClipPutNum.Text != null && textBoxClipPutNum.Text != "")
                numCase = textBoxClipPutNum.Text + "  ";
            Clipboard.SetText(numCase + textBoxClipPutName.Text + "  " + this.Text);

            // Если пол мужской, то один шаблон, если женский, то другой
            if (radioButtonSexMen.Checked)
            {
                // Сгенерировать ворд
                GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "ст 20.25 Муж", controlArrayToString);
            }
            else
            {
                // Сгенерировать ворд
                GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "ст 20.25 Жен", controlArrayToString);
            }

        }


    }
}
