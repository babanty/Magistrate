using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Magistrate
{
    /// <summary>
    /// Значение контрола
    /// </summary>
    public class ValueControl
    {
        /// <summary>
        /// Текст содержащиеся в контроле
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Ключ, который указан в ворде шаблоне
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Название контрола для поиска значений по названию
        /// </summary>
        public string NameControl { get; }

        /// <summary>
        /// Собственно сам контрол
        /// </summary>
        public readonly Control Control;

        /// <summary>
        /// конструктор с обязательными параметрами
        /// </summary>
        /// <param name="Control">Контрол, который мы записываем</param>
        /// <param name="Key">Его ключ</param>
        public ValueControl(Control Control)
        {
            this.Control = Control;
            Key = "";
            Text = Control.Text;
            NameControl = Control.Name;
        }
    }
}
