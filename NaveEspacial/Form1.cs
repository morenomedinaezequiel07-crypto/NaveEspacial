
namespace NaveEspacial
{
    public partial class Form1 : Form
    {
        private int naveX = 420;
        private int naveY = 450;
        private int velocidad = 8;

        private bool izquierda = false;
        private bool derecha = false;
        private bool arriba = false;
        private bool abajo = false;

        private List<Rectangle> disparos = new List<Rectangle>();

        private List<Point> estrellas = new List<Point>();
        private Random aleatorio = new Random();

        private List<Rectangle> enemigos = new List<Rectangle>();
        private int puntos = 0;
        public Form1()
        {
            InitializeComponent();

            DoubleBuffered = true;
            timer1.Interval = 20;

            for (int i = 0; i < 60; i++)
            {
                estrellas.Add(new Point(
                    aleatorio.Next(0, ClientSize.Width),
                    aleatorio.Next(0, ClientSize.Height)
                ));
            }
            for (int i = 0; i < 5; i++)
            {
                enemigos.Add(new Rectangle(
                    aleatorio.Next(20, ClientSize.Width - 40),
                    aleatorio.Next(20, ClientSize.Height / 2),
                    25,
                    25
                ));
            }

            timer1.Start();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            foreach (Point estrella in estrellas)
            {
                g.FillEllipse(
                    Brushes.White,
                    estrella.X,
                    estrella.Y,
                    3,
                    3
                );
            }
            foreach (Rectangle enemigo in enemigos)
            {
                g.FillEllipse(Brushes.OrangeRed, enemigo);
            }

            Point[] nave =
            {
        new Point(naveX + 20, naveY),
        new Point(naveX, naveY + 40),
        new Point(naveX + 40, naveY + 40)
    };

            g.FillPolygon(Brushes.Cyan, nave);
            foreach (Rectangle disparo in disparos)
            {
                g.FillRectangle(Brushes.Yellow, disparo);
            }
               g.DrawString(
                  "Puntos: " + puntos,
                  new Font("Arial", 14, FontStyle.Bold),
                      Brushes.White,
                  10,
    10
);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Left)
                izquierda = true;

            if (e.KeyCode == Keys.Right)
                derecha = true;

            if (e.KeyCode == Keys.Up)
                arriba = true;

            if (e.KeyCode == Keys.Down)
                abajo = true;

            if (e.KeyCode == Keys.Space)
            {
                Rectangle nuevoDisparo = new Rectangle(
                    naveX + 18,
                    naveY - 10,
                    4,
                    12
                );

                disparos.Add(nuevoDisparo);
            }
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.KeyCode == Keys.Left)
                izquierda = false;

            if (e.KeyCode == Keys.Right)
                derecha = false;

            if (e.KeyCode == Keys.Up)
                arriba = false;

            if (e.KeyCode == Keys.Down)
                abajo = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (izquierda)
                naveX -= velocidad;

            if (derecha)
                naveX += velocidad;

            if (arriba)
                naveY -= velocidad;

            if (abajo)
                naveY += velocidad;

            if (naveX < 0)
                naveX = 0;

            if (naveY < 0)
                naveY = 0;

            if (naveX > ClientSize.Width - 40)
                naveX = ClientSize.Width - 40;

            if (naveY > ClientSize.Height - 40)
                naveY = ClientSize.Height - 40;

            for (int i = disparos.Count - 1; i >= 0; i--)
            {
                Rectangle disparo = disparos[i];

                disparo.Y -= 10;

                if (disparo.Bottom < 0)
                {
                    disparos.RemoveAt(i);
                }
                else
                {
                    disparos[i] = disparo;
                }

            }
            for (int i = disparos.Count - 1; i >= 0; i--)
            {
                for (int j = enemigos.Count - 1; j >= 0; j--)
                {
                    if (disparos[i].IntersectsWith(enemigos[j]))
                    {
                        disparos.RemoveAt(i);
                        enemigos.RemoveAt(j);
                        puntos++;

                        break;
                    }
                }
            }
            if (enemigos.Count < 5)
            {
                enemigos.Add(new Rectangle(
                    aleatorio.Next(20, ClientSize.Width - 40),
                    aleatorio.Next(20, ClientSize.Height / 2),
                    25,
                    25
                ));
            }
            for (int i = 0; i < estrellas.Count; i++)
            {
                Point estrella = estrellas[i];

                estrella.Y += 2;

                if (estrella.Y > ClientSize.Height)
                {
                    estrella.Y = 0;
                    estrella.X = aleatorio.Next(0, ClientSize.Width);
                }

                estrellas[i] = estrella;
            }
            Invalidate();

        }
    }
    
}
