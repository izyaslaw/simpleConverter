using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.Linq;

namespace SimpleConverter
{
    public partial class Form1 : Form
    {
        internal SortedDictionary<string, string[,]> Storage { get; set; } = new SortedDictionary<string, string[,]>();
        internal RecipesToBeConv recipesToBeConv;
        private string path = readPathTxt();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadStorage();
            recipesToBeConv = new RecipesToBeConv(this);
            fillRecipeCB();
            CRUDDisableIfRecipesEmpty();
        }

        private void fillRecipeCB()
        {
            fillRecipeNameCB(recipeCB);
            recipeCB.TextUpdate += new EventHandler(cbRecipeName_TextUpdate);
            recipeCB.Leave += new EventHandler(recipeName_Leave);
            recipeCB.SelectionChangeCommitted += new EventHandler(recipeName_SelectionChangeCommitted);
            recipeCB.SelectedIndex = 0;
        }

        private static string readPathTxt()
        {
            try { return File.ReadAllText("path.txt"); }
            catch { return returnNewPathTxt(); }
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

        private void CRUDDisableIfRecipesEmpty()
        {
            if (Storage.Count > 0)
            {
                bChangeRecipe.Enabled = true;
                bDeleteRecipe.Enabled = true;
            }
        }

        internal void refreshRecipeNames()
        {
            recipesToBeConv.refreshRecipeNames();
            RecipeToBeConv.RefreshRecipeName(recipeCB, this);
            CRUDDisableIfRecipesEmpty();
        }

        internal void loadStorage()
        {
            string json;
            try
            {
                json = File.ReadAllText("recipes.json");
                Storage = JsonConvert.DeserializeObject<SortedDictionary<string, string[,]>>(json);
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

        private void DeleteRecipe(string key)
        {
            Storage.Remove(key);
            saveStorage();
            refreshRecipeNames();
        }

        internal bool tryGetIngredients(string recipeName, out string[,] ingredients)
        {
            try { ingredients = Storage[recipeName]; }
            catch
            {
                MessageBox.Show("Выберите существующий рецепт!", "Ошибка в наименовании рецепта");
                ingredients = new string[0,0];
                return false;
            }
            return true;
        }

        private bool tryGetMultiplier(string stringAmount, out double multiplier)
        {
            const double UNIT_KOEFF = 0.5;
            const double RECIPE_MASS = 1000;
            ulong amount;
            try { amount = ulong.Parse(stringAmount); }
            catch
            {
                MessageBox.Show("Количество должно быть положительным целым числом!", "Ошибка в строке Количество");
                multiplier = 0;
                return false;
            }
            multiplier = (amount * UNIT_KOEFF) / RECIPE_MASS;
            return true;
        }

        private void bAddRecipe_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2(this);
            newForm.Show();
        }

        private void bChangeRecipe_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2(this, "Изменить", recipeCB.Text);
            newForm.Show();
        }

        private void bDeleteRecipe_Click(object sender, EventArgs e)
        {
            string key = recipeCB.SelectedItem.ToString();
            DialogResult DeleteIt = MessageBox.Show(
                $"Удалить рецепт {key}?",
                "Удаление",
                MessageBoxButtons.YesNo
            );
            if (DeleteIt == DialogResult.Yes) DeleteRecipe(key);
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            string now = String.Join(".", DateTime.Now.ToString().Split(':'));
            string fileName = $"Конвертация {now}.xlsx";

            XLWorkbook workbook = new XLWorkbook();
            IXLWorksheet ws = workbook.Worksheets.Add("Конвертация");
            recipesToBeConv.fillXlDoc(ws);
            workbook.SaveAs(path + fileName);
            MessageBox.Show($"Файл {fileName} успешно сохранен", "Успешно");
        }

        private void bSaveAs_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog DirDialog = new FolderBrowserDialog();
            if (DirDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            rewritePathTxt(DirDialog.SelectedPath);
            path = readPathTxt();
            bSave_Click(null, EventArgs.Empty);
        }

        private void bAddRecipeToBeConv_Click(object sender, EventArgs e)
        {
            recipesToBeConv.AddRecipe();
        }

        private void bDelRecipeToBeConv_Click(object sender, EventArgs e)
        {
            recipesToBeConv.DeleteRecipe();
        }

        internal void bDelRecipeToBeConvEnable()
        {
            bDelRecipeToBeConv.Enabled = true;
        }

        internal void bDelRecipeToBeConvDisable()
        {
            bDelRecipeToBeConv.Enabled = false;
        }

        internal void cbRecipeName_TextUpdate(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            List<string> newValues = new List<string>();
            string[] allKeys = Storage.Keys.ToArray();
            string currentText = cb.Text;
            Cursor.Current = Cursors.Default;

            foreach (string key in allKeys) if (key.Contains(currentText)) newValues.Add(key);
            cb.Items.Clear();
            cb.SelectionStart = cb.Text.Length;
            cb.Select(cb.Text.Length, 0);
            if (newValues.Count == 0) cb.Items.AddRange(allKeys);
            else cb.Items.AddRange(newValues.ToArray());
            cb.DroppedDown = true;
        }

        public void fillRecipeNameCB(ComboBox cb)
        {
            string[] storageKeys = Storage.Keys.Select(i => i.ToString()).ToArray();
            cb.Items.AddRange(storageKeys);
        }

        internal void recipeName_Leave(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (!cb.Items.Contains(cb.Text)) 
            { 
                cb.SelectedIndex = 0;
                cb.Text = cb.SelectedItem.ToString();
            }
        }

        internal void recipeName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            RecipeToBeConv.RefreshRecipeName(cb, this);
        }
    }
}
