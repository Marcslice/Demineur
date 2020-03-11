using System;
using System.Collections.Generic;
using System.Text;

namespace Demineur
{
    public class Grille
    {
        Case[,] champs;
        short nbBombeGrille;

        public Grille(short colonne, short ligne, short nbBombe)
        {
            nbBombeGrille = nbBombe;
            champs = new Case[colonne, ligne];

            for(int i = 0; i < colonne; i++)
            {
                for(int j = 0; j < ligne; j++)
                {
                    new Case();
                }
            }

            RencontreVoisin(colonne,ligne);
            DisperserBombes(colonne, ligne);
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
                    
                    if ((i - 1 > 0) && (j - 1 > 0))//NW
                        destination[0] = champs[i - 1, j - 1];

                    if (j - 1 > 0)//N
                        destination[1] = champs[i, j - 1];

                    if ((i + 1 < colonne) && (j - 1 > 0))//NE
                        destination[2] = champs[i + 1, j - 1];

                    if (i - 1 > 0)//W
                        destination[3] = champs[i - 1, j];

                    if (i + 1 < colonne)//E
                        destination[4] = champs[i + 1, j];

                    if ((i - 1 > 0) && (j + 1 < ligne))//SW
                        destination[5] = champs[i - 1, j + 1];

                    if (j + 1 < ligne)//S
                        destination[6] = champs[i, j + 1];

                    if ((i + 1 < colonne) && (j + 1 < ligne))//SE
                        destination[7] = champs[i + 1, j + 1];
                }
            }
        }
    }
}
