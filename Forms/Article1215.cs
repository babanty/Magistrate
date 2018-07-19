using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Magistrate.FormLogic;
using Magistrate.PropertiesForm;

namespace Magistrate.Forms
{
    public partial class Article1215 : Form
    {

        private string nameForm; // название формы, идентичен ее названию ее класса

        // конструктор и инициализация контролов
        public Article1215()
        {
            InitializeComponent(); 

            // Заполнение полей ввода вариантами
            FormController.SetPropertiesComboBox(ref comboBoxPlaceOfBirth, NamePropertiesForComboBox.PlaceBirth); // Заполняем Населенный пункт, место рождения
            FormController.SetPropertiesComboBox(ref comboBoxResidenceCity, NamePropertiesForComboBox.PlaceResidenceCity); // Место жителства населенный пункт
            FormController.SetPropertiesComboBox(ref comboBoxResidenceStreet, NamePropertiesForComboBox.PlaceResidenceStreet); // Место жителства улица
            FormController.SetPropertiesComboBox(ref comboBoxResidenceHouse, NamePropertiesForComboBox.PlaceResidenceHouse); // Место жителства дом
            FormController.SetPropertiesComboBox(ref comboBoxCarBrand, NamePropertiesForComboBox.CarBrand); // Марка авто
            FormController.SetPropertiesComboBox(ref comboBoxRoute, NamePropertiesForComboBox.NameTrack); // Трасса
            FormController.SetPropertiesComboBox(ref comboBoxRecipientGIBDD, NamePropertiesForComboBox.DetailsTrafficPolice);

            FormController.SetStandartParamsInControls
                (nameForm,
                ref comboBoxLoad,               // заполнение вариантами сохранений
                ref comboBoxDateOfOrderMonth,   // заполнение месяца вынесения решения
                ref comboBoxDateOfOrderYear);   // заполнение года вынесения решения

            nameForm = this.GetType().ToString(); // Название формы
        }


        #region Автоматическое перезаполнение полей
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
            comboBoxWhoIsJudge.Text = FormController.GetMagistrateOrDeputy(comboBoxPlotNumber.Text);
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
            FormController.SetPropertiesComboBox(ref comboBoxRoute, NamePropertiesForComboBox.PlaceOffence); // Место жителства населенный пункт
        }

        // Нажали "на трассе"
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            // Изменение текста контролов
            label20.Text = "Км. трассы, на которой было правонарушение ";
            label19.Text = "М. трассы, на которой было правонарушение";
            label22.Text = "Трасса, на которой было правонарушение";

            comboBoxRoute.Items.Clear(); // очищаем варианты для городов
            FormController.SetPropertiesComboBox(ref comboBoxRoute, NamePropertiesForComboBox.NameTrack); // Место жителства населенный пункт
        }


        // Набивает УИН получателя для продолжения
        private void comboBox24_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormController.SetInLabelYIN(comboBoxRecipientGIBDD.Text, ref label34);
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

        // СГЕНЕРИРОВАТЬ WORD
        private void button1_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GeneratorWord.StandartListValueControl(Controls);


            //Явился или не явился разъяяснение
            string resultAppeared = FormController.AppearedOrNotExplanation(checkBoxAppearedOrNot.Checked, radioButtonSexWoomen.Checked);
            GeneratorWord.AddValueControl(ref controlArrayToString, resultAppeared, "#-1"); 

            //Явился или не явился смягчающие обстоятельства
            resultAppeared = FormController.AppearedOrNotCircumstances(checkBoxAppearedOrNot.Checked);
            GeneratorWord.AddValueControl(ref controlArrayToString, resultAppeared, "#-4"); 


            // Делаем инициалы ФИО
            string initials = FormController.GetInitials(textBoxFullNameNameIvana.Text, textBoxFullNamePatronymicIvanovicha.Text);
            GeneratorWord.AddValueControl(ref controlArrayToString, initials, "#-2"); 


            // Тип нарушения, с пересечением и проч.
            string textOffence = GetTextWhithAlternative(textBoxViolationAlternative.Text);
            GeneratorWord.AddValueControl(ref controlArrayToString, textOffence, "#-3"); 


            // На трассе или у дома
            string addressViolation = ""; // адрес нарушения
            if (radioButtonOnRoute.Checked) // если на трассе
                addressViolation = "на " + textBoxKmRouteOrNumHouse.Text + " км+" + textBoxMetrRoute.Text + " м а/дороги " + comboBoxRoute.Text;
            if (radioButtonOnHouse.Checked) // если у дома
                addressViolation = "у дома № " + textBoxKmRouteOrNumHouse.Text + " по " + textBoxMetrRoute.Text + " " + comboBoxRoute.Text;
            GeneratorWord.AddValueControl(ref controlArrayToString, addressViolation, "#-5"); 


            // Получатель и УИН
            string requisitesGAI = FormController.GenerateFullRequisitesGAI(comboBoxRecipientGIBDD.Text, textBoxYINgIBDD.Text); // полные реквзииты ГАИ
            GeneratorWord.AddValueControl(ref controlArrayToString, requisitesGAI, "#-6"); 


            // Вставляем название в буфер обмена
            FormController.ClipPutNameWord(textBoxClipPutNum.Text, textBoxClipPutName.Text, this.Text);


            FormController.GenerateWord(controlArrayToString, radioButtonSexMen.Checked, "ст 12.15 Муж", "ст 12.15 Жен");
        }


    }
}
