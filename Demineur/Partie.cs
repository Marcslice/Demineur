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
            switch ((short)optionDePartie[0])
            {
                case 1:
                    m_Grille = new Grille(10, 6, (short)optionDePartie[1]);
                    CestUnDepart();
                    break;

                case 2:
                    m_Grille = new Grille(16, 8, (short)optionDePartie[1]);
                    CestUnDepart();
                    break;

                case 3:
                    m_Grille = new Grille(22, 10, (short)optionDePartie[1]);
                    CestUnDepart();
                    break;
            }
        }

        public string ObtenirMetadonneesDeLaPartieActuellementTerminee()
        {
            return null;
        }

        void CestUnDepart()
        {
            m_Grille.DisperserBombes();
            //if(case.bombe)
            //case.bombe = false
            //decouvrir case
            //while(playing)
        }
    }
}
