namespace Calculator
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ButtonDecimalPoint = new System.Windows.Forms.Button();
            this.ButtonNegate = new System.Windows.Forms.Button();
            this.Button0 = new System.Windows.Forms.Button();
            this.ButtonEqual = new System.Windows.Forms.Button();
            this.Button2 = new System.Windows.Forms.Button();
            this.Button5 = new System.Windows.Forms.Button();
            this.Button8 = new System.Windows.Forms.Button();
            this.button45 = new System.Windows.Forms.Button();
            this.TextBoxPanel = new System.Windows.Forms.TextBox();
            this.ButtonTest = new System.Windows.Forms.Button();
            this.ButtonAdd = new System.Windows.Forms.Button();
            this.TextBoxSubPanel = new System.Windows.Forms.TextBox();
            this.ButtonMinus = new System.Windows.Forms.Button();
            this.ButtonMultiply = new System.Windows.Forms.Button();
            this.Button1 = new System.Windows.Forms.Button();
            this.Button3 = new System.Windows.Forms.Button();
            this.Button4 = new System.Windows.Forms.Button();
            this.Button6 = new System.Windows.Forms.Button();
            this.Button7 = new System.Windows.Forms.Button();
            this.Button9 = new System.Windows.Forms.Button();
            this.ButtonC = new System.Windows.Forms.Button();
            this.ButtonCE = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonDecimalPoint
            // 
            this.ButtonDecimalPoint.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ButtonDecimalPoint.Location = new System.Drawing.Point(155, 429);
            this.ButtonDecimalPoint.Name = "ButtonDecimalPoint";
            this.ButtonDecimalPoint.Size = new System.Drawing.Size(70, 74);
            this.ButtonDecimalPoint.TabIndex = 7;
            this.ButtonDecimalPoint.Text = ".";
            this.ButtonDecimalPoint.UseVisualStyleBackColor = true;
            this.ButtonDecimalPoint.Click += new System.EventHandler(this.Button_Click);
            // 
            // ButtonNegate
            // 
            this.ButtonNegate.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ButtonNegate.Location = new System.Drawing.Point(3, 429);
            this.ButtonNegate.Name = "ButtonNegate";
            this.ButtonNegate.Size = new System.Drawing.Size(70, 74);
            this.ButtonNegate.TabIndex = 8;
            this.ButtonNegate.Text = "±";
            this.ButtonNegate.UseVisualStyleBackColor = true;
            this.ButtonNegate.Click += new System.EventHandler(this.ButtonNegate_Click);
            // 
            // Button0
            // 
            this.Button0.AllowDrop = true;
            this.Button0.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Button0.Location = new System.Drawing.Point(79, 429);
            this.Button0.Name = "Button0";
            this.Button0.Size = new System.Drawing.Size(70, 74);
            this.Button0.TabIndex = 9;
            this.Button0.Tag = "Number";
            this.Button0.Text = "0";
            this.Button0.UseVisualStyleBackColor = true;
            this.Button0.Click += new System.EventHandler(this.Button_Click);
            // 
            // ButtonEqual
            // 
            this.ButtonEqual.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ButtonEqual.Location = new System.Drawing.Point(245, 429);
            this.ButtonEqual.Name = "ButtonEqual";
            this.ButtonEqual.Size = new System.Drawing.Size(70, 74);
            this.ButtonEqual.TabIndex = 10;
            this.ButtonEqual.Tag = "Operator";
            this.ButtonEqual.Text = "=";
            this.ButtonEqual.UseVisualStyleBackColor = true;
            this.ButtonEqual.Click += new System.EventHandler(this.Button_Equal_Click);
            // 
            // Button2
            // 
            this.Button2.AllowDrop = true;
            this.Button2.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Button2.Location = new System.Drawing.Point(79, 349);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(70, 74);
            this.Button2.TabIndex = 11;
            this.Button2.Tag = "Number";
            this.Button2.Text = "2";
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button_Click);
            // 
            // Button5
            // 
            this.Button5.AllowDrop = true;
            this.Button5.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Button5.Location = new System.Drawing.Point(79, 269);
            this.Button5.Name = "Button5";
            this.Button5.Size = new System.Drawing.Size(70, 74);
            this.Button5.TabIndex = 12;
            this.Button5.Tag = "Number";
            this.Button5.Text = "5";
            this.Button5.UseVisualStyleBackColor = true;
            this.Button5.Click += new System.EventHandler(this.Button_Click);
            // 
            // Button8
            // 
            this.Button8.AllowDrop = true;
            this.Button8.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Button8.Location = new System.Drawing.Point(79, 189);
            this.Button8.Name = "Button8";
            this.Button8.Size = new System.Drawing.Size(70, 74);
            this.Button8.TabIndex = 13;
            this.Button8.Tag = "Number";
            this.Button8.Text = "8";
            this.Button8.UseVisualStyleBackColor = true;
            this.Button8.Click += new System.EventHandler(this.Button_Click);
            // 
            // button45
            // 
            this.button45.AllowDrop = true;
            this.button45.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button45.Location = new System.Drawing.Point(79, 109);
            this.button45.Name = "button45";
            this.button45.Size = new System.Drawing.Size(70, 74);
            this.button45.TabIndex = 14;
            this.button45.Text = "0";
            this.button45.UseVisualStyleBackColor = true;
            // 
            // TextBoxPanel
            // 
            this.TextBoxPanel.Font = new System.Drawing.Font("新細明體", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TextBoxPanel.Location = new System.Drawing.Point(3, 37);
            this.TextBoxPanel.Multiline = true;
            this.TextBoxPanel.Name = "TextBoxPanel";
            this.TextBoxPanel.Size = new System.Drawing.Size(303, 66);
            this.TextBoxPanel.TabIndex = 15;
            this.TextBoxPanel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ButtonTest
            // 
            this.ButtonTest.Location = new System.Drawing.Point(3, 125);
            this.ButtonTest.Name = "ButtonTest";
            this.ButtonTest.Size = new System.Drawing.Size(75, 23);
            this.ButtonTest.TabIndex = 16;
            this.ButtonTest.Text = "Test";
            this.ButtonTest.UseVisualStyleBackColor = true;
            this.ButtonTest.Click += new System.EventHandler(this.ButtonTest_Click);
            // 
            // ButtonAdd
            // 
            this.ButtonAdd.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ButtonAdd.Location = new System.Drawing.Point(245, 349);
            this.ButtonAdd.Name = "ButtonAdd";
            this.ButtonAdd.Size = new System.Drawing.Size(70, 74);
            this.ButtonAdd.TabIndex = 17;
            this.ButtonAdd.Tag = "";
            this.ButtonAdd.Text = "+";
            this.ButtonAdd.UseVisualStyleBackColor = true;
            this.ButtonAdd.Click += new System.EventHandler(this.Button_Operate_Click);
            // 
            // TextBoxSubPanel
            // 
            this.TextBoxSubPanel.Font = new System.Drawing.Font("新細明體", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TextBoxSubPanel.Location = new System.Drawing.Point(3, 3);
            this.TextBoxSubPanel.Multiline = true;
            this.TextBoxSubPanel.Name = "TextBoxSubPanel";
            this.TextBoxSubPanel.Size = new System.Drawing.Size(303, 37);
            this.TextBoxSubPanel.TabIndex = 18;
            this.TextBoxSubPanel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ButtonMinus
            // 
            this.ButtonMinus.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ButtonMinus.Location = new System.Drawing.Point(245, 269);
            this.ButtonMinus.Name = "ButtonMinus";
            this.ButtonMinus.Size = new System.Drawing.Size(70, 74);
            this.ButtonMinus.TabIndex = 19;
            this.ButtonMinus.Tag = "";
            this.ButtonMinus.Text = "-";
            this.ButtonMinus.UseVisualStyleBackColor = true;
            this.ButtonMinus.Click += new System.EventHandler(this.Button_Operate_Click);
            // 
            // ButtonMultiply
            // 
            this.ButtonMultiply.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ButtonMultiply.Location = new System.Drawing.Point(245, 189);
            this.ButtonMultiply.Name = "ButtonMultiply";
            this.ButtonMultiply.Size = new System.Drawing.Size(70, 74);
            this.ButtonMultiply.TabIndex = 20;
            this.ButtonMultiply.Tag = "";
            this.ButtonMultiply.Text = "x";
            this.ButtonMultiply.UseVisualStyleBackColor = true;
            this.ButtonMultiply.Click += new System.EventHandler(this.Button_Operate_Click);
            // 
            // Button1
            // 
            this.Button1.AllowDrop = true;
            this.Button1.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Button1.Location = new System.Drawing.Point(3, 349);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(70, 74);
            this.Button1.TabIndex = 21;
            this.Button1.Tag = "Number";
            this.Button1.Text = "1";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button_Click);
            // 
            // Button3
            // 
            this.Button3.AllowDrop = true;
            this.Button3.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Button3.Location = new System.Drawing.Point(155, 349);
            this.Button3.Name = "Button3";
            this.Button3.Size = new System.Drawing.Size(70, 74);
            this.Button3.TabIndex = 22;
            this.Button3.Tag = "Number";
            this.Button3.Text = "3";
            this.Button3.UseVisualStyleBackColor = true;
            this.Button3.Click += new System.EventHandler(this.Button_Click);
            // 
            // Button4
            // 
            this.Button4.AllowDrop = true;
            this.Button4.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Button4.Location = new System.Drawing.Point(3, 269);
            this.Button4.Name = "Button4";
            this.Button4.Size = new System.Drawing.Size(70, 74);
            this.Button4.TabIndex = 23;
            this.Button4.Tag = "Number";
            this.Button4.Text = "4";
            this.Button4.UseVisualStyleBackColor = true;
            this.Button4.Click += new System.EventHandler(this.Button_Click);
            // 
            // Button6
            // 
            this.Button6.AllowDrop = true;
            this.Button6.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Button6.Location = new System.Drawing.Point(155, 269);
            this.Button6.Name = "Button6";
            this.Button6.Size = new System.Drawing.Size(70, 74);
            this.Button6.TabIndex = 24;
            this.Button6.Tag = "Number";
            this.Button6.Text = "6";
            this.Button6.UseVisualStyleBackColor = true;
            this.Button6.Click += new System.EventHandler(this.Button_Click);
            // 
            // Button7
            // 
            this.Button7.AllowDrop = true;
            this.Button7.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Button7.Location = new System.Drawing.Point(3, 189);
            this.Button7.Name = "Button7";
            this.Button7.Size = new System.Drawing.Size(70, 74);
            this.Button7.TabIndex = 25;
            this.Button7.Tag = "Number";
            this.Button7.Text = "7";
            this.Button7.UseVisualStyleBackColor = true;
            this.Button7.Click += new System.EventHandler(this.Button_Click);
            // 
            // Button9
            // 
            this.Button9.AllowDrop = true;
            this.Button9.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Button9.Location = new System.Drawing.Point(155, 189);
            this.Button9.Name = "Button9";
            this.Button9.Size = new System.Drawing.Size(70, 74);
            this.Button9.TabIndex = 26;
            this.Button9.Tag = "Number";
            this.Button9.Text = "9";
            this.Button9.UseVisualStyleBackColor = true;
            this.Button9.Click += new System.EventHandler(this.Button_Click);
            // 
            // ButtonC
            // 
            this.ButtonC.AllowDrop = true;
            this.ButtonC.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ButtonC.Location = new System.Drawing.Point(155, 109);
            this.ButtonC.Name = "ButtonC";
            this.ButtonC.Size = new System.Drawing.Size(70, 74);
            this.ButtonC.TabIndex = 27;
            this.ButtonC.Text = "C";
            this.ButtonC.UseVisualStyleBackColor = true;
            this.ButtonC.Click += new System.EventHandler(this.ButtonC_Click);
            // 
            // ButtonCE
            // 
            this.ButtonCE.AllowDrop = true;
            this.ButtonCE.Font = new System.Drawing.Font("新細明體", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ButtonCE.Location = new System.Drawing.Point(245, 109);
            this.ButtonCE.Name = "ButtonCE";
            this.ButtonCE.Size = new System.Drawing.Size(70, 74);
            this.ButtonCE.TabIndex = 28;
            this.ButtonCE.Text = "CE";
            this.ButtonCE.UseVisualStyleBackColor = true;
            this.ButtonCE.Click += new System.EventHandler(this.ButtonCE_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 505);
            this.Controls.Add(this.ButtonCE);
            this.Controls.Add(this.ButtonC);
            this.Controls.Add(this.Button9);
            this.Controls.Add(this.Button7);
            this.Controls.Add(this.Button6);
            this.Controls.Add(this.Button4);
            this.Controls.Add(this.Button3);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.ButtonMultiply);
            this.Controls.Add(this.ButtonMinus);
            this.Controls.Add(this.TextBoxSubPanel);
            this.Controls.Add(this.ButtonAdd);
            this.Controls.Add(this.ButtonTest);
            this.Controls.Add(this.TextBoxPanel);
            this.Controls.Add(this.button45);
            this.Controls.Add(this.Button8);
            this.Controls.Add(this.Button5);
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.ButtonEqual);
            this.Controls.Add(this.Button0);
            this.Controls.Add(this.ButtonNegate);
            this.Controls.Add(this.ButtonDecimalPoint);
            this.Name = "Form1";
            this.Text = "小算盤";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonDecimalPoint;
        private System.Windows.Forms.Button ButtonNegate;
        private System.Windows.Forms.Button Button0;
        private System.Windows.Forms.Button ButtonEqual;
        private System.Windows.Forms.Button Button2;
        private System.Windows.Forms.Button Button5;
        private System.Windows.Forms.Button Button8;
        private System.Windows.Forms.Button button45;
        private System.Windows.Forms.TextBox TextBoxPanel;
        private System.Windows.Forms.Button ButtonTest;
        private System.Windows.Forms.Button ButtonAdd;
        private System.Windows.Forms.TextBox TextBoxSubPanel;
        private System.Windows.Forms.Button ButtonMinus;
        private System.Windows.Forms.Button ButtonMultiply;
        private System.Windows.Forms.Button Button1;
        private System.Windows.Forms.Button Button3;
        private System.Windows.Forms.Button Button4;
        private System.Windows.Forms.Button Button6;
        private System.Windows.Forms.Button Button7;
        private System.Windows.Forms.Button Button9;
        private System.Windows.Forms.Button ButtonC;
        private System.Windows.Forms.Button ButtonCE;
    }
}

