using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Magistrate
{
    /// <summary>
    ///  Набор статических инструментов для обработки текста в контролах и для контролов win forms
    /// </summary>
    public static class HandlerTextControls
    {

        #region IntInRubAndCop - Число с плавающей запятой переделать в формат "0 руб. 0 коп."
        /// <summary>
        /// Число с плавающей запятой переделать в формат "0 руб. 0 коп."
        /// </summary>
        /// <param name="num">Число</param>
        public static string IntInRubAndCop(decimal num)
        {
            int whole = (int)Math.Truncate(num); // Целая часть, это рубли
            decimal numToDecimal = (decimal)num; // для точности вычислений
            int fraction = (int)((numToDecimal - whole) * 100); // дробная часть, это копейки
            string fractionString = fraction.ToString();
            if (fraction < 10)
                fractionString = "0" + fractionString;

            string result = whole + " руб. " + fractionString + " коп.";
            return result;
        }

        /// <summary>
        /// Число с плавающей запятой переделать в формат "0 руб. 0 коп."
        /// </summary>
        /// <param name="num">Число</param>
        public static string IntInRubAndCop(double num)
        {
            return IntInRubAndCop((decimal)num);
        }

        /// <summary>
        /// Число с плавающей запятой переделать в формат "0 руб. 0 коп."
        /// </summary>
        /// <param name="strNum">Число</param>
        public static string IntInRubAndCop(string strNum)
        {
            try
            {
                double num = double.Parse(strNum);
                return IntInRubAndCop(num);
            }
            catch (FormatException)
            {
                MessageBox.Show("Не правильно введена цифра " + strNum + ", дробная часть должна отделяться запятой.");
                return null;
            }
        }
        #endregion IntInRubAndCop - Число с плавающей запятой переделать в формат "0 руб. 0 коп."


        /// <summary> 01 в явнваря. Если делать через "МММ", то он пишет январь, это не подходит </summary>
        public static string MonthInString(int month)
        {
            switch (month)
            {
                case 1:
                    return "января";
                case 2:
                    return "февраля";
                case 3:
                    return "марта";
                case 4:
                    return "апреля";
                case 5:
                    return "мая";
                case 6:
                    return "июня";
                case 7:
                    return "июля";
                case 8:
                    return "августа";
                case 9:
                    return "сентября";
                case 10:
                    return "октября";
                case 11:
                    return "ноября";
                case 12:
                    return "декабря";
            }
            return "";
        }


    }
}
