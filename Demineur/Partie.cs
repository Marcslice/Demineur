using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System;

namespace Demineur
{
    public class Partie
    {
        Grille m_Grille;
        bool enMarche;
        int[] selection = new int[2];
        //int nombreCouts = 0;
        Regex rx = new Regex(@"^\d+\s\d+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //Joueur m_Joueur;
        //IA m_IA;
        //string tempsEcoule;
        //bool automatique;

        public Partie(short[] optionDePartie)
        {
            enMarche = true;
            m_Grille = new Grille(optionDePartie[0], optionDePartie[1], optionDePartie[2]); //Ajouter AI plus tard
            //m_IA = new IA(optionDePartie[0], optionDePartie[1]);
            InterfaceUsager.DessinerGrille(optionDePartie[0], optionDePartie[1], m_Grille.ToString());
            VerificationSelection(selection = Cout(optionDePartie[1], optionDePartie[0], m_Grille.ToString(), 6, 5));
            while (enMarche)
            {
                InterfaceUsager.DessinerGrille(optionDePartie[0], optionDePartie[1], m_Grille.ToString());
                VerificationSelection(selection = Cout(optionDePartie[1], optionDePartie[0], m_Grille.ToString(), selection[0], selection[1]));
            }
            InterfaceUsager.DessinerGrille(optionDePartie[0], optionDePartie[1], m_Grille.ToString());//dessine la grille on game over
            //InterfaceUsager.MessageDefaite();
        }

        public int[] Cout(int iCol, int iLig, string tab, int xActuel, int yActuel)
        {
            int[] positionActuelle = new int[2] { xActuel, yActuel };
            string entreeUtilisateur = "";
            ConsoleKeyInfo touche;

            Console.SetCursorPosition(positionActuelle[0], positionActuelle[1]);
            do
            {
                touche = Console.ReadKey(true);

                switch ((int)touche.Key)
                {
                    case 37: // left arrow
                        do
                        {
                            if (Console.CursorLeft < 10)
                                Console.SetCursorPosition((iCol * 4) + 2, Console.CursorTop);
                            else
                                Console.SetCursorPosition(Console.CursorLeft - 4, Console.CursorTop);
                            positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        } while (m_Grille[positionActuelle[1] / 3 - 1, positionActuelle[0] / 4 - 1].Ouvert && m_Grille[positionActuelle[1] / 3 - 1, positionActuelle[0] / 4 - 1].Value == 0);                      
                        break;
                    case 39: // right arrow
                        do
                        {
                            if (Console.CursorLeft > (iCol * 4))
                                Console.SetCursorPosition(6, Console.CursorTop);
                            else
                                Console.SetCursorPosition(Console.CursorLeft + 4, Console.CursorTop);
                            positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        } while (m_Grille[positionActuelle[1] / 3 - 1, positionActuelle[0] / 4 - 1].Ouvert && m_Grille[positionActuelle[1] / 3 - 1, positionActuelle[0] / 4 - 1].Value == 0);
                        break;
                    case 38: // up arrow
                        do
                        {
                            if (Console.CursorTop < 8)
                                Console.SetCursorPosition(Console.CursorLeft, (iLig * 3) + 2);
                            else
                                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 3);
                            positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        } while (m_Grille[positionActuelle[1] / 3 - 1, positionActuelle[0] / 4 - 1].Ouvert && m_Grille[positionActuelle[1] / 3 - 1, positionActuelle[0] / 4 - 1].Value == 0) ;
                        break;
                    case 40: // down arrow
                        do { 
                            if (Console.CursorTop > iLig * 3)
                                Console.SetCursorPosition(Console.CursorLeft, 5);
                            else
                                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 3);
                            positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        } while (m_Grille[positionActuelle[1] / 3 - 1, positionActuelle[0] / 4 - 1].Ouvert && m_Grille[positionActuelle[1] / 3 - 1, positionActuelle[0] / 4 - 1].Value == 0);
                        break;
                    case 13: // enter key
                        break;
                    case 70: // f pour controler avec fleches                                          
                        InterfaceUsager.Saisie = true;
                        InterfaceUsager.DessinerGrille(iCol, iLig, tab);
                        break;
                    case 67: // c pour coordonnées manuelles    
                        
                        InterfaceUsager.Saisie = false;
                        InterfaceUsager.PositionnerCursorPourRepondre();
                        break;
                    case 65: // a pour aciver l'intelligence artificiel
                        break;
                }


                if (InterfaceUsager.Saisie)
                {
                    InterfaceUsager.PositionnerCursorPourRepondre();
                    Console.Write(positionActuelle[0] / 4 + " " + positionActuelle[1] / 3 + "    ");
                    Console.SetCursorPosition(positionActuelle[0], positionActuelle[1]);
                }
                else
                {                  
                    InterfaceUsager.PositionnerCursorPourRepondre();
                    Console.Write("       ");
                    InterfaceUsager.PositionnerCursorPourRepondre();
                    entreeUtilisateur = Console.ReadLine();
                    MatchCollection matches = rx.Matches(entreeUtilisateur);
                    if (entreeUtilisateur == "f")
                    {
                        Console.Clear();
                        InterfaceUsager.Saisie = true;
                        InterfaceUsager.DessinerGrille(iCol, iLig, tab);
                        Console.SetCursorPosition(6, 5);
                    }
                    else if (matches.Count == 1) {
                        positionActuelle[0] = Int32.Parse(entreeUtilisateur.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]) * 4 + 2;
                        positionActuelle[1] = Int32.Parse(entreeUtilisateur.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) * 3 + 2;
                        Console.SetCursorPosition(positionActuelle[0], positionActuelle[1]);
                        return positionActuelle;
                    }
                }

            } while (touche.Key != ConsoleKey.Enter);
            return positionActuelle; 
        }

        void VerificationSelection(int[] selection) {
            int[] cible = new int[2];
            cible[1] = selection[1] / 3 - 1;
            cible[0] = selection[0] / 4 - 1;

            if (!m_Grille[cible[1], cible[0]].Ouvert) // N'ouvre pas case déjà ouverte.
            {

                if (m_Grille.OuvrirCase(cible[1], cible[0]) == false)//modifier pour permettre le game over
                {
                    m_Grille.DecouvrirBombes();
                    enMarche = false;
                }
            }
<<<<<<< Updated upstream
=======
           /* else
            {
                if (m_Grille[cible[1], cible[0]].Value == 0)
                {

                }
            }*/
>>>>>>> Stashed changes
        }

        public string ObtenirMetadonneesDeLaPartieActuellementTerminee()
        {
            return null;
        }
    }
}
