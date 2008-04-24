using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;


namespace littleRunner
{
    enum SyntaxCategory
    {
        General,
        Syntax,
        MainKeyword,
        ImporantVariableName,
        Number,
        Constant,
        Keyword,
        String,
        Comment
    }

    class CategoryInfo
    {
        public Color color;
        public Font font;

        public CategoryInfo(Color color, Font font)
        {
            this.color = color;
            this.font = font;
        }
    }

    class Syntax
    {
        public SyntaxCategory category;
        public Regex regex;

        public Syntax(SyntaxCategory category, string regex)
        {
            this.category = category;
            this.regex = new Regex(regex, RegexOptions.Compiled);
        }
    }


    class SimpleEditor : RichTextBox
    {
        public List<Syntax> Highl;
        public Dictionary<SyntaxCategory, CategoryInfo> Categories;
        bool canPaint = true;
        bool ignoreTextChange = false;


        public SimpleEditor()
        {
            Highl = new List<Syntax>();
            Categories = new Dictionary<SyntaxCategory, CategoryInfo>();
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x00f)
            {
                if (canPaint)
                    base.WndProc(ref m);
                else
                    m.Result = IntPtr.Zero;
            }
            else
                base.WndProc(ref m);
        }


        private void HighlightLine(string line, int startLine)
        {
            int oldSelect = SelectionStart;
            ignoreTextChange = true;

            foreach (Syntax text in Highl)
            {
                if (line == "class Test(foo):")
                {
                }
                foreach (Match match in text.regex.Matches(line))
                {
                    int start;
                    int len;

                    if (match.Groups["h"].Success)
                    {
                        start = match.Groups["h"].Index;
                        len = match.Groups["h"].Length;
                    }
                    else
                    {
                        start = match.Index;
                        len = match.Length;
                    }

                    SelectionStart = startLine + start;
                    SelectionLength = len;
                    SelectionColor = Categories[text.category].color;
                    SelectionFont = Categories[text.category].font;
                }
            }


            SelectionStart = oldSelect;
            SelectionLength = 0;
            SelectionColor = Categories[SyntaxCategory.General].color;
            SelectionFont = Categories[SyntaxCategory.General].font;

            ignoreTextChange = false;
        }
        private void ResetLine(int start, int end)
        {
            int oldSelStart = SelectionStart;
            int oldSelLen = SelectionLength;

            SelectionStart = start;
            SelectionLength = end - start;
            SelectionColor = Categories[SyntaxCategory.General].color;
            SelectionFont = Categories[SyntaxCategory.General].font;

            SelectionStart = oldSelStart;
            SelectionLength = oldSelLen;
        }


        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (!ignoreTextChange)
            {
                int startLine = SelectionStart;
                while (startLine > 0 && Text[startLine - 1] != '\n')
                    startLine--;

                int endLine = SelectionStart;
                while (endLine < Text.Length && Text[endLine] != '\n')
                    endLine++;

                string line = Text.Substring(startLine, endLine - startLine);

                canPaint = false;
                ResetLine(startLine, endLine);
                HighlightLine(line, startLine);
                canPaint = true;
            }
        }


        public void HighlightAll()
        {
            int cur = 0;
            canPaint = false;
            ignoreTextChange = true;

            for (int i = 0; i < Lines.Length; i++)
            {
                HighlightLine(Lines[i], cur);
                cur += Lines[i].Length + 1;
            }

            ignoreTextChange = false;
            canPaint = true;
        }

    }
}