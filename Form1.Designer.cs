namespace SimpleConverter
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.lName = new System.Windows.Forms.Label();
            this.bAddRecipe = new System.Windows.Forms.Button();
            this.bSave = new System.Windows.Forms.Button();
            this.lAmount = new System.Windows.Forms.Label();
            this.bChangeRecipe = new System.Windows.Forms.Button();
            this.bDeleteRecipe = new System.Windows.Forms.Button();
            this.bSaveAs = new System.Windows.Forms.Button();
            this.lContainer = new System.Windows.Forms.Label();
            this.lContVolume = new System.Windows.Forms.Label();
            this.bAddRecipeToBeConv = new System.Windows.Forms.Button();
            this.bDelRecipeToBeConv = new System.Windows.Forms.Button();
            this.recipeCB = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lName
            // 
            this.lName.AutoSize = true;
            this.lName.Location = new System.Drawing.Point(20, 18);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(83, 13);
            this.lName.TabIndex = 1;
            this.lName.Text = "Наименование";
            // 
            // bAddRecipe
            // 
            this.bAddRecipe.Location = new System.Drawing.Point(55, 46);
            this.bAddRecipe.Name = "bAddRecipe";
            this.bAddRecipe.Size = new System.Drawing.Size(149, 25);
            this.bAddRecipe.TabIndex = 2;
            this.bAddRecipe.Text = "Добавить рецепт";
            this.bAddRecipe.UseVisualStyleBackColor = true;
            this.bAddRecipe.Click += new System.EventHandler(this.bAddRecipe_Click);
            // 
            // bSave
            // 
            this.bSave.Location = new System.Drawing.Point(613, 62);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(106, 25);
            this.bSave.TabIndex = 3;
            this.bSave.Text = "Сохранить";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // lAmount
            // 
            this.lAmount.AutoSize = true;
            this.lAmount.Location = new System.Drawing.Point(230, 18);
            this.lAmount.Name = "lAmount";
            this.lAmount.Size = new System.Drawing.Size(66, 13);
            this.lAmount.TabIndex = 5;
            this.lAmount.Text = "Количество";
            // 
            // bChangeRecipe
            // 
            this.bChangeRecipe.Enabled = false;
            this.bChangeRecipe.Location = new System.Drawing.Point(55, 77);
            this.bChangeRecipe.Name = "bChangeRecipe";
            this.bChangeRecipe.Size = new System.Drawing.Size(149, 25);
            this.bChangeRecipe.TabIndex = 6;
            this.bChangeRecipe.Text = "Изменить рецепт";
            this.bChangeRecipe.UseVisualStyleBackColor = true;
            this.bChangeRecipe.Click += new System.EventHandler(this.bChangeRecipe_Click);
            // 
            // bDeleteRecipe
            // 
            this.bDeleteRecipe.Enabled = false;
            this.bDeleteRecipe.Location = new System.Drawing.Point(55, 108);
            this.bDeleteRecipe.Name = "bDeleteRecipe";
            this.bDeleteRecipe.Size = new System.Drawing.Size(149, 25);
            this.bDeleteRecipe.TabIndex = 7;
            this.bDeleteRecipe.Text = "Удалить рецепт";
            this.bDeleteRecipe.UseVisualStyleBackColor = true;
            this.bDeleteRecipe.Click += new System.EventHandler(this.bDeleteRecipe_Click);
            // 
            // bSaveAs
            // 
            this.bSaveAs.Location = new System.Drawing.Point(613, 93);
            this.bSaveAs.Name = "bSaveAs";
            this.bSaveAs.Size = new System.Drawing.Size(106, 25);
            this.bSaveAs.TabIndex = 9;
            this.bSaveAs.Text = "Сохранить как...";
            this.bSaveAs.UseVisualStyleBackColor = true;
            this.bSaveAs.Click += new System.EventHandler(this.bSaveAs_Click);
            // 
            // lContainer
            // 
            this.lContainer.AutoSize = true;
            this.lContainer.Location = new System.Drawing.Point(329, 18);
            this.lContainer.Name = "lContainer";
            this.lContainer.Size = new System.Drawing.Size(32, 13);
            this.lContainer.TabIndex = 11;
            this.lContainer.Text = "Тара";
            // 
            // lContVolume
            // 
            this.lContVolume.AutoSize = true;
            this.lContVolume.Location = new System.Drawing.Point(473, 18);
            this.lContVolume.Name = "lContVolume";
            this.lContVolume.Size = new System.Drawing.Size(70, 13);
            this.lContVolume.TabIndex = 13;
            this.lContVolume.Text = "Объем тары";
            // 
            // bAddRecipeToBeConv
            // 
            this.bAddRecipeToBeConv.Location = new System.Drawing.Point(613, 18);
            this.bAddRecipeToBeConv.Name = "bAddRecipeToBeConv";
            this.bAddRecipeToBeConv.Size = new System.Drawing.Size(37, 38);
            this.bAddRecipeToBeConv.TabIndex = 14;
            this.bAddRecipeToBeConv.Text = "+";
            this.bAddRecipeToBeConv.UseVisualStyleBackColor = true;
            this.bAddRecipeToBeConv.Click += new System.EventHandler(this.bAddRecipeToBeConv_Click);
            // 
            // bDelRecipeToBeConv
            // 
            this.bDelRecipeToBeConv.Enabled = false;
            this.bDelRecipeToBeConv.Location = new System.Drawing.Point(656, 18);
            this.bDelRecipeToBeConv.Name = "bDelRecipeToBeConv";
            this.bDelRecipeToBeConv.Size = new System.Drawing.Size(37, 38);
            this.bDelRecipeToBeConv.TabIndex = 15;
            this.bDelRecipeToBeConv.Text = "-";
            this.bDelRecipeToBeConv.UseVisualStyleBackColor = true;
            this.bDelRecipeToBeConv.Click += new System.EventHandler(this.bDelRecipeToBeConv_Click);
            // 
            // recipeCB
            // 
            this.recipeCB.FormattingEnabled = true;
            this.recipeCB.Location = new System.Drawing.Point(6, 19);
            this.recipeCB.Name = "recipeCB";
            this.recipeCB.Size = new System.Drawing.Size(198, 21);
            this.recipeCB.TabIndex = 16;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.recipeCB);
            this.groupBox1.Controls.Add(this.bAddRecipe);
            this.groupBox1.Controls.Add(this.bDeleteRecipe);
            this.groupBox1.Controls.Add(this.bChangeRecipe);
            this.groupBox1.Location = new System.Drawing.Point(613, 161);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(211, 141);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Рецепты";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 314);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bDelRecipeToBeConv);
            this.Controls.Add(this.bAddRecipeToBeConv);
            this.Controls.Add(this.lContVolume);
            this.Controls.Add(this.lContainer);
            this.Controls.Add(this.bSaveAs);
            this.Controls.Add(this.lAmount);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.lName);
            this.Name = "Form1";
            this.Text = "Рецепты";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.Button bAddRecipe;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Label lAmount;
        private System.Windows.Forms.Button bChangeRecipe;
        private System.Windows.Forms.Button bDeleteRecipe;
        private System.Windows.Forms.Button bSaveAs;
        private System.Windows.Forms.Label lContainer;
        private System.Windows.Forms.Label lContVolume;
        private System.Windows.Forms.Button bAddRecipeToBeConv;
        private System.Windows.Forms.Button bDelRecipeToBeConv;
        private System.Windows.Forms.ComboBox recipeCB;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

