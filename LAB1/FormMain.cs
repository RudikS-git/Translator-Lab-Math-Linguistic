using System;
using System.Linq;
using System.Windows.Forms;
using LAB1.Exceptions;
using LAB1.SA;

namespace LAB1
{
    public partial class FormMain : Form
    {
        private LexicalAnalyzer lexAn;
        
        public FormMain()
        {
            InitializeComponent();
        }

        private LexicalAnalyzer GetMethod(string [] inputLines)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    return new GotoMethod(inputLines);
                case 1:
                    return new SwitchMethod(inputLines);
                case 2:
                    return new TableMethod(inputLines);
            }

            throw new Exception("Не выбран метод анализа");
        }
        
        private void buttonAnalyze_Click(object sender, EventArgs e)
        {
            // Очищаем поле сообщений и таблицу распознанных токенов.
            richTextBoxMessages.Clear();
            dataGridViewRecognizedTokens.Rows.Clear();
            treeViewSyntaxTree.Nodes.Clear(); // Очищаем дерево.

            try
            {
                lexAn = GetMethod(richTextBoxInput.Lines);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }

            try
            {
                if (comboBoxTypeAnalyzer.SelectedIndex == 1)
                {
                    SyntaxAnalyzer syntaxAnalyzer = new SyntaxAnalyzer(lexAn);
                    syntaxAnalyzer.ParseText(out SyntaxTreeNode treeRoot);
                    richTextBoxMessages.AppendText("Текст правильный");
                    VisualizeSyntaxTree(treeRoot); // Визуализируем синтаксическое дерево в компоненте treeViewSyntaxTree.
                }
                else
                {
                    int k = 0; // Инициализируем счетчик распознанных токенов.
                    
                    // Цикл чтения текста от начала до конца.
                    do
                    {
                        lexAn.RecognizeNextToken(); // Распознаем очередной токен в тексте.

                        k++;
                        dataGridViewRecognizedTokens.Rows.Add(k, lexAn.Token.Value, lexAn.Token.Type,
                            lexAn.Token.LineIndex + 1,
                            lexAn.Token.SymStartIndex + 1); // Добавляем распознанный токен в таблицу.
                    } 
                    while (lexAn.Token.Type != TokenKind.EndOfText); // Цикл работает до тех пор, пока не будет возвращен токен "Конец текста".

                    richTextBoxMessages.AppendText("Текст правильный");
                }
            }
            catch (AnalyzerException analyzerException)
            {
                richTextBoxMessages.AppendText(analyzerException.ToString()); // Добавляем описание ошибки в поле сообщений.
                LocateCursorAtErrorPosition(analyzerException.LineIndex, analyzerException.SymIndex); // Располагаем курсор в исходном тексте на позиции ошибки.
            }
        }

        /// <summary>
        /// Располагает курсор в исходном тексте на позиции ошибки
        /// </summary>
        /// <param name="lineIndex"></param>
        /// <param name="symIndex"></param>
        private void LocateCursorAtErrorPosition(int lineIndex, int symIndex)
        {
            int k = 0;

            // Подсчитываем суммарное количество символов во всех строках до lineIndex.
            for (int i = 0; i < lineIndex; i++)
            {
                k += richTextBoxInput.Lines[i].Count() + 1;
            }

            // Прибавляем символы из строки lineIndex.
            k += symIndex;

            // Располагаем курсор на вычисленной позиции.
            richTextBoxInput.Select();
            richTextBoxInput.Select(k, 1);
        }

        private void comboBoxTypeAnalyzer_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedIndex == 0)
            {
                dataGridViewRecognizedTokens.Visible = true;
                label5.Visible = true;

                label8.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                label11.Visible = false;
                label12.Visible = false;
                label13.Visible = false;
                treeViewSyntaxTree.Visible = false;

                richTextBoxInput.Text = @"abcd <!--abcd 101--> 101 011101110110 dbcdcd
<!----ff
dadaadad--> ";
            }
            else
            {
                dataGridViewRecognizedTokens.Visible = false;
                label5.Visible = false;

                label8.Visible = true;
                label9.Visible = true;
                label10.Visible = true;
                label11.Visible = true;
                label12.Visible = true;
                label13.Visible = true;

                treeViewSyntaxTree.Visible = true;

                richTextBoxInput.Text = @"<!-- 
пример html комментария 
-->      
[[[[[[[[[  <!-- пример html 
комментария -->      
[    
<!-- пример html комментария -->                              
    dbcddcddcdcddcddddcccccd
    [
         011011011011101
<!-- пример html комментария -->      
         101
         011101110
         101110
         011101       
    ] 
]
]]]]]]]]]
<!-- пример html комментария -->      ";
            }
        }

        // Визуализировать синтаксическое дерево в компоненте treeViewSyntaxTree.
        private void VisualizeSyntaxTree(SyntaxTreeNode treeRoot)
        {
            treeViewSyntaxTree.BeginUpdate();

            treeViewSyntaxTree.Nodes.Add(treeRoot.Name); // Создаем в компоненте корневой узел.

            RecurAddSubNodes(treeViewSyntaxTree.Nodes[0], treeRoot); // Рекурсивно добавляем подчиненные узлы.

            treeViewSyntaxTree.ExpandAll(); // Раскрываем в компоненте все узлы дерева.

            treeViewSyntaxTree.TopNode = treeViewSyntaxTree.Nodes[0]; // Делаем видимым корневой узел.

            treeViewSyntaxTree.EndUpdate();
        }

        // Рекурсивно добавить подчиненные узлы.
        private void RecurAddSubNodes(TreeNode subTreeRoot1, SyntaxTreeNode subTreeRoot2)
        {
            foreach (SyntaxTreeNode item in subTreeRoot2.SubNodes) // Цикл по всем подчиненным узлам.
            {
                TreeNode n;
                n = subTreeRoot1.Nodes.Add(item.Name); // Добавляем очередной подчиненный узел.
                RecurAddSubNodes(n, item); // Рекурсивно добавляем также и его подчиненные узлы.
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void richTextBoxInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }
    }
}
