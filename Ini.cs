using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text.RegularExpressions;

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
        /// Магия позволяющая пользоваться kernel32.dll и ее методом GetPrivateProfileSection
        /// </summary>
        /// <param name="strSectionName">Имя секции</param>
        /// <param name="pReturnedString">Магия</param>
        /// <param name="nSize">Магия связанная с размером возврощаемой строки</param>
        /// <param name="strFileName">Путь к ini файлу</param>
        /// <returns></returns>
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern UInt32 GetPrivateProfileSection
        (
        [In] [MarshalAs(UnmanagedType.LPStr)] string strSectionName,
        // Note that because the key/value pars are returned as null-terminated
        // strings with the last string followed by 2 null-characters, we cannot
        // use StringBuilder.
        [In] IntPtr pReturnedString,
        [In] UInt32 nSize,
        [In] [MarshalAs(UnmanagedType.LPStr)] string strFileName
        );

        /// <summary>
        /// Вернуть простой массив значения ключей в секции, где [-,0] - имя ключа, а [-, 1] - значение ключа или null
        /// </summary>
        /// <param name="strSectionName">Имя секции</param>
        /// <returns> Вернуть простой массив значения ключей в секции, где [-,0] - имя ключа, а [-, 1] - значение ключа  или null</returns>
        public string[,] IniReadSection(string strSectionName)
        {
            string[] array = IniReadSectionGetArray(strSectionName);
            string[,] returnArray = new string[array.Length,2];
            if (array == null) // если в секции нет пар ключей-значений
                return null;

            string stringPars; // строка для распарсинга
            List<string> coincidence; // совпадения
            for (int i = 0; i < array.Length; i++)
            {
                // Оборачивает строку в символы, чтобы мочь распарсить, своего рода костыль
                stringPars = "`" + array[i] + "`";
                // Вернуть первое совпадение между двумя строчками и это будет ключ
                coincidence = ВернутьВсеСовпаденияМеждуДвумяСтроками(stringPars, "`", "=");
                if (coincidence == null)
                    return null;
                returnArray[i, 0] = coincidence[0];

                // Вернуть первое совпадение между двумя строчками и это будет значение
                coincidence = ВернутьВсеСовпаденияМеждуДвумяСтроками(stringPars, "=", "`");
                returnArray[i, 1] = coincidence[0];
                if (returnArray[i, 1] == null)
                    returnArray[i, 1] = "";
            }
            return returnArray;
        }

        /// <summary>
        /// Вернуть количество ключей в секции, если секция пустая или не существует возвращает 0
        /// </summary>
        /// <param name="section">имя секции</param>
        /// <returns></returns>
        public int GetNumKeyInSection(string section)
        {
            string[] array = IniReadSectionGetArray(section);
            if (array == null)
                return 0;

            return array.Length;
        }

        /// <summary>
        /// Вернуть имена всех секций, сделан из костылей
        /// </summary>
        /// <returns></returns>
        public string[] GetSectionNames()
        {
            try
            {
                FileStream file1 = new FileStream(PathAndName, FileMode.Open); //создаем файловый поток
                StreamReader reader = new StreamReader(file1, Encoding.GetEncoding(1251)); // создаем «потоковый читатель» и связываем его с файловым потоком 
                string fileInString = reader.ReadToEnd(); //вытащить весь текст из файла
                reader.Close(); //закрываем поток
                string[] returnArr = ВернутьВсеСовпаденияМеждуДвумяСтроками(fileInString, "[", "]").ToArray();
                return returnArr; // распарсить текст
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Вернуть простой массив значения ключей в секции по формату "Ключ=Значение"
        /// </summary>
        /// <param name="strSectionName">Имя секции</param>
        /// <returns>Вернуть простой массив значения ключей в секции</returns>
        private string[] IniReadSectionGetArray(string strSectionName)
        {
            // Allocate in unmanaged memory a buffer of suitable size.
            // I have specified here the max size of 32767 as documentated
            // in MSDN.
            IntPtr pBuffer = Marshal.AllocHGlobal(32767);
            // Start with an array of 1 string only.
            // Will embellish as we go along.
            string[] strArray = new string[0];
            UInt32 uiNumCharCopied = 0;

            uiNumCharCopied = GetPrivateProfileSection(strSectionName, pBuffer, 32767, PathAndName);

            // iStartAddress will point to the first character of the buffer,
            int iStartAddress = pBuffer.ToInt32();
            // iEndAddress will point to the last null char in the buffer.
            int iEndAddress = iStartAddress + (int)uiNumCharCopied;

            // Navigate through pBuffer.
            while (iStartAddress < iEndAddress)
            {
                // Determine the current size of the array.
                int iArrayCurrentSize = strArray.Length;
                // Increment the size of the string array by 1.
                Array.Resize(ref strArray, iArrayCurrentSize + 1);
                // Get the current string which starts at "iStartAddress".
                string strCurrent = Marshal.PtrToStringAnsi(new IntPtr(iStartAddress));
                // Insert "strCurrent" into the string array.
                strArray[iArrayCurrentSize] = strCurrent;
                // Make "iStartAddress" point to the next string.
                iStartAddress += (strCurrent.Length + 1);
            }

            Marshal.FreeHGlobal(pBuffer);
            pBuffer = IntPtr.Zero;

            return strArray;
        }


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

        /// <summary>
        /// Читаем ini-файл и возвращаем значение указного ключа из заданной секции.
        /// </summary>
        /// <param name="Section">Секция</param>
        /// <param name="Key">Ключ</param>
        /// <param name="strDefault">Вернет эту строку, если ключ не найден</param>
        /// <returns></returns>
        public string IniReadKey(string Section, string Key, string strDefault = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", RetVal, 255, PathAndName);
            if (RetVal == null)
                return strDefault;
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
            return IniReadKey(Section, Key).Length > 0;
        }

        private static List<string> ВернутьВсеСовпаденияМеждуДвумяСтроками(string текстДляПоиска, string перваяСтрока, string втораяСтрока)
        {
            List<string> возврат = new List<string>();
            string pattern = @"(?i)\" + перваяСтрока + @"(.*?)\" + втораяСтрока + @""; //  @"<div class=""date"">(?<val>.*?)<\/span>"; перваяСтрока + @"(.*)" + втораяСтрока
            Regex regex = new Regex(pattern);
            //Regex regex = new Regex(@"([^\" + перваяСтрока + @"\" + перваяСтрока + @"]+)(?=\" + втораяСтрока + @")"); // ([^\]\[]+)(?=\])
            MatchCollection matches = regex.Matches(текстДляПоиска);
            string значениеРегулярки = "";
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    значениеРегулярки = match.Value; // Выводим текстровую информацию из совпадений регулярных выражений
                    if (значениеРегулярки == "")
                        return null;
                    if (перваяСтрока.Length > 0)
                        значениеРегулярки = значениеРегулярки.Remove(0, перваяСтрока.Length); // Удаляем перваяСтрока
                    if (втораяСтрока.Length > 0 && значениеРегулярки.Length - втораяСтрока.Length > 0)
                        значениеРегулярки = значениеРегулярки.Remove(значениеРегулярки.Length - втораяСтрока.Length, втораяСтрока.Length); // Удаляем втораяСтрока
                    возврат.Add(значениеРегулярки);
                }

            }
            else
            {
                return null;
            }
            return возврат;
        }

    }
}

