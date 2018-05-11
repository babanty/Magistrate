using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Magistrate
{
    public static class SaveLoadForm
    {
        static Ini ini = new Ini("SaveForm"); // Создаем инифайл для последующе обработки autoit exe-шником

        /// <summary>
        /// Сохранить значение полей ввода
        /// </summary>
        /// <param name="nameForm">Назавине формы</param>
        /// <param name="Params">Массив строк впихиваемых вместо ключей</param>
        public static void SaveForm(string nameForm, List<ValueControl> Params)
        {
            string nameSectionValue = nameForm; // Название секции в ini файле, в которой будут храниться ini-ключи-значения полей ввода

            try // Очищаем старую информацию в секциях ini 
            {
                ini.DeleteSection(nameSectionValue);
            }
            catch { }

            try
            {
                foreach (ValueControl Param in Params) //Записывает все ключи полей
                    ini.Write(nameSectionValue, Param.Key, Param.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ПРОБЛЕМА С ЗАПИСЬЮ КЛЮЧЕЙ В ИНИ-ФАЙЛ ДЛЯ СКРИПТА AUTOIT: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Возвращает измененный лист контролов с загруженным текстом. Возврат этого метода необходимо
        /// перенести в контролы формы через for(){} т.к. через ref Controls передать нельзя (свойство только get)
        /// </summary>
        /// <param name="nameForm">Имя формы</param>
        /// <param name="nameSave">Имя сохранения</param>
        /// <param name="controls">массив контролов, обычно в form это переменная Controls</param>
        /// <returns></returns>
        public static Control.ControlCollection LoadForm(string nameForm, string nameSave, Control.ControlCollection controls)
        {
            // имя сохранения
            string fuulNameSave = nameSave;
            if (fuulNameSave == "" || fuulNameSave == null) // если пустой поле
                fuulNameSave = "имя сохранения";
            string column = nameForm + "$" + fuulNameSave;

            string text = ""; // текст для формы
            string tagIndex = ""; // индекс в ini-файле
            foreach (Control control in controls)
            {
                // Делаем правильный индекс - ключ для ini файла
                if (control.Tag == null)
                    continue;
                tagIndex = control.Tag.ToString();
                if (tagIndex.Length == 1)
                    tagIndex = "0" + tagIndex;
                tagIndex = "#" + tagIndex;

                // Считываем и заполняем
                text = ini.IniReadKey(column, tagIndex);
                if (text != null && text != "")
                    control.Text = text;
            }

            return controls;
        }

        /// <summary>
        /// Заполнить комбобокс вариантами сохранений
        /// </summary>
        /// <param name="nameForm">Название формы</param>
        /// <param name="comboBox">Комбобокс, который необходимо заполнить</param>
        public static void SetVariantsSaveInComboBox(string nameForm, ref ComboBox comboBox)
        {
            var getColumnsName = GetColumnsName(); // вернуть названия всех колонок
            List<string> str = new List<string>(); // создаем локальное хранилище строк
            if (getColumnsName != null) // если файл не пустой
            {
                str = getColumnsName.ToList(); // заполняем локальное хранилище вариантами
            }
            else // значит нет файла save.ini
            {
                return;
            }
            

            // Распарсиваем, вынимаем только название т.к. строка приходит в виде "названиеФормы$Имя сохранения"
            // Исключаем варианты сохранений не относящиеся к заполняемой форме
            string[] strParse;
            List<string> returnStr = new List<string>();
            foreach(string strin in str)
            {
                strParse = strin.Split('$');
                if (strParse != null && strParse.Length == 2) // если сохранение корректно
                    if (strParse[0] == nameForm) // Если относится к текущей форме
                        returnStr.Add(strParse[1]);
            }

            if (returnStr != null)
            {
                // сортировка по возрастанию, LINQ запрос
                var sortedStr = from s in returnStr // определяем каждый объект из str как s
                                orderby s  // упорядочиваем по возрастанию
                                select s; // выбираем объект
                comboBox.Items.AddRange(sortedStr.ToArray());
            }
        }

        /// <summary>
        /// Удалить сохранение
        /// </summary>
        /// <param name="nameSave">Имя сохраненич</param>
        /// <param name="nameForm">Имя формы</param>
        public static void DeleteSave(string nameSave, string nameForm)
        {
            string nameSection = nameForm + "$" + nameSave;
            ini.DeleteSection(nameSection);
        }

        /// <summary>
        /// Вернуть названия всех колонок
        /// </summary>
        private static string[] GetColumnsName()
        {
            return ini.GetSectionNames();
        }


        /// <summary>
        /// Возвращает контрол по его таб индексу
        /// </summary>
        /// <param name="controls">коллекция контролов</param>
        /// <param name="tabIndex">номер таб индекса</param>
        /// <returns></returns>
        private static Control GetControlAtTabIndex(Control.ControlCollection controls, int tabIndex)
        {
            foreach (Control control in controls)
            {
                if (control.TabIndex == tabIndex)
                    return control;
            }

            return null;
        }
    }
}
