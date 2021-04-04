using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAB1.SA;

namespace LAB1.Output
{
    public class BinaryToDecimalTranslation
    {
        private string text;
        private int index;

        public BinaryToDecimalTranslation(string text)
        {
            this.text = text;
            index = 0;
        }

        private void NextNum()
        {
            index++;
        }
        
        private char GetCurrentNum()
        {
            if (text.Length == index)
                return ' ';
            
            return text[index];
        }
        
        public void Start(ref int d, ref int n)
        {
            L(ref d, ref n);
        }

        private void L(ref int d, ref int n)
        {
            if (GetCurrentNum() == '0')
            {
                LFin(ref d, ref n);

            }
            else if(GetCurrentNum() == '1')
            {
                LFin(ref d, ref n);
            }
        }

        private void LFin(ref int d, ref int n)
        {
            if (GetCurrentNum() == '0')
            {
                NextNum();
                
                if (GetCurrentNum() == ' ')
                {
                    d = 0;
                    n = 1;
                }
                else
                {
                    LFin(ref d, ref n);
                    n++;
                }
            }
            else if(GetCurrentNum() == '1')
            {
                NextNum();
                
                if (GetCurrentNum() == ' ')
                {
                    d = 1;
                    n = 1;
                }
                else
                {
                    LFin(ref d, ref n);
                    d = (int)Math.Pow(2, n) + d;
                    n++;
                }
            }

            NextNum();
        }
    }
}
