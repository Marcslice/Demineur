using System;
using System.Collections.Generic;
using System.Text;

namespace Demineur
{
    public class Joueur
    {
        string m_Nom;
        string[] m_Scores;

        public Joueur(string p_Nom)
        {
            m_Nom = p_Nom;
            m_Scores = new string[] {null,null,null,null,null,null,null,null,null};
        }

        public Joueur(string p_Nom, string[] p_Scores){
            m_Nom = p_Nom;
            m_Scores = new string[9] { p_Scores[0], p_Scores[1], p_Scores[2], p_Scores[3], p_Scores[4], p_Scores[5], p_Scores[6], p_Scores[7], p_Scores[8] };
        }

        public string ObtenirNom()
        {
            return this.m_Nom;
        }

        public string[] ObtenirScore()
        {
            return m_Scores;
        }

        public bool ModifierScore(char grosseur, char difficulte, string temps)
        {
            
            return true;
        }

        public override string ToString()
        {
            return this.m_Nom + ";" + m_Scores[0] + "," + m_Scores[1] + ',' + m_Scores[2] + ',' +
                m_Scores[3] + ',' + m_Scores[4] + ',' + m_Scores[5] + ',' + m_Scores[6] + ',' + m_Scores[7] + ',' + m_Scores[8];
        }
    }
}
