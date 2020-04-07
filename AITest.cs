using System;
using System.Collections.Generic;

namespace Demineur
{
    public class AITest
    {

        int nbLignes, nbColonnes, score;
        int[,] grille;
        int[] meilleurCoup;
        bool nouvelleBombe;

        public AITest(int lignes, int colonnes)
        {
            nbLignes = lignes;
            nbColonnes = colonnes;
            grille = new int[lignes, colonnes];
            for (int l = 0; l < lignes; l++)
            {
                for (int c = 0; c < colonnes; c++)
                {
                    grille[l, c] = 10;
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

            for (int l = 0; l < nbLignes; l++)
            {
                charTranscrit = nbColonnes * l;
                for (int c = 0; c < nbColonnes; c++)
                {
                    if (aConvertir[charTranscrit + c] != '?')
                        switch (aConvertir[charTranscrit + c])
                        {
                            case ' ':
                                grille[l, c] = 0;
                                break;

                            default:
                                grille[l, c] = ((int)aConvertir[charTranscrit + c] - 48);
                                break;
                        }
                }
            }
            AnalyserGrille();
        }

        void AnalyserGrille()//cherche pour une case fermer
        {
            nouvelleBombe = false;
            meilleurCoup[2] = 420;
            int nbCaseFermer = 0;

            for (int l = 0; l < nbLignes; l++)
            {
                for (int c = 0; c < nbColonnes; c++)
                {
                    if (grille[l, c] < 9)
                    {
                        AnalyseCaseO(l, c);
                    }
                    if (grille[l, c] == 10)
                    {
                        nbCaseFermer++;
                        VoisinOuvert(l, c);
                    }
                }
            }
            if (nbCaseFermer == 0)
            {
                ResetCaseBombes();
            }
        }

        void ResetCaseBombes()
        {
            for (int l = 0; l < nbLignes; l++)
            {
                for (int c = 0; c < nbColonnes; c++)
                {
                    if (grille[l, c] == 9)
                    {
                        grille[l, c] = 10;
                    }
                }
            }
            AnalyserGrille();
        }

        void AnalyseCaseO(int coordL, int coordC)
        {
            int caseF = 0;
            int bombe = 0;
            int valVoisin;
            Stack<int> caseFermer;

            caseFermer = new Stack<int>();

            if ((coordL > 0) && (coordC > 0))//NW
            {
                valVoisin = grille[coordL - 1, coordC - 1];

                if (valVoisin > 8)
                {
                    if (valVoisin == 9)
                    {
                        bombe++;
                    }
                    else
                    {
                        caseFermer.Push(coordC - 1);
                        caseFermer.Push(coordL - 1);//dessu
                        caseF++;
                    }
                }
            }

            if (coordL > 0)//N
            {
                valVoisin = grille[coordL - 1, coordC];

                if (valVoisin > 8)
                {
                    if (valVoisin == 9)
                    {
                        bombe++;
                    }
                    else
                    {
                        caseFermer.Push(coordC);
                        caseFermer.Push(coordL - 1);//dessu
                        caseF++;
                    }
                }
            }

            if ((coordL > 0) && (coordC + 1 < nbColonnes))//NE
            {
                valVoisin = grille[coordL - 1, coordC + 1];

                if (valVoisin > 8)
                {
                    if (valVoisin == 9)
                    {
                        bombe++;
                    }
                    else
                    {
                        caseFermer.Push(coordC + 1);
                        caseFermer.Push(coordL - 1);//dessu
                        caseF++;
                    }
                }
            }

            if (coordC > 0)//W
            {
                valVoisin = grille[coordL, coordC - 1];

                if (valVoisin > 8)
                {
                    if (valVoisin == 9)
                    {
                        bombe++;
                    }
                    else
                    {
                        caseFermer.Push(coordC - 1);
                        caseFermer.Push(coordL);//dessu
                        caseF++;
                    }
                }
            }

            if (coordC + 1 < nbColonnes)//E
            {
                valVoisin = grille[coordL, coordC + 1];

                if (valVoisin > 8)
                {
                    if (valVoisin == 9)
                    {
                        bombe++;
                    }
                    else
                    {
                        caseFermer.Push(coordC + 1);
                        caseFermer.Push(coordL);//dessu
                        caseF++;
                    }
                }
            }

            if ((coordL + 1 < nbLignes) && (coordC - 1 > 0))//SW
            {
                valVoisin = grille[coordL + 1, coordC - 1];

                if (valVoisin > 8)
                {
                    if (valVoisin == 9)
                    {
                        bombe++;
                    }
                    else
                    {
                        caseFermer.Push(coordC - 1);
                        caseFermer.Push(coordL + 1);//dessu
                        caseF++;
                    }
                }
            }

            if (coordL + 1 < nbLignes)//S
            {
                valVoisin = grille[coordL + 1, coordC];

                if (valVoisin > 8)
                {
                    if (valVoisin == 9)
                    {
                        bombe++;
                    }
                    else
                    {
                        caseFermer.Push(coordC);
                        caseFermer.Push(coordL + 1);//dessu
                        caseF++;
                    }
                }
            }

            if ((coordL + 1 < nbLignes) && (coordC + 1 < nbColonnes))//SE
            {
                valVoisin = grille[coordL + 1, coordC + 1];

                if (valVoisin > 8)
                {
                    if (valVoisin == 9)
                    {
                        bombe++;
                    }
                    else
                    {
                        caseFermer.Push(coordC + 1);
                        caseFermer.Push(coordL + 1);//dessu
                        caseF++;
                    }
                }
            }

            if (caseF > 0)
            {
                if ((grille[coordL, coordC] - bombe) == caseF)
                {
                    while (caseFermer.Count > 0)
                    {
                        grille[caseFermer.Pop(), caseFermer.Pop()] = 9;
                    }
                }
            }

        }

        void VoisinOuvert(int coordL, int coordC)//coordonnee ligne, coordonnee colonne      cherche les case ouvertes autour non-bombe
        {
            int valeurMaxDanger = 1;
            int valeurTester;

            if ((coordL > 0) && (coordC > 0))//NW
            {
                valeurTester = grille[coordL - 1, coordC - 1];
                if (valeurTester != 10 && valeurTester != 9)
                    if (CalculerDanger(coordL - 1, coordC - 1))
                        VerifierScore(coordL, coordC, ref valeurMaxDanger);
            }

            if (!nouvelleBombe)
                if (coordL > 0)//N
                {
                    valeurTester = grille[coordL - 1, coordC];
                    if (valeurTester != 10 && valeurTester != 9)
                        if (CalculerDanger(coordL - 1, coordC))
                            VerifierScore(coordL, coordC, ref valeurMaxDanger);
                }

            if (!nouvelleBombe)
                if ((coordL > 0) && (coordC + 1 < nbColonnes))//NE
                {
                    valeurTester = grille[coordL - 1, coordC + 1];
                    if (valeurTester != 10 && valeurTester != 9)
                        if (CalculerDanger(coordL - 1, coordC + 1))
                            VerifierScore(coordL, coordC, ref valeurMaxDanger);
                }

            if (!nouvelleBombe)
                if (coordC > 0)//W
                {
                    valeurTester = grille[coordL, coordC - 1];
                    if (valeurTester != 10 && valeurTester != 9)
                        if (CalculerDanger(coordL, coordC - 1))
                            VerifierScore(coordL, coordC, ref valeurMaxDanger);
                }

            if (!nouvelleBombe)
                if (coordC + 1 < nbColonnes)//E
                {
                    valeurTester = grille[coordL, coordC + 1];
                    if (valeurTester != 10 && valeurTester != 9)
                        if (CalculerDanger(coordL, coordC + 1))
                            VerifierScore(coordL, coordC, ref valeurMaxDanger);
                }

            if (!nouvelleBombe)
                if ((coordL + 1 < nbLignes) && (coordC - 1 > 0))//SW
                {
                    valeurTester = grille[coordL + 1, coordC - 1];
                    if (valeurTester != 10 && valeurTester != 9)
                        if (CalculerDanger(coordL + 1, coordC - 1))
                            VerifierScore(coordL, coordC, ref valeurMaxDanger);
                }

            if (!nouvelleBombe)
                if (coordL + 1 < nbLignes)//S
                {
                    valeurTester = grille[coordL + 1, coordC];
                    if (valeurTester != 10 && valeurTester != 9)
                        if (CalculerDanger(coordL + 1, coordC))
                            VerifierScore(coordL, coordC, ref valeurMaxDanger);
                }

            if (!nouvelleBombe)
                if ((coordL + 1 < nbLignes) && (coordC + 1 < nbColonnes))//SE
                {
                    valeurTester = grille[coordL + 1, coordC + 1];
                    if (valeurTester != 10 && valeurTester != 9)
                        if (CalculerDanger(coordL + 1, coordC + 1))
                            VerifierScore(coordL, coordC, ref valeurMaxDanger);
                }

            if (nouvelleBombe)
            {
                grille[coordL, coordC] = 9;
                AnalyserGrille();
                return;
            }

            if (valeurMaxDanger != 1 && valeurMaxDanger <= meilleurCoup[2])
            {
                if (valeurMaxDanger == meilleurCoup[2])
                {
                    var rand = new Random();

                    if (rand.Next() % 2 == 0)
                    {
                        meilleurCoup[2] = valeurMaxDanger;
                        Meilleur(coordL, coordC);
                    }
                }
                else
                {
                    meilleurCoup[2] = valeurMaxDanger;
                    Meilleur(coordL, coordC);
                }
            }
        }

        bool CalculerDanger(int coordL, int coordC)
        {
            int nbBombes = 0;
            int nbCaseFermer = 0;

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
                if (nbCaseFermer != 0)
                {
                    score = (((grille[coordL, coordC] - nbBombes) * 100) / nbCaseFermer);


                    if (score == 100)
                        nouvelleBombe = true; //la case non decouvert est une bombe

                    return true;
                }
            }
            return false;//ne pas tenir compte
        }

        void VerifierScore(int coordL, int coordC, ref int valeurMaxDanger)
        {
            {
                if (score == 0)
                {
                    meilleurCoup[2] = score;
                    Meilleur(coordL, coordC);
                }
                else if (ComparerDeuxScore(score, valeurMaxDanger))
                    valeurMaxDanger = score;
            }
        }

        bool ComparerDeuxScore(int score1, int score2)
        {
            if (score1 == score2)
            {
                var rand = new Random();

                if (rand.Next() % 2 == 0)
                    return true;
            }
            else if (score1 > score2)
            {
                return true;
            }

            return false;
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

