using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System;

namespace Demineur
{
    public class Partie
    {
        Regex rx = new Regex(@"^\d+\s\d+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        int[] positionActuelle,
              selection;
        Grille m_Grille;
        bool enMarche, mort;
        Joueur j;
        string difficulte, temps, grosseur;
        IA intelligence;
        
        public Partie(short[] optionDePartie)
        {
            enMarche = mort = false;
            selection = new int[2] { 6, 5 }; // 1,1 dans l'interface graphique
            m_Grille = new Grille(optionDePartie[0], optionDePartie[1], optionDePartie[2]);
            j = new Joueur(InterfaceUsager.QuiEtesVous());
            difficulte = Convert.ToString(optionDePartie[2]);
            grosseur = Convert.ToString(optionDePartie[0]);
            if(optionDePartie[3] > 1)
                intelligence = new IA(optionDePartie[0], optionDePartie[1]);
        }

        public bool CommencerPartie()
        {
            Stopwatch minuterie = new Stopwatch();
            minuterie.Start();

            //Premier Tour            
            InterfaceUsager.DessinerPlateau(m_Grille.Lignes(), m_Grille.Colonnes(), m_Grille.ToString(), selection);
            VerificationOuvertureEtContenue(selection = Touches(m_Grille.Colonnes(), m_Grille.Lignes(), m_Grille.ToString(), selection[0], selection[1]));

            //Autres Tours
            enMarche = true;
            while (enMarche)
            {
                InterfaceUsager.DessinerGrille(m_Grille.Lignes(), m_Grille.Colonnes(), m_Grille.ToString(), selection);
                VerificationOuvertureEtContenue(selection = Touches(m_Grille.Colonnes(), m_Grille.Lignes(), m_Grille.ToString(), selection[0], selection[1]));
                EstCeGagner();
            }

            //Partie Terminé
            minuterie.Stop();
            temps = minuterie.Elapsed.TotalSeconds.ToString("F");

            if (mort)
            {
                InterfaceUsager.DessinerGrille(m_Grille.Lignes(), m_Grille.Colonnes(), m_Grille.ToString(), selection); //Dessine la grille on game over
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
            ConsoleKeyInfo touche;
            string entree;
            do
            {
                entree = "";
                if (InterfaceUsager.Saisie)
                {
                    ActiverModeFleche();
                    do
                    {
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
                            case 65: // a pour aciver l'intelligence artificiel
                                float[] retourIA = intelligence.JouerTour(m_Grille.ToString());
                                positionActuelle[0] = (int)retourIA[0];
                                positionActuelle[1] = (int)retourIA[1];
                                break;
                        }
                    } while (InterfaceUsager.Saisie && touche.Key != ConsoleKey.Enter);
                }
                else
                    do
                    {
                        ActiverModeSaisieManuelle();
                        entree = Console.ReadLine();
                    } while (!EntreeManuelle(entree));
            } while ((entree.Length < 3 && !InterfaceUsager.Saisie ) || (entree == "f" && InterfaceUsager.Saisie));
            return positionActuelle;
        }

        public void AllerGauche()//OK
        {

                if (Console.CursorLeft < 10)
                    Console.SetCursorPosition((m_Grille.Colonnes() * 4) + 2, Console.CursorTop);
                else
                    Console.SetCursorPosition(Console.CursorLeft - 4, Console.CursorTop);
                positionActuelle[0] = Console.CursorLeft;
                positionActuelle[1] = Console.CursorTop;
        }

        public void AllerDroite()//OK
        {

                if (Console.CursorLeft > (m_Grille.Colonnes() * 4))
                    Console.SetCursorPosition(6, Console.CursorTop);
                else
                    Console.SetCursorPosition(Console.CursorLeft + 4, Console.CursorTop);
                positionActuelle[0] = Console.CursorLeft;
                positionActuelle[1] = Console.CursorTop;
        }

        public void AllerHaut()//OK
        {
                if (Console.CursorTop < 8)
                    Console.SetCursorPosition(Console.CursorLeft, (m_Grille.Lignes() * 3) + 2);
                else
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 3);
                positionActuelle[0] = Console.CursorLeft;
                positionActuelle[1] = Console.CursorTop;       
        }

        public void AllerBas()//OK
        {
                if (Console.CursorTop > m_Grille.Lignes() * 3)
                    Console.SetCursorPosition(Console.CursorLeft, 5);
                else
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 3);
                positionActuelle[0] = Console.CursorLeft;
                positionActuelle[1] = Console.CursorTop;
        }

        public void ActiverModeFleche()//OK
        {
            InterfaceUsager.Saisie = true;
            Console.SetCursorPosition(positionActuelle[0], positionActuelle[1]);
        }

        void ActiverModeSaisieManuelle()//OK
        {
            InterfaceUsager.Saisie = false;
            InterfaceUsager.PositionnerCursorPourRepondre();
        }

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
            else
                InterfaceUsager.MessageFormatDentreeErronee();
            return false;
        }

        bool VerificationOuvertureEtContenue(int[] selection)//OK
        {
            int[] cible = new int[2];
            cible[1] = selection[1] / 3 - 1; // Conversion coordonnées pour tableau grille
            cible[0] = selection[0] / 4 - 1; // Conversion coordonnées pour tableau grille

            if (!m_Grille[cible[1], cible[0]].Ouvert) // N'ouvre pas case déjà ouverte et fix les nombre qui changent.
            {
                if (m_Grille.OuvrirCase(cible[1], cible[0]) == false && enMarche)//modifier pour permettre le game over
                {
                    m_Grille.DecouvrirBombes();
                    mort = true;
                    enMarche = false;
                }
                else if (m_Grille.OuvrirCase(cible[1], cible[0]) == false && !enMarche) //Bouger pour l'OO dans grille
                {
                    m_Grille[cible[1], cible[0]].Bombe = false;
                    m_Grille[cible[1], cible[0]].CalculerDanger();
                }
                return true;
            }
            InterfaceUsager.MessageCaseDejaOuverte();
            return false;
        }

        public bool VerificationFormatDeLentree(string entree)
        {
            MatchCollection matches = rx.Matches(entree);

            if (matches.Count == 1)
                return VerificationDesMinMaxDeLentree(entree);
            InterfaceUsager.MessageFormatDentreeErronee();
            return false;
        }

        public bool VerificationDesMinMaxDeLentree(string entree)
        {
            if (Int32.Parse(entree.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]) <= m_Grille.Colonnes() &&
                Int32.Parse(entree.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) <= m_Grille.Lignes())
                return true;
            InterfaceUsager.MessageHorsLimites();
            return false;
        }

        private bool EstCeGagner() 
        {
            if (m_Grille.CasesFermer() <= m_Grille.NombreDeBombes())
            {
                enMarche = false;
                return true;
            }
            return false;
        }

        public  string[] InfoDepartie()
        {

            return new string[] { j.ObtenirNom(), grosseur, difficulte, temps };
        }
    }
}
