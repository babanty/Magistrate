using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magistrate
{
    /// <summary>
    /// Логика настроек приложения
    /// </summary>
    public static class PropertiesMyApp
    {
        // Инициализация файла для сохранения настроек
        private static IniFacade properties = new IniFacade("PropertiesApp");

        /// <summary>
        /// Возвращает значение указанной настройки
        /// </summary>
        /// <param name="typeProperties">тип настройки</param>
        /// <param name="ifNull">если настройка не установлена, вернет это значение</param>
        /// <returns>Возвращает значение указанной настройки</returns>
        public static string GetPropertiesValue(TypeProperties typeProperties, string ifNull = null)
        {
            return properties.IniReadKey("PropertiesApp", typeProperties.ToString(), ifNull);
        }

        /// <summary>
        /// Установить настройку
        /// </summary>
        /// <param name="value">значение настройки</param>
        /// <param name="typeProperties">тип настройки</param>
        public static void SetPropertiesValue(string value , TypeProperties typeProperties)
        {
            properties.Write("PropertiesApp", typeProperties.ToString(), value);
        }
    }


    /// <summary> Тип настройки </summary>
    public enum TypeProperties
    {
        /// <summary> номер основного участка </summary>
        PlaceNum
    }
}
