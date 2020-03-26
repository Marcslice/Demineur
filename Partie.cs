using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

namespace Demineur
{
    public class Partie
    {
        Regex rx = new Regex(@"^\d+\s\d+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        int[] positionActuelle,
              selection;
        Grille m_Grille;
        bool enMarche, mort, auto;
        Joueur j;
        string difficulte, temps, grosseur;
        //IA intelligence;
        AITest intel;

        public Partie(string nom, short[] optionDePartie)
        {
            enMarche = mort = false;
            selection = new int[2] { 6, 5 }; // 1,1 dans l'interface graphique
            m_Grille = new Grille(optionDePartie[0], optionDePartie[1], optionDePartie[2]);
            j = new Joueur(nom);
            difficulte = Convert.ToString(optionDePartie[2]);
            grosseur = Convert.ToString(optionDePartie[0]);
            if (optionDePartie[3] > 1)
                //intelligence = new IA(optionDePartie[0], optionDePartie[1]);
                intel = new AITest(optionDePartie[0], optionDePartie[1], optionDePartie[2]);
            if (optionDePartie[3] > 2)
                auto = true;
            else
                auto = false;
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
            InterfaceUsager.DessinerPlateau(j.ObtenirNom(),m_Grille.Lignes(), m_Grille.Colonnes(), m_Grille.ToString(), selection, m_Grille.NombreDeBombes, mort);
            VerificationOuvertureEtContenue(selection = Touches(m_Grille.Colonnes(), m_Grille.Lignes(), m_Grille.ToString(), selection[0], selection[1]));

            //Autres Tours
            enMarche = true;
            while (enMarche)
            {
                InterfaceUsager.DessinerGrille(j.ObtenirNom(),m_Grille.Lignes(), m_Grille.Colonnes(), m_Grille.ToString(), selection, m_Grille.NombreDeBombes, mort);
                VerificationOuvertureEtContenue(selection = Touches(m_Grille.Colonnes(), m_Grille.Lignes(), m_Grille.ToString(), selection[0], selection[1]));
                InterfaceUsager.DessinerGrille(j.ObtenirNom(),m_Grille.Lignes(), m_Grille.Colonnes(), m_Grille.ToString(), selection, m_Grille.NombreDeBombes, mort);
            }

            //Partie Terminé
            minuterie.Stop();
            temps = minuterie.Elapsed.TotalMinutes.ToString("F");

            if (mort)
            {
                InterfaceUsager.DessinerGrille(j.ObtenirNom(),m_Grille.Lignes(), m_Grille.Colonnes(), m_Grille.ToString(), selection, m_Grille.NombreDeBombes, mort); //Dessine la grille on game over
                InterfaceUsager.MessageDefaite();              
                return false;
            }
            else
            {
                InterfaceUsager.MessageVictoire();
                return true;
            }
        }

        public int[] Touches(int iCol, int iLig, string s_Grille, int xActuel, int yActuel)
        {
            positionActuelle = new int[2] { xActuel, yActuel };
            ConsoleKeyInfo touche = new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
            string entree;
            do
            {
                entree = "";
                if (InterfaceUsager.Saisie)
                {
                    ActiverModeFleche();
                    do
                    {
                        if (auto)
                        {
                            touche = new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false);
                            Thread.Sleep(1000);
                        }
                        else
                            touche = Console.ReadKey(true);

                        switch ((int)touche.Key)
                        {
                            case 37:
                                AllerGauche();
                                InterfaceUsager.MettreAJourSelection(positionActuelle);
                                break;
                            case 39:
                                AllerDroite();
                                InterfaceUsager.MettreAJourSelection(positionActuelle);
                                break;
                            case 38:
                                AllerHaut();
                                InterfaceUsager.MettreAJourSelection(positionActuelle);
                                break;
                            case 40:
                                AllerBas();
                                InterfaceUsager.MettreAJourSelection(positionActuelle);
                                break;
                            case 67: // c pour coordonnées manuelles                      
                                ActiverModeSaisieManuelle();
                                break;
                        }
                    } while (InterfaceUsager.Saisie && touche.Key != ConsoleKey.Enter && touche.Key != ConsoleKey.A);
                }
                else
                    do
                    {
                        ActiverModeSaisieManuelle();
                        entree = Console.ReadLine();
                    } while (!EntreeManuelle(entree));

            } while ((entree.Length < 3 && !InterfaceUsager.Saisie) || (entree == "f" && InterfaceUsager.Saisie) || (touche.Key == ConsoleKey.A && !AppelerIA()));
            return positionActuelle;
        }

        /// <summary>
        /// En mode flèche, selectionne la case à gauche.
        /// </summary>
        public void AllerGauche()//OK
        {

            if (Console.CursorLeft < 10)
                Console.SetCursorPosition((m_Grille.Colonnes() * 4) + 2, Console.CursorTop);
            else
                Console.SetCursorPosition(Console.CursorLeft - 4, Console.CursorTop);
            positionActuelle[0] = Console.CursorLeft;
            positionActuelle[1] = Console.CursorTop;
        }

        /// <summary>
        /// En mode flèche, selectionne la case à droite.
        /// </summary>
        public void AllerDroite()//OK
        {

            if (Console.CursorLeft > (m_Grille.Colonnes() * 4))
                Console.SetCursorPosition(6, Console.CursorTop);
            else
                Console.SetCursorPosition(Console.CursorLeft + 4, Console.CursorTop);
            positionActuelle[0] = Console.CursorLeft;
            positionActuelle[1] = Console.CursorTop;
        }

        /// <summary>
        /// En mode flèche, selectionne la case en haut.
        /// </summary>
        public void AllerHaut()//OK
        {
            if (Console.CursorTop < 8)
                Console.SetCursorPosition(Console.CursorLeft, (m_Grille.Lignes() * 3) + 2);
            else
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 3);
            positionActuelle[0] = Console.CursorLeft;
            positionActuelle[1] = Console.CursorTop;
        }

        /// <summary>
        /// En mode flèche, selectionne la case en bas.
        /// </summary>
        public void AllerBas()//OK
        {
            if (Console.CursorTop > m_Grille.Lignes() * 3)
                Console.SetCursorPosition(Console.CursorLeft, 5);
            else
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 3);
            positionActuelle[0] = Console.CursorLeft;
            positionActuelle[1] = Console.CursorTop;
        }

        /// <summary>
        /// Appele l'intelligence artificiel.
        /// </summary>
        /// <returns>bool : vrai si IA actif, faux si partie sans IA</returns>
        public bool AppelerIA()
        {
            if (intel != null)
            {
                int[] retourIA = intel.MeilleurCoup(m_Grille.ToString()); //Methode 
                positionActuelle[0] = (retourIA[1] + 1) * 4 + 2;
                positionActuelle[1] = (retourIA[0] + 1) * 3 + 2;
                InterfaceUsager.MettreAJourSelection(positionActuelle);
                return true;
            }
            InterfaceUsager.MessageIAInactif();
            return false;
        }

        /// <summary>
        /// Active la sélection des cases avec les flèches.
        /// </summary>
        void ActiverModeFleche()//OK
        {
            InterfaceUsager.Saisie = true;
            InterfaceUsager.DessinerModeDeSaisie();
            Console.SetCursorPosition(positionActuelle[0], positionActuelle[1]);
        }

        /// <summary>
        /// Active la sélection des cases avec les flèches.
        /// </summary>
        void ActiverModeSaisieManuelle()//OK
        {
            InterfaceUsager.Saisie = false;
            InterfaceUsager.DessinerModeDeSaisie();
            InterfaceUsager.PositionnerCursorPourRepondre();
        }

        /// <summary>
        /// Active la sélection manuelle des cases avec des coordonnées clavier.
        /// Saisir 'f' active la saisie avec flèches
        /// Saisir 'a' fait joueur l'IA si l'option de partie le permet.
        /// Saisir autres choses qu'un coordonnées valide entrainera une erreur.
        /// </summary>
        /// <param name="entree">Saisie du joueur.</param>
        /// <returns>bool : Retourne true si saisie valide, false si invalide.</returns>
        public bool EntreeManuelle(string entree)
        {
            if (entree.Length > 2)
            {
                if (VerificationFormatDeLentree(entree))
                {
                    positionActuelle[0] = Int32.Parse(entree.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]) * 4 + 2;
                    positionActuelle[1] = Int32.Parse(entree.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) * 3 + 2;
                    return true;
                }
            }
            else if (entree == "f")
            {
                ActiverModeFleche();
                return true;
            }
            else if (entree == "a")
                return AppelerIA();
            else
                InterfaceUsager.MessageFormatDentreeErronee();
            return false;
        }

        /// <summary>
        /// Une fois la saisie validé, une ouvre la case si elle n'est pas déjà ouverte.
        /// </summary>
        /// <param name="selection"></param>
        /// <returns>bool : vrai si case a été ouverte, faux si case déjà ouverte.</returns>
        bool VerificationOuvertureEtContenue(int[] selection)//OK
        {
            int[] cible = new int[2];
            cible[1] = selection[1] / 3 - 1; // Conversion coordonnées pour tableau grille
            cible[0] = selection[0] / 4 - 1; // Conversion coordonnées pour tableau grille

            if (!m_Grille[cible[1], cible[0]].Ouvert) // N'ouvre pas case déjà ouverte.
            {
                if (m_Grille.OuvrirCase(cible[1], cible[0]) == false && enMarche)//modifier pour permettre le game over
                {
                    m_Grille.DecouvrirBombes();
                    mort = true;
                    enMarche = false;
                }
                else if (m_Grille.OuvrirCase(cible[1], cible[0]) == false && !enMarche) //Bouger pour l'OO dans grille
                    m_Grille.BombePremierTour(cible);
                else
                    if (m_Grille.CalculerNbCaseFermer() == m_Grille.NombreDeBombes)
                        enMarche = false;
                return true;
            }
            InterfaceUsager.MessageCaseDejaOuverte();
            return false;
        }

        /// <summary>
        /// Agit comme un masque de saisie.
        /// </summary>
        /// <param name="entree">Saisie manuelle du l'utilisateur.</param>
        /// <returns></returns>
        public bool VerificationFormatDeLentree(string entree)
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
        public bool VerificationDesMinMaxDeLentree(string entree)
        {
            if (Int32.Parse(entree.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]) <= m_Grille.Colonnes() &&
                Int32.Parse(entree.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) <= m_Grille.Lignes())
                return true;
            InterfaceUsager.MessageHorsLimites();
            return false;
        }

        /// <summary>
        /// Utilise pour mettre à jour le score du joueur si la partie est gagnée.
        /// </summary>
        /// <returns>string[] : Informations de partie {nom_joueur, grosseur, difficulté, temps}</returns>
        public string[] InfoDepartie()
        {
            return new string[] { j.ObtenirNom(), grosseur, difficulte, temps };
        }
    }
}