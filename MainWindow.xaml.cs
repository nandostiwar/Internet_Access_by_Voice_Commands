using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using System.Windows.Threading;
using System.Windows.Forms;
using System.Drawing;
using AutoItX3Lib;

using Emgu.CV;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using Microsoft.CognitiveServices.SpeechRecognition;
using System.Globalization;

namespace _7_Tesis_Maestria
{
    public partial class MainWindow : Window
    {
        MicrophoneRecognitionClient micClient;
        Output_GUI Imprimir;
        Output_SO Ejecutar;

        int Cnt, X_Old, Y_Old, Pos;
        string Estado = "menu", Dictadu, Idioma;
        bool Activar_Click = false, Insertar_Dictado = false;
        bool Izquierda = false, Derecha = false;

        AutoItX3 AutoIt = new AutoItX3();
        Rectangle Monitor;
        DispatcherTimer Timer;
        System.Drawing.Point cursor;

        Capture capture;
        HaarCascade Rostro;
        Image<Bgr, Byte> Color;
        Image<Gray, Byte> Gray;
        bool RostroFlag = false, Camera = true;
        int posX=0, posY=0, PromX=0, PromY=0, CntM=0, MouseX=0, MouseY=0, fps=25;
        int Velocidad_Click = 3, Sensibilidad_Cursor = 15;
        Rectangle Patron;
        MAFilter MAfiltroX = new MAFilter() { FilterLength = 15 };
        MAFilter MAfiltroY = new MAFilter() { FilterLength = 15 };

        public MainWindow()
        {
            Imprimir = new Output_GUI();
            Ejecutar = new Output_SO();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Idioma = CultureInfo.InstalledUICulture.NativeName;
            Monitor = Screen.PrimaryScreen.Bounds;
            Activar_Click = true;

            capture = new Capture(0);
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 320);
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 240);
            Rostro = new HaarCascade("Rostro2.xml");

            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Timer_);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            Timer.Start();

            StartSpeech();
        }
        
        private void StartSpeech()
        {
            micClient = SpeechRecognitionServiceFactory.CreateMicrophoneClient(
            SpeechRecognitionMode.ShortPhrase, "es_MX",
            "8d485a73767b431e8c10b06152203c62"); //fef69731fd5f42d9a474a0e39b880d6c
            micClient.OnResponseReceived += OnMicShortDictationResponseReceivedHandler;
            micClient.OnConversationError += MicClient_OnConversationError;
            micClient.StartMicAndRecognition();
        }

        private void MicClient_OnConversationError(object sender, SpeechErrorEventArgs e)
        {
            Dispatcher.Invoke(() => { Dictado.Content = "Error: " + e.SpeechErrorText; });
        }

        private void OnMicShortDictationResponseReceivedHandler(object sender, SpeechResponseEventArgs e)
        {
            if (e.PhraseResponse.Results.Length > 0)
            {
                Dispatcher.Invoke(() => { Dictado.Content = e.PhraseResponse.Results[0].DisplayText; });
                if (Insertar_Dictado == false) { Comparar(e.PhraseResponse.Results[0].DisplayText); }
                else { Dictadu = e.PhraseResponse.Results[0].DisplayText; AutoIt.Send(Dictadu); Insertar_Dictado = false; }                
            }
            micClient.EndMicAndRecognition();
            StartSpeech();
        }

        private void Timer_(object sender, EventArgs e)
        {
            Color = capture.QueryFrame();
            if (Color != null)
            {
                CalculateFps();

                if (Camera)
                {
                    RostroFlag = false;
                    //Color._Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);
                    Gray = Color.Convert<Gray, Byte>();
                    Gray._EqualizeHist();

                    MCvAvgComp[] RostrosDetectados = Rostro.Detect(
                        Gray,
                        1.1,
                        5,
                        Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.FIND_BIGGEST_OBJECT,
                        new System.Drawing.Size(80, 80));

                    if (RostrosDetectados.Length > 0)
                    {
                        foreach (MCvAvgComp r in RostrosDetectados)
                        {
                            if (RostroFlag == false)
                            {
                                RostroFlag = true;
                                Color.Draw(r.rect, new Bgr(0, 255, 255), 3);

                                //------ Movimiento del Cursor -------------
                                posX = r.rect.X + (r.rect.Width / 2);
                                posY = r.rect.Y + (r.rect.Height / 2);

                                if (CntM < fps)
                                {
                                    CntM++;
                                    PromX = PromX + posX;
                                    PromY = PromY + posY;
                                }
                                else
                                if (CntM == fps)
                                {
                                    CntM++;
                                    PromX = (int)(PromX / fps);
                                    PromY = (int)(PromY / fps);

                                    Patron.Width = r.rect.Width;
                                    Patron.Height = r.rect.Height;
                                    Patron.X = PromX - (r.rect.Width / 2);
                                    Patron.Y = PromY - (r.rect.Height / 2);
                                }
                                else
                                if (CntM > fps)
                                {
                                    Color.Draw(new LineSegment2D(new System.Drawing.Point(r.rect.X + (r.rect.Width / 2), r.rect.Y + (r.rect.Height / 2)), new System.Drawing.Point(Patron.X + (Patron.Width / 2), Patron.Y + (Patron.Height / 2))), new Bgr(255, 0, 0), 3);
                                    Color.Draw(Patron, new Bgr(0, 0, 255), 3);

                                    if (Math.Abs(posX - PromX) > 30 || Math.Abs(posY - PromY) > 30)
                                    {
                                        CntM = 0;
                                        PromX = 0;
                                        PromY = 0;
                                    }
                                        
                                    if (posX - PromX <= 30)
                                    {
                                        if (posX - PromX >= 0) { MouseX = (Monitor.Width / 2) + (int)((posX - PromX) * ((Monitor.Width / 2) / 15)); }
                                        else { MouseX = (Monitor.Width / 2) - (int)((PromX - posX) * (Monitor.Width / 2) / 15); }
                                        this.MAfiltroX.NewDatum = MouseX;
                                        MouseX = this.MAfiltroX.FilteredDatum;
                                        //MouseX = FiltrarMouseX(MouseX);
                                    }

                                    if (posX - PromX <= 30)
                                    {
                                        if (posY - PromY >= 0) { MouseY = (Monitor.Height / 2) + (int)((posY - PromY) * ((Monitor.Height / 2) / 15)); }
                                        else { MouseY = (Monitor.Height / 2) - (int)((PromY - posY) * (Monitor.Height / 2) / 15); }
                                        this.MAfiltroY.NewDatum = MouseY;
                                        MouseY = this.MAfiltroY.FilteredDatum;
                                        //MouseY = FiltrarMouseX(MouseY);
                                    }

                                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(MouseX, MouseY);
                                    //AutoIt.MouseMove(MouseX, MouseY);
                                }
                            }
                        }
                    }
                    else
                    if (RostrosDetectados.Length == 0)
                    {
                        CntM = 0;
                        PromX = 0;
                        PromY = 0;
                    }
                }
                
                Imagen.Source = ToBitmapSource(Color);
            }                            

            //Manejo de la interfaz propuesta                
            X_Old = cursor.X;
            Y_Old = cursor.Y;
            cursor = System.Windows.Forms.Cursor.Position;
                
            if (cursor.Y < Monitor.Height / 30 && Estado != "menu") { Imprimir.Cursor(cursor.X, cursor.Y, 0); Imprimir.Marco(1); Cnt++; if(Cnt == 8) { Cnt = 0; Ejecutar.Comando(2); }}
            else if (cursor.Y > Monitor.Height * 29 / 30 && Estado != "menu") { Imprimir.Cursor(cursor.X, cursor.Y, 0); Imprimir.Marco(2); Cnt++; if (Cnt == 8) { Cnt = 0; Ejecutar.Comando(3); } }
                
            else if (cursor.X < Monitor.Width / 40 && Izquierda == false && Estado != "menu") { Izquierda = true; }
            else if (cursor.X > Monitor.Width * 39 / 40 && Derecha == false && Estado != "menu") { Derecha = true; }

            else if (cursor.X > Monitor.Width / 5 && Izquierda == true) { Izquierda = false; Imprimir.Marco(0); }
            else if (cursor.X < Monitor.Width * 4 / 5 && Derecha == true) { Derecha = false; Imprimir.Marco(0); }

            else if (cursor.X < Monitor.Width / 5 && Izquierda == true)
            {
                if (cursor.Y < Monitor.Height / 4) { if (Estado == "facebook") { Imprimir.Marco(3); } if (Estado == "google") { Imprimir.Marco(11); } if (Estado == "gmail") { Imprimir.Marco(15); } if (Pos != 1) { Cnt = 0; Pos = 1; } }
                else if (cursor.Y > Monitor.Height / 4 && cursor.Y < Monitor.Height / 2) { if (Estado == "facebook") { Imprimir.Marco(4); } if (Estado == "google") { Imprimir.Marco(12); } if (Estado == "gmail") { Imprimir.Marco(16); } if (Pos != 2) { Cnt = 0; Pos = 2; } }
                else if (cursor.Y > Monitor.Height / 2 && cursor.Y < Monitor.Height * 3 / 4) { if (Estado == "facebook") { Imprimir.Marco(5); } if (Estado == "google") { Imprimir.Marco(13); } if (Estado == "gmail") { Imprimir.Marco(17); } if (Pos != 3) { Cnt = 0; Pos = 3; } }
                else if (cursor.Y > Monitor.Height * 3 / 4) { if (Estado == "facebook") { Imprimir.Marco(6); } if (Estado == "google") { Imprimir.Marco(14); } if (Estado == "gmail") { Imprimir.Marco(18); } if (Pos != 4) { Cnt = 0; Pos = 4; } }

                if ((Math.Abs(X_Old - cursor.X) > Sensibilidad_Cursor) || (Math.Abs(Y_Old - cursor.Y) > Sensibilidad_Cursor)) { Cnt = 0; }
                else { Cnt++; }
                if (Cnt / Velocidad_Click == 13) { if (Estado == "facebook") { Ejecutar.Comando(Pos + 7); Output_SintesisVoz.Hablar(Pos + 14); } if (Estado == "google") { Ejecutar.Comando(Pos + 11); Output_SintesisVoz.Hablar(Pos + 18); } if (Estado == "gmail") { Ejecutar.Comando(Pos + 15); Output_SintesisVoz.Hablar(Pos + 22); } Cnt = 14 * 5; }
                if (Cnt / Velocidad_Click > 13) { Imprimir.Cursor(cursor.X, cursor.Y, 0); }
                else { Imprimir.Cursor(cursor.X, cursor.Y, Cnt / Velocidad_Click); }
            }

            else if (cursor.X > Monitor.Width *4/5 && Derecha == true)
            {
                if (cursor.Y < Monitor.Height / 4) { Imprimir.Marco(7); if (Pos != 1) { Cnt = 0; Pos = 1; } }
                else if (cursor.Y > Monitor.Height / 4 && cursor.Y < Monitor.Height / 2) { Imprimir.Marco(8); if (Pos != 2) { Cnt = 0; Pos = 2; } }
                else if (cursor.Y > Monitor.Height / 2 && cursor.Y < Monitor.Height * 3 / 4) { Imprimir.Marco(9); if (Pos != 3) { Cnt = 0; Pos = 3; } }
                else if (cursor.Y > Monitor.Height * 3 / 4) { Imprimir.Marco(10); if (Pos != 4) { Cnt = 0; Pos = 4; } }

                if ((Math.Abs(X_Old - cursor.X) > Sensibilidad_Cursor) || (Math.Abs(Y_Old - cursor.Y) > Sensibilidad_Cursor)) { Cnt = 0; }
                else { Cnt++; }
                if (Cnt / Velocidad_Click == 13) { Ejecutar.Comando(Pos + 3); if (Pos + 3 == 4) { Output_SintesisVoz.Hablar(5); } if (Pos + 3 == 5) { Output_SintesisVoz.Hablar(7); } if (Pos + 3 == 6) { Output_SintesisVoz.Hablar(8); } if (Pos + 3 == 7) { Output_SintesisVoz.Hablar(9); } if (Pos == 1) { Estado = "menu"; } Cnt = 14 * 5; }
                if (Cnt / Velocidad_Click > 13) { Imprimir.Cursor(cursor.X, cursor.Y, 0); }
                else { Imprimir.Cursor(cursor.X, cursor.Y, Cnt / Velocidad_Click); }
            }
                
            else if (Izquierda == false && Derecha == false)
            {
                Imprimir.Marco(0);
                if (Activar_Click)
                {
                    if ((Math.Abs(X_Old - cursor.X) > Sensibilidad_Cursor) || (Math.Abs(Y_Old - cursor.Y) > Sensibilidad_Cursor)) { Cnt = 0; }
                    else { Cnt++; }
                    if (Cnt / Velocidad_Click == 13){ Ejecutar.Comando(1); Cnt = 14 * 5; }
                    if (Cnt / Velocidad_Click > 13) { Imprimir.Cursor(cursor.X, cursor.Y, 0); }
                    else { Imprimir.Cursor(cursor.X, cursor.Y, Cnt / Velocidad_Click); }
                }
                else
                {
                    Imprimir.Cursor(cursor.X, cursor.Y, 0);
                }
            }                            
        }

        private void Facebook_Click(object sender, RoutedEventArgs e)
        {
            Abrir_Facebook();
        }

        private void Abrir_Facebook()
        {
            Estado = "facebook";
            Output_SintesisVoz.Hablar(1);
            AutoIt.Run("C://Program Files (x86)//Google//Chrome//Application//chrome.exe");
            if (Idioma.Contains("English")) { AutoIt.WinWaitActive("New Tab - Google Chrome", "", 60); }
            else { AutoIt.WinWaitActive("Nueva pestaña - Google Chrome", "", 60); }
            AutoIt.Send("facebook.com{ENTER}");
            AutoIt.WinWaitActive("Facebook", "", 60);
            AutoIt.WinSetState("Facebook", "", AutoIt.SW_RESTORE);
            AutoIt.WinMove("Facebook", "", 0, 0);
            AutoIt.WinSetState("Facebook", "", AutoIt.SW_MAXIMIZE);
        }

        private void Google_Click(object sender, RoutedEventArgs e)
        {
            Abrir_Google();
        }

        private void Abrir_Google()
        {
            Estado = "google";
            Output_SintesisVoz.Hablar(3);
            AutoIt.Run("C://Program Files (x86)//Google//Chrome//Application//chrome.exe");
            if (Idioma.Contains("English"))
            {
                AutoIt.WinWaitActive("New Tab - Google Chrome", "", 60);
                AutoIt.WinSetState("New Tab - Google Chrome", "", AutoIt.SW_RESTORE);
                AutoIt.WinMove("New Tab - Google Chrome", "", 0, 0);
                AutoIt.WinSetState("New Tab - Google Chrome", "", AutoIt.SW_MAXIMIZE); 
            }
            else
            {
                AutoIt.WinWaitActive("Nueva pestaña - Google Chrome", "", 60);
                AutoIt.WinSetState("Nueva pestaña - Google Chrome", "", AutoIt.SW_RESTORE);
                AutoIt.WinMove("Nueva pestaña - Google Chrome", "", 0, 0);
                AutoIt.WinSetState("Nueva pestaña - Google Chrome", "", AutoIt.SW_MAXIMIZE);
            }
        }

        private void Gmail_Click(object sender, RoutedEventArgs e)
        {
            Abrir_Gmail();
        }

        private void Abrir_Gmail()
        {
            Estado = "gmail";
            Output_SintesisVoz.Hablar(2);
            AutoIt.Run("C://Program Files (x86)//Google//Chrome//Application//chrome.exe");
            if (Idioma.Contains("English")) { AutoIt.WinWaitActive("New Tab - Google Chrome", "", 60); }
            else { AutoIt.WinWaitActive("Nueva pestaña - Google Chrome", "", 60); }
            AutoIt.Send("gmail.com{ENTER}");
            AutoIt.WinWaitActive("Gmail", "", 60);
            AutoIt.WinSetState("Gmail", "", AutoIt.SW_RESTORE);
            AutoIt.WinMove("Gmail", "", 0, 0);
            AutoIt.WinSetState("Gmail", "", AutoIt.SW_MAXIMIZE);
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Output_SintesisVoz.Hablar(4);
            System.Environment.Exit(0);
        }

        private void Camara_Click(object sender, RoutedEventArgs e)
        {
            if(Camera == false) { Camera = true; }
            else { Camera = false; }
        }

        internal static class NativeMethods
        {
            [DllImport("gdi32")]
            public static extern int DeleteObject(IntPtr o);
        }

        public static BitmapSource ToBitmapSource(IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap();

                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                NativeMethods.DeleteObject(ptr);
                return bs;
            }
        }

        int count = 0;
        DateTime starttime;
        private void CalculateFps()
        {
            if (starttime == DateTime.MinValue)
            {
                starttime = DateTime.Now;
            }
            count++;
            var dif = DateTime.Now - starttime;
            fps = (int)(count / dif.TotalSeconds);
            Frames.Content = fps.ToString() + " FPS";
        }

        private void Comparar(string texto)
        {
            if (texto.Contains("alibrar")) { Output_SintesisVoz.Hablar(28); CntM = 0; PromX = 0; PromY = 0;
            }
            else if (Estado == "menu")
            {
                if (texto.Contains("brir Facebook")) { Abrir_Facebook(); }
                else if (texto.Contains("brir Gmail")) { Abrir_Gmail(); }
                else if (texto.Contains("brir Google")) { Abrir_Google(); }
                else if (texto.Contains("alir")) { Output_SintesisVoz.Hablar(4); System.Environment.Exit(0); }
            }
            else if (Estado != "menu")
            {
                if (texto.Contains("enú")) { Output_SintesisVoz.Hablar(5); Ejecutar.Comando(4); Estado = "menu"; }
                else if (texto.Contains("ctualizar")) { Output_SintesisVoz.Hablar(6); Ejecutar.Comando(20); }
                else if (texto.Contains("cercar")) { Output_SintesisVoz.Hablar(7); Ejecutar.Comando(5); }
                else if (texto.Contains("lejar")) { Output_SintesisVoz.Hablar(8); Ejecutar.Comando(6); }
                else if (texto.Contains("trás")) { Output_SintesisVoz.Hablar(9); Ejecutar.Comando(7); }
                else if (texto.Contains("esactivar click")) { Output_SintesisVoz.Hablar(10); Activar_Click = false; }
                else if (texto.Contains("ncender click")) { Output_SintesisVoz.Hablar(11); Activar_Click = true; }
                else if (texto.Contains("nsertar dictado")) { Output_SintesisVoz.Hablar(12); Insertar_Dictado = true; }
                else if (texto.Contains("nter")) { Output_SintesisVoz.Hablar(13); Ejecutar.Comando(21); }
                else if (texto.Contains("eshacer")) { Output_SintesisVoz.Hablar(14); Ejecutar.Comando(22); }
                else if (texto.Contains("scape")) { Output_SintesisVoz.Hablar(14); Ejecutar.Comando(23); }

                else if (Estado == "facebook")
                {
                    if (texto.Contains("brir Gmail")) { Output_SintesisVoz.Hablar(2); Ejecutar.Comando(4); Abrir_Gmail(); }
                    else if (texto.Contains("brir Google")) { Output_SintesisVoz.Hablar(3); Ejecutar.Comando(4); Abrir_Google(); }
                    else if (texto.Contains("uro")) { Output_SintesisVoz.Hablar(15); Ejecutar.Comando(8); }
                    else if (texto.Contains("otificaciones")) { Output_SintesisVoz.Hablar(16); Ejecutar.Comando(9); }
                    else if (texto.Contains("erfil")) { Output_SintesisVoz.Hablar(17); Ejecutar.Comando(10); }
                    else if (texto.Contains("ensajes")) { Output_SintesisVoz.Hablar(18); Ejecutar.Comando(11); }
                }

                else if (Estado == "google")
                {
                    if (texto.Contains("brir Facebook")) { Output_SintesisVoz.Hablar(1); Ejecutar.Comando(4); Abrir_Facebook(); }
                    else if (texto.Contains("brir Gmail")) { Output_SintesisVoz.Hablar(2); Ejecutar.Comando(4); Abrir_Gmail(); }
                    else if (texto.Contains("ueva búsqueda")) { Output_SintesisVoz.Hablar(19); Ejecutar.Comando(12); }
                    else if (texto.Contains("escargas")) { Output_SintesisVoz.Hablar(20); Ejecutar.Comando(13); }
                    else if (texto.Contains("istorial")) { Output_SintesisVoz.Hablar(21); Ejecutar.Comando(14); }
                    else if (texto.Contains("mprimir")) { Output_SintesisVoz.Hablar(22); Ejecutar.Comando(15); }
                }

                else if (Estado == "gmail")
                {
                    if (texto.Contains("brir Facebook")) { Output_SintesisVoz.Hablar(1); Ejecutar.Comando(4); Abrir_Facebook(); }
                    else if (texto.Contains("brir Google")) { Output_SintesisVoz.Hablar(3); Ejecutar.Comando(4); Abrir_Google(); }
                    else if (texto.Contains("nviados")) { Output_SintesisVoz.Hablar(23); Ejecutar.Comando(16); }
                    else if (texto.Contains("andeja de entrada")) { Output_SintesisVoz.Hablar(24); Ejecutar.Comando(17); }
                    else if (texto.Contains("edactar correo")) { Output_SintesisVoz.Hablar(25); Ejecutar.Comando(18); }
                    else if (texto.Contains("orradores")) { Output_SintesisVoz.Hablar(26); Ejecutar.Comando(18); }
                }
            }
        }        
    }
}
