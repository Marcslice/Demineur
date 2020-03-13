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
        int nombreCouts = 0;
        //Joueur m_Joueur;
        //IA m_IA;
        //string tempsEcoule;
        //bool automatique;

        public Partie(short[] optionDePartie)
        {
            enMarche = true;
            m_Grille = new Grille(optionDePartie[0], optionDePartie[1], optionDePartie[2]); //Ajouter AI plus tard
            InterfaceUsager.DessinerGrille(optionDePartie[0], optionDePartie[1], m_Grille.ToString());
            VerificationSelection(selection = Cout(optionDePartie[1], optionDePartie[0], m_Grille.ToString(), 6, 5));
            while (enMarche)
            {
                InterfaceUsager.DessinerGrille(optionDePartie[0], optionDePartie[1], m_Grille.ToString());
                VerificationSelection(selection = Cout(optionDePartie[1], optionDePartie[0], m_Grille.ToString(), selection[0], selection[1]));
            }
            InterfaceUsager.DessinerGrille(optionDePartie[0], optionDePartie[1], m_Grille.ToString());//dessine la grille on game over
        }

        public static int[] Cout(int iCol, int iLig, string tab, int xActuel, int yActuel)
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

                        if (Console.CursorLeft < 10)
                            Console.SetCursorPosition((iCol * 4) + 2, Console.CursorTop);
                        else
                            Console.SetCursorPosition(Console.CursorLeft - 4, Console.CursorTop);

                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        break;
                    case 39: // right arrow
                        if (Console.CursorLeft > (iCol * 4))
                            Console.SetCursorPosition(6, Console.CursorTop);
                        else
                            Console.SetCursorPosition(Console.CursorLeft + 4, Console.CursorTop);

                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        break;
                    case 38: // up arrow
                        if (Console.CursorTop < 8)
                            Console.SetCursorPosition(Console.CursorLeft, (iLig * 3) + 2);
                        else
                            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 3);

                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        break;
                    case 40: // down arrow
                        if (Console.CursorTop > iLig * 3)
                            Console.SetCursorPosition(Console.CursorLeft, 5);
                        else
                            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 3);

                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
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
                    Console.Write("       ");
                    InterfaceUsager.PositionnerCursorPourRepondre();
                    entreeUtilisateur = Console.ReadLine();
                    if (entreeUtilisateur == "f")
                    {
                        Console.Clear();
                        InterfaceUsager.Saisie = true;
                        InterfaceUsager.DessinerGrille(iCol, iLig, tab);
                        Console.SetCursorPosition(6, 5);
                    }                        
                }

            } while (touche.Key != ConsoleKey.Enter);
            return positionActuelle; 
        }

        void VerificationSelection(int[] selection) {
            if (m_Grille.OuvrirCase(selection[1] / 3 - 1, selection[0] / 4 - 1) == false)//modifier pour permettre le game over
            {
                m_Grille.DecouvrirBombes();
                enMarche = false;
            }
        }

        public string ObtenirMetadonneesDeLaPartieActuellementTerminee()
        {
            return null;
        }
    }
}
