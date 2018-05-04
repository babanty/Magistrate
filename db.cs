using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Magistrate
{
    /// <summary>
    /// data base база данных
    /// </summary>
    public static class Db
    {
        private static Ini db = new Ini("db");
        /// <summary>
        /// Вернуть значения одной колонки
        /// </summary>
        /// <param name="nameColumn">название колонки</param>
        /// <returns></returns>
        public static List<string> GetColumn(string nameColumn)
        {
            
            string[,] iniReadSection = db.IniReadSection(nameColumn);
            if (iniReadSection == null)
                return null;

            List<string> returnList = new List<string>();
            for (int i = 0; i < iniReadSection.GetLength(0); i++)
            {
                returnList.Add(iniReadSection[i, 1]);
            }

            return returnList;
        }

        /// <summary>
        /// Вернуть значение из колонки
        /// </summary>
        /// <param name="key">Ключ, как правило, это номер</param>
        /// <param name="column">Название колонки</param>
        /// <returns>Вернуть значение из колонки</returns>
        public static string GetValueInColumn(string key, string column)
        {
            return db.IniReadKey(column, key);
        }

        /// <summary>
        /// Добавить значение в колонну, если не указывать ключ значения, то будет его номер в полследовательности
        /// </summary>
        /// <param name="value">добавляемое значение</param>
        /// <param name="column">колонка в которую добавляют значение</param>
        /// <param name="key">ключ, если пустой, то то будет его номер в полследовательности</param>
        public static void SetValueInColumn(string value, string column, string key = "")
        {
            if (key == "")
                key = db.GetNumKeyInSection(column).ToString();

            db.Write(column, key, value);
        }

        /// <summary>
        /// Удалить пару ключ-значение из бд
        /// </summary>
        /// <param name="key">ключ</param>
        /// <param name="column">колонка</param>
        public static void DeleteValueInColumn(string value, string column)
        {
            string[,] readSection = db.IniReadSection(column); // читаем все варианты в секции
            string key = "";

            for(int i = 0; i < readSection.GetLength(0); i++)
            {
                if (readSection[i, 1] == value)
                    key = readSection[i, 0];
            }

            db.DeleteKey(key, column);
        }

        /// <summary>
        /// Вернуть названия всех колонок
        /// </summary>
        public static string[] GetColumnsName()
        {
            return db.GetSectionNames();
        }

        /// <summary>
        /// Заполняет поля для ввода комбобокс вариантами из БД
        /// </summary>
        /// <param name="comboBox">Комбобокс который надо заполнить</param>
        /// <param name="column">Название колоки в БД</param>
        public static void SetPropertiesComboBox(ref ComboBox comboBox, string column)
        {
            string[] str = GetColumn(column).ToArray();
            if (str != null)
                comboBox.Items.AddRange(str);
        }
    }
}