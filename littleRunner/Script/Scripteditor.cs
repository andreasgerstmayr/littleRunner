using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace littleRunner
{
    public partial class Scripteditor : Form
    {
        public Scripteditor()
        {
            InitializeComponent();

            script.Categories.Add(SyntaxCategory.General,
                new CategoryInfo(script.ForeColor, script.Font)
            );
            script.Categories.Add(SyntaxCategory.MainKeyword,
                new CategoryInfo(Color.SteelBlue, new Font(script.Font, FontStyle.Bold))
            );
            script.Categories.Add(SyntaxCategory.ImporantVariableName,
                new CategoryInfo(Color.DarkCyan, new Font(script.Font, FontStyle.Bold))
            );
            script.Categories.Add(SyntaxCategory.Number,
                new CategoryInfo(Color.DarkSlateBlue, new Font(script.Font, FontStyle.Bold))
            );
            script.Categories.Add(SyntaxCategory.Constant,
                new CategoryInfo(Color.DarkGreen, new Font(script.Font, FontStyle.Bold))
            );
            script.Categories.Add(SyntaxCategory.Keyword,
                new CategoryInfo(Color.DarkGreen, new Font(script.Font, FontStyle.Regular))
            );
            script.Categories.Add(SyntaxCategory.String,
                new CategoryInfo(Color.Orange, new Font(script.Font, FontStyle.Regular))
            );
            script.Categories.Add(SyntaxCategory.Comment,
                new CategoryInfo(Color.Green, new Font(script.Font, FontStyle.Regular))
            );


            script.Highl.Add(new Syntax(SyntaxCategory.MainKeyword, @"(import|from|class|def|if|elif|else)"));
            script.Highl.Add(new Syntax(SyntaxCategory.ImporantVariableName, @"class (?<h>\w+)((\(\w+\))|):"));
            script.Highl.Add(new Syntax(SyntaxCategory.ImporantVariableName, @"class (\w+)((\((?<h>\w+)\))|):"));
            script.Highl.Add(new Syntax(SyntaxCategory.ImporantVariableName, @"from (?<h>\w+) "));
            script.Highl.Add(new Syntax(SyntaxCategory.Number, @"(\W)(?<h>\d+)(\W)"));
            script.Highl.Add(new Syntax(SyntaxCategory.Constant, @"(True|False)"));
            script.Highl.Add(new Syntax(SyntaxCategory.Keyword, @"(self|pass|return)"));
            script.Highl.Add(new Syntax(SyntaxCategory.String, @""".*?"""));
            script.Highl.Add(new Syntax(SyntaxCategory.String, @"'.*?'"));
            script.Highl.Add(new Syntax(SyntaxCategory.Comment, @"#(.*)$"));
        }

        public string ScriptText
        {
            get { return script.Text; }
            set { script.Text = value; }
        }

        private void Scripteditor_Shown(object sender, EventArgs e)
        {
            script.HighlightAll();
            script.Focus();
        }
    }
}
