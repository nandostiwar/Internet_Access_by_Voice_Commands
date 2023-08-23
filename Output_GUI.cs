using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;

namespace _7_Tesis_Maestria
{
    public class Output_GUI
    {
        PointerForm circulo;
        AllForm marco;
        System.Drawing.Point cursor;
        public int old, click_old;        

        public Output_GUI()
        {
            circulo = new PointerForm();
            marco = new AllForm();
            cursor = new System.Drawing.Point();
        }
        
        public void Cursor (int x, int y, int Click)
        {            
            cursor.X = x; cursor.Y = y;
            cursor.Offset(-100, -100);
            circulo.Location = cursor;

            circulo.click = Click;
            if (circulo.click != click_old) { click_old = circulo.click; circulo.Invalidate(); }            
            if (!circulo.Visible) { circulo.Show(); }            
        }

        public class PointerForm : Form
        {
            public int click;
            public Rectangle Draw_1 = new Rectangle(0, 0, 200, 200);

            Bitmap Img_Cursor_0 = new Bitmap("Imagenes/Cursor_1.png");
            Bitmap Img_Cursor_1 = new Bitmap("Imagenes/Cursor_2.png");
            Bitmap Img_Cursor_2 = new Bitmap("Imagenes/Cursor_3.png");
            Bitmap Img_Cursor_3 = new Bitmap("Imagenes/Cursor_4.png");
            Bitmap Img_Cursor_4 = new Bitmap("Imagenes/Cursor_5.png");
            Bitmap Img_Cursor_5 = new Bitmap("Imagenes/Cursor_6.png");
            Bitmap Img_Cursor_6 = new Bitmap("Imagenes/Cursor_7.png");
            Bitmap Img_Cursor_7 = new Bitmap("Imagenes/Cursor_8.png");
            Bitmap Img_Cursor_8 = new Bitmap("Imagenes/Cursor_9.png");
            Bitmap Img_Cursor_9 = new Bitmap("Imagenes/Cursor_10.png");
            Bitmap Img_Cursor_10 = new Bitmap("Imagenes/Cursor_11.png");
            Bitmap Img_Cursor_11 = new Bitmap("Imagenes/Cursor_12.png");
            Bitmap Img_Cursor_12 = new Bitmap("Imagenes/Cursor_13.png");
            Bitmap Img_Cursor_13 = new Bitmap("Imagenes/Cursor_14.png");

            public PointerForm()
            {
                TopMost = true;
                ShowInTaskbar = false;
                FormBorderStyle = FormBorderStyle.None;
                BackColor = Color.White;
                TransparencyKey = Color.White;
                Width = 200;
                Height = 200;
                Opacity = .65;
                Paint += new PaintEventHandler(PointerForm_Paint);
            }

            public void PointerForm_Paint(object sender, PaintEventArgs e)
            {
                switch (click)
                {
                    case 0: e.Graphics.DrawImage(Img_Cursor_0, Draw_1); break;
                    case 1: e.Graphics.DrawImage(Img_Cursor_1, Draw_1); break;
                    case 2: e.Graphics.DrawImage(Img_Cursor_2, Draw_1); break;
                    case 3: e.Graphics.DrawImage(Img_Cursor_3, Draw_1); break;
                    case 4: e.Graphics.DrawImage(Img_Cursor_4, Draw_1); break;
                    case 5: e.Graphics.DrawImage(Img_Cursor_5, Draw_1); break;
                    case 6: e.Graphics.DrawImage(Img_Cursor_6, Draw_1); break;
                    case 7: e.Graphics.DrawImage(Img_Cursor_7, Draw_1); break;
                    case 8: e.Graphics.DrawImage(Img_Cursor_8, Draw_1); break;
                    case 9: e.Graphics.DrawImage(Img_Cursor_9, Draw_1); break;
                    case 10: e.Graphics.DrawImage(Img_Cursor_10, Draw_1); break;
                    case 11: e.Graphics.DrawImage(Img_Cursor_11, Draw_1); break;
                    case 12: e.Graphics.DrawImage(Img_Cursor_12, Draw_1); break;
                    case 13: e.Graphics.DrawImage(Img_Cursor_13, Draw_1); break;
                }
            }
        }

        public void Marco(int marcox)
        {
            marco.activo = marcox;
            if (marco.activo != old) { old = marco.activo; marco.Invalidate(); }
            if (!marco.Visible) { marco.Show(); }
        }

        public class AllForm : Form
        {
            public int activo;
            public Rectangle Monitor;
            public Rectangle Draw_1, Draw_2, Draw_3, Draw_4, Draw_5, Draw_6;
            public Rectangle Draw_7, Draw_8, Draw_9, Draw_10;

            Bitmap Img_1 = new Bitmap("3.png");
            Bitmap Img_2 = new Bitmap("4.png");
            
            Bitmap Img_Inicio_N = new Bitmap("Imagenes/Inicio_N.png");
            Bitmap Img_Inicio_E = new Bitmap("Imagenes/Inicio_E.png");

            Bitmap Img_Notificaciones_N = new Bitmap("Imagenes/Notificaciones_N.png");
            Bitmap Img_Notificaciones_E = new Bitmap("Imagenes/Notificaciones_E.png");

            Bitmap Img_Perfil_N = new Bitmap("Imagenes/Perfil_N.png");
            Bitmap Img_Perfil_E = new Bitmap("Imagenes/Perfil_E.png");

            Bitmap Img_Mensajes_N = new Bitmap("Imagenes/Mensaje_N.png");
            Bitmap Img_Mensajes_E = new Bitmap("Imagenes/Mensaje_E.png");

            Bitmap Img_Menu_N = new Bitmap("Imagenes/Menu_N.png");
            Bitmap Img_Menu_E = new Bitmap("Imagenes/Menu_E.png");

            Bitmap Img_ZoomMas_N = new Bitmap("Imagenes/ZoomMas_N.png");
            Bitmap Img_ZoomMas_E = new Bitmap("Imagenes/ZoomMas_E.png");
            
            Bitmap Img_ZoomMenos_N = new Bitmap("Imagenes/ZoomMenos_N.png");
            Bitmap Img_ZoomMenos_E = new Bitmap("Imagenes/ZoomMenos_E.png");
            
            Bitmap Img_Atras_N = new Bitmap("Imagenes/Atras_N.png");
            Bitmap Img_Atras_E = new Bitmap("Imagenes/Atras_E.png");
            
            Bitmap Img_Arriba_N = new Bitmap("Imagenes/Up_N.png");
            Bitmap Img_Arriba_E = new Bitmap("Imagenes/Up_E.png");
            
            Bitmap Img_Abajo_N = new Bitmap("Imagenes/Down_N.png");
            Bitmap Img_Abajo_E = new Bitmap("Imagenes/Down_E.png");
            
            
            public AllForm()
            {
                Monitor = Screen.PrimaryScreen.Bounds;

                Draw_1 = new Rectangle(0, 0, Monitor.Width, Monitor.Height / 30);
                Draw_2 = new Rectangle(0, Monitor.Height * 29 / 30, Monitor.Width, Monitor.Height / 30);

                Draw_3 = new Rectangle(0, 0, Monitor.Width / 5, Monitor.Height / 4);
                Draw_4 = new Rectangle(0, Monitor.Height / 4, Monitor.Width / 5, Monitor.Height / 4);
                Draw_5 = new Rectangle(0, Monitor.Height / 2, Monitor.Width / 5, Monitor.Height / 4);
                Draw_6 = new Rectangle(0, Monitor.Height * 3 / 4, Monitor.Width / 5, Monitor.Height / 4);

                Draw_7 = new Rectangle(Monitor.Width * 4 / 5, 0, Monitor.Width / 5, Monitor.Height / 4);
                Draw_8 = new Rectangle(Monitor.Width * 4 / 5, Monitor.Height / 4, Monitor.Width / 5, Monitor.Height / 4);
                Draw_9 = new Rectangle(Monitor.Width * 4 / 5, Monitor.Height / 2, Monitor.Width / 5, Monitor.Height / 4);
                Draw_10 = new Rectangle(Monitor.Width * 4 / 5, Monitor.Height * 3 / 4, Monitor.Width / 5, Monitor.Height / 4);


                TopMost = true;
                ShowInTaskbar = false;
                FormBorderStyle = FormBorderStyle.None;
                BackColor = Color.White;
                TransparencyKey = Color.White;
                Rectangle resolution = Screen.PrimaryScreen.Bounds;
                Width = resolution.Width;
                Height = resolution.Height;
                Opacity = .40;
                Paint += new PaintEventHandler(AllForm_Paint);
            }

            void AllForm_Paint(object sender, PaintEventArgs e)
            {
                    switch (activo)
                    {
                        case 1: e.Graphics.DrawImage(Img_Arriba_N, Draw_1); break;
                        case 2: e.Graphics.DrawImage(Img_Abajo_N, Draw_2); break;

                        case 3: e.Graphics.DrawImage(Img_Inicio_E, Draw_3);
                            e.Graphics.DrawImage(Img_Notificaciones_N, Draw_4);
                            e.Graphics.DrawImage(Img_Perfil_N, Draw_5);
                            e.Graphics.DrawImage(Img_Mensajes_N, Draw_6);
                            break;
                        case 4:
                            e.Graphics.DrawImage(Img_Inicio_N, Draw_3);
                            e.Graphics.DrawImage(Img_Notificaciones_E, Draw_4);
                            e.Graphics.DrawImage(Img_Perfil_N, Draw_5);
                            e.Graphics.DrawImage(Img_Mensajes_N, Draw_6);
                            break;
                        case 5:
                            e.Graphics.DrawImage(Img_Inicio_N, Draw_3);
                            e.Graphics.DrawImage(Img_Notificaciones_N, Draw_4);
                            e.Graphics.DrawImage(Img_Perfil_E, Draw_5);
                            e.Graphics.DrawImage(Img_Mensajes_N, Draw_6);
                            break;
                        case 6:
                            e.Graphics.DrawImage(Img_Inicio_N, Draw_3);
                            e.Graphics.DrawImage(Img_Notificaciones_N, Draw_4);
                            e.Graphics.DrawImage(Img_Perfil_N, Draw_5);
                            e.Graphics.DrawImage(Img_Mensajes_E, Draw_6);
                            break;

                        case 7:
                            e.Graphics.DrawImage(Img_Menu_E, Draw_7);
                            e.Graphics.DrawImage(Img_ZoomMas_N, Draw_8);
                            e.Graphics.DrawImage(Img_ZoomMenos_N, Draw_9);
                            e.Graphics.DrawImage(Img_Atras_N, Draw_10);
                            break;
                        case 8:
                            e.Graphics.DrawImage(Img_Menu_N, Draw_7);
                            e.Graphics.DrawImage(Img_ZoomMas_E, Draw_8);
                            e.Graphics.DrawImage(Img_ZoomMenos_N, Draw_9);
                            e.Graphics.DrawImage(Img_Atras_N, Draw_10);
                            break;
                        case 9:
                            e.Graphics.DrawImage(Img_Menu_N, Draw_7);
                            e.Graphics.DrawImage(Img_ZoomMas_N, Draw_8);
                            e.Graphics.DrawImage(Img_ZoomMenos_E, Draw_9);
                            e.Graphics.DrawImage(Img_Atras_N, Draw_10);
                            break;
                        case 10:
                            e.Graphics.DrawImage(Img_Menu_N, Draw_7);
                            e.Graphics.DrawImage(Img_ZoomMas_N, Draw_8);
                            e.Graphics.DrawImage(Img_ZoomMenos_N, Draw_9);
                            e.Graphics.DrawImage(Img_Atras_E, Draw_10);
                            break;

                        case 11:
                            e.Graphics.DrawImage(Img_Inicio_E, Draw_3);
                            e.Graphics.DrawImage(Img_Notificaciones_N, Draw_4);
                            e.Graphics.DrawImage(Img_Perfil_N, Draw_5);
                            e.Graphics.DrawImage(Img_Mensajes_N, Draw_6);
                            break;
                        case 12:
                            e.Graphics.DrawImage(Img_Inicio_N, Draw_3);
                            e.Graphics.DrawImage(Img_Notificaciones_E, Draw_4);
                            e.Graphics.DrawImage(Img_Perfil_N, Draw_5);
                            e.Graphics.DrawImage(Img_Mensajes_N, Draw_6);
                            break;
                        case 13:
                            e.Graphics.DrawImage(Img_Inicio_N, Draw_3);
                            e.Graphics.DrawImage(Img_Notificaciones_N, Draw_4);
                            e.Graphics.DrawImage(Img_Perfil_E, Draw_5);
                            e.Graphics.DrawImage(Img_Mensajes_N, Draw_6);
                            break;
                        case 14:
                            e.Graphics.DrawImage(Img_Inicio_N, Draw_3);
                            e.Graphics.DrawImage(Img_Notificaciones_N, Draw_4);
                            e.Graphics.DrawImage(Img_Perfil_N, Draw_5);
                            e.Graphics.DrawImage(Img_Mensajes_E, Draw_6);
                            break;

                        case 15:
                            e.Graphics.DrawImage(Img_Inicio_E, Draw_3);
                            e.Graphics.DrawImage(Img_Notificaciones_N, Draw_4);
                            e.Graphics.DrawImage(Img_Perfil_N, Draw_5);
                            e.Graphics.DrawImage(Img_Mensajes_N, Draw_6);
                            break;
                        case 16:
                            e.Graphics.DrawImage(Img_Inicio_N, Draw_3);
                            e.Graphics.DrawImage(Img_Notificaciones_E, Draw_4);
                            e.Graphics.DrawImage(Img_Perfil_N, Draw_5);
                            e.Graphics.DrawImage(Img_Mensajes_N, Draw_6);
                            break;
                        case 17:
                            e.Graphics.DrawImage(Img_Inicio_N, Draw_3);
                            e.Graphics.DrawImage(Img_Notificaciones_N, Draw_4);
                            e.Graphics.DrawImage(Img_Perfil_E, Draw_5);
                            e.Graphics.DrawImage(Img_Mensajes_N, Draw_6);
                            break;
                        case 18:
                            e.Graphics.DrawImage(Img_Inicio_N, Draw_3);
                            e.Graphics.DrawImage(Img_Notificaciones_N, Draw_4);
                            e.Graphics.DrawImage(Img_Perfil_N, Draw_5);
                            e.Graphics.DrawImage(Img_Mensajes_E, Draw_6);
                            break;
                }
            }
        }
    }
}