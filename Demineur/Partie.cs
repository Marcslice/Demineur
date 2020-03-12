﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Demineur
{
    public class Partie
    {
        Grille m_Grille;
        bool enMarche;
        int[] prochainePosition = new int[2];
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
            prochainePosition = Cout(optionDePartie[0], optionDePartie[1], m_Grille.ToString(), 6, 5);

            if (m_Grille[prochainePosition[0], prochainePosition[1]].Bombe) // sauve le joueur au premier tour
                m_Grille[prochainePosition[0], prochainePosition[1]].Bombe = false;

            while (enMarche)
            {
                VerifierEntree(prochainePosition = Cout(optionDePartie[0], optionDePartie[1], m_Grille.ToString(), prochainePosition[0], prochainePosition[1]));
            }
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
                        positionActuelle = new int[2] { Console.CursorLeft, Console.CursorTop };
                        return positionActuelle;

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
                    else;                              
                }

            } while (touche.Key != ConsoleKey.Enter);
            return null; // filtered data
        }


        public bool VerifierEntree(int[] entree)
        {
            Case selection = m_Grille[entree[0], entree[1]];
            if (selection.estTuOuverte())
                return false;
            else
            {
                selection.Ouvert = true;
                return true;
            }
        }

        public string ObtenirMetadonneesDeLaPartieActuellementTerminee()
        {
            return null;
        }
    }
}
