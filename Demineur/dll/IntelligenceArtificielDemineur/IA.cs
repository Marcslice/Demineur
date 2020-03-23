using System;

namespace IntelligenceArtificielDemineur
{
    public class IA
    {

        int nbLignes, nbColonnes;
        int[,] grille;
        string[,] grilleDeBombe;
        float[] meilleurCoup;
        bool nouvelleBombe;

        public IA(int lignes, int colonnes/*, string aConvertir*/)
        {
            nbLignes = lignes;
            nbColonnes = colonnes;
            string[,] grilleDeBombe = new string[lignes, colonnes];
            for (int l = 0; l < lignes; l++)
            {
                for (int c = 0; c < colonnes; c++)
                {
                    grilleDeBombe[l, c] = null;
                }
            }
            //GenererGrille(aConvertir);
        }

        public float[] MeilleurCoup(string grilleDeJeu)
        {
            meilleurCoup = new float[3];//ligne, colonne, valeurDanger
            GenererGrille(grilleDeJeu);
            return meilleurCoup;
        }

        void GenererGrille(string aConvertir)
        {
            int charTranscrit;

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
            meilleurCoup[2] = 42;

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
            int l = 0;//ligne
            int c = 0;//colonne

            while (l < nbLignes)
            {
                while (c < nbColonnes)
                {
                    if (grille[l, c] == 10)
                    {
                        VoisinOuvert(l, c);
                    }
                    c++;
                }
                l++;
            }
        }

        void VoisinOuvert(int coordL, int coordC)//coordonnee ligne, coordonnee colonne      cherche les case ouvertes autour non-bombe
        {
            bool encours = true;
            while (!nouvelleBombe || encours)
            {
                if ((coordL > 0) && (coordC > 0))//NW
                    if (grille[coordL - 1, coordC - 1] != 10 && grille[coordL - 1, coordC - 1] != 9)
                        if (CalculerDanger(coordL - 1, coordC - 1))
                            Meilleur(coordL, coordC);

                if (coordL > 0)//N
                    if (grille[coordL - 1, coordC] != 10 && grille[coordL - 1, coordC] != 9)
                        if (CalculerDanger(coordL - 1, coordC))
                            Meilleur(coordL, coordC);

                if ((coordL > 0) && (coordC + 1 < nbColonnes))//NE
                    if (grille[coordL - 1, coordC + 1] != 10 && grille[coordL - 1, coordC + 1] != 9)
                        if (CalculerDanger(coordL - 1, coordC + 1))
                            Meilleur(coordL, coordC);

                if (coordC > 0)//W
                    if (grille[coordL, coordC - 1] != 10 && grille[coordL, coordC - 1] != 9)
                        if (CalculerDanger(coordL, coordC - 1))
                            Meilleur(coordL, coordC);

                if (coordC + 1 < nbColonnes)//E
                    if (grille[coordL, coordC + 1] != 10 && grille[coordL, coordC + 1] != 9)
                        if (CalculerDanger(coordL, coordC + 1))
                            Meilleur(coordL, coordC);

                if ((coordL + 1 < nbLignes) && (coordC - 1 > 0))//SW
                    if (grille[coordL + 1, coordC - 1] != 10 && grille[coordL + 1, coordC - 1] != 9)
                        if (CalculerDanger(coordL + 1, coordC - 1))
                            Meilleur(coordL, coordC);

                if (coordL + 1 < nbLignes)//S
                    if (grille[coordL + 1, coordC] != 10 && grille[coordL + 1, coordC] != 9)
                        if (CalculerDanger(coordL + 1, coordC))
                            Meilleur(coordL, coordC);

                if ((coordL + 1 < nbLignes) && (coordC + 1 < nbColonnes))//SE
                    if (grille[coordL + 1, coordC + 1] != 10 && grille[coordL + 1, coordC + 1] != 9)
                        if (CalculerDanger(coordL + 1, coordC + 1))
                            Meilleur(coordL, coordC);

                encours = false;
            }

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
            float score;

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

            score = (grille[coordL, coordC] - nbBombes) / nbCaseFermer;
            if (score == 1)
                nouvelleBombe = true;//la case non decouvert est une bombe
            else if (score < meilleurCoup[2])
            {
                meilleurCoup[2] = score;
                return true;//changement
            }
            return false;//ne pas tenir compte
        }

        void Meilleur(int ligne, int colonne)
        {
            meilleurCoup[0] = ligne;
            meilleurCoup[1] = colonne;
        }

        public void TourIA(string grille)
        {
            Console.WriteLine("l'ia a joué.");
        }
    }
}
