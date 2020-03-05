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
            m_Scores = new string[9] {null, null , null, null, null, null, null, null, null};
        }

        public string ObtenirNom()
        {
            return null;
        }

        public string[] ObtenirScore(string p_Nom)
        {
            return null;
        }

        public bool ModifierScore(char grosseur, char difficulte, string temps)
        {
            return true;
        }

        public string ToString()
        {
            return null;
        }
    }
}
