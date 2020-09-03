using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace SimpleConverter
{
    public partial class Form1 : Form
    {
        internal Dictionary<string, string[,]> Storage { get; set; } = new Dictionary<string, string[,]>();
        private string path = readPathTxt();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2(this);
            newForm.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadStorage();
            refreshComboBox();
        }

        private static string readPathTxt()
        {
            try
            {
                return File.ReadAllText("path.txt");
            }
            catch 
            {
                return returnNewPathTxt();
            }
        }

        private static string returnNewPathTxt()
        {
            string pathText = AppDomain.CurrentDomain.BaseDirectory;
            StreamWriter file = File.CreateText("path.txt");
            file.Write(pathText);
            file.Close();
            return pathText;
        }

        private void rewritePathTxt(string newPath)
        {
            StreamWriter file = File.CreateText("path.txt");
            if (newPath[newPath.Length - 1] != '\\') newPath += "\\";
            file.Write(newPath);
            file.Close();
        }

        internal void fillComboBox()
        {
            var keys = Storage.Keys;
            foreach (string key in keys)
            {
                comboBox1.Items.Add(key);
            }
        }

        internal void refreshComboBox()
        {
            comboBox1.Items.Clear();
            fillComboBox();
            if (Storage.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
                button3.Enabled = true;
                button4.Enabled = true;
            } 
        }

        internal void loadStorage()
        {
            string json;
            try
            {
                json = File.ReadAllText("recipes.json");
                Storage = JsonConvert.DeserializeObject<Dictionary<string, string[,]>>(json);
            }
            catch { }
        }

        internal void saveStorage()
        {
            string json;
            StreamWriter file = File.CreateText("recipes.json");
            json = JsonConvert.SerializeObject(Storage);
            file.Write(json);
            file.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string key = comboBox1.SelectedItem?.ToString();
            DialogResult DeleteIt = MessageBox.Show(
                $"Удалить рецепт {key}?",
                "Удаление",
                MessageBoxButtons.YesNo
            );
            if (DeleteIt == DialogResult.Yes) DeleteRecipe(key);
            if (Storage.Count == 0)
            {
                comboBox1.Text = "";
                button3.Enabled = false;
                button4.Enabled = false;
            }

        }

        private void DeleteRecipe(string key)
        {
            Storage.Remove(key);
            saveStorage();
            refreshComboBox();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2(this, "Изменить", comboBox1.Text);
            newForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string recipeName = comboBox1.Text;
            string stringAmount = textBox1.Text;
            string[,] ingredient = Storage[recipeName];
            double multiplier;

            if (tryGetMultiplier(stringAmount, out multiplier))
            {
                XLWorkbook workbook = new XLWorkbook();
                IXLWorksheet ws = workbook.Worksheets.Add("Sample Sheet");
                fillXlDoc(recipeName, stringAmount, ingredient, ws, multiplier);
                customizeXlDoc(ws, ingredient);
                workbook.SaveAs(path + $"{recipeName} {stringAmount}.xlsx");
            }
        }

        private bool tryGetMultiplier(string stringAmount, out double multiplier)
        {
            const double UNIT_KOEFF = 0.5;
            const double RECIPE_MASS = 1000;
            ulong amount;
            try
            {
                amount = ulong.Parse(stringAmount);
            }
            catch
            {
                MessageBox.Show("Количество должно быть положительным целым числом!", "Ошибка в строке Количество");
                multiplier = 0;
                return false;
            }
            multiplier = (amount * UNIT_KOEFF) / RECIPE_MASS;
            return true;
        }

        private void fillXlDoc(string recipeName, string stringAmount, string[,] ingredient, IXLWorksheet ws, double multiplier)
        {
            ws.Name = recipeName + " " + stringAmount;
            ws.Cell("A1").Value = "Наименование";
            ws.Cell("A3").Value = recipeName;
            ws.Cell("B1").Value = "На объем ГП";
            ws.Cell("B3").Value = stringAmount;
            ws.Cell("C1").Value = "Потребность";
            ws.Cell("C2").Value = "Наименование";
            ws.Cell("D2").Value = "Количество";
            ws.Cell("E2").Value = "Ед. Изм";

            fillXlIngredients(ingredient, multiplier, ws);
        }

        private void fillXlIngredients(string[,] ingredient, double multiplier, IXLWorksheet ws)
        {
            for (int i = 0; i < ingredient.GetLength(0); i++)
            {
                for (int j = 0; j < ingredient.GetLength(1); j++)
                {
                    if (j == 1)
                    {
                        ws.Cell(3 + i, 3 + j).Value = Math.Round(Double.Parse(ingredient[i, j]) * multiplier, 4);
                    }
                    else ws.Cell(3 + i, 3 + j).Value = ingredient[i, j];
                }
            }
        }

        private void customizeXlDoc(IXLWorksheet ws, string[,] ingredient)
        {
            IXLRange allTable = ws.Range("A1", $"E{2 + ingredient.GetLength(0)}");
            ws.Columns().AdjustToContents();
            ws.Range("A1:A2").Column(1).Merge();
            ws.Range("B1:B2").Column(1).Merge();
            ws.Range("C1:E1").Row(1).Merge();
            allTable.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            allTable.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            allTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            allTable.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            allTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            allTable.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            allTable.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog DirDialog = new FolderBrowserDialog();
            if (DirDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            rewritePathTxt(DirDialog.SelectedPath);
            path = readPathTxt();
            button2_Click(null, EventArgs.Empty);
        }
    }
}
