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
        static int[] positionDeReponse, positionDeMessage;

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
            Console.Write("                                                    ");
            Console.SetCursorPosition(positionDeReponse[0], positionDeReponse[1]);
        }

        public static void PositionnerCursorPourMessageErreur()
        { 
            Console.SetCursorPosition(positionDeMessage[0], positionDeMessage[1]);
            Console.ForegroundColor = ConsoleColor.Red;
        }

        static void DessinerStats(int nColonne, string nomJoueur, int nbCouts)
        {

            DessinerLigneDuHaut(nColonne); // ligne horizontale dessus
            Console.Write(marge + "|");
            DessinerRangeVide(nColonne);  // rangé vide
            Console.WriteLine("\b|");

            Console.Write(marge + "|");
            DessinerRangeVide(nColonne); // rangé vide
            Console.WriteLine("\b|");

            Console.Write(marge + "|");
            for (int c = 0; c < nColonne; c++)
                if (c == 1)
                    Console.Write(nomJoueur);
                else if (c == nColonne - 2)
                    Console.Write(nbCouts); // doit être formatté pour prendre un espace fixe.
                else if (c == (nColonne / 2) - 1)
                    Console.Write("  :D   ");
                else
                    Console.Write(marge);
            Console.WriteLine("\b|");

            Console.Write(marge + "|");
            DessinerRangeVide(nColonne); // rangé vide
            Console.WriteLine("\b|");

            Console.Write(marge + "|");
            for (int c = 0; c < nColonne; c++) // ligne du bas
                Console.Write("___ ");
                Console.Write("\b|\n");
        
        }

        public static void DessinerPlateau(int nLigne, int nColonne, string grille, int[] positionActuelle)
        {
            Console.SetWindowSize(nColonne * 4 + 65, nLigne * 4 + 10);
            positionDeMessage = new int[2] {4, nLigne * 3 + 14};
            positionDeReponse = new int[2] {43, nLigne * 3 + 11};
            int positionDuGuide = nColonne * 4 + 8;
            Console.Clear();
            
            DessinerTitreJeu(nColonne);
            DessinerInstructions(positionDuGuide);
            DessinerModeDeSaisie(positionDuGuide);

            DessinerGrille(nLigne, nColonne, grille, positionActuelle);           
        }

        public static void DessinerGrille(int nLigne, int nColonne, string grille, int[] positionActuelle) {
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

            Console.Write("\n" + marge + "Quelle case souhaitez-vous ouvrir ? >> ");
            MettreAJourSelection(positionActuelle);
        }

        public static void MettreAJourSelection(int[] positionActuelle) {
            InterfaceUsager.PositionnerCursorPourRepondre();
            Console.Write(positionActuelle[0] / 4 + " " + positionActuelle[1] / 3 + "    ");
            Console.SetCursorPosition(positionActuelle[0], positionActuelle[1]);
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
            {
                if (grille[c + (ligne) * (colonne)] == '?')
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                else if (grille[c + (ligne) * (colonne)] == '¤')
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(grille[c + (ligne) * (colonne)]); // Contenue
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" | ");
            }
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

        public static void MessageVictoire()
        {
            PositionnerCursorPourMessageErreur();
            Console.WriteLine("Vous êtes un champion du démineur!");
            Console.ReadLine();
        }

        public static void MessageDefaite()
        {
            PositionnerCursorPourMessageErreur();
            Console.WriteLine("Tu as perdu et j'te juge.");
            Console.ReadLine();
            //Dessin du prof
        }

        public static void MessageCaseDejaOuverte() {
            PositionnerCursorPourMessageErreur();
            Console.WriteLine("Désolé cette case est déjà ouverte. Appuyez sur entrer pour continuer...");
            Console.ReadLine();
            PositionnerCursorPourMessageErreur();
            Console.WriteLine("                                                                        ");           
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void MessageFormatDentreeErronee()
        {
            PositionnerCursorPourMessageErreur();
            Console.WriteLine("Désolé veuillez vous assurez d'entrer vos coordonnées comme suit : (colonne ligne). \n" +
                marge + "Appuyez sur une entrer pour continuer.");            
            Console.ReadLine();
            PositionnerCursorPourMessageErreur();
            Console.WriteLine("                                                                                    \n" +
                "                                          ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void MessageHorsLimites()
        {
            PositionnerCursorPourMessageErreur();
            Console.WriteLine("Hé, ho tu ne vois pas les chiffres en haut et à gauche ? Appuie sur entrer la...");           
            Console.ReadLine();
            PositionnerCursorPourMessageErreur();
            Console.WriteLine("                                                                                ");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static string QuiEtesVous(){
            Console.Write("Qui êtes-vous ? >> ");
            return Console.ReadLine();
        }
    }
}