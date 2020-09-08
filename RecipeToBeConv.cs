using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SimpleConverter
{
    internal class RecipeToBeConv
    {
        public int ID;
        public ComboBox RecipeNameCB { get; }
        public TextBox AmountTB { get; }
        public ComboBox ContTypeCB { get; }
        public ComboBox ContVolumeCB { get; }
        public static readonly Dictionary<string, double> BarrelVolumesDict = new Dictionary<string, double>
        {
            {"1 л", 1},
            {"0,5 л", 0.5},
            {"0,27 л", 0.27}
        };
        public static readonly Dictionary<string, string> BarrelLidDict = new Dictionary<string, string>
        {
            {"1 л", "Б"},
            {"0,5 л", "Б"},
            {"0,27 л", "М"}
        };
        public static readonly Dictionary<string, double> BucketVolumesDict = new Dictionary<string, double>
        {
            {"10 кг", 10},
            {"5 кг", 5},
            {"3 кг", 3}
        };
        public static readonly Dictionary<string, string> BucketLidDict = new Dictionary<string, string>
        {
            {"10 кг", "10кг"},
            {"5 кг", "5кг"},
            {"3 кг", "3кг"}
        };
        public static readonly Dictionary<string, Dictionary<string, double>> ContTypesDict = 
            new Dictionary<string, Dictionary<string, double>>
            {
                {"Бочка", BarrelVolumesDict},
                {"Ведро", BucketVolumesDict},
            };
        public static readonly Dictionary<string, Dictionary<string, string>> ContLidDict =
            new Dictionary<string, Dictionary<string, string>>
            {
                {"Бочка", BarrelLidDict},
                {"Ведро", BucketLidDict},
            };
        string[] ContTypes = ContTypesDict.Keys.Select(i => i.ToString()).ToArray();
        string[] BarrelVolumes = BarrelVolumesDict.Keys.Select(i => i.ToString()).ToArray();
        string[] BucketVolumes = BucketVolumesDict.Keys.Select(i => i.ToString()).ToArray();
        private const int RECIPE_NAME_X = 23;
        private const int AMOUNT_X = 233;
        private const int CONT_TYPE_X = 332;
        private const int CONT_VOLUME_X = 476;
        private const int START_Y = 34;
        private const int DIST_BETW = 27;
        private const int RECIPE_NAME_SZ_X = 186;
        private const int RECIPE_NAME_SZ_Y = 21;
        private const int AMOUNT_SZ_X = 73;
        private const int AMOUNT_SZ_Y = 21;
        private const int CONT_TYPE_SZ_X = 121;
        private const int CONT_TYPE_SZ_Y = 21;
        private const int CONT_VOLUME_SZ_X = 121;
        private const int CONT_VOLUME_SZ_Y = 21;
        private Form1 F1;



        public RecipeToBeConv(int id, Form1 F1)
        {
            ID = id;
            this.F1 = F1;
            RecipeNameCB = CreateRecipeNameCB();
            AmountTB = CreateAmountTB();
            ContTypeCB = CreateContTypeCB();
            ContVolumeCB = CreateContVolumeCB();
            addEvents();
        }

        private void addEvents()
        {
            RecipeNameCB.TextUpdate += new EventHandler(F1.cbRecipeName_TextUpdate);
            RecipeNameCB.Leave += new EventHandler(F1.recipeName_Leave);
            RecipeNameCB.SelectionChangeCommitted += new System.EventHandler(F1.recipeName_SelectionChangeCommitted);
            AmountTB.TextChanged += new EventHandler(tbAmount_TextChanged);
            ContTypeCB.SelectedIndexChanged += new EventHandler(cbContainerType_SelectedIndexChanged);
        }

        private ComboBox CreateRecipeNameCB()
        {
            ComboBox newCB = new ComboBox
            {
                Location = new Point(RECIPE_NAME_X, START_Y + DIST_BETW * ID),
                Name = "cbRecipeName" + ID.ToString(),
                Size = new Size(RECIPE_NAME_SZ_X, RECIPE_NAME_SZ_Y)
            };

            F1.fillRecipeNameCB(newCB);
            F1.Controls.Add(newCB);
            newCB.SelectedIndex = 0;
            return newCB;
        }

        private TextBox CreateAmountTB()
        {
            TextBox newTB = new TextBox
            {
                Location = new Point(AMOUNT_X, START_Y + DIST_BETW * ID),
                Name = "tbAmount" + ID.ToString(),
                Size = new Size(AMOUNT_SZ_X, AMOUNT_SZ_Y)
            };
            F1.Controls.Add(newTB);
            newTB.Text = "0";
            return newTB;
        }

        private ComboBox CreateContTypeCB()
        {
            ComboBox newCB = new ComboBox
            {
                Location = new Point(CONT_TYPE_X, START_Y + DIST_BETW * ID),
                Name = "cbContainerType" + ID.ToString(),
                Size = new Size(CONT_TYPE_SZ_X, CONT_TYPE_SZ_Y),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            newCB.Items.AddRange(ContTypes);
            F1.Controls.Add(newCB);
            newCB.SelectedIndex = 0;
            return newCB;
        }

        private ComboBox CreateContVolumeCB()
        {
            ComboBox newCB = new ComboBox
            {
                Location = new Point(CONT_VOLUME_X, START_Y + DIST_BETW * ID),
                Name = "cbContainerVolume" + ID.ToString(),
                Size = new Size(CONT_VOLUME_SZ_X, CONT_VOLUME_SZ_Y),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            newCB.Items.AddRange(BarrelVolumes);
            F1.Controls.Add(newCB);
            newCB.SelectedIndex = 0;
            return newCB;
        }

        internal void Remove()
        {
            F1.Controls.Remove(RecipeNameCB);
            F1.Controls.Remove(AmountTB);
            F1.Controls.Remove(ContTypeCB);
            F1.Controls.Remove(ContVolumeCB);
        }

        private void cbContainerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ContVolumeCB.Items.Clear();
            switch (ContTypeCB.SelectedItem.ToString())
            {
                case "Бочка":
                    ContVolumeCB.Items.AddRange(BarrelVolumes);
                    break;
                case "Ведро":
                    ContVolumeCB.Items.AddRange(BucketVolumes);
                    break;
            }
            ContVolumeCB.SelectedIndex = 0;
        }

        public static void RefreshRecipeName(ComboBox cb, Form1 F1)
        {
            object currentItem = cb.SelectedItem;
            cb.Items.Clear();
            F1.fillRecipeNameCB(cb);
            if (cb.Items.Contains(currentItem)) cb.SelectedItem = currentItem;
            else cb.SelectedIndex = 0;
        }

        private void tbAmount_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string amount = tb.Text;
            for (int i = 0; i < amount.Length; i++)
            {
                if (!Char.IsNumber(amount[i]))
                {
                    tb.Text = amount.Remove(i, 1);
                    tb.SelectionStart = amount.Length - 1;
                }
            }
        }

        internal void RefreshRecipeName()
        {
            RefreshRecipeName(RecipeNameCB, F1);
        }
    }
}