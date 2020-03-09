using System;
using System.Collections.Generic;
using System.Text;

namespace Demineur
{
    public static class Menu
    {
        public static int[] AfficherMenu()
        {
            int[] optionsDePartie = new int[] {'0','0','0'};
            char choix;
            do {
                Console.WriteLine("##########################################################");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                         MENU                           #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#          Bienvenue dans l'ultime expérience            #");
                Console.WriteLine("#                 Démineur du JUGEMENT                   #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                 1.  JOUEUR                             #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                 2.  AFFICHER CLASSEMENT                #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                 3.  Quitter                            #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("##########################################################");
                Console.Write("Quel est votre choix ? >> ");
                choix = Console.ReadLine()[0];
            } while (choix != '1' && choix != '2' && choix != '3');

            switch (choix)
            {
                case '1':
                    MenuJouerGrosseur(optionsDePartie);
                    MenuJouerDifficulte(optionsDePartie);
                    MenuJouerAI(optionsDePartie);
                    return optionsDePartie;
                case '2':
                    return AfficherClassement();
                case '3':
                default:
                    return optionsDePartie = new int[3] {'0', '0', '0'};
            }
        }

        public static int[] MenuJouerGrosseur(int[] optionsDePartie) {

            char choix;
            do
            {
                Console.Clear();
                Console.WriteLine("##########################################################");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                   Options de partie                    #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                       GRANDEUR                         #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                 1.  PETIT                              #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                 2.  MOYEN                              #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                 3.  GRAND                              #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                 4.  MENU PRINCIPAL                     #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("##########################################################");

                choix = Console.ReadLine()[0];
            } while (choix != '1' || choix != '2' || choix != '3' || choix != '4');
            optionsDePartie[0] = choix;
            return optionsDePartie;
        }

        public static int[] MenuJouerDifficulte(int[] optionsDePartie)
        {

            char choix;
            do
            {
                Console.Clear();
                Console.WriteLine("##########################################################");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                   Options de partie                    #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                      DIFFICULTÉ                        #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                 1.  FACILE                             #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                 2.  NORMAL                             #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                 3.  DIFFICILE                          #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                 4.  MENU PRINCIPAL                     #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("##########################################################");

                choix = Console.ReadLine()[0];
            } while (choix != '1' || choix != '2' || choix != '3' || choix != '4');
            optionsDePartie[1] = choix;
            return optionsDePartie;
        }

        public static int[] MenuJouerAI(int[] optionsDePartie)
        {

            char choix;
            do
            {
                Console.Clear();
                Console.WriteLine("##########################################################");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                   Options de partie                    #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#           ASSISTANT INTELLIGENCE ARTIFICIEL            #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                 1.  SANS                               #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                 2.  AVEC                               #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                 3.  AUTOMATIQUE                        #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("#                 4.  MENU PRINCIPAL                     #");
                Console.WriteLine("#                                                        #");
                Console.WriteLine("##########################################################");

                choix = Console.ReadLine()[0];
            } while (choix != '1' || choix != '2' || choix != '3' || choix != '4');
            optionsDePartie[2] = choix;
            return optionsDePartie;
        }

        public static int[] AfficherClassement() {
            return null;
        }

        public static char[] OptionDePartie()//Grandeur [P 10x6:1,M 16x8:2,G 22x10:3], Difficulter [EZ:0.1,Medium:0.2,TOUGH:Sizex0.3], AI[T,F,A]
        {
            return null;
        }

        public static string DemanderNom()
        {
            string nom;

            do
                nom = Console.ReadLine();
            while (nom.Length >= 3);
            return nom;
        }
    }
}
