using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    public static class Generator
    {
        public static Random Random = new Random(DateTime.Now.Millisecond);

        public static List<Domino> CreatePool()
        {
            var pool = new List<Domino>();
            for (var i = 0; i < 7; i++)
                for (var j = i; j < 7; j++)
                {
                    pool.Add(new Domino(i, j));
                }
            return pool;
        }

        public static List<Domino> CreateDeckPlayer(List<Domino> pool, int count = 7)
        {
            var deck = new List<Domino>();
            for (var i = 0; i < count; i++)
            {
                var id = Random.Next(0, pool.Count);
                deck.Add(pool[id]);
                pool.RemoveAt(id);
            }
            return deck;
        }

        #region Generating pictures for dominoes
        public static void GeneratePictureStringDomino()
        {
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + $@"\domino");
            foreach (var domino in CreatePool())
            {
                var img = Properties.Resources.DominoEmpty;
                var graph = Graphics.FromImage(img);
                graph.DrawString($"{domino.FirstValue}   {domino.SecondValue}", new Font("Arial", 50),
                new SolidBrush(System.Drawing.Color.Black), 30, img.Height / 2 - 30);

                img.Save(Directory.GetCurrentDirectory() + $@"\domino\{domino.FirstValue}-{domino.SecondValue}.png",
                    System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        public static void GeneratePictureEllipseDomino()
        {
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + $@"\domino");
            foreach (var domino in CreatePool())
            {
                var img = Properties.Resources.DominoEmpty;
                var graph = Graphics.FromImage(img);

                switch (domino.FirstValue)
                {
                    case 1:
                        G1(graph, img);
                        break;
                    case 2:
                        G2(graph, img);
                        break;
                    case 3:
                        G3(graph, img);
                        break;
                    case 4:
                        G4(graph, img);
                        break;
                    case 5:
                        G5(graph, img);
                        break;
                    case 6:
                        G6(graph, img);
                        break;
                }

                var a = 105;
                switch (domino.SecondValue)
                {
                    case 1:
                        G1(graph, img, a);
                        break;
                    case 2:
                        G2(graph, img, a);
                        break;
                    case 3:
                        G3(graph, img, a);
                        break;
                    case 4:
                        G4(graph, img, a);
                        break;
                    case 5:
                        G5(graph, img, a);
                        break;
                    case 6:
                        G6(graph, img, a);
                        break;
                }

                img.Save(Directory.GetCurrentDirectory() + $@"\domino\{domino.FirstValue}-{domino.SecondValue}.png",
                    System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private static readonly float h = 20f;

        public static void G1(Graphics graph, System.Drawing.Image img, int a = 0)
        {
            graph.FillEllipse(Brushes.Black, a + img.Width / 4 - h / 2, img.Height / 2 - h / 2, h, h);
        }

        public static void G2(Graphics graph, Image img, int a = 0)
        {
            graph.FillEllipse(Brushes.Black, a + img.Width / 8 - h / 2, img.Height / 4 - h / 2, h, h);
            graph.FillEllipse(Brushes.Black, a + (3 * img.Width / 8) - h / 2, 3 * img.Height / 4 - h / 2, h, h);
        }

        public static void G3(Graphics graph, Image img, int a = 0)
        {
            G1(graph, img, a);
            G2(graph, img, a);
        }

        public static void G4(Graphics graph, Image img, int a = 0)
        {
            graph.FillEllipse(Brushes.Black, a + img.Width / 8 - h / 2, img.Height / 4 - h / 2, h, h);
            graph.FillEllipse(Brushes.Black, a + (3 * img.Width / 8) - h / 2, img.Height / 4 - h / 2, h, h);
            graph.FillEllipse(Brushes.Black, a + img.Width / 8 - h / 2, 3 * img.Height / 4 - h / 2, h, h);
            graph.FillEllipse(Brushes.Black, a + (3 * img.Width / 8) - h / 2, 3 * img.Height / 4 - h / 2, h, h);
        }

        public static void G5(Graphics graph, Image img, int a = 0)
        {
            G1(graph, img, a);
            G4(graph, img, a);
        }

        public static void G6(Graphics graph, Image img, int a = 0)
        {
            G4(graph, img, a);
            graph.FillEllipse(Brushes.Black, a + img.Width / 4 - h / 2, img.Height / 4 - h / 2, h, h);
            graph.FillEllipse(Brushes.Black, a + img.Width / 4 - h / 2, 3 * img.Height / 4 - h / 2, h, h);
        }
        #endregion
    }
}
