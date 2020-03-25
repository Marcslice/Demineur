using System;

namespace Demineur
{
    public class AITest
    {

        int nbLignes, nbColonnes;
        int[,] grille;
        string[,] grilleDeBombe;
        int[] meilleurCoup;
        bool nouvelleBombe;

        public AITest(int lignes, int colonnes)
        {
            nbLignes = lignes;
            nbColonnes = colonnes;
            grilleDeBombe = new string[lignes, colonnes];
            for (int l = 0; l < lignes; l++)
            {
                for (int c = 0; c < colonnes; c++)
                {
                    grilleDeBombe[l, c] = "o";
                }
            }
        }

        public int[] MeilleurCoup(string grilleDeJeu)
        {
            meilleurCoup = new int[3] { 0, 0, 0 };//ligne, colonne, valeurDanger
            GenererGrille(grilleDeJeu);

            if (meilleurCoup[2] == 420)
            {
                var rand = new Random();

                do
                {
                    meilleurCoup[0] = rand.Next() % nbLignes;
                    meilleurCoup[1] = rand.Next() % nbColonnes;
                } while (grille[meilleurCoup[0], meilleurCoup[1]] != 10);
            }

            return meilleurCoup;
        }

        void GenererGrille(string aConvertir)
        {
            int charTranscrit;

            grille = new int[nbLignes, nbColonnes];

            for (int l = 0; l < nbLignes; l++)
            {
                charTranscrit = nbColonnes * l;
                for (int c = 0; c < nbColonnes; c++)
                {
                    switch (aConvertir[charTranscrit + c])
                    {
                        case '?':
                            grille[l, c] = 10;// 10 equivaut a une casse n'etant pas encore ouverte dans grille
                            break;

                        case ' ':
                            grille[l, c] = 0;
                            break;

                        default:
                            grille[l, c] = ((int)aConvertir[charTranscrit + c] - 48);
                            break;
                    }
                }
            }
            ComparerGrille();
        }

        void ComparerGrille()
        {
            nouvelleBombe = false;
            meilleurCoup[2] = 420;

            for (int l = 0; l < nbLignes; l++)
            {
                for (int c = 0; c < nbColonnes; c++)
                {
                    if (grilleDeBombe[l, c] == "x")
                        grille[l, c] = 9;//9 equivaut a une bombe dans grille
                }
            }
            AnalyserGrille();
        }

        void AnalyserGrille()//cherche pour une case fermer
        {
            for (int l = 0; l < nbLignes; l++)
            {
                for (int c = 0; c < nbColonnes; c++)
                {
                    if (grille[l, c] == 10)
                        VoisinOuvert(l, c);
                }
            }
        }

        void VoisinOuvert(int coordL, int coordC)//coordonnee ligne, coordonnee colonne      cherche les case ouvertes autour non-bombe
        {

            if ((coordL > 0) && (coordC > 0))//NW
                if (grille[coordL - 1, coordC - 1] != 10 && grille[coordL - 1, coordC - 1] != 9)
                    if (CalculerDanger(coordL - 1, coordC - 1))
                        Meilleur(coordL, coordC);

            if (!nouvelleBombe)
                if (coordL > 0)//N
                    if (grille[coordL - 1, coordC] != 10 && grille[coordL - 1, coordC] != 9)
                        if (CalculerDanger(coordL - 1, coordC))
                            Meilleur(coordL, coordC);

            if (!nouvelleBombe)
                if ((coordL > 0) && (coordC + 1 < nbColonnes))//NE
                    if (grille[coordL - 1, coordC + 1] != 10 && grille[coordL - 1, coordC + 1] != 9)
                        if (CalculerDanger(coordL - 1, coordC + 1))
                            Meilleur(coordL, coordC);

            if (!nouvelleBombe)
                if (coordC > 0)//W
                    if (grille[coordL, coordC - 1] != 10 && grille[coordL, coordC - 1] != 9)
                        if (CalculerDanger(coordL, coordC - 1))
                            Meilleur(coordL, coordC);

            if (!nouvelleBombe)
                if (coordC + 1 < nbColonnes)//E
                    if (grille[coordL, coordC + 1] != 10 && grille[coordL, coordC + 1] != 9)
                        if (CalculerDanger(coordL, coordC + 1))
                            Meilleur(coordL, coordC);

            if (!nouvelleBombe)
                if ((coordL + 1 < nbLignes) && (coordC - 1 > 0))//SW
                    if (grille[coordL + 1, coordC - 1] != 10 && grille[coordL + 1, coordC - 1] != 9)
                        if (CalculerDanger(coordL + 1, coordC - 1))
                            Meilleur(coordL, coordC);

            if (!nouvelleBombe)
                if (coordL + 1 < nbLignes)//S
                    if (grille[coordL + 1, coordC] != 10 && grille[coordL + 1, coordC] != 9)
                        if (CalculerDanger(coordL + 1, coordC))
                            Meilleur(coordL, coordC);

            if (!nouvelleBombe)
                if ((coordL + 1 < nbLignes) && (coordC + 1 < nbColonnes))//SE
                    if (grille[coordL + 1, coordC + 1] != 10 && grille[coordL + 1, coordC + 1] != 9)
                        if (CalculerDanger(coordL + 1, coordC + 1))
                            Meilleur(coordL, coordC);

            if (nouvelleBombe)
            {
                grilleDeBombe[coordL, coordC] = "x";
                ComparerGrille();
            }
        }

        bool CalculerDanger(int coordL, int coordC)
        {
            int nbBombes = 0;
            int nbCaseFermer = 0;
            int score;

            if ((coordL > 0) && (coordC > 0))//NW
                if (grille[coordL - 1, coordC - 1] == 9)
                    nbBombes++;
                else if (grille[coordL - 1, coordC - 1] == 10)
                    nbCaseFermer++;

            if (coordL > 0)//N
                if (grille[coordL - 1, coordC] == 9)
                    nbBombes++;
                else if (grille[coordL - 1, coordC] == 10)
                    nbCaseFermer++;

            if ((coordL > 0) && (coordC + 1 < nbColonnes))//NE
                if (grille[coordL - 1, coordC + 1] == 9)
                    nbBombes++;
                else if (grille[coordL - 1, coordC + 1] == 10)
                    nbCaseFermer++;

            if (coordC > 0)//W
                if (grille[coordL, coordC - 1] == 9)
                    nbBombes++;
                else if (grille[coordL, coordC - 1] == 10)
                    nbCaseFermer++;

            if (coordC + 1 < nbColonnes)//E
                if (grille[coordL, coordC + 1] == 9)
                    nbBombes++;
                else if (grille[coordL, coordC + 1] == 10)
                    nbCaseFermer++;

            if ((coordL + 1 < nbLignes) && (coordC - 1 > 0))//SW
                if (grille[coordL + 1, coordC - 1] == 9)
                    nbBombes++;
                else if (grille[coordL + 1, coordC - 1] == 10)
                    nbCaseFermer++;

            if (coordL + 1 < nbLignes)//S
                if (grille[coordL + 1, coordC] == 9)
                    nbBombes++;
                else if (grille[coordL + 1, coordC] == 10)
                    nbCaseFermer++;

            if ((coordL + 1 < nbLignes) && (coordC + 1 < nbColonnes))//SE
                if (grille[coordL + 1, coordC + 1] == 9)
                    nbBombes++;
                else if (grille[coordL + 1, coordC + 1] == 10)
                    nbCaseFermer++;

            if (grille[coordL, coordC] != 0)
            {
                /*Console.WriteLine(grille[coordL, coordC] * 100);
                Console.WriteLine(((grille[coordL, coordC]) - nbBombes)*100);
                Console.WriteLine((((grille[coordL, coordC]) - nbBombes) * 100) / nbCaseFermer);*/

                
                if (nbCaseFermer != 0)
                {
                    score = (((grille[coordL, coordC] - nbBombes) * 100) / nbCaseFermer);


                    if (score == 100)
                        nouvelleBombe = true; //la case non decouvert est une bombe
                    else if (score < meilleurCoup[2])
                    {
                        meilleurCoup[2] = score;
                        return true; //changement
                    }
                }
            }
            return false;//ne pas tenir compte
        }

        void Meilleur(int ligne, int colonne)
        {
            meilleurCoup[0] = ligne;
            meilleurCoup[1] = colonne;
        }
    }
    //quand trouve 10 nbCaseFermer++         quand trouve 9 nbBombeATrouver --
    // changer alo pour garder valeur danger plus elever parmis info voisin puis comparer avec meilleurCoup[2]
}

