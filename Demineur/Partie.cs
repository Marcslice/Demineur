using System;
using System.Collections.Generic;
using System.Text;

namespace Demineur
{
    public class Partie
    {
        Grille m_Grille;
        Joueur m_Joueur;
        IA m_IA;
        string tempsEcoule;
        bool automatique;

        public Partie(char[] optionDePartie)
        {
            switch (optionDePartie[0])
            {
                case '1':
                    m_Grille = new Grille(10, 6, optionDePartie[1]);
                    InterfaceUsager.DessinerGrille(10, 6, "ree", 1, 1);    // ajout ligne, colonne donnée membre grille 
                  //  CestUnDepart();
                    break;

                case '2':
                    m_Grille = new Grille(16, 8, optionDePartie[1]);
                    InterfaceUsager.DessinerGrille(16, 8, "ree", 1, 1);    
                  //  CestUnDepart();
                    break;

                case '3':
                    m_Grille = new Grille(22, 10, optionDePartie[1]);
                    InterfaceUsager.DessinerGrille(22, 10, "ree", 1, 1);    
                  //  CestUnDepart();
                    break;
            }
        }

        public string ObtenirMetadonneesDeLaPartieActuellementTerminee()
        {
            return null;
        }

        void CestUnDepart()
        {
            //if(case.bombe)
            //case.bombe = false
            //decouvrir case
            //while(playing)
        }
    }
}
