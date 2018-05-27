using Magistrate.FormLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Magistrate.Forms
{
    public partial class CancellationOrderKAS : Form
    {
        private string nameForm; // название формы, идентичен ее названию ее класса

        #region Инициализация

        public CancellationOrderKAS()
        {
            InitializeComponent();

            FormController.SetStandartParamsInControls
                (nameForm,
                ref comboBoxLoad,               // заполнение вариантами сохранений
                ref comboBoxDateOfOrderMonth,   // заполнение месяца вынесения решения
                ref comboBoxDateOfOrderYear);   // заполнение года вынесения решения

            nameForm = this.GetType().ToString(); // Название формы
        }

        /// <summary>Обновить контролы на форме</summary>
        /// <param name="controls">контролы, которыми будут заменяться контролы на форме</param>
        private void UpdateControls(Control.ControlCollection controls)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                Controls[i].Text = controls[i].Text;
            }
        }
        #endregion Инициализация


        #region Автоматическое заполнение полей

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxWhoIsJudge.Text = FormController.GetMagistrateOrDeputy(comboBoxPlotNumber.Text);
        }


        // название при сохранении
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBoxClipPutName.Text = textBoxFullNameSurNameIvanova.Text;
        }

        #endregion Автоматическое заполнение полей


        #region Сохранение

        // Сохранить заполненные поля
        private void buttonSave_Click(object sender, EventArgs e)
        {
            FormController.SaveForm(Controls, nameForm, textBoxForSave.Text, ref comboBoxLoad);
        }

        // Загрузить сохраненные поля
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            // получаем заполненные сейвом контролы
            var controls = FormController.GetControlsLoadForm(Controls, nameForm, ref comboBoxLoad);

            UpdateControls(controls);
        }

        // Удалить сохранение
        private void buttonDeleteSave_Click(object sender, EventArgs e)
        {
            FormController.DeleteSave(ref comboBoxLoad, nameForm);
        }

        #endregion Сохранение


        // СГЕНЕРИРОВАТЬ WORD
        private void button1_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GenerationWord.StandartListValueControl(Controls);


            // Вставляем название в буфер обмена
            FormController.ClipPutNameWord(textBoxClipPutNum.Text, textBoxClipPutName.Text, this.Text);


            // Если с восстановлением, то один шаблон, если нет, то другой
            FormController.GenerateWord(controlArrayToString, checkBoxWithRestoration.Checked,
                "Отмена приказа с восстановлением КАС", "Отмена приказа без восстановления КАС");
        }
    }
}
