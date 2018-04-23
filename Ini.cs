using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Magistrate
{
    /// <summary>
    /// Создать файл ини
    /// </summary>
    class Ini
    {
        private string PathAndName; //Имя файла.

        [DllImport("kernel32")] // Подключаем kernel32.dll и описываем его функцию WritePrivateProfilesString
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32")] // Еще раз подключаем kernel32.dll, а теперь описываем функцию GetPrivateProfileString
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        /// <summary>
        /// С помощью конструктора записываем путь до файла и его имя.
        /// </summary>
        /// <param name="IniName">Название ини файла с указанием расширения</param>
        /// <param name="AbsolutelyPath">Если путь относительный, то false (по дефолту), если путь абсолютный, то ставить true</param>
        public Ini(string IniName, bool AbsolutelyPath = false)
        {
            string path = "";

            if (AbsolutelyPath == false)
            {
               path = Application.StartupPath;
            }
            // Application.ExecutablePath путь к исполняемому файлу
            string IniPath = path + "\\" + IniName + ".ini"; // Путь до инифайла
            PathAndName = new FileInfo(IniPath).FullName.ToString();
        }

        //Читаем ini-файл и возвращаем значение указного ключа из заданной секции.
        public string ReadINI(string Section, string Key)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", RetVal, 255, PathAndName);
            return RetVal.ToString();
        }
        //Записываем в ini-файл. Запись происходит в выбранную секцию в выбранный ключ.
        public void Write(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, PathAndName);
        }

        //Удаляем ключ из выбранной секции.
        public void DeleteKey(string Key, string Section = null)
        {
            Write(Section, Key, null);
        }
        //Удаляем выбранную секцию
        public void DeleteSection(string Section = null)
        {
            Write(Section, null, null);
        }
        //Проверяем, есть ли такой ключ, в этой секции
        public bool KeyExists(string Key, string Section = null)
        {
            return ReadINI(Section, Key).Length > 0;
        }

    }
}

