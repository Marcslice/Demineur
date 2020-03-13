using System;
using System.Collections.Generic;
using System.Text;

namespace Demineur
{
    public class Grille
    {
        Case[,] champs;
        int nbBombeGrille;
        int colonne, ligne;

        // arrays in c# rows, columns
        public Grille(short ligne, short colonne, short difficulte)
        {
            nbBombeGrille = CalculerBombes(ligne, colonne, difficulte);
            champs = new Case[ligne, colonne];
            this.ligne = ligne;
            this.colonne = colonne;
            
            for(int i = 0; i < ligne; i++)
            {
                for(int j = 0; j < colonne; j++)
                {
                    champs[i,j] = new Case();
                }
            }
            RencontreVoisin(ligne, colonne);
            DisperserBombes(ligne, colonne);
        }

        int CalculerBombes(short ligne, short colonne, short difficulte) {
            double pourcentage = 0;
            switch (difficulte) {
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
            return Convert.ToInt32((ligne * colonne) * pourcentage);
        }

        public void DisperserBombes(short ligne, short colonne)
        {
            Random random = new Random();

            Case destination;

            for (int k = nbBombeGrille; k > 0; k--)
            {
                destination = champs[random.Next(0, ligne), random.Next(0, colonne)];

                while(destination.Bombe)
                    destination = champs[random.Next(0, ligne), random.Next(0, colonne)];

                destination.Bombe = true;
            }
        }
        
        public Case this[int ligne, int colonne] {

            get { return champs[ligne, colonne]; }
            set {
                for (int l = ligne - 1; l < ligne + 1; l++)
                    for (int c = colonne - 1; c < colonne + 1; c++)
                        RencontreVoisin(l, c);
            }
        }

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

        public void OuvrirCase(int ligne, int colonne)
        {
            Case cible = champs[ligne, colonne];
            cible.Ouvert = true;
            cible.CalculerDanger();
        }

        public override string ToString()
        {
            string grille = "";

            for (int l = 0; l < ligne; l++)
            {
                for (int c = 0; c < colonne; c++)
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
    }
}
