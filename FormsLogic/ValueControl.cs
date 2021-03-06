﻿using System;
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
        /// Название контрола для поиска значений по названию, может быть равен null если ручное заполнение
        /// </summary>
        public string NameControl { get; }

        /// <summary>
        /// Собственно сам контрол, может быть равен null если ручное заполнение
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
            if (Control != null)
                NameControl = Control.Name;
        }

        /// <summary>
        /// конструктор с обязательными параметрами
        /// </summary>
        /// <param name="Control">Контрол, который мы записываем, может равняться null как обозначение ручного значения контрола</param>
        /// <param name="Key">Его ключ</param>
        /// <param name="Text">Текст контрола забить в ручную</param>
        public ValueControl(Control Control, string Text)
        {
            this.Control = Control;
            Key = "";
            this.Text = Text;
            if(Control != null)
                NameControl = Control.Name;
        }
    }
}
