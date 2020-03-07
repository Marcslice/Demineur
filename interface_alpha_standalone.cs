using System;
using System.Threading;

namespace draw
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.SetWindowSize(100, 40);
            Console.Title = "Démineur";

            char[] tab = new char[400];
            for (int x = 0; x < 400; x++)
                tab[x] = '*';

            DessinerGrille(18, 8, tab); // rectangulaire sur le long, plus facile a lire sur console.
            Console.ReadLine();
        }

        public static void DessinerEntete(int iCol, string nomJoueur, int nbCouts) { //Dessine le timer, le nom du joueur et le petit bonhomme

          
            Console.Write("  ");
            for (int y = 0; y < iCol; y++) {
                Console.Write(" ___");
            }
            Console.Write("\n");
            for (int x = 0; x < 5; x++)
            {
                Console.Write("  |");
                if (x == 4)
                    for (int y = 0; y < iCol; y++)
                            Console.Write("___ ");
                else if (x == 2) {
                    for (int y = 0; y < iCol; y++)
                        if (y == 1)
                            Console.Write(nomJoueur);
                        else if (y == iCol - 2)
                            Console.Write(nbCouts); // doit être formatté pour prendre un espace fixe.
                        else if (y == (iCol / 2)-1)
                            Console.Write(" 0  0  ");
                        else
                            Console.Write("    ");
                }
                else if (x == 3)
                {
                    for (int y = 0; y < iCol; y++)
                        if (y == (iCol / 2)-1)
                            Console.Write("  D`");
                        else
                            Console.Write("    ");
                }
                else
                    for (int y = 0; y < iCol; y++)
                        Console.Write("    ");
               
                Console.Write("\b|");
                Console.Write("\n");
            }
        }


        public static void DessinerGrille(int col, int lig, char[] tab) {

            for (int x = 0; x < lig; x++)
            {
                if (x == 0)
                {
                    Console.Write("  ");
                    for (int y = 0; y < col; y++)
                    {                      
                        if (y < 10)
                            Console.Write(" ");
                        Console.Write(" " + (y+1) + " ");                       
                    }
                    Console.Write("\n");

                    for (int y = 0; y < col; y++)
                    {
                        if (y == 0)
                            Console.Write("  ");
                        Console.Write(" ___");
                    }
                    Console.Write("\n");
                }

                for (int y = 0; y < col; y++)
                {
                    if (y == 0)
                        Console.Write("  |   |");
                    else
                        Console.Write("   |");
                }
                Console.Write("\n");

                for (int y = 0; y < col; y++)
                {
                    if(y == 0)
                        Console.Write(x+1 + " | ");
                    Console.Write(tab[y] + " | ");
                }
                Console.Write("\n");

                for (int y = 0; y < col; y++)
                {
                    if (y == 0)
                        Console.Write("  |___|");
                    else
                        Console.Write("___|");
                }
                Console.Write("\n");
            }
            DessinerEntete(18, "marc", 3);
            Console.Write("\n   Quelle case souhaitez-vous ouvrir ? >> ");
        }
    }
}
