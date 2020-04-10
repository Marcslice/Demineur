using System;
using System.Collections.Generic;

namespace Demineur
{
    public class AITest
    {
        /// <summary>
        /// nbLignes : nombre total de lignes dans la partie
        /// nbColonnes : nombre total de colonnes dans la partie
        /// Score : le risque d'ouvrir un bombe dans la case analyser (nombre en %)
        /// grille : simule la grille de la partie de demineur
        /// meilleurCoup : est un tableau contenant: [la ligne, la colonne, score] de la case avec le moins de chance detre une bombe
        /// nouvelleBombe : boolean permettant de savoir si une nouvelle bombe a ete detecter dans la grille
        /// caseFermer : un stack servant a empiler des coordonnees de case fermer lors des analyse de la grille
        /// </summary>
        int nbLignes, nbColonnes, score;
        int[,] grille;
        int[] meilleurCoup;
        bool nouvelleBombe;
        Stack<int> caseFermer;

        /// <summary>
        /// constructeur appeler lors de la creation d'une partie avec AI
        /// </summary>
        /// <param name="lignes">nombre de lignes dans la grille de la partie</param>
        /// <param name="colonnes">nombre de colonnes dans la grille de la partie</param>
        public AITest(int lignes, int colonnes)
        {
            nbLignes = lignes;
            nbColonnes = colonnes;
            grille = new int[lignes, colonnes];
            caseFermer = new Stack<int>();

            for (int l = 0; l < lignes; l++)
            {
                for (int c = 0; c < colonnes; c++)
                {
                    grille[l, c] = 10;
                }
            }
        }

        /// <summary>
        /// Methode public qu'enplois la classe partie pour obtenir la meilleur case a jouer
        /// </summary>
        /// <param name="grilleDeJeu">version string de la grille de jeu generer par le demineur</param>
        /// <returns>Retourne des coordonner d'ordre ligne, colonne sous la forme d'un tableau d'entier a la partie</returns>
        public int[] MeilleurCoup(string grilleDeJeu)
        {
            meilleurCoup = new int[3] { 0, 0, 0 };

            GenererGrille(grilleDeJeu);
            if (caseFermer.Count < 1)
            {
                do
                {
                    AnalyserGrille();
                } while (caseFermer.Count < 1 && nouvelleBombe);
            }

            if (caseFermer.Count > 0)
            {
                do
                {
                    if (caseFermer.Count < 1) {
                       AnalyserGrille();
                    }
                    if(caseFermer.Count > 0)
                    {
                        meilleurCoup[0] = caseFermer.Pop();
                        meilleurCoup[1] = caseFermer.Pop();
                        meilleurCoup[2] = 0;
                    }
                } while ((grille[meilleurCoup[0], meilleurCoup[1]] != 10) && meilleurCoup[2] != 420);
                if(meilleurCoup[2] != 420)
                    return meilleurCoup;
            }

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

        /// <summary>
        /// Analyse toutes les case ouvertes et store leur valeur dans la grille de l'AI pour qu'il puisse faire son analyse. Utilise les variable 
        /// global nblignes et nbcolonnes pour rester dans les limites de la grille
        /// charTranscrit : permet de garder en memoire la position a laquel la boucle est par rapport a la string
        /// </summary>
        /// <param name="aConvertir">String representant la grille de jeu connu sous le nom grilleDeJeu lors de l'appel de MeilleurCoup()</param>
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
        }

        /// <summary>
        /// Analyse toutes les valeur presente dans la grille de notre AI. Dependant de la valeur de la case emploi une methode d'analyse differente.
        /// Initialise les valeurs nouvelleBombe et meilleurCoup[2] a leur valeur par default.
        /// l : int representant le numero de la ligne
        /// c : int represantant le numero de la colonne
        /// </summary>
        void AnalyserGrille()
        {
            nouvelleBombe = false;
            meilleurCoup[2] = 420;

            for (int l = 0; l < nbLignes; l++)
            {
                for (int c = 0; c < nbColonnes; c++)
                {
                    if (grille[l, c] < 9 )
                    {
                        AnalyseCaseO(l, c);
                        if (caseFermer.Count > 0)
                            return;
                    }
                    else if (grille[l, c] == 10)
                    {
                        AnalyseCaseF(l, c);
                        if (caseFermer.Count > 0)
                            return;
                    }
                }
            }
        }

        /// <summary>
        /// Verifie si la case selectionne a des voisin a tout les points cardinaux. Verifie si cest voisin sont des 9 (bombe) ou 10 (case fermer) 
        /// afin de determiner si toutes les bombes a proximiter on ete trouvees. Si une case est fermer cest coordonnees sont mises dans une pile 
        /// preemptivement au cas ou toutes les bombes auraient ete trouvees. Si la difference entre la valeur de la case et le nombre de bombe trouve
        /// est de 0 les coordonnees dans la pile sont garder en memoire comme etant des case securitaire. Si la valeur de la case moins le nombre de 
        /// bombe avoisinante est egale au nombre de case fermee, les coordonnees dans la pile sont marquer comme etant des bombe. Sinon la pile est vider
        /// de son contenu
        /// caseF : int servant a calculer le nombre de case fermer
        /// bombe : int servant a calculer le nombre de bombe detecter
        /// valVoisin : int represantant la valeur d'un voisin de la case analyser
        /// </summary>
        /// <param name="coordL">numero de la ligne de la case a analyser</param>
        /// <param name="coordC">numero de la colonne de la case a analyser</param>
        void AnalyseCaseO(int coordL, int coordC)
        {
            int caseF = 0;
            int bombe = 0;
            int valVoisin;

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
                        caseFermer.Push(coordC - 1);//2e a etre pop()
                        caseFermer.Push(coordL - 1);//1er a etre pop()
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
                        caseFermer.Push(coordL - 1);
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
                        caseFermer.Push(coordL - 1);
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
                        caseFermer.Push(coordL);
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
                        caseFermer.Push(coordL);
                        caseF++;
                    }
                }
            }

            if ((coordL + 1 < nbLignes) && (coordC > 0))//SW
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
                        caseFermer.Push(coordL + 1);
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
                        caseFermer.Push(coordL + 1);
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
                        caseFermer.Push(coordL + 1);
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
                        nouvelleBombe = true;
                    }
                }
                else if ((grille[coordL, coordC] - bombe) != 0)
                {
                    caseFermer.Clear();
                }
            }
        }

        /// <summary>
        /// Analyse le danger que peut presenter l'ouverture de la case selectionne en fonction des information disponible sur les case ouverte autour
        /// d'elle qui ne sont pas des bombe. La menace la plus grande est garder en memoir et comparer avec le danger du meilleur coup actuel. Si la 
        /// menace represente un risque moins elever d'ouvrir un bombe que le meilleur coup actuel, les coordonnees son storer dans meilleurCoup[] ainsi
        /// que le nombre representant le danger
        /// valeurMaxDanger : int representant les chance en % d'ouvrir une bombe du cas le plus probable
        /// valeurTester : int representant la valeur de la case tester
        /// </summary>
        /// <param name="coordL">numero de la ligne de la case a analyser</param>
        /// <param name="coordC">numero de la colonne de la case a analyser</param>
        void AnalyseCaseF(int coordL, int coordC)
        {
            int valeurMaxDanger = 1;
            int valeurTester;

            if ((coordL > 0) && (coordC > 0) && caseFermer.Count < 1 && !nouvelleBombe)//NW
            {
                valeurTester = grille[coordL - 1, coordC - 1];
                if (valeurTester != 10 && valeurTester != 9)
                {
                    CalculerDanger(coordL - 1, coordC - 1);
                    VerifierScore(ref valeurMaxDanger);
                }
            }


            if (coordL > 0 && caseFermer.Count < 1 && caseFermer.Count < 1 && !nouvelleBombe)//N
            {
                valeurTester = grille[coordL - 1, coordC];
                if (valeurTester != 10 && valeurTester != 9)
                {
                    CalculerDanger(coordL - 1, coordC);
                    VerifierScore(ref valeurMaxDanger);
                }
            }


            if ((coordL > 0) && (coordC + 1 < nbColonnes) && caseFermer.Count < 1 && !nouvelleBombe)//NE
            {
                valeurTester = grille[coordL - 1, coordC + 1];
                if (valeurTester != 10 && valeurTester != 9)
                {
                    CalculerDanger(coordL - 1, coordC + 1);
                    VerifierScore(ref valeurMaxDanger);
                }
            }


            if (coordC > 0 && caseFermer.Count < 1 && !nouvelleBombe)//W
            {
                valeurTester = grille[coordL, coordC - 1];
                if (valeurTester != 10 && valeurTester != 9)
                {
                    CalculerDanger(coordL, coordC - 1);
                    VerifierScore(ref valeurMaxDanger);
                }
            }


            if (coordC + 1 < nbColonnes && caseFermer.Count < 1 && !nouvelleBombe)//E
            {
                valeurTester = grille[coordL, coordC + 1];
                if (valeurTester != 10 && valeurTester != 9)
                {
                    CalculerDanger(coordL, coordC + 1);
                    VerifierScore(ref valeurMaxDanger);
                }
            }


            if ((coordL + 1 < nbLignes) && (coordC > 0) && caseFermer.Count < 1 && !nouvelleBombe)//SW
            {
                valeurTester = grille[coordL + 1, coordC - 1];
                if (valeurTester != 10 && valeurTester != 9)
                {
                    CalculerDanger(coordL + 1, coordC - 1);
                    VerifierScore(ref valeurMaxDanger);
                }
            }


            if (coordL + 1 < nbLignes && caseFermer.Count < 1 && !nouvelleBombe)//S
            {
                valeurTester = grille[coordL + 1, coordC];
                if (valeurTester != 10 && valeurTester != 9)
                {
                    CalculerDanger(coordL + 1, coordC);
                    VerifierScore(ref valeurMaxDanger);
                }
            }


            if ((coordL + 1 < nbLignes) && (coordC + 1 < nbColonnes) && caseFermer.Count < 1 && !nouvelleBombe)//SE
            {
                valeurTester = grille[coordL + 1, coordC + 1];
                if (valeurTester != 10 && valeurTester != 9)
                {
                    CalculerDanger(coordL + 1, coordC + 1);
                    VerifierScore(ref valeurMaxDanger);
                }
            }


            if (valeurMaxDanger != 1 && valeurMaxDanger <= meilleurCoup[2] && caseFermer.Count < 1 && !nouvelleBombe)
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

        /// <summary>
        /// Genere un score en analysant les case autours de la case appele. Calcule le nombre de case ferme et le nombre de bombe trouve et determine un 
        /// score corespondant au danger d'ouvrir une bombe dans les cases voisines.Si la valuer de la case moin le nombre de bombe trouver egal 0 les cases 
        /// fermees qui on ete storees son defini comme securitaire et CoupFacile() est appeler. Si la difference est egale au nombre de case fermer la pile 
        /// est considerer comme etant des bombe et une fois les cases transformer en bombes AnalyserGrille() est rappeler et on sort de la methode. Sinon la 
        /// pile est videe
        /// nbBombes : int representant le nombre de bombe detecter autour de la case analyser
        /// nbCaseFermer : int representant le nombe de case fermer detecter autour de la case analyser
        /// </summary>
        /// <param name="coordL">numero de la ligne de la case a analyser</param>
        /// <param name="coordC">numero de la colonne de la case a analyser</param>
        /// <returns></returns>
        void CalculerDanger(int coordL, int coordC)
        {
            int nbBombes = 0;
            int nbCaseFermer = 0;

            if ((coordL > 0) && (coordC > 0))//NW
            {
                if (grille[coordL - 1, coordC - 1] == 9)
                {
                    nbBombes++;
                }
                else if (grille[coordL - 1, coordC - 1] == 10)
                {
                    caseFermer.Push(coordC - 1);//2e a etre pop()
                    caseFermer.Push(coordL - 1);//1er a etre pop()
                    nbCaseFermer++;
                }
            }

            if (coordL > 0)//N
            {
                if (grille[coordL - 1, coordC] == 9)
                {
                    nbBombes++;
                }
                else if (grille[coordL - 1, coordC] == 10)
                {
                    caseFermer.Push(coordC);
                    caseFermer.Push(coordL - 1);
                    nbCaseFermer++;
                }
            }

            if ((coordL > 0) && (coordC + 1 < nbColonnes))//NE
            {
                if (grille[coordL - 1, coordC + 1] == 9)
                {
                    nbBombes++;
                }
                else if (grille[coordL - 1, coordC + 1] == 10)
                {
                    caseFermer.Push(coordC + 1);
                    caseFermer.Push(coordL - 1);
                    nbCaseFermer++;
                }
            }

            if (coordC > 0)//W
            {
                if (grille[coordL, coordC - 1] == 9)
                {
                    nbBombes++;
                }
                else if (grille[coordL, coordC - 1] == 10)
                {
                    caseFermer.Push(coordC - 1);
                    caseFermer.Push(coordL);
                    nbCaseFermer++;
                }
            }

            if (coordC + 1 < nbColonnes)//E
            {
                if (grille[coordL, coordC + 1] == 9)
                {
                    nbBombes++;
                }
                else if (grille[coordL, coordC + 1] == 10)
                {
                    caseFermer.Push(coordC + 1);
                    caseFermer.Push(coordL);
                    nbCaseFermer++;
                }
            }

            if ((coordL + 1 < nbLignes) && (coordC > 0))//SW
            {
                if (grille[coordL + 1, coordC - 1] == 9)
                {
                    nbBombes++;
                }
                else if (grille[coordL + 1, coordC - 1] == 10)
                {
                    caseFermer.Push(coordC - 1);
                    caseFermer.Push(coordL + 1);
                    nbCaseFermer++;
                }
            }

            if (coordL + 1 < nbLignes)//S
            {
                if (grille[coordL + 1, coordC] == 9)
                {
                    nbBombes++;
                }
                else if (grille[coordL + 1, coordC] == 10)
                {
                    caseFermer.Push(coordC);
                    caseFermer.Push(coordL + 1);
                    nbCaseFermer++;
                }
            }

            if ((coordL + 1 < nbLignes) && (coordC + 1 < nbColonnes))//SE
            {
                if (grille[coordL + 1, coordC + 1] == 9)
                {
                    nbBombes++;
                }
                else if (grille[coordL + 1, coordC + 1] == 10)
                {
                    caseFermer.Push(coordC + 1);
                    caseFermer.Push(coordL + 1);
                    nbCaseFermer++;
                }
            }

            
                score = (((grille[coordL, coordC] - nbBombes) * 100) / nbCaseFermer);


                if (score == 100)
                {
                    while (caseFermer.Count > 0)
                    {
                        grille[caseFermer.Pop(), caseFermer.Pop()] = 9;
                    }

                    nouvelleBombe = true;
                }
                else if (score != 0)
                {
                    caseFermer.Clear();
                }
            
        }

        /// <summary>
        /// Verifie si le score et la valeur maximal de danger sont egal, si cest le cas un des deux sera retenu au hasard. Sinon il verifie si score est plus grand
        /// que la valeur maximal de danger si cest le cas la valeur maximal change pour score
        /// </summary>
        /// <param name="valeurMaxDanger">la valeur de danger maximal que represanterait ouvrir cette case fermer</param>
        void VerifierScore(ref int valeurMaxDanger)
        {
            if (caseFermer.Count < 1)
            {
                if (score == valeurMaxDanger)
                {
                    var rand = new Random();

                    if (rand.Next() % 2 == 0)
                        valeurMaxDanger = score;
                }
                else if (score > valeurMaxDanger)
                {
                    valeurMaxDanger = score;
                }
            }
        }


        /// <summary>
        /// Methode qui attribue de nouvelles coordonnees au meilleurcoup
        /// </summary>
        /// <param name="ligne">numero de ligne</param>
        /// <param name="colonne">numero de colonne</param>
        void Meilleur(int ligne, int colonne)
        {
            meilleurCoup[0] = ligne;
            meilleurCoup[1] = colonne;
        }
    }
}

