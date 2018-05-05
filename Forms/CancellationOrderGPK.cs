﻿using System;
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
    public partial class CancellationOrderGPK : Form
    {
        public CancellationOrderGPK()
        {
            InitializeComponent();

            Db.SetPropertiesComboBox(ref comboBox7, "bank incomplete"); // Заполняем банки
        }

        // Выбранный участок, автоматически подставляет кто судья
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == PropertiesMyApp.GetPropertiesValue(TypeProperties.PlaceNum))
            {
                comboBox2.Text = "Мировой судья";
                comboBox3.Text = "мировым судьей";
            }
            else
            {
                comboBox2.Text = "И.о. мирового судьи";
                comboBox3.Text = "и.о. мирового судьи";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GenerationWord.StandartListValueControl(Controls);
            
            // Если с восстановлением, то один шаблон, если нет, то другой
            if (radioButton1.Checked)
            {
                // Сгенерировать ворд
                GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "Отмена приказа по ГПК Без восстановления", controlArrayToString);
            }
            else if (radioButton2.Checked)
            {
                // Сгенерировать ворд
                GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "Отмена приказа по ГПК С восстановлением", controlArrayToString);
            }
            else
            {
                MessageBox.Show("Не выбрано с восстановлением или без");
            }
        }
    }
}