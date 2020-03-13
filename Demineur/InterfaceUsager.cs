using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace Demineur
{   

    public static class InterfaceUsager //static pour test
    {
        static string marge = "    "; // Unité d'espacement.
        static bool saisie = true; // true = mode avec flèche
        static int[] positionDeReponse, positionActuelle;
        static int positionDuGuide;

        static void DessinerTitreJeu(int colonne)
        {
            string titre = "Démineur 2020, année du JUGEMENT dernier.";
            int posTitre = (((colonne * 4) / 2) - (titre.Length / 2)) + 4; // Centrage automatique
            Console.SetCursorPosition(posTitre, 0);
            Console.WriteLine(titre + "\n");
        }

        static void DessinerInstructions(int offset)
        {           
            Console.SetCursorPosition(offset, 4);
            Console.Write("Appuyez sur f et entrer en mode manuel pour");
            Console.SetCursorPosition(offset, 5);
            Console.Write("retrouver le contrôle avec flèches.");
            Console.SetCursorPosition(offset, 7);
            Console.Write("Appuyez sur c pour entrer des coordonnées au clavier.");
            Console.SetCursorPosition(offset, 9);
            Console.Write("Appuyez sur Entrer pour confirmer votre sélection.");
        }

        static void DessinerModeDeSaisie(int offset){
            Console.SetCursorPosition(offset, 11);
            if(Saisie)
                Console.Write("Mode de jeu actif : Contrôle avec flèches");
            else
                Console.Write("Mode de jeu actif : Saisie manuelle      ");
        }

        public static void PositionnerCursorPourRepondre() {
            Console.SetCursorPosition(positionDeReponse[0], positionDeReponse[1]);
        }

        static void DessinerStats(int nColonne, string nomJoueur, int nbCouts)
        {
            int hauteurStats = 5;
            DessinerLigneDuHaut(nColonne);

            for (int h = 0; h < hauteurStats; h++) // Dessine ligne 
            {
                Console.Write(marge + "|");
                if (h == 4)
                    for (int c = 0; c < nColonne; c++)
                        Console.Write("___ ");
                else if (h == 2)    //Dessine ligne 3 de l'entete (Nom, yeux, nbCout)
                {
                    for (int c = 0; c < nColonne; c++)
                        if (c == 1)
                            Console.Write(nomJoueur);
                        else if (c == nColonne - 2)
                            Console.Write(nbCouts); // doit être formatté pour prendre un espace fixe.
                        else if (c == (nColonne / 2) - 1)
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

        public static void DessinerGrille(int nLigne, int nColonne, string grille)
        {
            Console.SetWindowSize(nColonne * 4 + 65, nLigne * 4 + 10);
            positionDeReponse = new int[2] {43, nLigne * 3 + 11};
            positionDuGuide = nColonne * 4 + 8;
            Console.Clear();
            
            DessinerTitreJeu(nColonne);
            DessinerInstructions(positionDuGuide);
            DessinerModeDeSaisie(positionDuGuide);

            Console.SetCursorPosition(0, 2); // Nécessaire pour dessiner la grille au bonne endroit.

            DessinerChiffreColonne(nColonne);
            DessinerLigneDuHaut(nColonne);

            for (int l = 0; l < nLigne; l++)
            {
                DessinerRangeHautCase(nColonne);
                DessinerRangeCentraleCase(l, nColonne, grille);
                DessinerRangeBasCase(nColonne);              
            }

            DessinerStats(nColonne, "marc", 3);

            Console.Write("\n"+ marge + "Quelle case souhaitez-vous ouvrir ? >> 1 1");          
        }
        
        static void DessinerChiffreColonne(int colonne){
            Console.Write(marge);
            for (int c = 0; c < colonne; c++)
            {
                if (c < 10) // spacing between top numbers
                    Console.Write(" ");
                Console.Write(" " + (c + 1) + " ");
            }
            Console.Write("\n");
        }

        static void DessinerLigneDuHaut(int colonne){
            Console.Write(marge);
            for (int y = 0; y < colonne; y++)
                Console.Write(" ___");
            Console.Write("\n");
        }

        static void DessinerRangeHautCase(int colonne){
            Console.Write(marge + "|   |");
            for (int c = 1; c < colonne; c++)
                Console.Write("   |"); //Ajoute ligne vertical de droite pour chaque autre colonne.
            Console.Write("\n");
        }

        static void DessinerRangeCentraleCase(int ligne, int colonne, string grille){

            if (ligne + 1 < 10)
                Console.Write(" " + (ligne + 1) + "  | "); // spacing de gauche pour les chiffres des rangés < 10
            else
                Console.Write(" " + (ligne + 1) + " | "); // spacing de gauche pour les chiffres des rangés >= 10
            for (int c = 0; c < colonne; c++)
                 Console.Write(grille[c + (ligne) * (colonne)] + " | "); // Contenue de tableau en test
            Console.Write("\n");
        }

        static void DessinerRangeBasCase(int colonne){
            Console.Write(marge + "|___|");
            for (int c = 1; c < colonne; c++)
                Console.Write("___|");
            Console.Write("\n");
        }

        static void DessinerRangeVide(int nColonne){
            for (int c = 0; c < nColonne; c++)
                Console.Write(marge);
        }

        public static bool Saisie {

            get { return saisie; }
            set { saisie = value; }
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