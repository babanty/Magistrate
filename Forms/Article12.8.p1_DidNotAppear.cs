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
    public partial class Article1281notAppear : Form
    {
        public Article1281notAppear()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GenerationWord.StandartListValueControl(Controls);

            // Сгенерировать ворд
            GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "ст.12.8 ч.1 не явился", controlArrayToString);
        }
    }
}
