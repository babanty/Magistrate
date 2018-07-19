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
    public partial class Article1533 : Form
    {
        private string nameForm; // название формы, идентичен ее названию ее класса


        #region Инициализация

        public Article1533()
        {
            InitializeComponent();

            // Заполнение полей ввода вариантами
            FormController.SetPropertiesComboBox(ref comboBoxPlaceOfBirth, NamePropertiesForComboBox.PlaceBirth); // Заполняем Населенный пункт, место рождения
            FormController.SetPropertiesComboBox(ref comboBoxResidenceCity, NamePropertiesForComboBox.PlaceResidenceCity); // Место жителства населенный пункт
            FormController.SetPropertiesComboBox(ref comboBoxResidenceStreet, NamePropertiesForComboBox.PlaceResidenceStreet); // Место жителства улица
            FormController.SetPropertiesComboBox(ref comboBoxResidenceHouse, NamePropertiesForComboBox.PlaceResidenceHouse); // Место жителства дом

            // Автозаполнение 
            FormController.SetStandartParamsInControls
    (nameForm,
    ref comboBoxLoad,               // заполнение вариантами сохранений
    ref comboBoxDateOfOrderMonth,   // заполнение месяца вынесения решения
    ref comboBoxDateOfOrderYear);   // заполнение года вынесения решения

            nameForm = this.GetType().ToString(); // Название формы

            DateTime dateTimeNow = DateTime.Now;
            //СЗВ-м за какой год
            comboBoxSZVMYear.Text = (dateTimeNow.Year - 1).ToString();
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


            //Явился или не явился
            string resultAppeared = FormController.AppearedOrNotExplanation(checkBoxAppearedOrNot.Checked, radioButtonSexWoomen.Checked);
            GeneratorWord.AddValueControl(ref controlArrayToString, resultAppeared, "#-1"); // в ручную добавляем новый ключ

            // Делаем инициалы ФИО
            string initials = FormController.GetInitials(textBoxFullNameNameIvana.Text, textBoxFullNamePatronymicIvanovicha.Text);
            GeneratorWord.AddValueControl(ref controlArrayToString, initials, "#-2"); // в ручную добавляем новый ключ

            // Указание если явился ,кроме признания своей вины,
            if (checkBoxAppearedOrNot.Checked)
            {
                GeneratorWord.AddValueControl(ref controlArrayToString, ", кроме признания своей вины,", "#-3"); // в ручную добавляем новый ключ
            }
            else
            {
                GeneratorWord.AddValueControl(ref controlArrayToString, "", "#-3"); // в ручную добавляем новый ключ
            }


            // Вставляем название в буфер обмена
            FormController.ClipPutNameWord(textBoxClipPutNum.Text, textBoxClipPutName.Text, this.Text);

            FormController.GenerateWord(controlArrayToString, radioButtonSexMen.Checked, "ст 15.33 Муж", "ст 15.33 Жен");
        }
    }
}
