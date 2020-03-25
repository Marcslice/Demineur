using System;

namespace Demineur
{
    public static class Menu
    {
        static short[] optionsDePartie; // Sera retourné à Démineur afin qu'il puisse démarrer une partie ayant ces options là.
        static string recap; // Permet d'afficher une récapilation des choix du joueur.
        public static short AfficherMenu()
        {
            char choix;
            optionsDePartie = new short[] { 0, 0, 0, 0 };
            recap = "";

            Console.Clear();
            Console.WriteLine("##########################################################");
            Console.WriteLine("#                                                        #");
            Console.WriteLine("#                         MENU                           #");
            Console.WriteLine("#                                                        #");
            Console.WriteLine("#          Bienvenue dans l'ultime expérience            #");
            Console.WriteLine("#                 Démineur du JUGEMENT                   #");
            Console.WriteLine("#                                                        #");
            Console.WriteLine("#                 1.  JOUER                              #");
            Console.WriteLine("#                                                        #");
            Console.WriteLine("#                 2.  AFFICHER CLASSEMENT                #");
            Console.WriteLine("#                                                        #");
            Console.WriteLine("#                 3.  Quitter                            #");
            Console.WriteLine("#                                                        #");
            Console.WriteLine("##########################################################");
            Console.Write("Quel est votre choix ? >> ");
            do
                choix = Console.ReadKey(true).KeyChar;
            while (choix != '1' && choix != '2' && choix != '3');


            switch (choix)
            {
                case '1': // Jouer
                    if (MenuJouerGrosseur() == 4) // 4 = retour au menu principal
                        return 4;
                    if (MenuJouerDifficulte() == 4)
                        return 4;
                    if (MenuJouerAI() == 4)
                        return 4;
                    if (RecapFinal() == 4)
                        return 4;
                    return 1;
                case '2': // Afficher classement
                    return 2;
                case '3': // Quitter
                default:
                    return 3;
            }
        }

        static short MenuJouerGrosseur()
        {

            char choix;

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
            Console.WriteLine("#                                                        #");
            Console.WriteLine("#                                                        #");
            Console.WriteLine("##########################################################");
            Console.Write("Quel est votre choix ? >> ");
            do
                choix = Console.ReadKey(true).KeyChar;
            while (choix != '1' && choix != '2' && choix != '3' && choix != '4');

            switch (choix)
            {
                case '1':
                    optionsDePartie[0] = 6; // ligne
                    optionsDePartie[1] = 10;  // colonne
                    break;
                case '2':
                    optionsDePartie[0] = 8;
                    optionsDePartie[1] = 16;
                    break;
                case '3':
                    optionsDePartie[0] = 10;
                    optionsDePartie[1] = 22;
                    break;
            }
            return Int16.Parse(choix.ToString());
        }

        static short MenuJouerDifficulte()
        {

            char choix;

            switch (optionsDePartie[1])
            { //check nb colonne
                case 10:
                    recap += "PETIT ";
                    break;
                case 16:
                    recap += "MOYEN ";
                    break;
                case 22:
                    recap += "GRAND ";
                    break;
            }

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
            Console.Write("#        Options : " + recap); for (short x = 1; x < (58 - (19 + recap.Length)); x++) { Console.Write(" "); }
            Console.WriteLine("#"); // Ajustement auto
            Console.WriteLine("#                                                        #");
            Console.WriteLine("##########################################################");
            Console.Write("Quel est votre choix ? >> ");
            do
                choix = Console.ReadKey(true).KeyChar;
            while (choix != '1' && choix != '2' && choix != '3' && choix != '4');
            return optionsDePartie[2] = Int16.Parse(choix.ToString());
        }
        static short MenuJouerAI()
        {

            char choix;

            switch (optionsDePartie[2])
            {
                case 1:
                    recap += ", FACILE ";
                    break;
                case 2:
                    recap += ", NORMAL ";
                    break;
                case 3:
                    recap += ", DIFFICILE ";
                    break;
            }

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
            Console.Write("#        Options : " + recap); for (short x = 1; x < (58 - (19 + recap.Length)); x++) { Console.Write(" "); }
            Console.WriteLine("#"); // Ajustement auto
            Console.WriteLine("#                                                        #");
            Console.WriteLine("##########################################################");
            Console.Write("Quel est votre choix ? >> ");
            do
                choix = Console.ReadKey(true).KeyChar;
            while (choix != '1' && choix != '2' && choix != '3' && choix != '4');
            return optionsDePartie[3] = Int16.Parse(choix.ToString());
        }

        static short RecapFinal()
        {

            char choix;

            switch (optionsDePartie[3])
            {
                case 1:
                    recap += ", SANS AI ";
                    break;
                case 2:
                    recap += ", AVEC AI ";
                    break;
                case 3:
                    recap += ", AUTOMATIQUE ";
                    break;
            }

            Console.Clear();
            Console.WriteLine("##########################################################");
            Console.WriteLine("#                                                        #");
            Console.WriteLine("#                    Début de partie                     #");
            Console.WriteLine("#                                                        #");
            Console.WriteLine("#                                                        #");
            Console.WriteLine("#     Vous êtez sur le point de commencer la partie.     #");
            Console.WriteLine("#                                                        #");
            Console.WriteLine("#                                                        #");
            Console.WriteLine("#               Appuyez sur 4 pour annuler               #");
            Console.WriteLine("#                          ou                            #");
            Console.WriteLine("#                   sur C pour commencer.                #");
            Console.WriteLine("#                                                        #");
            Console.WriteLine("#                                                        #");
            Console.WriteLine("#                                                        #");
            Console.WriteLine("#                                                        #");
            Console.Write("#        Options : " + recap); for (short x = 1; x < (58 - (19 + recap.Length)); x++) { Console.Write(" "); }
            Console.WriteLine("#"); // Ajustement auto
            Console.WriteLine("#                                                        #");
            Console.WriteLine("##########################################################");
            do
                choix = Console.ReadKey(true).KeyChar;
            while (choix != '4' && choix != 'c' && choix != 'C');
            if (choix == '4')
                return 4;
            return 0;
        }

        public static short[] OptionDePartie()
        {
            return optionsDePartie;
        }

        public static void AfficherClassement()
        {
            Console.WriteLine("Classement... Veuillez appuyez sur un touche pour continuer.");
        }

        public static string DemanderNom()
        {
            string nom;

            do
            {
                Console.WriteLine("Le nom doit contenir entre 3 et 10 caractères.");
                Console.Write("Quel nom voulez-vous utiliser ? >> ");
                nom = Console.ReadLine();
            }
            while (nom.Length < 3 && nom.Length > 10);
            return nom;
        }
    }
}
