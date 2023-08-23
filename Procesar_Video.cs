using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Video_Control_Navegacion
{
    class Procesar_Video
    {
        internal static int[] Filtro(Rectangle Monitor, int x, int x_old, int y, int y_old, int Modo, int Contador, int MarcoAnterior, int ComandoSO, int ComandoGUI, int ComandoSintesisVoz, int Cambio, int Click)
        {
            int[] SalidaProceso = new int[9];
            int MarcoActual = 0;
            int Velocidad = 10;
            int MarcoSiguiente = MarcoAnterior;
            
            //------- Filtro Marcos -------            
            if (Modo == 1)
            {
                if (y <= Monitor.Height / 20) { MarcoActual = 1; }
                if (x >= 0 && x < Monitor.Width / 30 && y >= Monitor.Height / 20 && y < Monitor.Height * 19 / 20) { MarcoActual = 2; }
                if (x >= Monitor.Width / 30 && x < Monitor.Width * 29 / 30 && y >= Monitor.Height / 20 && y < Monitor.Height * 19 / 20) { MarcoActual = 3; }
                if (x >= Monitor.Width * 29 / 30 && x < Monitor.Width && y >= Monitor.Height / 20 && y < Monitor.Height * 19 / 20) { MarcoActual = 4; }
                if (y >= Monitor.Height * 19 / 20) { MarcoActual = 5; }
            }

            if (Modo == 2)
            {
                if (x >= 0 && x < Monitor.Height / 4 && y >= 0 && y < Monitor.Height / 4) { MarcoActual = 6; }
                if (x >= 0 && x < Monitor.Height / 4 && y >= Monitor.Height / 4 && y < Monitor.Height / 2) { MarcoActual = 7; }
                if (x >= 0 && x < Monitor.Height / 4 && y >= Monitor.Height / 2 && y < Monitor.Height * 3 / 4) { MarcoActual = 8; }
                if (x >= 0 && x < Monitor.Height / 4 && y >= Monitor.Height * 3 / 4 && y < Monitor.Height) { MarcoActual = 9; }
                if (x >= Monitor.Height / 4 && x < Monitor.Width - (Monitor.Height / 4) && y >= 0 && y < Monitor.Height / 20) { MarcoActual = 10; }
                if (x >= Monitor.Height / 4 && x < Monitor.Width - (Monitor.Height / 4) && y >= Monitor.Height / 20 && y < Monitor.Height * 19 / 20) { MarcoActual = 11; }
                if (x >= Monitor.Height / 4 && x < Monitor.Width - (Monitor.Height / 4) && y >= Monitor.Height * 19 / 20 && y < Monitor.Height) { MarcoActual = 12; }
                if (x >= Monitor.Width - (Monitor.Height / 4) && x < Monitor.Width && y >= 0 && y < Monitor.Height / 4) { MarcoActual = 13; }
                if (x >= Monitor.Width - (Monitor.Height / 4) && x < Monitor.Width && y >= Monitor.Height / 4 && y < Monitor.Height / 2) { MarcoActual = 14; }
                if (x >= Monitor.Width - (Monitor.Height / 4) && x < Monitor.Width && y >= Monitor.Height / 2 && y < Monitor.Height * 3 / 4) { MarcoActual = 15; }
                if (x >= Monitor.Width - (Monitor.Height / 4) && x < Monitor.Width && y >= Monitor.Height * 3 / 4 && y < Monitor.Height) { MarcoActual = 16; }
            }

            //------- Maquina de estados -------

            Contador++;

            //------ Analiza Marcos ------
            //----------------------------
            
            if (MarcoActual == 1 && (MarcoAnterior == 2 || MarcoAnterior == 3 || MarcoAnterior == 4 || MarcoAnterior == 13))
            { Contador = 0; MarcoSiguiente = 1; ComandoSO = 0; ComandoGUI = 1; ComandoSintesisVoz = 0; Cambio = 0; }

            if (MarcoActual == 1 && MarcoAnterior == 1)
            {   
                if(Contador == Velocidad / 3)
                { Contador = 0; MarcoSiguiente = 1; ComandoSO = 1; ComandoGUI = 1; ComandoSintesisVoz = 0; }
                else { ComandoSO = 0; }
            }

            //----------------------------
            
            if (MarcoActual == 2 && (MarcoAnterior == 1 || MarcoAnterior == 3 || MarcoAnterior == 5))
            { Contador = 0; MarcoSiguiente = 2; ComandoSO = 0; ComandoGUI = 2; ComandoSintesisVoz = 0; }

            if (MarcoActual == 2 && MarcoAnterior == 2)
            {
                ComandoSO = 0; ComandoGUI = 2; Modo = 1; Cambio = 0; ComandoSintesisVoz = 0;
                if (Contador == Velocidad * 3)
                {
                    Contador = 0;
                    if (Click == 0) { Click = 1; ComandoSintesisVoz = 8; }
                    else if (Click == 1) { Click = 0; ComandoSintesisVoz = 9; }                    
                }
            }

            //-----------------------------

            if (MarcoActual == 3 && (MarcoAnterior == 1 || MarcoAnterior == 2 || MarcoAnterior == 4 || MarcoAnterior == 5 || MarcoAnterior == 13))
            { Contador = 0; MarcoSiguiente = 3; ComandoSO = 0; ComandoGUI = 3; ComandoSintesisVoz = 0; Cambio = 0; }

            if (MarcoActual == 3 && MarcoAnterior == 3)                    
            {
                ComandoSO = 0; MarcoSiguiente = 3; ComandoGUI = 3; ComandoSintesisVoz = 0;

                if (Contador == Velocidad * 3 && Click == 0) { Contador = 0; }
                if (Click >= 1)
                {
                    if ((Math.Abs(x - x_old) > 10) || (Math.Abs(y - y_old) > 10)) { Contador = 0; }
                    switch (Contador)
                    {
                        case 0: Click = 1; break;
                        case 3: Click = 2; break;
                        case 6: Click = 3; break;
                        case 9: Click = 4; break;
                        case 12: Click = 5; break;
                        case 15: Click = 6; break;
                        case 18: Click = 7; break;
                        case 21: Click = 8; break;
                        case 24: Click = 9; break;
                        case 27: Click = 10; break;
                        case 30: Click = 11; break;
                        case 33: Click = 12; break;
                        case 36: Click = 13; break;
                        case 40: Click = 0; ComandoSO = 10; Contador = 0; break;
                    }
                }                    
            }

            //----------------------------

            if (MarcoActual == 4 && (MarcoAnterior == 1 || MarcoAnterior == 3 || MarcoAnterior == 5))
            { Contador = 0; MarcoSiguiente = 4; ComandoSO = 0; ComandoGUI = 4; ComandoSintesisVoz = 0; }

            if (MarcoActual == 4 && MarcoAnterior == 4)
            {
                if (Cambio == 1) { ComandoGUI = 3; Contador = 0; ComandoSintesisVoz = 0; }
                else if (Contador == Velocidad * 3)
                { 
                    Contador = 0; ComandoSO = 0; ComandoGUI = 11; ComandoSintesisVoz = 2; Modo = 2; Cambio = 1;
                    if (x >= Monitor.Width - (Monitor.Height / 4) && x < Monitor.Width && y >= 0 && y < Monitor.Height / 4) { MarcoSiguiente = 13; }
                    if (x >= Monitor.Width - (Monitor.Height / 4) && x < Monitor.Width && y >= Monitor.Height / 4 && y < Monitor.Height / 2) { MarcoSiguiente = 14; }
                    if (x >= Monitor.Width - (Monitor.Height / 4) && x < Monitor.Width && y >= Monitor.Height / 2 && y < Monitor.Height * 3 / 4) { MarcoSiguiente = 15; }
                    if (x >= Monitor.Width - (Monitor.Height / 4) && x < Monitor.Width && y >= Monitor.Height * 3 / 4 && y < Monitor.Height) { MarcoSiguiente = 16; }
                }
            }

            //----------------------------

            if (MarcoActual == 5 && (MarcoAnterior == 2 || MarcoAnterior == 3 || MarcoAnterior == 4 || MarcoAnterior == 13))
            { Contador = 0; MarcoSiguiente = 5; ComandoSO = 0; ComandoGUI = 5; ComandoSintesisVoz = 0; Cambio = 0; }

            if (MarcoActual == 5 && MarcoAnterior == 5)
            {
                if (Contador == Velocidad / 3)
                { Contador = 0; MarcoSiguiente = 5; ComandoSO = 2; ComandoGUI = 5; ComandoSintesisVoz = 0; }
                else { ComandoSO = 0; }
            }

            //----------------------------

            if (MarcoActual == 6 && (MarcoAnterior == 7 || MarcoAnterior == 10 || MarcoAnterior == 11))
            { Contador = 0; MarcoSiguiente = 6; ComandoSO = 0; ComandoGUI = 6; ComandoSintesisVoz = 0; }

            if (MarcoActual == 6 && MarcoAnterior == 6)
            {
                if (Contador == Velocidad * 3)
                { Contador = 39; MarcoSiguiente = 6; ComandoSO = 6; ComandoGUI = 11; ComandoSintesisVoz = 3; }
                else if (Contador == 40) { Contador = 39; ComandoSintesisVoz = 0; ComandoSO = 0; }
            }

            //----------------------------

            if (MarcoActual == 7 && (MarcoAnterior == 6 || MarcoAnterior == 8 || MarcoAnterior == 10 || MarcoAnterior == 11))
            { Contador = 0; MarcoSiguiente = 7; ComandoSO = 0; ComandoGUI = 7; ComandoSintesisVoz = 0; }

            if (MarcoActual == 7 && MarcoAnterior == 7)
            {
                if (Contador == Velocidad * 3)
                { Contador = 39; MarcoSiguiente = 7; ComandoSO = 7; ComandoGUI = 11; ComandoSintesisVoz = 4; }
                else if (Contador == 40) { Contador = 39; ComandoSintesisVoz = 0; ComandoSO = 0; }
            }

            //----------------------------

            if (MarcoActual == 8 && (MarcoAnterior == 7 || MarcoAnterior == 9 || MarcoAnterior == 11 || MarcoAnterior == 12))
            { Contador = 0; MarcoSiguiente = 8; ComandoSO = 0; ComandoGUI = 8; ComandoSintesisVoz = 0; }

            if (MarcoActual == 8 && MarcoAnterior == 8)
            {
                if (Contador == Velocidad * 3)
                { Contador = 39; MarcoSiguiente = 8; ComandoSO = 8; ComandoGUI = 11; ComandoSintesisVoz = 5; }
                else if (Contador == 40) { Contador = 39; ComandoSintesisVoz = 0; ComandoSO = 0; }
            }

            //----------------------------

            if (MarcoActual == 9 && (MarcoAnterior == 8 || MarcoAnterior == 11 || MarcoAnterior == 12))
            { Contador = 0; MarcoSiguiente = 9; ComandoSO = 0; ComandoGUI = 9; ComandoSintesisVoz = 0; }

            if (MarcoActual == 9 && MarcoAnterior == 9)
            {
                if (Contador == Velocidad * 3)
                { Contador = 39; MarcoSiguiente = 9; ComandoSO = 9; ComandoGUI = 11; ComandoSintesisVoz = 6; }
                else if (Contador == 40) { Contador = 39; ComandoSintesisVoz = 0; ComandoSO = 0; }
            }

            //----------------------------
            
            if (MarcoActual == 10 && (MarcoAnterior == 6 || MarcoAnterior == 11 || MarcoAnterior == 13))
            { Contador = 0; MarcoSiguiente = 10; ComandoSO = 0; ComandoGUI = 10; ComandoSintesisVoz = 0; Cambio = 0; }

            if (MarcoActual == 10 && MarcoAnterior == 10)
            {
                if (Contador == Velocidad / 3)
                { Contador = 0; MarcoSiguiente = 10; ComandoSO = 1; ComandoGUI = 10; ComandoSintesisVoz = 0; }
                else { ComandoSO = 0; }
            }

            //----------------------------

            if (MarcoActual == 11 && ((MarcoAnterior >= 6 && MarcoAnterior <= 10) || (MarcoAnterior >= 12 && MarcoAnterior <= 16)))
            { Contador = 0; MarcoSiguiente = 11; ComandoSO = 0; ComandoGUI = 11; ComandoSintesisVoz = 0; Cambio = 0; }

            if (MarcoActual == 11 && MarcoAnterior == 11)
            {
                ComandoSO = 0; MarcoSiguiente = 11; ComandoGUI = 11; ComandoSintesisVoz = 0;

                if (Contador == Velocidad * 3 && Click == 0) { Contador = 0; }
                if (Click >= 1)
                {
                    if ((Math.Abs(x - x_old) > 10) || (Math.Abs(y - y_old) > 10)) { Contador = 0; }
                    switch (Contador)
                    {
                        case 0: Click = 1; break;
                        case 6: Click = 2; break;
                        case 12: Click = 3; break;
                        case 18: Click = 4; break;
                        case 24: Click = 5; break;
                        case 30: Click = 6; break;
                        case 36: Click = 7; break;
                        case 42: Click = 8; break;
                        case 48: Click = 9; break;
                        case 54: Click = 10; break;
                        case 60: Click = 11; break;
                        case 66: Click = 12; break;
                        case 72: Click = 13; break;
                        case 78: Click = 0; ComandoSO = 10; Contador = 0; break;
                    }
                }
            }

            //----------------------------

            if (MarcoActual == 12 && (MarcoAnterior == 9 || MarcoAnterior == 11 || MarcoAnterior == 16))
            { Contador = 0; MarcoSiguiente = 12; ComandoSO = 0; ComandoGUI = 12; ComandoSintesisVoz = 0; Cambio = 0; }

            if (MarcoActual == 12 && MarcoAnterior == 12)
            {
                if (Contador == Velocidad / 3)
                { Contador = 0; MarcoSiguiente = 12; ComandoSO = 2; ComandoGUI = 12; ComandoSintesisVoz = 0; }
                else { ComandoSO = 0; }
            }

            //----------------------------

            if (MarcoActual == 13 && (MarcoAnterior == 10 || MarcoAnterior == 11 || MarcoAnterior == 14))
            { Contador = 0; MarcoSiguiente = 13; ComandoSO = 0; ComandoGUI = 13; ComandoSintesisVoz = 0; Cambio = 0; }

            if (MarcoActual == 13 && MarcoAnterior == 13)
            {
                if (Cambio == 1) { ComandoGUI = 11; Contador = 0; ComandoSintesisVoz = 0; }
                else
                {
                    if (Contador == Velocidad * 3)
                    { Contador = 0; MarcoSiguiente = 4; ComandoSO = 0; ComandoGUI = 3; ComandoSintesisVoz = 1; Modo = 1; Cambio = 1; }
                }
            }

            //----------------------------

            if (MarcoActual == 14 && (MarcoAnterior == 11 || MarcoAnterior == 13 || MarcoAnterior == 15))
            { Contador = 0; MarcoSiguiente = 14; ComandoSO = 0; ComandoGUI = 14; ComandoSintesisVoz = 0; Cambio = 0; }

            if (MarcoActual == 14 && MarcoAnterior == 14)
            {
                if (Cambio == 1) { ComandoGUI = 11; Contador = 0; ComandoSintesisVoz = 0; }
                else
                {
                    if (Contador == Velocidad)
                    { Contador = 0; MarcoSiguiente = 14; ComandoSO = 3; ComandoGUI = 14; ComandoSintesisVoz = 0; }
                    else { ComandoSO = 0; }
                }
            }

            //----------------------------

            if (MarcoActual == 15 && (MarcoAnterior == 11 || MarcoAnterior == 14 || MarcoAnterior == 16))
            { Contador = 0; MarcoSiguiente = 15; ComandoSO = 0; ComandoGUI = 15; ComandoSintesisVoz = 0; Cambio = 0; }


            if (MarcoActual == 15 && MarcoAnterior == 15)
            {
                if (Cambio == 1) { ComandoGUI = 11; Contador = 0; ComandoSintesisVoz = 0; }
                else
                {
                    if (Contador == Velocidad)
                    { Contador = 0; MarcoSiguiente = 15; ComandoSO = 4; ComandoGUI = 15; ComandoSintesisVoz = 0; }
                    else { ComandoSO = 0; }
                }
            }

            //----------------------------

            if (MarcoActual == 16 && (MarcoAnterior == 11 || MarcoAnterior == 12 || MarcoAnterior == 15))
            { Contador = 0; MarcoSiguiente = 16; ComandoSO = 0; ComandoGUI = 16; ComandoSintesisVoz = 0; Cambio = 0; }

            if (MarcoActual == 16 && MarcoAnterior == 16)
            {
                if (Cambio == 1) { ComandoGUI = 11; Contador = 0; ComandoSintesisVoz = 0; }
                else
                {
                    if (Contador == Velocidad * 3)
                    { Contador = 39; MarcoSiguiente = 16; ComandoSO = 5; ComandoGUI = 11; ComandoSintesisVoz = 7; }
                    else if (Contador == 40) { Contador = 39; ComandoSintesisVoz = 0; ComandoSO = 0; }
                }
            }

            //----------------------------


            SalidaProceso[0] = Modo;
            SalidaProceso[1] = MarcoActual;
            SalidaProceso[2] = Contador;
            SalidaProceso[3] = MarcoSiguiente;
            SalidaProceso[4] = ComandoSO;
            SalidaProceso[5] = ComandoGUI;
            SalidaProceso[6] = ComandoSintesisVoz;
            SalidaProceso[7] = Cambio;
            SalidaProceso[8] = Click;
            
            return SalidaProceso;
        }
    }
}
