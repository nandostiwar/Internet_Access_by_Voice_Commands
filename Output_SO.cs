using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoItX3Lib;

namespace _7_Tesis_Maestria
{
    class Output_SO
    {
        AutoItX3 AutoIt;

        public Output_SO()
        {
            AutoIt = new AutoItX3();
        }

        public void Comando(int dato)
        {
            switch (dato)
            {
                case 1: AutoIt.MouseClick("LEFT"); break;

                case 2: AutoIt.Send("{PGUP}"); break;
                case 3: AutoIt.Send("{PGDN}"); break;

                case 4: AutoIt.Send("!+{F4}", 0); break;
                case 5: AutoIt.Send("{CTRLDOWN}{+}{CTRLUP}"); break;
                case 6: AutoIt.Send("{CTRLDOWN}{-}{CTRLUP}"); break;
                case 7: AutoIt.Send("{BROWSER_BACK}"); break;

                case 8: AutoIt.Send("{ALTDOWN}1{ALTUP}"); break;
                case 9: AutoIt.Send("{ALTDOWN}5{ALTUP}"); break;
                case 10: AutoIt.Send("{ALTDOWN}2{ALTUP}"); break;
                case 11: AutoIt.Send("{ALTDOWN}4{ALTUP}"); break;

                case 12: AutoIt.Send("{F6}google.com{ENTER}"); break;                                
                case 13: AutoIt.Send("{CTRLDOWN}j{CTRLUP}"); break;
                case 14: AutoIt.Send("{CTRLDOWN}h{CTRLUP}"); break;
                case 15: AutoIt.Send("{CTRLDOWN}p{CTRLUP}"); break;

                case 16: AutoIt.Send("g"); AutoIt.Send("t"); break;
                case 17: AutoIt.Send("g"); AutoIt.Send("i"); break;
                case 18: AutoIt.Send("c"); break;
                case 19: AutoIt.Send("g"); AutoIt.Send("d"); break;

                case 20: AutoIt.Send("{F5}"); break;
                case 21: AutoIt.Send("{ENTER}"); break;
                case 22: AutoIt.Send("{CTRLDOWN}z{CTRLUP}"); break;
                case 23: AutoIt.Send("{ESC}"); break;
            }
        }
    }
}
