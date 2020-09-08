using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleConverter
{
    public partial class Form2 : Form
    {
        private const int TB1_X = 231;
        private const int TB1_Y = 46;
        private const int DIST_BETW_TB_H = 186;
        private const int DIST_BETW_TB_V = 30;
        public const int COLUMN_AMOUNT = 3;
        private Form1 F1 { get; set; } = null;
        private List<TextBox[]> Ingredients { get; set; } = new List<TextBox[]>();

        public Form2()
        {
            InitializeComponent();
            button_plus_Click(null, EventArgs.Empty);
        }

        public Form2(Form1 f)
            : this()
        {
            F1 = f;
            F1.Enabled = false;
        }

        public Form2(Form1 f, string caption, string key)
            : this(f)
        {
            this.Text = caption;
            formRestore(key);
            formFilling(key);
        }

        private void formRestore(string key)
        {
            F1.tryGetIngredients(key, out string[,] ingredient_values);
            int rows = ingredient_values.GetUpperBound(0);
            for (int i = 0; i < rows; i++)
            {
                button_plus_Click(null, EventArgs.Empty);
            }
        }

        private void formFilling(string key)
        {
            F1.tryGetIngredients(key, out string[,] ingredient_values);

            textBox1.Text = key;
            for (int i = 0; i < Ingredients.Count; i++)
            {
                for (int j = 0; j < COLUMN_AMOUNT; j++)
                {
                    try { Ingredients[i][j].Text = ingredient_values[i, j]; }
                    catch { Ingredients[i][j].Text = ""; };
                }
            }
        }
        private void InitializeIngeidients()
        {
            Ingredients.ForEach((TextBox[] ingredient) =>
            {
                for (int i = 0; i < COLUMN_AMOUNT; i++)
                {
                    Controls.Add(ingredient[i]);
                }
            }
            );
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            F1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (GetrecipeNameFieldIsOK() &&
                GetingredientNameFieldsIsOK() &&
                amountFieldsIsOK())
            {
                fromFormToStorage();
            }
        }

        private bool amountFieldsIsOK()
        {
            normalizeDoubleInAmount();
            foreach (TextBox[] ingredient in Ingredients)
            {
                try
                {
                    Double.Parse(ingredient[1].Text);
                }
                catch 
                {
                    MessageBox.Show($"Количество ингредиента {ingredient[0].Text} должно быть вещественным числом");
                    return false;
                }
            }
            return true;
        }

        private bool GetingredientNameFieldsIsOK()
        {
            foreach (TextBox[] ingredient in Ingredients)
            {
                if (ingredient[0].Text.Length > 0) return true;
                else
                {
                    MessageBox.Show("У ингредиента должно быть название!", "Ошибка");
                    return false;
                }
            }
            return true;
        }

        private bool GetrecipeNameFieldIsOK()
        {
            if (textBox1.Text.Length > 0) return true;
            else
            {
                MessageBox.Show("У рецепта должно быть название!", "Ошибка");
                return false;
            }
        }

        private void normalizeDoubleInAmount()
        {
            foreach (TextBox[] ingredient in Ingredients) 
            {
                ingredient[1].Text = String.Join(",", ingredient[1].Text.Split('.'));
            }
        }

        private bool updateRecipe(string[,] ingredients_values)
        {
            var result = MessageBox.Show(
                    $"Изменить рецепт {textBox1.Text}?",
                    "Изменение",
                    MessageBoxButtons.YesNo
                );
            if (result == DialogResult.Yes)
            {
                F1.Storage.Remove(textBox1.Text);
                F1.Storage.Add(textBox1.Text, ingredients_values);
                return true;
            }
            else return false;
        }

        private void fromFormToStorage()
        {
            bool existRecipe = F1.Storage.TryGetValue(textBox1.Text, out _);
            string[,] ingredients_values = fillIngValues();

            if (existRecipe)
            {
                if (!updateRecipe(ingredients_values)) return;
            }
            else F1.Storage.Add(textBox1.Text, ingredients_values);
            F1.saveStorage();
            F1.refreshRecipeNames();
            MessageBox.Show($"Рецепт {textBox1.Text} успешно сохранен!", "Успешно!");
            this.Close();
        }

        private string[,] fillIngValues()
        {
            string [,] ingredients_values = new string[Ingredients.Count, COLUMN_AMOUNT];

            for (int i = 0; i < Ingredients.Count; i++)
            {
                for (int j = 0; j < COLUMN_AMOUNT; j++)
                {
                    ingredients_values[i, j] = Ingredients[i][j].Text;
                }
            }
            return ingredients_values;
        }

        private void button_plus_Click(object sender, EventArgs e)
        {
            TextBox[] newBoxes = new TextBox[COLUMN_AMOUNT];
            for (int i = 0; i < newBoxes.Length; i++)
            {
                newBoxes[i] = CreateTextBox(TB1_X + i * DIST_BETW_TB_H,
                              TB1_Y + DIST_BETW_TB_V * Ingredients.Count);
                if (i == newBoxes.Length - 1) newBoxes[i].Text = "кг";
            }
            foreach (TextBox newBox in newBoxes) 
                newBox.KeyDown += new KeyEventHandler(this.textBox_KeyDown);
            Ingredients.Add(newBoxes);
            InitializeIngeidients();
            button_minus.Enabled = true;
        }

        private static TextBox CreateTextBox(int x, int y)
        {
            TextBox textBox = new TextBox();
            textBox.Location = new Point(x, y);
            textBox.Name = "textBox";
            textBox.Size = new Size(180, 20);
            return textBox;
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox senderTextBox = (TextBox)sender;
                for (int i = 0; i < Ingredients.Count; i++)
                {
                    foreach (TextBox textBox in Ingredients[i])
                    {
                        if (senderTextBox.TabIndex == textBox.TabIndex) enterKeyAction(i);
                    }
                }
            }
        }

        private void enterKeyAction(int index)
        {
            if (index == Ingredients.Count - 1) button_plus_Click(null, EventArgs.Empty);
            this.ActiveControl = Ingredients[index+1][0];
        }

        private void button_minus_Click(object sender, EventArgs e)
        {
            foreach (TextBox ingredient in Ingredients[Ingredients.Count - 1])
            {
                Controls.Remove(ingredient);
            }
            Ingredients.RemoveAt(Ingredients.Count - 1);
            InitializeIngeidients();
            if (Ingredients.Count < 2) button_minus.Enabled = false;
        }
    }
}
