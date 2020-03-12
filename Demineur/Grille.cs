using System;
using System.Collections.Generic;
using System.Text;

namespace Demineur
{
    public class Grille
    {
        Case[,] champs;
        int nbBombeGrille;

        public Grille(short colonne, short ligne, short difficulte)
        {
            nbBombeGrille = CalculerBombes(colonne, ligne, difficulte);
            champs = new Case[colonne, ligne];

            for(int i = 0; i < colonne; i++)
            {
                for(int j = 0; j < ligne; j++)
                {
                    champs[i,j] = new Case();
                }
            }

            RencontreVoisin(colonne,ligne);
            DisperserBombes(colonne, ligne);
        }

        int CalculerBombes(short colonne, short ligne, short difficulte) {
            double pourcentage = 0;
            switch (difficulte) {
                case 1:
                    pourcentage = 0.1;
                    break;
                case 2:
                    pourcentage = 0.2;
                    break;
                case 3:
                    pourcentage = 0.3;
                    break;
            }
            return Convert.ToInt32((colonne * ligne) * pourcentage);
        }

        public void DisperserBombes(short colonne, short ligne)
        {
            Random random = new Random();

            Case destination;

            for (int k = nbBombeGrille; k > 0; k--)
            {
                destination = champs[random.Next(0, colonne), random.Next(0, ligne)];

                while(destination.Bombe)
                    destination = champs[random.Next(0, colonne), random.Next(0, ligne)];

                destination.Bombe = true;
            }
        }

        public void RencontreVoisin(short colonne, short ligne)
        {
            for (int i = 0; i < colonne; i++)
            {
                for (int j = 0; j < ligne; j++)
                {
                    Case destination = champs[i, j];
                    Case voisin = champs[i,j]; // initialisation obligatoire.

                    if ((i - 1 > 0) && (j - 1 > 0))//NW
                        destination.SetCase(0, voisin = champs[i - 1, j - 1]);

                    if (j - 1 > 0)//N
                        destination.SetCase(1, voisin = champs[i, j - 1]);

                    if ((i + 1 < colonne) && (j - 1 > 0))//NE
                        destination.SetCase(2, voisin = champs[i + 1, j - 1]);

                    if (i - 1 > 0)//W
                        destination.SetCase(3, voisin = champs[i - 1, j]);

                    if (i + 1 < colonne)//E
                        destination.SetCase(4, voisin = champs[i + 1, j]);

                    if ((i - 1 > 0) && (j + 1 < ligne))//SW
                        destination.SetCase(5, voisin = champs[i - 1, j + 1]);

                    if (j + 1 < ligne)//S
                        destination.SetCase(6, voisin = champs[i, j + 1]);

                    if ((i + 1 < colonne) && (j + 1 < ligne))//SE
                        destination.SetCase(7, voisin = champs[i + 1, j + 1]);

                    if (voisin.Bombe)
                        destination.Value++;
                }
            }
        }

        public override string ToString()
        {
            string grille = "";
            foreach (Case c in champs) {
                if (!c.estTuOuverte())
                    grille += '?';
                else if (c.Value == 0)
                    grille += ' ';
                else
                    grille += c.Value;
            }
            return grille;
        }
    }
}
