﻿using System;
using System.Threading;
using System.Diagnostics;

namespace draw
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(140, 40);
            Console.Title = "Démineur";

            char[] tab = new char[400]; //Émule la string que va recevoir l'interface
            for (int x = 0; x < 400; x++)
                tab[x] = '*';

            DessinerGrille(16, 8, tab,5,5); // rectangulaire sur le long, plus facile a lire sur console.
        }

        public static void DessinerEntete(int iCol, string nomJoueur, int nbCouts)
        { //Dessine le timer, le nom du joueur et le petit bonhomme

            Console.Write("    ");
            for (int y = 0; y < iCol; y++) //Dessine ligne du haut
                Console.Write(" ___");

            Console.Write("\n");
            for (int x = 0; x < 5; x++) // Dessine ligne 
            {
                Console.Write("    |");
                if (x == 4)
                    for (int y = 0; y < iCol; y++)
                        Console.Write("___ ");
                else if (x == 2)    //Dessine ligne 3 de l'entete (Nom, yeux, nbCout)
                {
                    for (int y = 0; y < iCol; y++)
                        if (y == 1)
                            Console.Write(nomJoueur);
                        else if (y == iCol - 2)
                            Console.Write(nbCouts); // doit être formatté pour prendre un espace fixe.
                        else if (y == (iCol / 2) - 1)
                            Console.Write("  :D   ");
                        else
                            Console.Write("    ");
                }
                else
                    for (int y = 0; y < iCol; y++) //Dessine ligne vide
                        Console.Write("    ");

                Console.Write("\b|");
                Console.Write("\n");            
            }
        }


        public void DessinerTitreJeu() {

            // Dessine le titre centré
            string titre = "Démineur 2020, année du JUGEMENT dernier.";
            int posTitre = (((col * 4) / 2) - (titre.Length / 2)) + 4;
            drawInstructions(col * 4 + 8);
            Console.SetCursorPosition(posTitre, 0);
            Console.WriteLine(titre + "\n");
            // Fin du titre


        }

        public static void DessinerGrille(int col, int lig, char[] tab, short posX, short posY)
        {

            DessinerTitreJeu();

            for (int x = 0; x < lig; x++)
            {
                if (x == 0)
                {
                    Console.Write("   "); // left offset for top numbers
                    for (int y = 0; y < col; y++)
                    {
                        if (y < 10) // spacing between top numbers
                            Console.Write(" ");
                        Console.Write(" " +(y + 1) + " ");
                    }
                    Console.Write("\n");

                    for (int y = 0; y < col; y++)
                    {
                        if (y == 0)
                            Console.Write("    ");
                        Console.Write(" ___");
                    }
                    Console.Write("\n");
                }

                for (int y = 0; y < col; y++)
                {
                    if (y == 0)
                        Console.Write("    |   |");
                    else
                        Console.Write("   |");
                }
                Console.Write("\n");

                for (int y = 0; y < col; y++)
                {
                    if (y == 0)
                    {
                        if(x+1 < 10)
                            Console.Write(" " + (x + 1) + "  | ");
                        else
                            Console.Write(" " + (x + 1) + " | ");
                    }
                    Console.Write(tab[y] + " | ");
                }
                Console.Write("\n");

                for (int y = 0; y < col; y++)
                {
                    if (y == 0)
                        Console.Write("    |___|");
                    else
                        Console.Write("___|");
                }
                Console.Write("\n");
            }
            DessinerEntete(col, "marc", 3);
            Console.Write("\n   Quelle case souhaitez-vous ouvrir ? >> 1 1");
        moving(col,lig, tab);
        }

        public static void drawInstructions(int left) {
            Console.SetCursorPosition(left , 4);
            Console.Write("Appuyez sur f pour utiliser les flèches.");
            Console.SetCursorPosition(left, 5);
            Console.Write("Appuyez sur c pour entrer des coordonnées au clavier.");
            Console.SetCursorPosition(left,6);
            Console.Write("Appuyez sur r pour afficher les règles.");
        }

        public static void moving(int iCol, int iLig, char[] tab) {
            ConsoleKeyInfo arrow;
            bool boucle = true;
            int[] positionActuelle;
            string choix;

            Console.SetCursorPosition(6, 5);
            while (boucle)
            {
                
                arrow = Console.ReadKey();

                switch ((int)arrow.Key)
                {
                    case 37: // left arrow
                        if (Console.CursorLeft < 10) {
                            Console.SetCursorPosition((iCol * 4) + 2, Console.CursorTop);
                        }   
                        else
                            Console.SetCursorPosition(Console.CursorLeft - 5, Console.CursorTop);
                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };                    
                        break;
                    case 39: // right arrow
                        if (Console.CursorLeft > (iCol * 4))
                            Console.SetCursorPosition(6, Console.CursorTop);
                        else
                            Console.SetCursorPosition(Console.CursorLeft + 3, Console.CursorTop);

                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        break;
                    case 38: // up arrow
                        if (Console.CursorTop < 8)
                            Console.SetCursorPosition(Console.CursorLeft-1, (iLig * 3)+2);
                        else
                            Console.SetCursorPosition(Console.CursorLeft -1, Console.CursorTop - 3);

                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        break;
                    case 40: //down arrow
                        if(Console.CursorTop > iLig * 3)
                            Console.SetCursorPosition(Console.CursorLeft - 1, 5);
                        else
                         Console.SetCursorPosition(Console.CursorLeft -1, Console.CursorTop + 3);

                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                       
                        break;
                    case 13:
                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        boucle = false;
                        break;
                    case 82: // r pour règlement
                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        break;
                    case 70: // f pour controler avec fleches 
                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        boucle = false;
                        break;
                    case 67: // c pour coordonnées manuelles
                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        boucle = false;
                        break;
                    case 65: // a pour aciver l'intelligence artificiel
                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        boucle = false;
                        break;
                    default:
                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        Console.Clear();
                        DessinerGrille(iCol, iLig, tab, (short)Console.CursorLeft, (short)Console.CursorTop);
                        break;
                }
                Console.SetCursorPosition(42, iLig * 3 + 11);
                Console.Write("      ");
                Console.SetCursorPosition(42, iLig * 3 + 11);
                choix = positionActuelle[0] / 4 + " " + positionActuelle[1] / 3;
                Console.Write(choix);
                Console.SetCursorPosition(positionActuelle[0], positionActuelle[1]);
            }          
        }
    }
}