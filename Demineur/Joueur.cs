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
            m_Scores = new string[9] { p_Scores[0], p_Scores[1], p_Scores[2], 
                p_Scores[3], p_Scores[4], p_Scores[5], p_Scores[6], p_Scores[7], p_Scores[8] };
        }

        public Joueur(string p_Nom, int index, string score) {
            m_Nom = p_Nom;
            m_Scores = new string[] { null, null, null, null, null, null, null, null, null };
            m_Scores[index] = score;
        }

        public string ObtenirNom()
        {
            return this.m_Nom;
        }

        public string[] ObtenirScore()
        {
            return m_Scores;
        }

        public bool ModifierScore(int index, string temp)
        {
            if (Double.Parse(m_Scores[index]) > Double.Parse(temp) || m_Scores[index] == "0" || m_Scores[index] == null) {
                m_Scores[index] = temp;
                return true;
            }
            else
                return false;
        }

        public override string ToString()
        {
            return this.m_Nom + ";" + m_Scores[0] + "," + m_Scores[1] + ',' + m_Scores[2] + ',' +
                m_Scores[3] + ',' + m_Scores[4] + ',' + m_Scores[5] + ',' + m_Scores[6] + ',' + m_Scores[7] + ',' + m_Scores[8];
        }
    }
}
