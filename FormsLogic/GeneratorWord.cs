using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Magistrate
{
    /// <summary>
    /// Набор статических инструментов для генерации ворда и заполнение его информацией
    /// </summary>
    public static class GeneratorWord
    {
        // Путь до скрипта генератора
        private static string PathToApp = Application.StartupPath + "\\AutoitGenerateWord.exe"; 

        /// <summary>
        /// Запихнуть в ворд вместо пронумерованных ключей необходимую инфомарцию
        /// </summary>
        /// <param name="PathToSample">Путь до папки с примерами</param>
        /// <param name="NameSample">Имя примера шаблона без расширения, .docx дописывает автоматом</param>
        /// <param name="Params">Массив строк впихиваемых вместо ключей</param>
        public static void GenerateWord(string PathToSamples, string NameSample , List<ValueControl> Params)
        {
            string nameSectionValue = "ValueForGenerate"; // Название секции в ini файле, в которой будут храниться ini-ключи-значения полей ввода
            string nameSectionProperties = "PropertiesForGenerate"; // Название секции в ini файле, в которой будут храниться ini-ключи-значения настройки для autoit-скрипта

            IniFacade ini = new IniFacade("PropertiesForAutoitScript"); // Создаем инифайл для последующе обработки autoit exe-шником

            try // Очищаем старую информацию в секциях ini 
            {
                ini.DeleteSection(nameSectionProperties); 
                ini.DeleteSection(nameSectionValue);
            } catch { } 
            
            try
            {
                ini.Write(nameSectionProperties, "PathToSamples", PathToSamples + "\\" + NameSample + ".docx"); // Путь до шаблона
            }
            catch (Exception ex)
            {
                MessageBox.Show("ПРОБЛЕМА С ЗАПИСЬЮ КЛЮЧЕЙ В ИНИ-ФАЙЛ ДЛЯ СКРИПТА AUTOIT: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            foreach (ValueControl Param in Params) //Записывает все ключи чекбоксов, которые потом будет использовать приложение autoit
                if(Param.Key != null)
                    ini.Write(nameSectionValue, Param.Key, Param.Text);
            
            OpenScriptGenerator(); // Открыть аутоит скрипт, генерирующий ворд на основе COM обхектов

            // ini-файл организуется так: в секции "PropertiesForGenerate" первый ключ-значение это путь до шаблона word-
            // файла, который используется для генерации. Далее в секции "ValueForGenerate" идут ключи-значения, 
            // которые будут подставляться в word-е, где название ключа в ini соотвествует ключу в word файле, 
            // а значение ключа в ini будет подставляться вместо ключа в word-файле. Если не понятно, смотри 
            // на шаблон word файла и в сам ini файл
        }


        /// <summary>
        /// Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
        /// </summary>
        /// <param name="controls">коллекция элементов(контролов) с формы, обычно это "Controls"</param>
        /// <returns></returns>
        public static List<ValueControl> StandartListValueControl(Control.ControlCollection controls)
        {
            if (controls == null) // Если передан пустой массив контролов
                return null;

            // Берет текст из всех форм заполнения пользователем и превращает в массив
            List<ValueControl> controlArrayToString = ControlArrayToString(controls); 
            if (controlArrayToString == null) // Если в массиве контролов нет полей для ввода
                return null;

            //Добавляет ключи для вставки их в шаблон к массиву строк, который потом будет замещать эти ключи в шаблоне
            СontrolAndKeyArrayToString(ref controlArrayToString);
            return controlArrayToString;
        }


        /// <summary>
        /// В ручную добавить ключ-значение в выгружаемый массив
        /// </summary>
        /// <param name="ValueControlArray">Массив, в который добавляется значение</param>
        /// <param name="control">Контрол которыйд добавляется</param>
        /// <param name="Key">Ключ контрола, желательно в формате #-01</param>
        public static void AddValueControl(ref List<ValueControl> ValueControlArray, Control control, string Key)
        {
            ValueControl newValueControl = new ValueControl(control)
            {
                Key = Key // Добавляение ключа
            };
            ValueControlArray.Add(newValueControl); // Записать в лист
            
        }


        /// <summary>
        /// В ручную добавить ключ-значение в выгружаемый массив без информации с котрола на форме
        /// </summary>
        /// <param name="ValueControlArray">Массив, в который добавляется значение</param>
        /// <param name="text">Текст, который добавляется вместо текста с котрола</param>
        /// <param name="Key">Ключ контрола, желательно в формате #-01</param>
        public static void AddValueControl(ref List<ValueControl> ValueControlArray, string text, string Key)
        {
            ValueControl newValueControl = new ValueControl(null, text)
            {
                Key = Key // Добавляение ключа
            };
            ValueControlArray.Add(newValueControl); // Записать в лист
        }


        /// <summary>
        /// В ручную добавить ключ-значение в выгружаемый массив. Ключ будет значение TaglIndex элемента
        /// </summary>
        /// <param name="ValueControlArray">Массив, в который добавляется значение</param>
        /// <param name="control">Контрол которыйд добавляется</param>
        public static void AddValueControl(ref List<ValueControl> ValueControlArray, Control control)
        {
            ValueControl newValueControl = new ValueControl(control);
            newValueControl.Key = GenerateKey(newValueControl);

            ValueControlArray.Add(newValueControl); // Записать в лист

        }


        /// <summary>
        /// Берет текст из всех форм заполнения пользователем и превращает в массив
        /// </summary>
        /// <param name="controls"> коллекция элементов(контролов) с формы</param>
        /// <returns>Возвращает лист значений контролов</returns>
        private static List<ValueControl> ControlArrayToString(Control.ControlCollection controls)
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
        private static void СontrolAndKeyArrayToString(ref List<ValueControl> ValueControlArray)
        {
            if (ValueControlArray == null)
                throw new Exception("В форме не найдено полей для ввода");

            foreach (ValueControl i in ValueControlArray)
            {
                i.Key = GenerateKey(i); // Сгенерировать ключ
            }
        }


        /// <summary>
        /// Сгенерировать ключ для контрола
        /// </summary>
        /// <param name="valueControl">собственно сам контрол</param>
        /// <returns>Ключ для word файла или null в случае не успеха</returns>
        private static string GenerateKey(ValueControl valueControl)
        {
            if (valueControl == null)
                return null;

            string returnResult = "#00";
            string numString = "";

            object tag = valueControl.Control.Tag;
            if (tag == null)
                return null;

            numString = valueControl.Control.Tag.ToString() ; // Номер ключа зависит от тега

            if (numString.Length == 1)
                numString = "0" + numString; // добавляет 0 перед числом если оно меньше 10, чтобы число всегда было из 2х цифр


            returnResult = "#" + numString; // Пример результата: #05
            return returnResult;
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
