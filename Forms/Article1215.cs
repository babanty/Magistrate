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
    public partial class Article1215 : Form
    {
        #region Инициализация
        public Article1215()
        {
            InitializeComponent();

            // Заполнение полей ввода вариантами
            Db.SetPropertiesComboBox(ref comboBoxPlaceOfBirth, NamePropertiesForComboBox.МестоРождения); // Заполняем Населенный пункт, место рождения
            Db.SetPropertiesComboBox(ref comboBoxResidenceCity, NamePropertiesForComboBox.МестоЖительстваГород); // Место жителства населенный пункт
            Db.SetPropertiesComboBox(ref comboBoxResidenceStreet, NamePropertiesForComboBox.МестоЖительстваУлица); // Место жителства улица
            Db.SetPropertiesComboBox(ref comboBoxResidenceHouse, NamePropertiesForComboBox.МестоЖительстваДом); // Место жителства дом
            Db.SetPropertiesComboBox(ref comboBoxCarBrand, NamePropertiesForComboBox.МаркаАвто); // Марка авто
            Db.SetPropertiesComboBox(ref comboBoxRoute, NamePropertiesForComboBox.НазваниеТрассы); // Трасса

            comboBoxRecipientGIBDD.Items.AddRange(Db.GetAllShortRequisitesGAI().ToArray()); // Получатель бабулесов, ГАИ, заполняем варианты

            SaveLoadForm.SetVariantsSaveInComboBox(nameForm , ref comboBoxLoad);// заполнение вариантами сохранений

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
        // Перенос фамилии
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBoxFullNameSurNameIvanova.Text = textBoxFullNameSurNameIvanov.Text;
            textBoxFullNameSurNameIvanovu.Text = textBoxFullNameSurNameIvanov.Text;

            // название при сохранении
            textBoxClipPutName.Text = textBoxFullNameSurNameIvanov.Text;
        }


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


        // разблочить поле для ввода альтернативных вариантов нарушений
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxViolationAlternative.Checked)
                textBoxViolationAlternative.Enabled = true;

            if (checkBoxViolationAlternative.Checked == false)
                textBoxViolationAlternative.Enabled = false;
        }


        // Выводит полную информацию что полчается с альтернативным вариантом нарушеня
        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if(checkBoxViolationAlternative.Checked == false) // Если альтернатива не чекнута 
            {
                labelViolationAlternative.Text = "";
                return;
            }

            string text = GetTextWhithAlternative(textBoxViolationAlternative.Text);

            labelViolationAlternative.Text = text;
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            labelViolationAlternative.Text = GetTextWhithAlternative();
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            labelViolationAlternative.Text = GetTextWhithAlternative();
        }


        // Нажали "у дома"
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            // Изменение текста контролов
            label20.Text = "№ дома, например: 5А";
            label19.Text = "Улица, например: ул. Клементьевская";
            label22.Text = "Город, есть выбор";

            comboBoxRoute.Items.Clear(); // очищаем варианты для городов
            Db.SetPropertiesComboBox(ref comboBoxRoute, NamePropertiesForComboBox.МестоПравонарушения); // Место жителства населенный пункт
        }

        // Нажали "на трассе"
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            // Изменение текста контролов
            label20.Text = "Км. трассы, на которой было правонарушение ";
            label19.Text = "М. трассы, на которой было правонарушение";
            label22.Text = "Трасса, на которой было правонарушение";

            comboBoxRoute.Items.Clear(); // очищаем варианты для городов
            Db.SetPropertiesComboBox(ref comboBoxRoute, NamePropertiesForComboBox.НазваниеТрассы); // Место жителства населенный пункт
        }


        // Набивает УИН получателя для продолжения
        private void comboBox24_SelectedIndexChanged(object sender, EventArgs e)
        {
            string yin = ""; // Статичная часть УИНа
            Db.GetRequisitesGAI(comboBoxRecipientGIBDD.Text, out string requisitesGAI, out yin); // Заполняем переменные с реквизитами

            // меняем текст в лэйбле
            if (yin == null)
            {
                label34.Text = "УИН получателя, получатель не опознан";
            }else
            {
                label34.Text = "УИН получателя, продолжить: " + yin;
            }
        }


        // Гос рег знак заглавные буквы
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            textBoxStateRegistrationMark.Text = textBoxStateRegistrationMark.Text.ToUpper();
        }
        #endregion Автоматическое заполнение полей


        #region Приватные методы

        /// <summary>
        /// Возвращает текст со всеми вариантами того как нарушил ПДД
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string GetTextWhithAlternative(string text = "")
        {
            string returnStr = "";

            if (checkBoxViolationP1dot1.Checked)
                returnStr += " " + checkBoxViolationP1dot1.Text;

            if (checkBoxViolationP3dot20.Checked)
                returnStr += " " + checkBoxViolationP3dot20.Text;

            if (text != "" && text != null)
                returnStr += " " + text;

            returnStr = "обгон транспортного средства в нарушение п.1.3 ПДД РФ " + returnStr +
                ", с выездом на полосу встречного движения.";

            return returnStr;
        }


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
        string nameForm = "Article1215";
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
            if (checkBoxAppearedOrNot.Checked)
            {
                resultAppeared = "Смягчающим ответственность обстоятельством суд считает признание им своей вины." +
                    " Отягчающих обстоятельств судом не установлено.";
            }else
            {
                resultAppeared = "Смягчающих и отягчающих обстоятельств судом не установлено.";
            }
            GenerationWord.AddValueControl(ref controlArrayToString, resultAppeared, "#-4"); // в ручную добавляем новый ключ


            // Делаем инициалы ФИО
            string initials = "";
            if (textBoxFullNameNameIvana.Text.Length > 2 && textBoxFullNamePatronymicIvanovicha.Text.Length > 2) // Если правильно заполнили имя и отчество
                initials = textBoxFullNameNameIvana.Text.Remove(1) + "." + textBoxFullNamePatronymicIvanovicha.Text.Remove(1) + ".";
            GenerationWord.AddValueControl(ref controlArrayToString, initials, "#-2"); // в ручную добавляем новый ключ


            // Тип нарушения, с пересечением и проч.
            string textOffence = GetTextWhithAlternative(textBoxViolationAlternative.Text);
            GenerationWord.AddValueControl(ref controlArrayToString, textOffence, "#-3"); // в ручную добавляем новый ключ


            // На трассе или у дома
            string addressViolation = ""; // адрес нарушения
            if (radioButtonOnRoute.Checked) // если на трассе
                addressViolation = "на " + textBoxKmRouteOrNumHouse.Text + " км+" + textBoxMetrRoute.Text + " м а/дороги " + comboBoxRoute.Text;
            if (radioButtonOnHouse.Checked) // если у дома
                addressViolation = "у дома № " + textBoxKmRouteOrNumHouse.Text + " по " + textBoxMetrRoute.Text + " " + comboBoxRoute.Text;
            GenerationWord.AddValueControl(ref controlArrayToString, addressViolation, "#-5"); // в ручную добавляем новый ключ


            // Получатель и УИН
            string requisitesGAI = ""; // полные реквзииты ГАИ
            Db.GetRequisitesGAI(comboBoxRecipientGIBDD.Text, out requisitesGAI, out string standartYIN); // Заполняем переменные с реквизитами
            requisitesGAI += textBoxYINgIBDD.Text; // Добавляем оставшийся УИН
            GenerationWord.AddValueControl(ref controlArrayToString, requisitesGAI, "#-6"); // в ручную добавляем новый ключ


            // Вставляем название в буфер обмена
            string numCase = ""; // номер дела
            if (textBoxClipPutNum.Text != null && textBoxClipPutNum.Text != "")
                numCase = textBoxClipPutNum.Text + "  ";
            Clipboard.SetText(numCase + textBoxClipPutName.Text + "  " + this.Text);

            // Если пол мужской, то один шаблон, если женский, то другой
            if (radioButtonSexMen.Checked)
            {
                // Сгенерировать ворд
                GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "ст 12.15 Муж", controlArrayToString);
            }
            else
            {
                // Сгенерировать ворд
                GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "ст 12.15 Жен", controlArrayToString);
            }
        }
    }
}
