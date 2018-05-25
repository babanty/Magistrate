using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MyIni;

namespace Magistrate
{
    /// <summary>
    /// Паттерн фасад для соседней сборки MyIni. MyIni предназначен для работы с ini-файлами
    /// </summary>
    class IniFacade
    {

        private Ini ini; //Имя файла.

        /// <summary>
        /// Вернуть простой массив значения ключей в секции, где [-,0] - имя ключа, а [-, 1] - значение ключа или null
        /// </summary>
        /// <param name="strSectionName">Имя секции</param>
        /// <returns> Вернуть простой массив значения ключей в секции, где [-,0] - имя ключа, а [-, 1] - значение ключа  или null</returns>
        public string[,] IniReadSection(string strSectionName)
        {
            return ini.IniReadSection(strSectionName);
        }


        /// <summary>
        /// Вернуть количество ключей в секции, если секция пустая или не существует возвращает 0
        /// </summary>
        /// <param name="section">имя секции</param>
        /// <returns></returns>
        public int GetNumKeyInSection(string section)
        {
            return ini.GetNumKeyInSection(section);
        }


        /// <summary>
        /// Вернуть имена всех секций, сделан из костылей
        /// </summary>
        /// <returns></returns>
        public string[] GetSectionNames()
        {
            return ini.GetSectionNames();
        }

        /// <summary>
        /// С помощью конструктора записываем путь до файла и его имя.
        /// </summary>
        /// <param name="IniName">Название ини файла с указанием расширения</param>
        /// <param name="AbsolutelyPath">Если путь относительный, то false (по дефолту), если путь абсолютный, то ставить true</param>
        public IniFacade(string IniName, bool AbsolutelyPath = false)
        {
            string path = "";

            if (AbsolutelyPath == false)
            {
               path = Application.StartupPath;
            }
            // Application.ExecutablePath путь к исполняемому файлу
            string IniPath = path + "\\" + IniName + ".ini"; // Путь до инифайла
            string PathAndName = new FileInfo(IniPath).FullName.ToString();

            ini = new Ini(PathAndName);
        }


        /// <summary>
        /// Читаем ini-файл и возвращаем значение указного ключа из заданной секции.
        /// </summary>
        /// <param name="Section">Секция</param>
        /// <param name="Key">Ключ</param>
        /// <param name="strDefault">Вернет эту строку, если ключ не найден</param>
        /// <returns></returns>
        public string IniReadKey(string Section, string Key, string strDefault = null)
        {
            return ini.IniReadKey(Section, Key, strDefault);
        }


        /// <summary>Записываем в ini-файл. Запись происходит в выбранную секцию в выбранный ключ.</summary>
        public void Write(string Section, string Key, string Value)
        {
            ini.Write(Section, Key, Value);
        }


        /// <summary>Удаляем ключ из выбранной секции.</summary>
        public void DeleteKey(string Key, string Section = null)
        {
            ini.DeleteKey(Key, Section);
        }


        /// <summary>Удаляем выбранную секцию</summary>
        public void DeleteSection(string Section = null)
        {
            ini.DeleteSection(Section);
        }


        /// <summary>Проверяем, есть ли такой ключ, в этой секции</summary>
        public bool KeyExists(string Key, string Section = null)
        {
            return KeyExists(Key, Section);
        }
    }
}

