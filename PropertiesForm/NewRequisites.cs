using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Magistrate.PropertiesForm
{
    public partial class NewRequisites : Form
    {
        /// <summary> Тип реквизитов которые будут меняться </summary>
        private TypeRequisites typeRequisites;


        /// <summary>
        /// Конструктор формы для создания/изменения/удаления реквизитов
        /// </summary>
        /// <param name="typeRequisites">Тип реквизтов что будут изменяться</param>
        public NewRequisites(TypeRequisites typeRequisites)
        {
            this.typeRequisites = typeRequisites;

            InitializeComponent();

            UpdateListRequisites();
        }


        // добавить
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            // если такой вариант уже есть
            if (IsHaveListName(textBoxName.Text))
                DeleteOption(textBoxName.Text);

            // получаем адреса реквизитов в базе данных
            GetNameColumn(typeRequisites, 
                out NamePropertiesForComboBox OnlyName, 
                out NamePropertiesForComboBox fullRequisites);

            // Добавляем название в бд
            Db.SetValueInColumn(textBoxName.Text, Db.DecodingEnumPropertiesForComboBox(OnlyName));

            // Добавляем полные реквизиты в бд
            string fullRequisitesInString = textBoxName.Text + ", располагающегося по адресу: "
                + textBoxAddress.Text + ", " + textBoxOtherRequisites.Text;
            Db.SetValueInColumn(fullRequisitesInString, Db.DecodingEnumPropertiesForComboBox(fullRequisites));

            // Обнуляем поля для ввода реквизитов
            textBoxName.Text = "";
            textBoxAddress.Text = "";
            textBoxOtherRequisites.Text = "";

            // обновляем список сохранений
            UpdateListRequisites();
        }

        /// <summary> Есть ли такой же вариант среди названий </summary>
        private bool IsHaveListName(string name)
        {
            foreach (string nameInListBox in listBoxAllNamesRequisites.Items)
            {
                if (name == nameInListBox)
                    return true;
            }

            return false;
        }


        // удалить
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listBoxAllNamesRequisites.Text != null || listBoxAllNamesRequisites.Text != "")
                DeleteOption(listBoxAllNamesRequisites.Text);
        }


        // удалить вариант, метод
        private void DeleteOption(string name)
        {
            GetNameColumn(typeRequisites,
                out NamePropertiesForComboBox OnlyName,
                out NamePropertiesForComboBox fullRequisites); // получаем адреса реквизитов в базе данных

            var nameColumn = Db.DecodingEnumPropertiesForComboBox(OnlyName);
            var fullRequisitesColumn = Db.DecodingEnumPropertiesForComboBox(fullRequisites);


            var fullRequisitesString = FormLogic.FormController.GetFullRequisites(
                name,
                fullRequisites);

            Db.DeleteValueInColumn(name, nameColumn); // Имя
            Db.DeleteValueInColumn(fullRequisitesString, fullRequisitesColumn); // Полные реквизиты


            // обновляем список сохранений
            UpdateListRequisites();
        }


        // изменить
        private void buttonChange_Click(object sender, EventArgs e)
        {
            var nameForChange = listBoxAllNamesRequisites.Text; // Имя того, что будем изменять

            textBoxName.Text = GetFullRequisites(nameForChange);
        }


        /// <summary>
        /// Вернуть полные реквизиты по сокращенному наименованию организации
        /// </summary>
        /// <param name="name">сокращенное наименование организации</param>
        /// <returns></returns>
        private string GetFullRequisites(string name)
        {
            GetNameColumn(typeRequisites,
    out NamePropertiesForComboBox OnlyName,
    out NamePropertiesForComboBox fullRequisites); // получаем адреса реквизитов в базе данных

            var fullRequisitesString = FormLogic.FormController.GetFullRequisites(
    name,
    fullRequisites);

            return fullRequisitesString;
        }


        /// <summary>
        /// Обновить список с вариантами реквизитов, которые можно удалять/изменять
        /// </summary>
        private void UpdateListRequisites()
        {
            // Заполняем вариантами внутри полей лист бокс
            string nameColumn; // название поля для ввода с вариантами

            GetNameColumn(typeRequisites,
                out NamePropertiesForComboBox OnlyName,
                out NamePropertiesForComboBox fullRequisites); // получаем адреса реквизитов в базе данных

            nameColumn = Db.DecodingEnumPropertiesForComboBox(OnlyName);


            List<string> array = Db.GetColumn(nameColumn); // возвращает список вариантов записанных в поле для ввода

            listBoxAllNamesRequisites.Items.Clear(); // обнуляет лист бокс
            listBoxAllNamesRequisites.Items.AddRange(array.ToArray()); // закидываем варианты в листбокс
        }


        /// <summary>
        /// Вернуть название колонок в бд, которые будут изменяться
        /// </summary>
        /// <param name="typeRequisites">Тип реквизитов</param>
        /// <param name="OnlyName">Возвращает только имя</param>
        /// <param name="fullRequisites">Возвращает полные реквизиты</param>
        private void GetNameColumn(TypeRequisites typeRequisites,
            out NamePropertiesForComboBox OnlyName,
            out NamePropertiesForComboBox fullRequisites)
        {
            OnlyName = new NamePropertiesForComboBox(); // заглушка
            fullRequisites = new NamePropertiesForComboBox(); // заглушка

            switch (typeRequisites)
            {
                case TypeRequisites.Bank:
                    OnlyName = NamePropertiesForComboBox.БанкСокращенный;
                    fullRequisites = NamePropertiesForComboBox.БанкПолный;
                    return;
                case TypeRequisites.Communal:
                    OnlyName = NamePropertiesForComboBox.КомуналкаСокращенная;
                    fullRequisites = NamePropertiesForComboBox.КомуналкаПолная;
                    return;
            }
        }
    }


    /// <summary> Тип реквизитов </summary>
    public enum TypeRequisites
    {
        Bank,
        Communal
    }
}
