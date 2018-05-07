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
    public partial class CreditOrder : Form
    {
        public CreditOrder()
        {
            InitializeComponent();

            Db.SetPropertiesComboBox(ref comboBox17, "bank incomplete"); // Заполняем банки
            Db.SetPropertiesComboBox(ref comboBox10, "mesto rogdeniya"); // Заполняем Населенный пункт, место рождения
            Db.SetPropertiesComboBox(ref comboBox13, "mesto jitelstva gorod"); // Место жителства населенный пункт
            Db.SetPropertiesComboBox(ref comboBox14, "mesto jitelstva ylitsa"); // Место жителства улица
            Db.SetPropertiesComboBox(ref comboBox15, "mesto jitelstva dom"); // Место жителства дом
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Сделать стандратный массив значений полей для ввода с формы с ключами для autoit скрипта генерирующего word 
            List<ValueControl> controlArrayToString = GenerationWord.StandartListValueControl(Controls);

            // Заполняем полные реквизиты банка
            string bankRequisites = GetBank(comboBox17.Text); // находим полные реквизиты банка
            if(bankRequisites == null) // банк не опознан
            {
                MessageBox.Show("Реквизиты банка не опознаны, после герации не забудьте их вписать");
                bankRequisites = "";
            }
            GenerationWord.AddValueControl(ref controlArrayToString, bankRequisites, "#-1"); // в ручную добавляем новый ключ

            // Заполнение второй даты, выделено отдельно т.к. иначе в word-е будут лишние точки
            string dateTwo = ""; // дата вторая 
            if (comboBox25.Text != "" && comboBox25.Text != null)
            {
                dateTwo = comboBox25.Text + "." + comboBox24.Text + "." + comboBox23.Text;
                dateTwo = " по " + dateTwo + " года";
            }
            GenerationWord.AddValueControl(ref controlArrayToString, dateTwo, "#-2"); // в ручную добавляем новый ключ

            // Изменение суммы задолженности по принципу 0 руб. 0 коп.
            string Debt = GenerationWord.IntInRubAndCop(textBox5.Text);
            if (Debt == null)
                return;
            GenerationWord.AddValueControl(ref controlArrayToString, Debt, "#-3"); // в ручную добавляем новый ключ

            // Изменение суммы государственной пошлины по принципу 0 руб. 0 коп.
            string Duty = GenerationWord.IntInRubAndCop(textBox6.Text);
            if (Duty == null)
                return;
            GenerationWord.AddValueControl(ref controlArrayToString, Duty, "#-4"); // в ручную добавляем новый ключ
            
            // Суммирование взыскиваемой суммы
            double summToPay = double.Parse(textBox5.Text) + double.Parse(textBox6.Text); // сумма к оплате
            string ToPay = GenerationWord.IntInRubAndCop(summToPay);
            GenerationWord.AddValueControl(ref controlArrayToString, ToPay, "#-5"); // в ручную добавляем новый ключ

            // Сгенерировать ворд
            GenerationWord.GenerateWord(Application.StartupPath + "\\Sample", "Приказ по банку или фин орг", controlArrayToString);
        }

        /// <summary>
        /// Возвращает полные реквизиты банка по названию или null
        /// </summary>
        /// <param name="Name">Название банка</param>
        private string GetBank(string Name)
        { // потом сделать динамчным вытгиванием из БД 
            int nameLength = Name.Length; // Количество символов в названии банка
            string result = null;


            // Проверяем сходится ли название с полными реквизитами
            List<string> allBanks = Db.GetColumn("bank complete");
            if (allBanks == null)
                return null;

            foreach(string bank in allBanks)
            {
                if (bank.Substring(0, nameLength) == Name) // первые слова в полных реквизитах соотвествуют 
                    return bank;
            }

            // Возвращаем результат null
            return result;


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == PropertiesMyApp.GetPropertiesValue(TypeProperties.PlaceNum))
            {
                comboBox5.Text = "Мировой судья";
            }
            else
            {
                comboBox5.Text = "И.о. мирового судьи";
            }
        }
    }
}

