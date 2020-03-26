using System;
using System.Collections.Generic;

namespace Demineur
{
    /// <summary>
    /// Grille représente le plateau de jeu.
    /// Elle contient des cases de type Case.
    /// Case[,] : champs est le tableau 2D [Lignes x Colonnes]
    /// Int : nbBombeGrille contient le nombre de bombe que la grille contient.
    /// Int : colonnes Dimension X du tableau 
    /// Int : lignes Dimension Y du table
    /// Int : casesFermer contient le nombre de base fermer en début de partie.
    /// Stack<Case> : aOuvrir Permet la recherche récursive pour l'ouverture automatique des cases dont le danger est de 0.
    /// </summary>
    public class Grille
    {
        Case[,] champs;
        int nbBombeGrille, colonnes, lignes, casesFermer;
        Stack<Case> aOuvrir;

        // arrays in c# rows, columns
        public Grille(short lignes, short colonnes, short difficulte)
        {
            nbBombeGrille = CalculerBombes(lignes, colonnes, difficulte);
            champs = new Case[lignes, colonnes];
            this.lignes = lignes;
            this.colonnes = colonnes;
            this.casesFermer = lignes * colonnes;

            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    champs[i, j] = new Case();
                }
            }
            RencontreVoisin(lignes, colonnes); // à mettre dans case ?
            DisperserBombes(lignes, colonnes);
        }

        /// <summary>
        /// Détermine le nombre de bombe a générer selon la difficulté. Le calcule se fait en pourcentage de cases.
        /// </summary>
        /// <param name="lignes">Nombre de lignes qu'il y a dans la grille</param>
        /// <param name="colonnes">Nombre de colonnes qu'il y a dans la grille</param>
        /// <param name="difficulte">Difficulté {1,2,3}</param>
        /// <returns>Int : Nombre de bombes</returns>
        int CalculerBombes(short lignes, short colonnes, short difficulte)
        {
            double pourcentage = 0;
            switch (difficulte)
            {
                case 1:
                    pourcentage = 0.2;
                    break;
                case 2:
                    pourcentage = 0.4;
                    break;
                case 3:
                    pourcentage = 0.6;
                    break;
            }
            return Convert.ToInt32((lignes * colonnes) * pourcentage);
        }

        /// <summary>
        /// Disperse les bombes dans la grille en mettant le boolean estTuBombe à true sur les cases ciblés.
        /// </summary>
        /// <param name="lignes">Nombre de lignes dans la grille de jeu.</param>
        /// <param name="colonnes">Nombre de colonnes dans la grille de jeu.</param>
        public void DisperserBombes(short lignes, short colonnes)
        {
            Random random = new Random();

            Case destination;

            for (int k = nbBombeGrille; k > 0; k--)
            {
                do
                    destination = champs[random.Next(0, lignes), random.Next(0, colonnes)];
                while (destination.Bombe);

                destination.Bombe = true;
            }
        }

        /// <summary>
        /// Rempli le tableau des voisins d'une case.
        /// </summary>
        /// <param name="ligne">Coordonnée en Y</param>
        /// <param name="colonne">Coordonnée en X</param>
        /// <returns>Case</returns>
        public Case this[int ligne, int colonne] // à mettre dans case ? Potentiel bug
        {
            get { return champs[ligne, colonne]; }
            set
            {
                for (int l = ligne - 1; l < ligne + 1; l++) // -1 ?
                    for (int c = colonne - 1; c < colonne + 1; c++)
                        RencontreVoisin(l, c);
            }
        }

        /// <summary>
        /// Fouille les case adjacentes à la case active pour remplir le tableau des cases voisines.
        /// </summary>
        /// <param name="ligne">Ligne en cours de traitement</param>
        /// <param name="colonne">Colonne en cours de traitement</param>
        public void RencontreVoisin(int ligne, int colonne)
        {
            for (int l = 0; l < ligne; l++)
            {
                for (int c = 0; c < colonne; c++)
                {
                    Case destination = champs[l, c];
                    Case voisin;

                    if ((l > 0) && (c > 0))//NW
                        destination.SetCase(0, voisin = champs[l - 1, c - 1]);

                    if (l > 0)//N
                        destination.SetCase(1, voisin = champs[l - 1, c]);

                    if ((l > 0) && (c + 1 < colonne))//NE
                        destination.SetCase(2, voisin = champs[l - 1, c + 1]);

                    if (c > 0)//W
                        destination.SetCase(3, voisin = champs[l, c - 1]);

                    if (c + 1 < colonne)//E
                        destination.SetCase(4, voisin = champs[l, c + 1]);

                    if ((l + 1 < ligne) && (c - 1 > 0))//SW
                        destination.SetCase(5, voisin = champs[l + 1, c - 1]);

                    if (l + 1 < ligne)//S
                        destination.SetCase(6, voisin = champs[l + 1, c]);

                    if ((l + 1 < ligne) && (c + 1 < colonne))//SE
                        destination.SetCase(7, voisin = champs[l + 1, c + 1]);
                }
            }
        }

        /// <summary>
        /// Utilisé seulement lorsque la première case ouverte est une bombe.
        /// </summary>
        /// <param name="ligne">Ligne en cours de traitement</param>
        /// <param name="colonne">Colonne en cours de traitement</param>
        /// <param name="l">Y de la bombe à neutraliser</param>
        /// <param name="c">X de la bombe à neutraliser</param>
        public void MettreAJourVoisin(int ligne, int colonne, int l, int c)
        {

            Case destination = champs[l, c];
            Case voisin;

            if ((l > 0) && (c > 0))//NW
                destination.SetCase(0, voisin = champs[l - 1, c - 1]);

            if (l > 0)//N
                destination.SetCase(1, voisin = champs[l - 1, c]);

            if ((l > 0) && (c + 1 < colonne))//NE
                destination.SetCase(2, voisin = champs[l - 1, c + 1]);

            if (c > 0)//W
                destination.SetCase(3, voisin = champs[l, c - 1]);

            if (c + 1 < colonne)//E
                destination.SetCase(4, voisin = champs[l, c + 1]);

            if ((l + 1 < ligne) && (c - 1 > 0))//SW
                destination.SetCase(5, voisin = champs[l + 1, c - 1]);

            if (l + 1 < ligne)//S
                destination.SetCase(6, voisin = champs[l + 1, c]);

            if ((l + 1 < ligne) && (c + 1 < colonne))//SE
                destination.SetCase(7, voisin = champs[l + 1, c + 1]);

        }

        /// <summary>
        /// Ouvre une case suite à la confirmation du joueur.
        /// </summary>
        /// <param name="ligne">Coordonnée Y de la case à ouvrir.</param>
        /// <param name="colonne">Coordonnée X de la case à ouvrir.</param>
        /// <returns>Bool : Retourne faux si la case est une bombe, retourne vrai si elle n'est pas une bombe.</returns>
        public bool OuvrirCase(int ligne, int colonne) //étape 1 ouvre case, si bombe arrete sinon appel OuvrirCase étape 2
        {
            aOuvrir = new Stack<Case>();

            Case cible = champs[ligne, colonne];//retour maintenant un bool pour le gameover
            cible.Ouvert = true;
            if (cible.Bombe)
                return false;
            if (cible.CalculerDanger() == 0)
                for (int i = 0; i < 8; i++)
                    if (cible[i] != null)
                        OuvrirCase(cible[i]);
            return true;
        }

        /// <summary>
        /// Ouvre automatiquement les cases voisines de la sélection
        /// </summary>
        /// <param name="voisin">Voisin de la case sélectionné.</param>
        /// <returns>Retourne vrai si ?</returns>
        bool OuvrirCase(Case voisin) // Ouvre les case vides adjacentes. À vérifier
        {
            if (!voisin.Ouvert)
            {
                voisin.Ouvert = true;
                if (voisin.CalculerDanger() == 0)
                    aOuvrir.Push(voisin);

                if (voisin.CalculerDanger() == 0)
                    for (int j = 0; j < 8; j++)
                        if (voisin[j] != null)
                            OuvrirCase(voisin[j]);
            }
            return true;
        }

        /// <summary>
        /// Neutralise la case si la première case est une bombe.
        /// </summary>
        /// <param name="cible">Case sélectionnée.</param>
        public void BombePremierTour(int[] cible)
        {
            this[cible[1], cible[0]].Bombe = false;
            this[cible[1], cible[0]].CalculerDanger();
            nbBombeGrille--;
            MettreAJourVoisin(this.lignes, this.colonnes, cible[1], cible[0]);
        }

        /// <summary>
        /// Ouvre les bombes lorsque le joueur explose.
        /// </summary>
        public void DecouvrirBombes()
        {
            for (int l = 0; l < lignes; l++)
            {
                for (int c = 0; c < colonnes; c++)
                    if (champs[l, c].Bombe)
                        champs[l, c].Ouvert = true;
            }
        }

        /// <summary>
        /// Retourne le nombre de colonnes de la grille.
        /// </summary>
        /// <returns>Int : Colonnes</returns>
        public int Colonnes() { return colonnes; }

        /// <summary>
        /// Retourne le nombre de Lignes de la grille.
        /// </summary>
        /// <returns>Int : Lignes</returns>
        public int Lignes() { return lignes; }

        /// <summary>
        /// Retourne ou modifie le nombre de bombes dans la grille.
        /// Utile au premier tour.
        /// </summary>
        public int NombreDeBombes
        {
            get { return nbBombeGrille; }
            set { nbBombeGrille = value; }
        }

        /// <summary>
        /// Retourne la grille sous forme de string.
        /// '?' signigie case fermer
        /// '¤' signifie bombe
        /// ' ' signigie Ouverte et danger 0
        /// </summary>
        /// <returns>string : grille</returns>
        public override string ToString()
        {
            string grille = "";

            for (int l = 0; l < lignes; l++)
            {
                for (int c = 0; c < colonnes; c++)
                {
                    if (!this[l, c].Ouvert)
                        grille += '?';
                    else if (this[l, c].Ouvert && this[l, c].Bombe)
                        grille += '¤';
                    else if (this[l, c].Ouvert && this[l, c].Value == 0)
                        grille += ' ';
                    else if (this[l, c].Ouvert && this[l, c].Value > 0)
                        grille += this[l, c].Value;
                }
            }
            return grille;
        }

        /// <summary>
        /// Calcule le nombre de case fermer.
        /// Permet de mettre à jour le compte pour
        /// déterminer si la partie est gagné.
        /// </summary>
        /// <returns></returns>
        public int CalculerNbCaseFermer()
        {
            casesFermer = lignes * colonnes;
            for (int l = 0; l < lignes; l++)
            {
                for (int c = 0; c < colonnes; c++)
                    if (this[l, c].Ouvert)
                        casesFermer--;
            }
            return casesFermer;
        }
    }
}

