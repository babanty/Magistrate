using Magistrate.Forms;
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
        private static IniFacade db = new IniFacade("db");

        /// <summary>
        /// Вернуть значения одной колонки
        /// </summary>
        /// <param name="nameColumn">название колонки</param>
        /// <returns></returns>
        public static List<string> GetColumn(string nameColumn)
        {
            if (nameColumn == null)
                return null;

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
        /// Вернуть значения одной колонки
        /// </summary>
        /// <param name="nameProperties">название колонки</param>
        /// <returns></returns>
        public static List<string> GetColumn(NamePropertiesForComboBox nameProperties)
        {
            // Расшифровываем значение настройки
            string nameColumn = DecodingEnumPropertiesForComboBox(nameProperties);

            if (nameColumn == null)
                return null;

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
        /// Расшифровывает что значат типы настроек для комбобокса, возвращает строку,
        /// по которой обращаться в БД, нарпимер "Банк полные реквизиты" или null
        /// </summary>
        /// <param name="nameProperties"></param>
        /// <returns>возвращает строку, по которой обращаться в БД, нарпимер "Банк полные реквизиты" или null</returns>
        public static string DecodingEnumPropertiesForComboBox(NamePropertiesForComboBox nameProperties)
        {
            string returnStr = null;

            switch (nameProperties)
            {
                case NamePropertiesForComboBox.МестоРождения:
                    returnStr = "Место Рождения";
                    break;
                case NamePropertiesForComboBox.МестоЖительстваГород:
                    returnStr = "Место жительства город";
                    break;
                case NamePropertiesForComboBox.МестоЖительстваУлица:
                    returnStr = "Место жительства улица";
                    break;
                case NamePropertiesForComboBox.МестоЖительстваДом:
                    returnStr = "Место жительства дом";
                    break;
                case NamePropertiesForComboBox.МаркаАвто:
                    returnStr = "Марка авто";
                    break;
                case NamePropertiesForComboBox.НазваниеТрассы:
                    returnStr = "Название трассы";
                    break;
                case NamePropertiesForComboBox.МестоПравонарушения:
                    returnStr = "Место правонарушения";
                    break;
                case NamePropertiesForComboBox.БанкСокращенный:
                    returnStr = "Банк только название";
                    break;
                case NamePropertiesForComboBox.БанкПолный:
                    returnStr = "Банк полные реквизиты";
                    break;
                case NamePropertiesForComboBox.КомуналкаСокращенная:
                    returnStr = "Комуналка организации только название";
                    break;
                case NamePropertiesForComboBox.КомуналкаПолная:
                    returnStr = "Комуналка организации полные реквизиты";
                    break;
            }

            return returnStr;
        }


        #region ГАИ
        /// <summary>
        /// Вернуть реквизиты ГАИ по сокращенному названию
        /// </summary>
        /// <param name="shortName">Сокращенное название, пример: Наше ГАИ</param>
        /// <param name="requisites">Возврат реквизитов с УИН-ом</param>
        /// <param name="standarnYIN">Отдельно возвращает стандартный УИН</param>
        public static void GetRequisitesGAI(string shortName, out string requisites, out string standarnYIN, string column = NewGAI.NameColumn)
        {
            requisites = null;
            standarnYIN = null;

            // берем все варианты реквизитов разных ГАИ
            List<string> getColumn = GetColumn(column);
            if (getColumn == null)
                return;

            // Проходим по всем и ищем совпадение с сокращенным название shortName
            foreach (string key in getColumn)
            {
                string[] processed = DecodingStringWithRequisites(key); // получаем расшифровку строки, где сокращенноеНазвание$РеквизитыБезУИН$ПостояннаяЧастьУИН
                if (processed != null) // если получилось расшифровать
                {
                    if (processed[0] == shortName) // если сокращенное названия соотвествуют
                    {
                        requisites = processed[1] + ", УИН: " + processed[2]; // сделать полные реквизиты
                        standarnYIN = processed[2]; // выделить отдельно постоянную часть УИН
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Вернуть все сокращенные названия ГАИ или null
        /// </summary>
        /// <param name="column">колонка, где их брать, по умолчанию та, что в форме по заполнению реквизитов</param>
        public static List<string> GetAllShortRequisitesGAI(string column = NewGAI.NameColumn)
        {
            List<string> returnList = new List<string>();
            // берем все варианты реквизитов разных ГАИ
            List<string> getColumn = GetColumn(column);
            if (getColumn == null)
                return null;

            string[] processed;
            foreach (string key in getColumn)
            {
                processed = DecodingStringWithRequisites(key);
                if (processed != null)
                    returnList.Add(processed[0]);
            }

            return returnList;
        }


        /// <summary>
        /// Расшифровка строки с реквизитами где сокращенноеНазвание$РеквизитыБезУИН$ПостояннаяЧастьУИН, смотреть подробности возврата
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Возвращает массив, где [0] - Сокращенное название, [1] - Возврат реквизитов без УИН, [2] - статичный УИН</returns>
        private static string[] DecodingStringWithRequisites(string key)
        {
            // получаем расшифровку строки, где сокращенноеНазвание$РеквизитыБезУИН$ПостояннаяЧастьУИН
            string[] processed = key.Split('$');

            // Если не три элемента, значит строка не соотвествует правильному заполнению
            if (processed.Length != 3)
                return null;

            return processed;
        }
        #endregion ГАИ
    }


    /// <summary> Имя массива вариантов для полей-контролов (comboBox) </summary>
    public enum NamePropertiesForComboBox
    {
        МестоРождения,
        МестоЖительстваГород,
        МестоЖительстваУлица,
        МестоЖительстваДом,
        МаркаАвто,
        НазваниеТрассы,
        МестоПравонарушения,
        БанкСокращенный,
        БанкПолный,
        КомуналкаСокращенная,
        КомуналкаПолная,
        РеквизитыГИБДД
    }
}