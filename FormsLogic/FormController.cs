using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Magistrate.FormLogic
{
    /// <summary>
    /// Контроллер формы выполняет действия общие для всех форм
    /// </summary>
    static class FormController
    {        
        /// <summary>
        /// Заполняет автоматически дату вынесения решения
        /// </summary>
        public static void SetDateOfOrderDay(ref ComboBox Month, ref ComboBox Year)
        {
            DateTime dateTimeNow = DateTime.Now;
            string month = HandlerTextControls.MonthInString(dateTimeNow.Month); // месяц
            string year = dateTimeNow.Year.ToString(); // год
            Month.Text = month;
            Year.Text = year;
        }


        /// <summary>
        /// Заполняет поля для ввода комбобокс вариантами из БД
        /// </summary>
        /// <param name="comboBox">Комбобокс который надо заполнить</param>
        /// <param name="column">Название колоки в БД</param>
        public static void SetPropertiesComboBox(ref ComboBox comboBox, NamePropertiesForComboBox nameProperties)
        {
            if(nameProperties == NamePropertiesForComboBox.РеквизитыГИБДД)
            {
                comboBox.Items.AddRange(Db.GetAllShortRequisitesGAI().ToArray());
            }

            SetPropertiesStandartComboBox(ref comboBox, nameProperties);
        }


        /// <summary>
        /// Возвращает полные реквизиты банка по названию или null
        /// </summary>
        /// <param name="Name">Название банка</param>
        public static string GetBank(string Name)
        {
            return GetFullRequisites(Name, NamePropertiesForComboBox.БанкПолный);
        }

        /// <summary>
        /// Возвращает полные реквизиты комунальщиков-организаций по названию или null
        /// </summary>
        /// <param name="Name">Название организации</param>
        public static string GetRecMunicipal(string Name)
        {
            return GetFullRequisites(Name, NamePropertiesForComboBox.КомуналкаПолная);
        }

        // Вернуть полные реквиты по названию организации
        private static string GetFullRequisites(string Name, NamePropertiesForComboBox nameProperties)
        {
            int nameLength = Name.Length; // Количество символов в названии банка
            string result = null;


            // Проверяем сходится ли название с полными реквизитами
            List<string> allRec = Db.GetColumn(nameProperties);
            if (allRec == null)
                return null;

            foreach (string bank in allRec)
            {
                if (bank.Substring(0, nameLength) == Name) // первые слова в полных реквизитах соотвествуют 
                    return bank;
            }

            // Возвращаем результат null
            return result;
        }

        
        /// <summary>
        /// Заполнение поля text контролов, имеющихся на всех (формах для генерирования word)
        /// </summary>
        /// <param name="nameForm">Имя формы совпадающее с названием класса</param>
        /// <param name="comboBoxLoad">Комбобокс с вариантами загрузки, как правило называется comboBoxLoad</param>
        /// <param name="comboBoxDateOfOrderMonth">месяц вынесения постановления, как правило называется comboBoxDateOfOrderMonth</param>
        /// <param name="comboBoxDateOfOrderYear">год вынесения постановления, как правило называется comboBoxDateOfOrderYear</param>
        public static void SetStandartParamsInControls
            (string nameForm, 
            ref ComboBox comboBoxLoad,
            ref ComboBox comboBoxDateOfOrderMonth,
            ref ComboBox comboBoxDateOfOrderYear)
        {
            // заполнение вариантами сохранений
            SaveLoadForm.SetVariantsSaveInComboBox(nameForm, ref comboBoxLoad);

            // Заполнение даты вынесения решения текущими датами
            DateTime dateTimeNow = DateTime.Now;
            string month = HandlerTextControls.MonthInString(dateTimeNow.Month); // месяц
            string year = dateTimeNow.Year.ToString(); // год
            comboBoxDateOfOrderMonth.Text = month;
            comboBoxDateOfOrderYear.Text = year;
        }


        /// <summary>
        /// Метод возвращающий формулировку в случае если явился или не явился или null если не правильные параметры
        /// Это разъяснения
        /// </summary>
        /// <param name="appeared">Если явился, то true</param>
        /// <param name="ItIsWoomen">Если женщина, то true</param>
        /// <returns>метод возвращающий формулировку в случае если явился или не явился или null</returns>
        public static string AppearedOrNotExplanation(bool appeared, bool ItIsWoomen)
        {
            if (appeared && ItIsWoomen)
                return "явилась, вину признала";
            if (appeared == false && ItIsWoomen)
                return "не явилась, извещена надлежащим образом о времени и месте судебного " +
                    "заседания. Учитывая, что имеются данные о ее надлежащем извещении о месте и " +
                    "времени рассмотрения дела и от нее не поступило ходатайство об отложении " +
                    "рассмотрения дела, суд на основании ч.2 ст.25.1 КоАП РФ считает возможным " +
                    "рассмотреть дело в ее отсутствие.";

            if (appeared && ItIsWoomen == false)
                return "явился, вину признал";
            if (appeared == false && ItIsWoomen == false)
                return "не явился, извещен надлежащим образом о времени и месте судебного " +
                    "заседания. Учитывая, что имеются данные о его надлежащем извещении о месте и " +
                    "времени рассмотрения дела и от него не поступило ходатайство об отложении " +
                    "рассмотрения дела, суд на основании ч.2 ст.25.1 КоАП РФ считает возможным " +
                    "рассмотреть дело в его отсутствие.";

            return null;
        }

        /// <summary>
        /// Метод возвращающий формулировку в случае если явился или не явился если не правильные параметры
        /// Это смягчающие обстятельства
        /// </summary>
        /// <param name="appeared">Если явился, то true</param>
        /// <returns>метод возвращающий формулировку в случае если явился или не явился</returns>
        public static string AppearedOrNotCircumstances(bool appeared)
        {
            if (appeared)
            {
                return "Смягчающим ответственность обстоятельством суд считает признание им своей вины." +
                    " Отягчающих обстоятельств судом не установлено.";
            }
            else
            {
                return "Смягчающих и отягчающих обстоятельств судом не установлено.";
            }
        }


        /// <summary>
        /// Вернуть инициалы
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="patronymic">Отчетсво</param>
        /// <returns>Инициалы, пример А.Б. </returns>
        public static string GetInitials(string name, string patronymic)
        {
            if (name.Length > 2 && patronymic.Length > 2)  // Если правильно заполнили имя и отчество
                return name.Remove(1) + "." + patronymic.Remove(1) + ".";
            return "";
        }


        /// <summary>
        /// Вернуть полные реквизиты ГИБДД с УИНом
        /// </summary>
        /// <param name="nameRecipientGIBDD">Название ГАИ</param>
        /// <param name="secondPartYIN">Вторая часть УИНа, которая всегда разная</param>
        /// <returns></returns>
        public static string GenerateFullRequisitesGAI(string nameRecipientGIBDD, string secondPartYIN)
        {
            string requisitesGAI = "";

            Db.GetRequisitesGAI(nameRecipientGIBDD, out requisitesGAI, out string standartYIN); // Заполняем переменные с реквизитами
            requisitesGAI += secondPartYIN; // Добавляем оставшийся УИН

            return requisitesGAI;
        }


        /// <summary>
        /// Сохранить значения полей для заполнения на форме
        /// </summary>
        /// <param name="controls">все контролы формы, обычно надо написать просто Сontrols</param>
        /// <param name="nameForm">имя формы, обычно это имя класса</param>
        /// <param name="nameSave">Название сохранения</param>
        /// <param name="comboBoxLoad">comboBox с вариантами сохранений</param>
        public static void SaveForm
            (Control.ControlCollection controls, 
            string nameForm, 
            string nameSave, 
            ref ComboBox comboBoxLoad)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GenerationWord.StandartListValueControl(controls);

            string fullNameSave = nameForm + "$" + nameSave; // имя сохранения
            SaveLoadForm.SaveForm(fullNameSave, controlArrayToString); // сохранить

            // перезаполняем варианты сохранений
            comboBoxLoad.Items.Clear(); // стираем текущие варианты
            SaveLoadForm.SetVariantsSaveInComboBox(nameForm, ref comboBoxLoad);// заполнение вариантами сохранений
        }


        /// <summary>
        /// Вернуть контролы с обновленными загрузкой из файла значениями полей. 
        /// После выполнения этого метода в самой форме надо вызвать метод updateControls
        /// </summary>
        /// <param name="controls">все контролы формы, обычно надо написать просто Сontrols</param>
        /// <param name="nameForm">имя формы, обычно это имя класса</param>
        /// <param name="comboBoxLoad">comboBox с вариантами сохранений</param>
        public static Control.ControlCollection GetControlsLoadForm
            (Control.ControlCollection controls,
            string nameForm,
            ref ComboBox comboBoxLoad)
        {
            return SaveLoadForm.LoadForm(nameForm, comboBoxLoad.Text, controls); // получаем заполненные сейвом контролы
        }


        /// <summary>
        /// Удалить сохранение
        /// </summary>
        /// <param name="comboBoxLoad">комбобокс с вариантами сохранений</param>
        /// <param name="nameForm">название формы</param>
        public static void DeleteSave(ref ComboBox comboBoxLoad, string nameForm)
        {
            SaveLoadForm.DeleteSave(comboBoxLoad.Text, nameForm); // Удаляем сохранение

            // перезаполняем варианты сохранений
            comboBoxLoad.Items.Clear(); // стираем текущие варианты
            comboBoxLoad.Text = ""; // стираем текущие варианты
            SaveLoadForm.SetVariantsSaveInComboBox(nameForm, ref comboBoxLoad);// заполнение вариантами сохранений
        }


        /// <summary>
        /// Судья или И.о мирового судьи
        /// </summary>
        /// <param name="plotNumber">Номер участка</param>
        /// <returns></returns>
        public static string GetMagistrateOrDeputy(string plotNumber)
        {
            if (plotNumber == PropertiesMyApp.GetPropertiesValue(TypeProperties.PlaceNum))
            {
                return "Мировой судья";
            }
            else
            {
                return "И.о. мирового судьи";
            }
        }


        /// <summary>
        /// Забить в лейбл статичный УИН ГИБДД для наглядности
        /// </summary>
        /// <param name="nameRecipientGIBDD">Название получателя ГИБДД</param>
        /// <param name="label">Лейбл, который изменится</param>
        public static void SetInLabelYIN(string nameRecipientGIBDD, ref Label label)
        {
            string yin = ""; // Статичная часть УИНа
            Db.GetRequisitesGAI(nameRecipientGIBDD, out string requisitesGAI, out yin); // Заполняем переменные с реквизитами

            // меняем текст в лэйбле
            if (yin == null)
            {
                label.Text = "УИН получателя, получатель не опознан";
            }
            else
            {
                label.Text = "УИН получателя, продолжить: " + yin;
            }
        }


        /// <summary>
        /// Закинуть в буфер обмена имя word для сохранения
        /// </summary>
        /// <param name="numCase">Номер дела</param>
        /// <param name="nameAccused">Имя обвиняемого</param>
        /// <param name="textForm">Текст окна, обычно сюда надо писать this.Text</param>
        public static void ClipPutNameWord(string numCase, string nameAccused, string textForm)
        {
            numCase = numCase ?? "";

            // если есть номер у дела
            if (numCase != "")
                numCase += "  ";

            Clipboard.SetText(numCase + nameAccused + "  " + textForm);
        }


        /// <summary>
        /// Сгенерировать word, метод для форм, где есть деление на мужской и женский пол 
        /// </summary>
        /// <param name="listValueControl">Лист с значениями-ключами сгенерированный StandartListValueControl</param>
        /// <param name="ItIsMen">Это мужчина? Если да, то тут true</param>
        /// <param name="namePatternWordForMen">Название шаблона word для мужчин</param>
        /// <param name="namePatternWordForWoomen">Название шаблона word для женщин</param>
        public static void GenerateWord(
            List<ValueControl> listValueControl, 
            bool ItIsMen,  
            string namePatternWordForMen, 
            string namePatternWordForWoomen)
        {
            // Если пол мужской, то один шаблон, если женский, то другой
            if (ItIsMen)
            {
                // Сгенерировать ворд
                GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", namePatternWordForMen, listValueControl);
            }
            else
            {
                // Сгенерировать ворд
                GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", namePatternWordForWoomen, listValueControl);
            }
        }

        /// <summary>
        /// Сгенерировать word, метод для форм, где есть деление на мужской и женский пол 
        /// </summary>
        /// <param name="listValueControl">Лист с значениями-ключами сгенерированный StandartListValueControl</param>
        /// <param name="namePatternWord">Название шаблона word</param>
        public static void GenerateWord(List<ValueControl> listValueControl, string namePatternWord)
        {
            // Сгенерировать ворд
            GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", namePatternWord, listValueControl);
        }


        /// <summary>
        /// Заполняет поля для ввода комбобокс вариантами из БД
        /// </summary>
        /// <param name="comboBox">Комбобокс который надо заполнить</param>
        /// <param name="column">Название колоки в БД</param>
        private static void SetPropertiesStandartComboBox(ref ComboBox comboBox, NamePropertiesForComboBox nameProperties)
        {
            List<string> str = Db.GetColumn(nameProperties);

            if (str != null)
            {
                // сортировка по возрастанию, LINQ запрос
                var sortedStr = from s in str // определяем каждый объект из str как s
                                orderby s  // упорядочиваем по возрастанию
                                select s; // выбираем объект
                comboBox.Items.AddRange(sortedStr.ToArray());
            }
        }



    }
}