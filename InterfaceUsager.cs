using System;

namespace Demineur
{
    /// <summary>
    /// Dessine l'interface usagé
    /// Titre
    /// Grille
    /// Statistiques
    /// Instructions
    /// </summary>
    public static class InterfaceUsager //static pour test
    {
        static string marge = "    "; // Unité d'espacement.
        static bool saisie = true; // true = mode avec flèche
        static int[] positionDeReponse, positionDeMessage;
        static int positionDuGuide;

        /// <summary>
        /// Détecte le mode de saisie en cours.
        /// </summary>
        public static bool Saisie
        {
            get { return saisie; }
            set { saisie = value; }
        }

        /// <summary>
        /// Affiche le titre du jeu.
        /// </summary>
        /// <param name="colonne"></param>
        static void DessinerTitreJeu(int colonne)
        {
            string titre = "Démineur 2020, année du JUGEMENT dernier.";
            int posTitre = (((colonne * 4) / 2) - (titre.Length / 2)) + 4; // Centrage automatique
            Console.SetCursorPosition(posTitre, 0);
            Console.WriteLine(titre + "\n");
        }

        /// <summary>
        /// Affiche les instructions
        /// </summary>
        static void DessinerInstructions()
        {
            Console.SetCursorPosition(positionDuGuide, 4);
            Console.Write("Appuyez sur f et entrer en mode manuel pour");
            Console.SetCursorPosition(positionDuGuide, 5);
            Console.Write("retrouver le contrôle avec flèches.");
            Console.SetCursorPosition(positionDuGuide, 7);
            Console.Write("Appuyez sur c pour entrer des coordonnées au clavier.");
            Console.SetCursorPosition(positionDuGuide, 9);
            Console.Write("Appuyez sur a(en mode fleche) ou a+Enter(en mode manuel)");
            Console.SetCursorPosition(positionDuGuide, 10);
            Console.Write("pour appeler l'intelligence artificiel.");
            Console.SetCursorPosition(positionDuGuide, 12);
            Console.Write("*** Ne fonctionne que si la partie a été créé avec un IA.");
            Console.SetCursorPosition(positionDuGuide, 14);
            Console.Write("Appuyez sur Entrer pour confirmer votre sélection.");
        }

        /// <summary>
        /// Affiche le mode de saisie actuel.
        /// </summary>
        public static void DessinerModeDeSaisie()
        {
            Console.SetCursorPosition(positionDuGuide, 16);
            if (Saisie)
                Console.Write("Mode de jeu actif : Contrôle avec flèches");
            else
                Console.Write("Mode de jeu actif : Saisie manuelle      ");
        }

        /// <summary>
        /// Positionne le cursor lorsqu'on doit mettre à jour l'affichage de la sélection(en bas à droite).
        /// </summary>
        public static void PositionnerCursorPourRepondre()
        {
            Console.SetCursorPosition(positionDeReponse[0], positionDeReponse[1]);
            Console.Write("                                                    ");
            Console.SetCursorPosition(positionDeReponse[0], positionDeReponse[1]);
        }

        /// <summary>
        /// Positionne le cursor et change la couleur du texte pour afficher l'erreur.
        /// </summary>
        public static void PositionnerCursorPourMessageErreur()
        {
            Console.SetCursorPosition(positionDeMessage[0], positionDeMessage[1]);
            Console.ForegroundColor = ConsoleColor.Red;
        }

        /// <summary>
        /// Dessine le rectangle des statistiques.
        /// </summary>
        /// <param name="nColonne">Nombre de colonnes de la grille.</param>
        /// <param name="nomJoueur">Nom du joueur.</param>
        /// /// <param name="mort">Joueur est il mort</param>
        /// <param name="nbBombes">Nom de bombes dans la grille.</param>
        static void DessinerStats(int nColonne, string nomJoueur, bool mort, int nbBombes)
        {
            string etat = "  :D  ";
            bool dessiner = false;
            if (mort)
                etat = "  X(  ";

            DessinerLigneDuHaut(nColonne); // ligne horizontale dessus
            Console.Write(marge + "|");
            DessinerRangeVide(nColonne);  // rangé vide
            Console.WriteLine("\b|");

            Console.Write(marge + "|");
            DessinerRangeVide(nColonne); // rangé vide
            Console.WriteLine("\b|");

            Console.Write(marge + "|");
            for (int c = 0; c < nColonne * 4; c++)
                if (c == 3)
                {
                    Console.Write(nomJoueur);
                    c += nomJoueur.Length;
                }
                else if (c > nColonne * 4 - 5 && !dessiner)
                {
                    Console.Write(nbBombes);
                    c += Convert.ToString(nbBombes).Length;
                    dessiner = true;
                }
                else if (c == (((nColonne * 4) / 2) - 2)) {
                    Console.Write(etat);
                    c += etat.Length;
                }
                else
                    Console.Write(" ");
            Console.WriteLine("  |");

            Console.Write(marge + "|");
            DessinerRangeVide(nColonne); // rangé vide
            Console.WriteLine("\b|");

            Console.Write(marge + "|");
            for (int c = 0; c < nColonne; c++) // ligne du bas
                Console.Write("___ ");
            Console.Write("\b|\n");

        }

        /// <summary>
        /// Dessine tout le plateau.
        /// Est appelé une fois au premier tour.
        /// Ajuste la taille de la fenêtre de la console.
        /// Calcule la position des modules de l'interface {titre, instruction, grille, statistique}
        /// </summary>
        /// <param name="p_NomJoueur">Nom du joueur</param>
        /// <param name="nLigne">Nombre de lignes de la grille</param>
        /// <param name="nColonne">Nombre de colonnes de la grille</param>
        /// <param name="grille">Grille en string</param>
        /// <param name="positionActuelle">Sélection du joueur.</param>
        /// <param name="nbBombes">Nombre de bombes dans la grille.</param>
        public static void DessinerPlateau(string p_NomJoueur, int nLigne, int nColonne, string grille, int[] positionActuelle, int nbBombes, bool mort)
        {
            Console.SetWindowSize(nColonne * 4 + 65, nLigne * 4 + 14);
            positionDeMessage = new int[2] { 4, nLigne * 3 + 14 };
            positionDeReponse = new int[2] { 43, nLigne * 3 + 11 };
            positionDuGuide = nColonne * 4 + 8; //instructions
            Console.Clear();

            DessinerTitreJeu(nColonne);
            DessinerInstructions();
            DessinerModeDeSaisie();

            DessinerGrille(p_NomJoueur,nLigne, nColonne, grille, positionActuelle,nbBombes, mort);
        }

        /// <summary>
        /// Dessine la grille de l'interface graphique.
        /// Elle appel ce qu'elle a besoin.
        /// </summary>
        /// <param name="p_NomJoueur">Nom du joueur</param>
        /// <param name="nLigne">Nombre de lignes de la grille</param>
        /// <param name="nColonne">Nombre de colonnes de la grille</param>
        /// <param name="grille">Grille en string</param>
        /// <param name="positionActuelle">Sélection du joueur.</param>
        /// <param name="nbBombes">Nombre de bombes dans la grille.</param>
        public static void DessinerGrille(string p_NomJoueur, int nLigne, int nColonne, string grille, int[] positionActuelle, int nbBombes, bool mort)
        {
            Console.SetCursorPosition(0, 2); // Nécessaire pour dessiner la grille au bonne endroit.

            DessinerChiffreColonne(nColonne);
            DessinerLigneDuHaut(nColonne);

            for (int l = 0; l < nLigne; l++)
            {
                DessinerRangeHautCase(nColonne);
                DessinerRangeCentraleCase(l, nColonne, grille);
                DessinerRangeBasCase(nColonne);
            }

            DessinerStats(nColonne, p_NomJoueur, mort, nbBombes);

            Console.Write("\n" + marge + "Quelle case souhaitez-vous ouvrir ? >> ");
            MettreAJourSelection(positionActuelle);
        }

        /// <summary>
        /// Lors de la navigation par flèche ou de l'utilisation de l'IA,
        /// cela met à jour les coordonnées en bas à droite. 
        /// </summary>
        /// <param name="positionActuelle"></param>
        public static void MettreAJourSelection(int[] positionActuelle)
        {
            InterfaceUsager.PositionnerCursorPourRepondre();
            Console.Write(positionActuelle[0] / 4 + " " + positionActuelle[1] / 3 + "    ");
            Console.SetCursorPosition(positionActuelle[0], positionActuelle[1]);
        }

        /// <summary>
        /// Dessine l'entête des colonnes de la grille de l'interface graphique.
        /// </summary>
        /// <param name="colonne">Nombre de colonnes de la grille.</param>
        static void DessinerChiffreColonne(int colonne)
        {
            Console.Write(marge);
            for (int c = 0; c < colonne; c++)
            {
                if (c < 10) // Espacement entre les entêtes de colonnes.
                    Console.Write(" ");
                Console.Write(" " + (c + 1) + " ");
            }
            Console.Write("\n");
        }

        /// <summary>
        /// Dessine la ligne horizontal du haut de la grille de l'interface graphique.
        /// </summary>
        /// <param name="colonne"></param>
        static void DessinerLigneDuHaut(int colonne)
        {
            Console.Write(marge);
            for (int y = 0; y < colonne; y++)
                Console.Write(" ___");
            Console.Write("\n");
        }

        // <summary>
        /// Dessine la rangé du haut de la grille de l'interface graphique.
        /// </summary>
        static void DessinerRangeHautCase(int colonne)
        {
            Console.Write(marge + "|   |");
            for (int c = 1; c < colonne; c++)
                Console.Write("   |"); //Ajoute ligne vertical de droite pour chaque autre colonne.
            Console.Write("\n");
        }

        /// <summary>
        /// Dessine la rangé du milieu de la grille de l'interface graphique.
        /// Elle affiche les données des cases.
        /// </summary>
        /// <param name="ligne">Nombre de lignes de la grille.</param>
        /// <param name="colonne">Nombre de colonnes de la grille.</param>
        /// <param name="grille">La grille en string.</param>
        static void DessinerRangeCentraleCase(int ligne, int colonne, string grille)
        {

            if (ligne + 1 < 10)
                Console.Write(" " + (ligne + 1) + "  | "); // espacement de gauche pour les chiffres des rangés < 10
            else
                Console.Write(" " + (ligne + 1) + " | "); // espacement de gauche pour les chiffres des rangés >= 10
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

        /// <summary>
        /// Dessine le bas des cases dans la grille de l'interface graphique.
        /// </summary>
        /// <param name="colonne"></param>
        static void DessinerRangeBasCase(int colonne)
        {
            Console.Write(marge + "|___|");
            for (int c = 1; c < colonne; c++)
                Console.Write("___|");
            Console.Write("\n");
        }

        /// <summary>
        /// Dessine une rangé vide dans le tableau des statistiques de partie.
        /// </summary>
        /// <param name="nColonne">Nombre de colonnes de la grille.</param>
        static void DessinerRangeVide(int nColonne)
        {
            for (int c = 0; c < nColonne; c++)
                Console.Write(marge);
        }

        /// <summary>
        /// Affiche le message de victoire.
        /// </summary>
        public static void MessageVictoire()
        {
            PositionnerCursorPourMessageErreur();
            Console.WriteLine("Vous êtes un champion du démineur!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
        }

        public static void MessageNouveauRecord() {
            PositionnerCursorPourMessageErreur();
            Console.WriteLine("C'est un nouveau record! Félicitation.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
        }

        /// <summary>
        /// Affiche le message de Défaite.
        /// </summary>
        public static void MessageDefaite()
        {
            PositionnerCursorPourMessageErreur();
            Console.WriteLine("Tu as perdu et j'te juge.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
            //Dessin du prof
        }

        /// <summary>
        /// Affiche le message de case déjà ouverte.
        /// </summary>
        public static void MessageCaseDejaOuverte()
        {
            PositionnerCursorPourMessageErreur();
            Console.WriteLine("Désolé cette case est déjà ouverte. Appuyez sur entrer pour continuer...");
            Console.ReadLine();
            PositionnerCursorPourMessageErreur();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("                                                                        ");
        }

        /// <summary>
        /// Affiche le message de format incorrecte si la siasie de respecte pas ^\d+\s\d+$.
        /// </summary>
        public static void MessageFormatDentreeErronee()
        {
            PositionnerCursorPourMessageErreur();
            Console.WriteLine("Désolé veuillez vous assurez d'entrer vos coordonnées comme suit : (colonne ligne). \n" +
                marge + "Appuyez sur une touche pour continuer.");
            Console.ReadLine();
            PositionnerCursorPourMessageErreur();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("                                                                                    \n" +
                "                                          ");
        }

        /// <summary>
        /// Affiche le message de Saisie hors limite si la saisie ne fait pas partie de la grille.
        /// </summary>
        public static void MessageHorsLimites()
        {
            PositionnerCursorPourMessageErreur();
            Console.WriteLine("Hé, ho tu ne vois pas les chiffres en haut et à gauche ? Appuie sur entrer la...");
            Console.ReadLine();
            PositionnerCursorPourMessageErreur();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("                                                                                ");
        }

        /// <summary>
        /// Affiche le message intelligence inactif si le joueur a choisi une partie sans IA.
        /// </summary>
        public static void MessageIAInactif()
        {
            PositionnerCursorPourMessageErreur();
            Console.WriteLine("L'intelligence artificiel n'est pas active.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
            PositionnerCursorPourMessageErreur();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("                                                                                ");
        }
    }
}