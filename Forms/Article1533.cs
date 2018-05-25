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
    public partial class Article1533 : Form
    {

        #region Инициализация
        public Article1533()
        {
            InitializeComponent();

            // Заполнение полей ввода вариантами
            Db.SetPropertiesComboBox(ref comboBoxPlaceOfBirth, NamePropertiesForComboBox.МестоРождения); // Заполняем Населенный пункт, место рождения
            Db.SetPropertiesComboBox(ref comboBoxResidenceCity, NamePropertiesForComboBox.МестоЖительстваГород); // Место жителства населенный пункт
            Db.SetPropertiesComboBox(ref comboBoxResidenceStreet, NamePropertiesForComboBox.МестоЖительстваУлица); // Место жителства улица
            Db.SetPropertiesComboBox(ref comboBoxResidenceHouse, NamePropertiesForComboBox.МестоЖительстваДом); // Место жителства дом
            // Save
            SaveLoadForm.SetVariantsSaveInComboBox(nameForm, ref comboBoxLoad);// заполнение вариантами сохранений

            // Автозаполнение 
            // Заполнение даты вынесения решения текущими датами
            DateTime dateTimeNow = DateTime.Now;
            string month = HandlerTextControls.MonthInString(dateTimeNow.Month); // месяц
            string year = dateTimeNow.Year.ToString(); // год
            comboBoxDateOfOrderMonth.Text = month;
            comboBoxDateOfOrderYear.Text = year;
            //СЗВ-м за какой год
            comboBoxSZVMYear.Text = (dateTimeNow.Year - 1).ToString();
        }
        #endregion Инициализация

        
        #region Автоматическое заполнение полей
       
        // Выбранный участок, автоматически подставляет кто судья
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPlotNumber.Text == PropertiesMyApp.GetPropertiesValue(TypeProperties.PlaceNum))
            {
                comboBoxWhoIsJudge.Text = "Мировой судья";
            }else
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


        // авотматически заполняет расположение организации
        private void comboBox17_SelectedIndexChanged(object sender, EventArgs e)
        {
            string placeOrganization = "";

            placeOrganization += comboBoxResidenceCountry.Text + ", "; // Зарегестрирован по адресу. Страна
            placeOrganization += comboBoxResidenceRegion.Text + ", "; // Область, например: Московская область
            placeOrganization += comboBoxResidenceCity.Text + ", "; // Населенный пункт, например: г.Сергиев Посад
            placeOrganization += comboBoxResidenceStreet.Text + ", "; // Поселок, улица, проспект и т.д., напрмер: ул.Громова
            placeOrganization += comboBoxResidenceHouse.Text; // Дом, квартира и т.д., например: д.20, кв 60

            textBoxPlaceOrganization.Text = placeOrganization;
        }

        // автоматическое заполнение следющего месяца 
        private void comboBox18_SelectedIndexChanged(object sender, EventArgs e)
        {
            string valueControl = comboBoxSZVMMonth.Text;
            switch (valueControl)
            {
                case "январь":
                    comboBoxSZVMGoodMonth.Text = "02";
                break;
                case "февраль":
                    comboBoxSZVMGoodMonth.Text = "03";
                    break;
                case "март":
                    comboBoxSZVMGoodMonth.Text = "04";
                    break;
                case "апрель":
                    comboBoxSZVMGoodMonth.Text = "05";
                    break;
                case "май":
                    comboBoxSZVMGoodMonth.Text = "06";
                    break;
                case "июнь":
                    comboBoxSZVMGoodMonth.Text = "07";
                    break;
                case "июль":
                    comboBoxSZVMGoodMonth.Text = "08";
                    break;
                case "август":
                    comboBoxSZVMGoodMonth.Text = "09";
                    break;
                case "сентябрь":
                    comboBoxSZVMGoodMonth.Text = "10";
                    break;
                case "октябрь":
                    comboBoxSZVMGoodMonth.Text = "11";
                    break;
                case "ноябрь":
                    comboBoxSZVMGoodMonth.Text = "12";
                    break;
                case "декабрь":
                    comboBoxSZVMGoodMonth.Text = "01";
                    break;
            }
        }

        // автоматическое выставление года "не позднее"
        private void comboBox16_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxSZVMFactYear.Text = comboBoxSZVMYear.Text; // Фактически отдал сзв-м в таком то году, авто перенос

            comboBoxSZVMGoodYear.Text = comboBoxSZVMYear.Text;
            // Если декабрь, то год пишется следующий
            if(comboBoxSZVMMonth.Text == "декабрь") 
                comboBoxSZVMGoodYear.Text = (int.Parse(comboBoxSZVMYear.Text)+1).ToString(); // преобразовываем год в число, прибавляем 1 и возвращаем обратно в строку

        }
        #endregion Автоматическое заполнение полей 


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
                    "рассмотреть дело в ее отсутствие.";

            if (appeared && ItIsWoomen == false)
                return "явился, вину признал";
            if (appeared == false && ItIsWoomen == false)
                return "не явился, извещен надлежащим образом о времени и месте судебного " +
                    "заседания. Учитывая, что имеются данные о его надлежащем извещении о месте и " +
                    "времени рассмотрения дела и от него не поступило ходатайство об отложении " +
                    "рассмотрения дела, суд на основании ч.2 ст.25.1 КоАП РФ считает возможным " +
                    "рассмотреть дело в его отсутствие.";

            return null;
        }
        #endregion Приватные методы


        #region Сохранение

        // Сохранить заполненные поля
        string nameForm = "Article1533";
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


            //Явился или не явился
            string resultAppeared = AppearedOrNot(checkBoxAppearedOrNot.Checked, radioButtonSexWoomen.Checked);
            GenerationWord.AddValueControl(ref controlArrayToString, resultAppeared, "#-1"); // в ручную добавляем новый ключ

            // Делаем инициалы ФИО
            string initials = "";
            if (textBoxFullNameNameIvana.Text.Length > 2 && textBoxFullNamePatronymicIvanovicha.Text.Length > 2) // Если правильно заполнили имя и отчество
                initials = textBoxFullNameNameIvana.Text.Remove(1) + "." + textBoxFullNamePatronymicIvanovicha.Text.Remove(1) + ".";
            GenerationWord.AddValueControl(ref controlArrayToString, initials, "#-2"); // в ручную добавляем новый ключ

            // Указание если явился ,кроме признания своей вины,
            if (checkBoxAppearedOrNot.Checked)
            {
                GenerationWord.AddValueControl(ref controlArrayToString, ", кроме признания своей вины,", "#-3"); // в ручную добавляем новый ключ
            }
            else
            {
                GenerationWord.AddValueControl(ref controlArrayToString, "", "#-3"); // в ручную добавляем новый ключ
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
                GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "ст 15.33 Муж", controlArrayToString);
            }
            else
            {
                // Сгенерировать ворд
                GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "ст 15.33 Жен", controlArrayToString);
            }
        }
    }
}
