using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace Magistrate
{
    /// <summary>
    /// Набор статических инструментов для генерации ворда и заполнение его информацией
    /// </summary>
    public static class GenerationWord
    {
        private static string PathToApp = Application.StartupPath + "\\AutoitGenerateWord.exe"; // Путь до скрипта генератора

        /// <summary>
        /// Запихнуть в ворд в пронумерованные ключи необходимую инфомарцию
        /// </summary>
        /// <param name="PathToSample">Путь до папки с примерами</param>
        /// <param name="NameSample">Имя примера шаблона без расширения, .docx дописывает автоматом</param>
        /// <param name="Params">Массив строк впихиваемых вместо ключей</param>
        public static void GenerateWord(string PathToSamples, string NameSample , List<ValueControl> Params)
        {
            Ini ini = new Ini("PropertiesForAutoitScript"); // Создаем инифайл для последующе обработки autoit exe-шником
            try
            {
                ini.Write(NameSample, "PathToSamples", PathToSamples + "\\" + NameSample + ".docx"); // Путь до шаблона
            }
            catch (Exception ex)
            {
                MessageBox.Show("ПРОБЛЕМА С ЗАПИСЬЮ КЛЮЧЕЙ В ИНИ-ФАЙЛ ДЛЯ СКРИПТА AUTOIT: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            foreach (ValueControl Param in Params)
            {
                ini.Write(NameSample, Param.Key, Param.Text); //Записывает все ключи, которые потом будет использовать приложение autoit
            }
            
            OpenScriptGenerator(); // Открыть аутоит скрипт, генерирующий ворд на основе COM обхектов
        }

        /// <summary>
        /// Берет текст из всех форм заполнения пользователем и превращает в массив
        /// </summary>
        /// <param name="controls"> коллекция элементов(контролов) с формы</param>
        /// <returns>Возвращает лист значений контролов</returns>
        public static List<ValueControl> ControlArrayToString(Control.ControlCollection controls)
        {
            List<ValueControl> valueControlsReturn = new List<ValueControl>(); // Возвращаемый лист значений контролов

            foreach (Control control in controls) // Перебор коллекци контролов на вычленение из него тех, что имеют поля для ввода
            {
                //Пример обхода всех TextBox-ов http://www.cyberforum.ru/windows-forms/thread110436.html#a_Q7
                if(control is TextBox || control is ComboBox) // Если содержит поле для ввода
                {
                    ValueControl newValueControl = new ValueControl(control);
                    valueControlsReturn.Add(newValueControl); // Записать в лист
                }
            }

            return valueControlsReturn;
        }

        /// <summary>
        /// Добавляет ключи для вставки их в шаблон к массиву строк, который потом будет замещать эти ключи в шаблоне
        /// </summary>
        /// <param name="ControlStringArray">массив строк, к которым добавлять их значение ключа</param>
        public static void СontrolAndKeyArrayToString(ref List<ValueControl> ValueControlArray)
        {
            if (ValueControlArray == null)
                throw new Exception("В форме не найдено полей для ввода");

            int num = 0;
            string numString = "";
            foreach (ValueControl i in ValueControlArray)
            {
                numString = num.ToString();
                if (num < 10)
                    numString = "0" + numString; // добавляет 0 перед числом если оно меньше 10, чтобы число всегда было из 2х цифр
                i.Key = "#" + numString;
                num++;
            }
        }

        /// <summary>
        /// Открыть аутоит скрипт, генерирующий ворд на основе COM обхектов
        /// </summary>
        private static void OpenScriptGenerator()
        {
            try
            {
                Process.Start(PathToApp); // открыть приложение
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show("Нет файла autoit генерирующего word: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Какая-то проблема с файлом генератором ворд: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
