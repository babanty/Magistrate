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
    public partial class NewGAI : Form
    {
        public const string NameColumn = "Details of GAI"; // название поля для ввода с вариантами

        public NewGAI()
        {
            InitializeComponent();
            RestartVariants();
        }

        // добавить
        private void button2_Click(object sender, EventArgs e)
        {
            // Если такое название уже имеется и производится замена
            if(IsHaveListNameGIBDD(textBoxNameGIBDD.Text))
                DeleteVariant(textBoxNameGIBDD.Text);

            string text = "";
            text += textBoxNameGIBDD.Text + "$";
            text += textBoxFullRequisites.Text + "$";
            text += textBoxStandartYIN.Text;


            Db.SetValueInColumn(text, NameColumn);
            
            // обнуляем поля для ввода
            textBoxFullRequisites.Text = "";
            textBoxNameGIBDD.Text = "";
            textBoxStandartYIN.Text = "";
            RestartVariants(); // Перезаполняем лист с вариантами

            //Распарсить таким макаром "вася/петя/саша".Split('/');
            //string[] {"вася", "петя", "саша"}
        }

        /// <summary> Есть ли такой же вариант среди названи ГИБДД </summary>
        private bool IsHaveListNameGIBDD(string name)
        {
            foreach(string nameInListBox in listBoxNameGIBDD.Items)
            {
                if (name == nameInListBox)
                    return true;
            }

            return false;
        }


        // удалить
        private void button1_Click(object sender, EventArgs e)
        {
            DeleteVariant(listBoxNameGIBDD.Text);
            RestartVariants(); // Перезаполняем лист с вариантами
        }

        /// <summary>
        /// Удалить вариант
        /// </summary>
        /// <param name="name">сокращенное название ГИБДД</param>
        private void DeleteVariant(string name)
        {
            if (name == null || name == "")
                return;

            List<string> array = Db.GetColumn(NameColumn); // возвращает список вариантов записанных в поле для ввода

            // Ищем удаляемую строку
            foreach(var variant in array)
            {
                var substring = variant.Substring(0, name.Length);
                if (substring == name)
                    Db.DeleteValueInColumn(variant, NameColumn);
            }
        }

        // Перезаполняем лист с вариантами
        private void RestartVariants()
        {
            // Заполняем вариантами внутри полей лист бокс
            List<string> array = Db.GetColumn(NameColumn); // возвращает список вариантов записанных в поле для ввода
            listBoxNameGIBDD.Items.Clear(); // обнуляет лист бокс
            listBoxNameGIBDD.Items.AddRange(Db.GetAllShortRequisitesGAI().ToArray()); // закидываем варианты в листбокс
        }


        // Изменить
        private void buttonChange_Click(object sender, EventArgs e)
        {
            var nameForChange = listBoxNameGIBDD.Text; // Имя того, что будем изменять

            textBoxNameGIBDD.Text = nameForChange; // Краткое имя

            Db.GetRequisitesGAI(nameForChange, out string requisites, out string standartYIN);

            textBoxFullRequisites.Text = requisites; // Полные реквизиты
            textBoxStandartYIN.Text = standartYIN; // Стандартный УИН
        }
    }
}
