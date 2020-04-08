using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

namespace Demineur
{

    //moving arrows to interface
    public class Partie
    {
        Regex rx = new Regex(@"^\d+\s\d+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        int[] positionActuelle,
              selection;
        Grille m_Grille;
        bool enMarche, mort, auto;
        string nom, difficulte, temps, grosseur;
        //IA intelligence;
        AITest intel;

        public Partie(string p_Nom, short[] optionDePartie)
        {
            InterfaceUsager.Saisie = true;
            enMarche = mort = auto = false;
            selection = new int[2] { 6, 5 }; // 1,1 dans l'interface graphique
            m_Grille = new Grille(optionDePartie[0], optionDePartie[1], optionDePartie[2]);
            difficulte = Convert.ToString(optionDePartie[2]);
            grosseur = Convert.ToString(optionDePartie[0]);
            nom = p_Nom;
            if (optionDePartie[3] > 1)
                //intelligence = new IA(optionDePartie[0], optionDePartie[1]);
                intel = new AITest(optionDePartie[0], optionDePartie[1]);
            if (optionDePartie[3] > 2)
                auto = true;
        }

        /// <summary>
        /// S'occupe du déroulement de la partie.
        /// </summary>
        /// <returns>bool : vrai si gagnée, faux si perdu.</returns>
        public bool CommencerPartie()
        {
            Stopwatch minuterie = new Stopwatch();
            minuterie.Start();

            //Premier Tour            
            InterfaceUsager.DessinerPlateau(nom, m_Grille.Lignes(), m_Grille.Colonnes(), m_Grille.ToString(), selection, m_Grille.NombreDeBombes, mort);
            VerificationOuvertureEtContenue(selection = Touches(m_Grille.Colonnes(), m_Grille.Lignes(), m_Grille.ToString(), selection[0], selection[1]));

            //Autres Tours
            enMarche = true;
            while (enMarche)
            {
                InterfaceUsager.DessinerGrille(nom, m_Grille.Lignes(), m_Grille.Colonnes(), m_Grille.ToString(), selection, m_Grille.NombreDeBombes, mort);
                VerificationOuvertureEtContenue(selection = Touches(m_Grille.Colonnes(), m_Grille.Lignes(), m_Grille.ToString(), selection[0], selection[1]));
                InterfaceUsager.DessinerGrille(nom, m_Grille.Lignes(), m_Grille.Colonnes(), m_Grille.ToString(), selection, m_Grille.NombreDeBombes, mort);
            }

            //Partie Terminé
            minuterie.Stop();
            temps = TimeSpan.FromMinutes(minuterie.Elapsed.TotalMinutes).ToString(@"mm\.ss");

            if (mort)
            {
                InterfaceUsager.DessinerGrille(nom, m_Grille.Lignes(), m_Grille.Colonnes(), m_Grille.ToString(), selection, m_Grille.NombreDeBombes, mort); //Dessine la grille on game over
                InterfaceUsager.MessageDefaite();
                return false;
            }
            InterfaceUsager.MessageVictoire();
            return true;
        }

        int[] Touches(int iCol, int iLig, string s_Grille, int xActuel, int yActuel)
        {
            ConsoleKeyInfo touche = new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
            positionActuelle = new int[2] { xActuel, yActuel };

            do
            {
                if (auto) //AI en mode automatique
                {
                    AppelerIA();
                    touche = new ConsoleKeyInfo('\n', ConsoleKey.Enter, false, false, false);
                    Thread.Sleep(1000);
                }
                else //Joueur non afk
                {
                    if (InterfaceUsager.Saisie) // En mode flèche
                    {
                        do
                        {
                            InterfaceUsager.ActiverModeFleche(positionActuelle);
                            touche = Console.ReadKey(true);

                            switch ((int)touche.Key)
                            {
                                case 37:
                                    positionActuelle = InterfaceUsager.AllerGauche(m_Grille.Colonnes());
                                    InterfaceUsager.MettreAJourSelection(positionActuelle);
                                    break;
                                case 39:
                                    positionActuelle = InterfaceUsager.AllerDroite(m_Grille.Colonnes());
                                    InterfaceUsager.MettreAJourSelection(positionActuelle);
                                    break;
                                case 38:
                                    positionActuelle = InterfaceUsager.AllerHaut(m_Grille.Lignes());
                                    InterfaceUsager.MettreAJourSelection(positionActuelle);
                                    break;
                                case 40:
                                    positionActuelle = InterfaceUsager.AllerBas(m_Grille.Lignes());
                                    InterfaceUsager.MettreAJourSelection(positionActuelle);
                                    break;
                                case 65:
                                    positionActuelle = InterfaceUsager.AllerBas(m_Grille.Lignes());
                                    ConsoleKey k = AppelerIA();
                                    touche = new ConsoleKeyInfo(k.ToString()[0], k, false, false, false);
                                    break;
                                case 67:
                                    InterfaceUsager.ActiverModeSaisieManuelle();
                                    break;
                            }
                        } while (InterfaceUsager.Saisie && touche.Key != ConsoleKey.Enter && touche.Key != ConsoleKey.A);
                    }
                    else // en mode manuelle
                    {
                        ConsoleKey entree;
                        do
                        {
                            InterfaceUsager.ActiverModeSaisieManuelle();
                            entree = VerificationEntreeManuelle(InterfaceUsager.EntreeManuelle());
                        }
                        while (entree == ConsoleKey.E);
                        touche = new ConsoleKeyInfo(entree.ToString()[0], entree, false, false, false);
                    }
                }
            } while (touche.Key != ConsoleKey.Enter && touche.Key != ConsoleKey.A);
            return positionActuelle;
        }

        /// <summary>
        /// Appele l'intelligence artificiel.
        /// </summary>
        /// <returns>bool : vrai si IA actif, faux si partie sans IA</returns>
        ConsoleKey AppelerIA()
        {
            if (intel != null)
            {
                int[] retourIA = intel.MeilleurCoup(m_Grille.ToString()); //Methode 
                positionActuelle[0] = (retourIA[1] + 1) * 4 + 2;
                positionActuelle[1] = (retourIA[0] + 1) * 3 + 2;
                InterfaceUsager.MettreAJourSelection(positionActuelle);
                return ConsoleKey.A;
            }
            InterfaceUsager.MessageIAInactif();
            return ConsoleKey.E;
        }

        /// <summary>
        /// Active la sélection manuelle des cases avec des coordonnées clavier.
        /// Saisir 'f' active la saisie avec flèches
        /// Saisir 'a' fait joueur l'IA si l'option de partie le permet.
        /// Saisir autres choses qu'un coordonnées valide entrainera une erreur.
        /// </summary>
        /// <param name="entree">Saisie du joueur.</param>
        /// <returns>bool : Retourne le consolekey correspondant au choix, si erreur retourne consolekey.e</returns>
        ConsoleKey VerificationEntreeManuelle(string entree)
        {
            if (entree.Length > 2)
            {
                if (VerificationFormatDeLentree(entree))
                {
                    positionActuelle[0] = Int32.Parse(entree.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]) * 4 + 2;
                    positionActuelle[1] = Int32.Parse(entree.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) * 3 + 2;
                    return ConsoleKey.Enter;
                }
            }
            else if (entree == "f")
            {
                InterfaceUsager.ActiverModeFleche(positionActuelle);
                return ConsoleKey.F;
            }
            else if (entree == "a")
                return AppelerIA();
            else
                InterfaceUsager.MessageFormatDentreeErronee();
            return ConsoleKey.E;
        }

        /// <summary>
        /// Une fois la saisie validée, ouvre la case si elle n'est pas déjà ouverte.
        /// </summary>
        /// <param name="selection"></param>
        /// <returns>bool : vrai si case a été ouverte, faux si case déjà ouverte.</returns>
        bool VerificationOuvertureEtContenue(int[] selection)
        {
            int[] cible = new int[2];
            cible[1] = selection[1] / 3 - 1; // Conversion coordonnées pour tableau grille
            cible[0] = selection[0] / 4 - 1; // Conversion coordonnées pour tableau grille

            if (!m_Grille[cible[1], cible[0]].Ouvert) // N'ouvre pas case déjà ouverte.
            {
                if (m_Grille.OuvrirCase(cible[1], cible[0]) == false && enMarche)//Stop la partie si le joueur meurt
                {
                    m_Grille.DecouvrirBombes();
                    mort = true;
                    enMarche = false;
                }
                else if (m_Grille.OuvrirCase(cible[1], cible[0]) == false && !enMarche)//Neutralize la bombe au premier tour
                {
                    m_Grille.BombePremierTour(cible);
                    m_Grille.OuvrirCase(cible[1], cible[0]); // fix bombes premier tour et l'ouverture des cases vide autour.
                }
                else if (m_Grille.CalculerNbCaseFermer() == m_Grille.NombreDeBombes)//Detection de victoire
                    enMarche = false;
                return true;
            }
            InterfaceUsager.MessageCaseDejaOuverte();
            return false;
        }

        /// <summary>
        /// Agit comme un masque de saisie.
        /// Si l'entrée ne match pas, un message d'erreur de format est affiché.
        /// </summary>
        /// <param name="entree">Saisie manuelle de l'utilisateur.</param>
        /// <returns></returns>
        bool VerificationFormatDeLentree(string entree)
        {
            MatchCollection matches = rx.Matches(entree);

            if (matches.Count == 1)
                return VerificationDesMinMaxDeLentree(entree);
            InterfaceUsager.MessageFormatDentreeErronee();
            return false;
        }

        /// <summary>
        /// Vérifie si la saisie fait partie du tableau.
        /// </summary>
        /// <param name="entree">Saisie manuelle du l'utilisateur.</param>
        /// <returns></returns>
        bool VerificationDesMinMaxDeLentree(string entree)
        {
            if (Int32.Parse(entree.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]) <= m_Grille.Colonnes() && Int32.Parse(entree.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) <= m_Grille.Lignes())
                return true;
            InterfaceUsager.MessageHorsLimites();
            return false;
        }

        /// <summary>
        /// Utilisé pour mettre à jour le score du joueur si la partie est gagnée.
        /// </summary>
        /// <returns>string[] : Informations de partie {nom_joueur, grosseur, difficulté, temps}</returns>
        public string[] InfoDepartie()
        {
            return new string[] { nom, grosseur, difficulte, temps };
        }
    }
}