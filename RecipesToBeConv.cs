using ClosedXML.Excel;
using System;
using System.Collections.Generic;

namespace SimpleConverter
{
    internal class RecipesToBeConv
    {
        public readonly List<RecipeToBeConv> recipesToBeConv = new List<RecipeToBeConv>();
        private Form1 F1;

        public RecipesToBeConv(Form1 F1)
        {
            this.F1 = F1;
            recipesToBeConv.Add(new RecipeToBeConv(0, F1));
        }

        public int Count => recipesToBeConv.Count;

        internal void AddRecipe()
        {
            recipesToBeConv.Add(new RecipeToBeConv(Count, F1));
            F1.bDelRecipeToBeConvEnable();
        }

        internal void DeleteRecipe()
        {
            recipesToBeConv[Count - 1].Remove();
            recipesToBeConv.RemoveAt(Count - 1);
            if (Count <= 1) F1.bDelRecipeToBeConvDisable();
        }

        internal void refreshRecipeNames()
        {
            foreach (RecipeToBeConv recipeToBeConv in recipesToBeConv)
                recipeToBeConv.RefreshRecipeName();
        }

        internal void fillXlDoc(IXLWorksheet ws)
        {
            ws.Name = "Конвертация";
            ws.Cell("A1").Value = "Наименование";
            ws.Cell("B1").Value = "На объем ГП";
            ws.Cell("C1").Value = "Потребность";
            ws.Cell("C2").Value = "Наименование";
            ws.Cell("D2").Value = "Количество";
            ws.Cell("E2").Value = "Ед. Изм";

            fillXlRecipes(ws);
            customizeXlDoc(ws);
        }

        private void fillXlRecipes(IXLWorksheet ws)
        {
            int startCellX = 3;
            foreach (RecipeToBeConv recipeToBeConv in recipesToBeConv)
            {
                const double RECIPE_MASS = 1000;
                string recipeName = recipeToBeConv.RecipeNameCB.Text;
                string[,] ingredients = F1.Storage[recipeToBeConv.RecipeNameCB.Text];
                string[,] ingrsWithStuff = addStuff(ingredients, recipeToBeConv);
                var currentContDict = RecipeToBeConv.ContTypesDict[recipeToBeConv.ContTypeCB.Text]; 
                double currentContVolume = currentContDict[recipeToBeConv.ContVolumeCB.Text];
                uint amount = UInt32.Parse(recipeToBeConv.AmountTB.Text);
                double multiplier = (amount * currentContVolume) / RECIPE_MASS;
                int id = recipeToBeConv.ID;


                fillXlIngredients(recipeName, amount, ingrsWithStuff, multiplier, id, startCellX, ws);
                startCellX += ingrsWithStuff.GetLength(0);
            }
        }

        private string[,] addStuff(string[,] ingredients, RecipeToBeConv recipeToBeConv)
        {
            string currentContLid = RecipeToBeConv.ContLidDict[recipeToBeConv.ContTypeCB.Text][recipeToBeConv.ContVolumeCB.Text];
            string amount = recipeToBeConv.AmountTB.Text;
            string[,] stuff = {
                {"Крышка " + currentContLid, amount, "шт" },
                {recipeToBeConv.ContTypeCB.Text, amount, "шт" },
                {"Этикетка", amount, "шт" }
            };
            int newIngredientsLength = ingredients.GetLength(0) + stuff.GetLength(0);
            string[,] newIngredients = new string[newIngredientsLength, Form2.COLUMN_AMOUNT];

            append(newIngredients, ingredients, 0);
            append(newIngredients, stuff, ingredients.GetLength(0));
            return newIngredients;
        }

        private void append(string[,] arr1, string[,] arr2, int inx)
        {
            for (int i = 0; i < arr2.GetLength(0); i++)
                for (int j = 0; j < arr2.GetLength(1); j++)
                {
                    arr1[inx + i, j] = arr2[i, j];
                }
        }


        private void fillXlIngredients(string recipeName, uint amount, string[,] ingredients, double multiplier, int id, int startCellX, IXLWorksheet ws)
        {
            ws.Cell(startCellX, 1).Value = recipeName;
            ws.Cell(startCellX, 2).Value = amount;
            for (int i = 0; i < ingredients.GetLength(0); i++)
                for (int j = 0; j < ingredients.GetLength(1); j++)
                {
                    if (j == 1 && ingredients.GetLength(0) - i > 3)
                    {
                        ws.Cell(startCellX + i, 3 + j).Value = Math.Round(Double.Parse(ingredients[i, j]) * multiplier, 4);
                    }
                    else ws.Cell(startCellX + i, 3 + j).Value = ingredients[i, j];
                }
            customizeXlDoc(ws, ingredients.GetLength(0), startCellX);
        }

        private void customizeXlDoc(IXLWorksheet ws, int ingredientsLength, int startCellX)
        {
            IXLRange allRecipe = ws.Range($"A{startCellX}", $"E{ingredientsLength + startCellX - 1}");
            allRecipe.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            allRecipe.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            allRecipe.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            allRecipe.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            allRecipe.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            allRecipe.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            allRecipe.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            allRecipe.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
        }
        private void customizeXlDoc(IXLWorksheet ws)
        {
            IXLRange allHead = ws.Range("A1", $"E2");
            ws.Columns().AdjustToContents();
            ws.Range("A1:A2").Column(1).Merge();
            ws.Range("B1:B2").Column(1).Merge();
            ws.Range("C1:E1").Row(1).Merge();
            allHead.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            allHead.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            allHead.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            allHead.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            allHead.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            allHead.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            allHead.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            allHead.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
        }

    }
}