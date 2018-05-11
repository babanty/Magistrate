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
        public Article1215()
        {
            InitializeComponent();

            // Заполнение полей ввода вариантами
            Db.SetPropertiesComboBox(ref comboBox10, NamePropertiesForComboBox.МестоРождения); // Заполняем Населенный пункт, место рождения
            Db.SetPropertiesComboBox(ref comboBox13, NamePropertiesForComboBox.МестоЖительстваГород); // Место жителства населенный пункт
            Db.SetPropertiesComboBox(ref comboBox14, NamePropertiesForComboBox.МестоЖительстваУлица); // Место жителства улица
            Db.SetPropertiesComboBox(ref comboBox15, NamePropertiesForComboBox.МестоЖительстваДом); // Место жителства дом
            Db.SetPropertiesComboBox(ref comboBox22, NamePropertiesForComboBox.МаркаАвто); // Марка авто
            Db.SetPropertiesComboBox(ref comboBox23, NamePropertiesForComboBox.НазваниеТрассы); // Трасса

            comboBox24.Items.AddRange(Db.GetAllShortRequisitesGAI().ToArray()); // Получатель бабулесов, ГАИ, заполняем варианты

            SaveLoadForm.SetVariantsSaveInComboBox(nameForm , ref comboBoxLoad);// заполнение вариантами сохранений

            // Автозаполнение 
            // Заполнение даты вынесения решения текущими датами
            DateTime dateTimeNow = DateTime.Now;
            string month = HandlerTextControls.MonthInString(dateTimeNow.Month); // месяц
            string year = dateTimeNow.Year.ToString(); // год
            comboBox3.Text = month;
            comboBox4.Text = year;
        }



        // Перенос фамилии
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = textBox5.Text;
            textBox6.Text = textBox5.Text;

            // название при сохранении
            textBoxClipPutName.Text = textBox5.Text;
        }


        // Выбранный участок, автоматически подставляет кто судья
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


        // Автоматически переносит полное ФИО на другие поля
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox11.Text = textBox1.Text;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox10.Text = textBox2.Text;
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox9.Text = textBox3.Text;
        }


        // разблочить поле для ввода альтернативных вариантов нарушений
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
                textBox13.Enabled = true;

            if (checkBox4.Checked == false)
                textBox13.Enabled = false;
        }


        // Выводит полную информацию что полчается с альтернативным вариантом нарушеня
        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if(checkBox4.Checked == false) // Если альтернатива не чекнута 
            {
                label33.Text = "";
                return;
            }

            string text = GetTextWhithAlternative(textBox13.Text);

            label33.Text = text;
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            label33.Text = GetTextWhithAlternative();
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            label33.Text = GetTextWhithAlternative();
        }


        /// <summary>
        /// Возвращает текст со всеми вариантами того как нарушил ПДД
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string GetTextWhithAlternative(string text = "")
        {
            string returnStr = "";

            if (checkBox2.Checked)
                returnStr += " " + checkBox2.Text;

            if (checkBox3.Checked)
                returnStr += " " + checkBox3.Text;

            if(text != "" && text != null)
                returnStr += " " + text;

            returnStr = "обгон транспортного средства в нарушение п.1.3 ПДД РФ " + returnStr + 
                ", с выездом на полосу встречного движения.";

            return returnStr;
        }

        // Нажали "у дома"
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            // Изменение текста контролов
            label20.Text = "№ дома, например: 5А";
            label19.Text = "Улица, например: ул. Клементьевская";
            label22.Text = "Город, есть выбор";

            comboBox23.Items.Clear(); // очищаем варианты для городов
            Db.SetPropertiesComboBox(ref comboBox23, NamePropertiesForComboBox.МестоПравонарушения); // Место жителства населенный пункт
        }

        // Нажали "на трассе"
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            // Изменение текста контролов
            label20.Text = "Км. трассы, на которой было правонарушение ";
            label19.Text = "М. трассы, на которой было правонарушение";
            label22.Text = "Трасса, на которой было правонарушение";

            comboBox23.Items.Clear(); // очищаем варианты для городов
            Db.SetPropertiesComboBox(ref comboBox23, NamePropertiesForComboBox.НазваниеТрассы); // Место жителства населенный пункт
        }


        // Набивает УИН получателя для продолжения
        private void comboBox24_SelectedIndexChanged(object sender, EventArgs e)
        {
            string yin = ""; // Статичная часть УИНа
            Db.GetRequisitesGAI(comboBox24.Text, out string requisitesGAI, out yin); // Заполняем переменные с реквизитами

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
            textBox8.Text = textBox8.Text.ToUpper();
        }


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
            if (textBox2.Text.Length > 2 && textBox3.Text.Length > 2) // Если правильно заполнили имя и отчество
                initials = textBox2.Text.Remove(1) + "." + textBox3.Text.Remove(1) + ".";
            GenerationWord.AddValueControl(ref controlArrayToString, initials, "#-2"); // в ручную добавляем новый ключ


            // Тип нарушения, с пересечением и проч.
            string textOffence = GetTextWhithAlternative(textBox13.Text);
            GenerationWord.AddValueControl(ref controlArrayToString, textOffence, "#-3"); // в ручную добавляем новый ключ


            // На трассе или у дома
            string addressViolation = ""; // адрес нарушения
            if (radioButton3.Checked) // если на трассе
                addressViolation = "на " + textBox7.Text + " км+" + textBox4.Text + " м а/дороги " + comboBox23.Text;
            if (radioButton4.Checked) // если у дома
                addressViolation = "у дома № " + textBox7.Text + " по " + textBox4.Text + " " + comboBox23.Text;
            GenerationWord.AddValueControl(ref controlArrayToString, addressViolation, "#-5"); // в ручную добавляем новый ключ


            // Получатель и УИН
            string requisitesGAI = ""; // полные реквзииты ГАИ
            Db.GetRequisitesGAI(comboBox24.Text, out requisitesGAI, out string standartYIN); // Заполняем переменные с реквизитами
            requisitesGAI += textBox14.Text; // Добавляем оставшийся УИН
            GenerationWord.AddValueControl(ref controlArrayToString, requisitesGAI, "#-6"); // в ручную добавляем новый ключ


            // Вставляем название в буфер обмена
            Clipboard.SetText(textBoxClipPutNum.Text + "  " + textBoxClipPutName.Text + "  " + this.Text);

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

    }
}
