using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Speech.Synthesis;

namespace _7_Tesis_Maestria
{
    class Output_SintesisVoz
    {
        internal static void Hablar(int dato)
        {
            if (dato > 0)
            {
                PromptBuilder pBuilder = new PromptBuilder();
                SpeechSynthesizer sSynth = new SpeechSynthesizer();

                pBuilder.ClearContent();

                switch (dato)
                {
                    case 1: pBuilder.AppendText("Abriendo Facebook"); break;
                    case 2: pBuilder.AppendText("Abriendo Gmail"); break;
                    case 3: pBuilder.AppendText("Abriendo Google"); break;
                    case 4: pBuilder.AppendText("Hasta Pronto"); break;

                    case 5: pBuilder.AppendText("Volviendo al menu"); break;
                    case 6: pBuilder.AppendText("Actualizando Página"); break;
                    case 7: pBuilder.AppendText("Acercando"); break;
                    case 8: pBuilder.AppendText("Alejando"); break;
                    case 9: pBuilder.AppendText("Volviendo atrás"); break;
                    case 10: pBuilder.AppendText("Desactivando Click"); break;
                    case 11: pBuilder.AppendText("Encendiendo Click"); break;
                    case 12: pBuilder.AppendText("Que deseas dictar"); break;
                    case 13: pBuilder.AppendText("Presionando Enter"); break;
                    case 14: pBuilder.AppendText("Deshaciendo"); break;

                    case 15: pBuilder.AppendText("Abriendo Muro"); break;
                    case 16: pBuilder.AppendText("Abriendo Notificaciones"); break;
                    case 17: pBuilder.AppendText("Abriendo Perfil"); break;
                    case 18: pBuilder.AppendText("Abriendo Mensajes"); break;

                    case 19: pBuilder.AppendText("Nueva búsqueda"); break;
                    case 20: pBuilder.AppendText("Estas son las descargas"); break;
                    case 21: pBuilder.AppendText("Este es el historial"); break;
                    case 22: pBuilder.AppendText("Configurando Impresión"); break;

                    case 23: pBuilder.AppendText("Abriendo mensajes enviados"); break;
                    case 24: pBuilder.AppendText("Abriendo bandeja de entrada"); break;
                    case 25: pBuilder.AppendText("Creando un nuevo correo"); break;
                    case 26: pBuilder.AppendText("Abriendo Borradores"); break;
                    case 27: pBuilder.AppendText("Presionando Escape"); break;
                    case 28: pBuilder.AppendText("Calibrando"); break;
                }

                sSynth.Speak(pBuilder);
            }
        }
    }
}
