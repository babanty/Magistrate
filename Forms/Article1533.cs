﻿using System;
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
        public Article1533()
        {
            InitializeComponent();

            // Заполнение полей ввода вариантами
            Db.SetPropertiesComboBox(ref comboBox10, "mesto rogdeniya"); // Заполняем Населенный пункт, место рождения
            Db.SetPropertiesComboBox(ref comboBox13, "mesto jitelstva gorod"); // Место жителства населенный пункт
            Db.SetPropertiesComboBox(ref comboBox14, "mesto jitelstva ylitsa"); // Место жителства улица
            Db.SetPropertiesComboBox(ref comboBox15, "mesto jitelstva dom"); // Место жителства дом
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GenerationWord.StandartListValueControl(Controls);


            //Явился или не явился
            string resultAppeared = AppearedOrNot(comboBox23.Text);
            if(resultAppeared == null)
            {
                MessageBox.Show("Не правильно задано значение Явился или не Явился, следует выбирать из предлаемых вариантов");
                return;
            }
            GenerationWord.AddValueControl(ref controlArrayToString, resultAppeared, "#-1"); // в ручную добавляем новый ключ

            // Делаем инициалы ФИО
            string initials = "";
            if (textBox2.Text.Length > 2 && textBox3.Text.Length > 2) // Если правильно заполнили имя и отчество
                initials = textBox2.Text.Remove(1) + "." + textBox3.Text.Remove(1) + ".";
            GenerationWord.AddValueControl(ref controlArrayToString, initials, "#-2"); // в ручную добавляем новый ключ

            // Указание если явился ,кроме признания своей вины,
            if (comboBox23.Text == "Явился" || comboBox23.Text == "Явилась")
            {
                GenerationWord.AddValueControl(ref controlArrayToString, ", кроме признания своей вины,", "#-3"); // в ручную добавляем новый ключ
            } else
            {
                GenerationWord.AddValueControl(ref controlArrayToString, "", "#-3"); // в ручную добавляем новый ключ
            }


            // Если пол мужской, то один шаблон, если женский, то другой
            if (comboBox22.Text == "Муж")
            {
                // Сгенерировать ворд
                GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "ст 15.33 Муж", controlArrayToString);
            }
            else if (comboBox22.Text == "Жен")
            {
                // Сгенерировать ворд
                GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "ст 15.33 Жен", controlArrayToString);
            }
            else
            {
                MessageBox.Show("Не правильно выбран пол, выберите либо \"Муж\" либо \"Жен\"");
            }
        }



        // Выбранный участок, автоматически подставляет кто судья
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == PropertiesMyApp.GetPropertiesValue(TypeProperties.PlaceNum))
            {
                comboBox5.Text = "Мировой судья";
            }else
            {
                comboBox5.Text = "И.о. мирового судьи";
            }
        }


        // автоматически переносит фамилию на другие поля ввода
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = textBox5.Text;
            textBox6.Text = textBox5.Text;
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


        // авотматически заполняет расположение организации
        private void comboBox17_SelectedIndexChanged(object sender, EventArgs e)
        {
            string placeOrganization = "";

            placeOrganization += comboBox11.Text + ", "; // Зарегестрирован по адресу. Страна
            placeOrganization += comboBox12.Text + ", "; // Область, например: Московская область
            placeOrganization += comboBox13.Text + ", "; // Населенный пункт, например: г.Сергиев Посад
            placeOrganization += comboBox14.Text + ", "; // Поселок, улица, проспект и т.д., напрмер: ул.Громова
            placeOrganization += comboBox15.Text; // Дом, квартира и т.д., например: д.20, кв 60

            textBox4.Text = placeOrganization;
        }

        // указание явился или не явился, зависит от пола
        private void comboBox22_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox22.Text == "Муж")
            {
                comboBox23.Text = "Явился";
            }
            else if (comboBox22.Text == "Жен")
            {
                comboBox23.Text = "Явилась";
            }
        }

        // автоматическое заполнение следющего месяца 
        private void comboBox18_SelectedIndexChanged(object sender, EventArgs e)
        {
            string valueControl = comboBox18.Text;
            switch (valueControl)
            {
                case "январь":
                    comboBox20.Text = "02";
                break;
                case "февраль":
                    comboBox20.Text = "03";
                    break;
                case "март":
                    comboBox20.Text = "04";
                    break;
                case "апрель":
                    comboBox20.Text = "05";
                    break;
                case "май":
                    comboBox20.Text = "06";
                    break;
                case "июнь":
                    comboBox20.Text = "07";
                    break;
                case "июль":
                    comboBox20.Text = "08";
                    break;
                case "август":
                    comboBox20.Text = "09";
                    break;
                case "сентябрь":
                    comboBox20.Text = "10";
                    break;
                case "октябрь":
                    comboBox20.Text = "11";
                    break;
                case "ноябрь":
                    comboBox20.Text = "12";
                    break;
                case "декабрь":
                    comboBox20.Text = "01";
                    break;
            }
        }

        // автоматическое выставление года "не позднее"
        private void comboBox16_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox19.Text = comboBox16.Text;
            // Если декабрь, то год пишется следующий
            if(comboBox18.Text == "декабрь") 
                comboBox19.Text = (int.Parse(comboBox16.Text)+1).ToString(); // преобразовываем год в число, прибавляем 1 и возвращаем обратно в строку

        }


        /// <summary>
        /// метод возвращающий формулировку в случае если явился или не явился или null если не правильные параметры
        /// </summary>
        /// <param name="value">Тут может быть значение только "Явился","Не явился","Явилась", "Не явилась"</param>
        /// <returns>метод возвращающий формулировку в случае если явился или не явился или null</returns>
        private string AppearedOrNot(string value)
        {
            if(value == "Явился")
            {
                return "явился, вину признал";
            }else if(value == "Не явился")
            {
                return "не явился, извещен надлежащим образом о времени и месте судебного " +
                    "заседания. Учитывая, что имеются данные о его надлежащем извещении о месте и " +
                    "времени рассмотрения дела и от него не поступило ходатайство об отложении " +
                    "рассмотрения дела, суд на основании ч.2 ст.25.1 КоАП РФ считает возможным " +
                    "рассмотреть дело в его отсутствие.";
            }
            else if (value == "Явилась")
            {
                return "явилась, вину признала";
            }
            else if (value == "Не явилась")
            {
                return "не явилась, извещена надлежащим образом о времени и месте судебного " +
                    "заседания. Учитывая, что имеются данные о ее надлежащем извещении о месте и " +
                    "времени рассмотрения дела и от нее не поступило ходатайство об отложении " +
                    "рассмотрения дела, суд на основании ч.2 ст.25.1 КоАП РФ считает возможным " +
                    "рассмотреть дело в ее отсутствие.";
            }

            return null;
        }
    }
}