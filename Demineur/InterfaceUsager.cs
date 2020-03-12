using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace Demineur
{   

    public static class InterfaceUsager //static pour test
    {
        static string marge = "    "; // Unité d'espacement.
        static bool modeDeSaisie = true; // true = mode avec flèche
        static int[] positionDeReponse;
        static int positionDuGuide;

        static void DessinerTitreJeu(int col)
        {
            string titre = "Démineur 2020, année du JUGEMENT dernier.";
            int posTitre = (((col * 4) / 2) - (titre.Length / 2)) + 4; // Centrage automatique
            Console.SetCursorPosition(posTitre, 0);
            Console.WriteLine(titre + "\n");
        }

        static void DessinerInstructions(int offset)
        {           
            Console.SetCursorPosition(offset, 4);
            Console.Write("Appuyez sur f et entrer en mode manuel");
            Console.SetCursorPosition(offset, 5);
            Console.Write(" pour retrouver le contrôle avec flèches.");
            Console.SetCursorPosition(offset, 6);
            Console.Write("Appuyez sur c pour entrer des coordonnées au clavier.");
            Console.SetCursorPosition(offset, 7);
            Console.Write("Appuyez sur Entrer pour confirmer votre sélection.");
        }

        static void DessinerModeDeSaisie(int offset){
            Console.SetCursorPosition(offset, 10);
            if(modeDeSaisie)
                Console.Write("Mode de jeu actif : Contrôle avec flèches");
            else
                Console.Write("Mode de jeu actif : Saisie manuelle      ");
        }

        static void DessinerStats(int nColonne, string nomJoueur, int nbCouts)
        {

            DessinerLigneDuHaut(nColonne);

            for (int x = 0; x < 5; x++) // Dessine ligne 
            {
                Console.Write(marge + "|");
                if (x == 4)
                    for (int y = 0; y < nColonne; y++)
                        Console.Write("___ ");
                else if (x == 2)    //Dessine ligne 3 de l'entete (Nom, yeux, nbCout)
                {
                    for (int y = 0; y < nColonne; y++)
                        if (y == 1)
                            Console.Write(nomJoueur);
                        else if (y == nColonne - 2)
                            Console.Write(nbCouts); // doit être formatté pour prendre un espace fixe.
                        else if (y == (nColonne / 2) - 1)
                            Console.Write("  :D   ");
                        else
                            Console.Write(marge);
                }
                else
                    DessinerRangeVide(nColonne);

                Console.Write("\b|");
                Console.Write("\n");
            }
        }

        public static void DessinerGrille(int nColonne, int nRange, string grille, short posX, short posY)
        {
            Console.SetWindowSize(nColonne * 4 + 65, nRange * 4 + 10);
            positionDeReponse = new int[2] {43, nRange * 3 + 11};
            positionDuGuide = nColonne * 4 + 8;
            Console.Clear();
            
            DessinerTitreJeu(nColonne);
            DessinerInstructions(positionDuGuide);
            DessinerModeDeSaisie(positionDuGuide);

            Console.SetCursorPosition(0, 2); // Nécessaire pour dessiner la grille au bonne endroit.

            DessinerChiffreColonne(nColonne);
            DessinerLigneDuHaut(nColonne);

            for (int x = 0; x < nRange; x++)
            {
                DessinerRangeHautCase(nColonne);
                DessinerRangeCentraleCase(nColonne, x, grille);
                DessinerRangeBasCase(nColonne, nRange);              
            }

            DessinerStats(nColonne, "marc", 3);

            Console.Write("\n"+ marge + "Quelle case souhaitez-vous ouvrir ? >> 1 1");
            
            Cout(nColonne, nRange, grille); 
        }
        
        static void DessinerChiffreColonne(int col){
            Console.Write(marge);
            for (int y = 0; y < col; y++)
            {
                if (y < 10) // spacing between top numbers
                    Console.Write(" ");
                Console.Write(" " + (y + 1) + " ");
            }
            Console.Write("\n");
        }

        static void DessinerLigneDuHaut(int col){
            Console.Write(marge);
            for (int y = 0; y < col; y++)
                Console.Write(" ___");
            Console.Write("\n");
        }

        static void DessinerRangeHautCase(int col){
            Console.Write(marge + "|   |");
            for (int y = 1; y < col; y++)
                Console.Write("   |"); //Ajoute ligne vertical de droite pour chaque autre colonne.
            Console.Write("\n");
        }

        static void DessinerRangeCentraleCase(int col, int range, string grille){

            if (range + 1 < 10)
                Console.Write(" " + (range + 1) + "  | "); // spacing de gauche pour les chiffres des rangés < 10
            else
                Console.Write(" " + (range + 1) + " | "); // spacing de gauche pour les chiffres des rangés >= 10
            for (int y = 0; y < col; y++)
                Console.Write(grille[y * range+1] + " | "); // Contenue de tableau en test
            Console.Write("\n");
        }

        static void DessinerRangeBasCase(int col, int range){
            Console.Write(marge + "|___|");
            for (int y = 1; y < col; y++)
                Console.Write("___|");
            Console.Write("\n");
        }

        static void DessinerRangeVide(int nColonne){
            for (int y = 0; y < nColonne; y++)
                Console.Write(marge);
        }

        public static int[] Cout(int iCol, int iLig, string tab)
        {
            int[] positionActuelle = new int[2] {6, 5}; // Position au début de la partie.
            string entreeUtilisateur = "";
            ConsoleKeyInfo touche;                       // Info de la flèche appuyé.
 
            Console.SetCursorPosition(positionActuelle[0], positionActuelle[1]);
            do
            {
                
                touche = Console.ReadKey();

                /*
                    if(a)
                        AIPLAY();
                    else if(modeDeSaisie)
                        Detecte(c, up, down, right, left, enter)
                    else
                        Detecte(f, digits, spacebar)              
                */
                switch ((int)touche.Key)
                {
                    case 37: // left arrow
                    
                        if (Console.CursorLeft < 10)
                            Console.SetCursorPosition((iCol * 4) + 2, Console.CursorTop);
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
                            Console.SetCursorPosition(Console.CursorLeft - 1, (iLig * 3) + 2);
                        else
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop - 3);
                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        break;
                    case 40: // down arrow
                        if (Console.CursorTop > iLig * 3)
                            Console.SetCursorPosition(Console.CursorLeft - 1, 5);
                        else
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop + 3);
                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        break;
                    case 13: // enter key
                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        break;
                    case 70: // f pour controler avec fleches                                          
                        DessinerModeDeSaisie(iCol * 4 + 8);
                        modeDeSaisie = true;                             
                        break;
                    case 67: // c pour coordonnées manuelles    
                        DessinerModeDeSaisie(iCol * 4 + 8);
                        Console.SetCursorPosition(positionDeReponse[0], positionDeReponse[1]);
                        modeDeSaisie = false;
                        break;
                    case 65: // a pour aciver l'intelligence artificiel

                        break;
                    default: // autres touche non supportés
                        DessinerGrille(iCol, iLig, tab, (short)Console.CursorLeft, (short)Console.CursorTop);
                        break;
                }

                DessinerModeDeSaisie(iCol * 4 + 8);

                Console.SetCursorPosition(positionDeReponse[0], positionDeReponse[1]);

                if(modeDeSaisie){                   
                    Console.Write(positionActuelle[0] / 4 + " " + positionActuelle[1] / 3 + "    ");
                    Console.SetCursorPosition(positionActuelle[0], positionActuelle[1]);
                }else{
                    Console.Write("       ");
                    Console.SetCursorPosition(positionDeReponse[0], positionDeReponse[1]);
                    entreeUtilisateur = Console.ReadLine();
                    if(entreeUtilisateur == "f"){
                        Console.Clear();
                        modeDeSaisie = true;
                        DessinerGrille(iCol, iLig, tab, (short)Console.CursorLeft, (short)Console.CursorTop);     
                    }else;
                       // filtre data entreeUtilisateur                 
                }

            }while(touche.Key != ConsoleKey.Enter);
            return null; // filtered data
        }
        static void MessageVictoire()
        {
            Console.Clear();
            Console.WriteLine("Vous êtes un champion du démineur!");
        }
        static void MessageDefaite()
        {
            Console.Clear();
            Console.WriteLine("Je te juge.");
            //Dessin du prof
        }

        static string QuiEtesVous(){
            Console.Write("Qui êtes-vous ? >> ");
            return Console.ReadLine();
        }
    }
}