using System.Collections.Generic;
using System;

namespace Demineur
{
    /// <summary>
    /// Représente un joueur.
    /// string : m_nom est le nom du joueur.
    /// string[] : m_Scores et le tableau des scores(en temps) du joueur.
    /// </summary>
    public class Joueur
    {
        string m_Nom;
        string[] m_Scores;
        public Joueur(string p_Nom, string[] p_Scores)
        {
            m_Nom = p_Nom;
            m_Scores = new string[9] { p_Scores[0], p_Scores[1], p_Scores[2],
                p_Scores[3], p_Scores[4], p_Scores[5], p_Scores[6], p_Scores[7], p_Scores[8] };
        }

        public Joueur(string p_Nom, int index, string score)
        {
            m_Nom = p_Nom;
            m_Scores = new string[] { "00.00", "00.00", "00.00", "00.00", "00.00", "00.00", "00.00", "00.00", "00.00" };
            m_Scores[index] = score;
        }

        /// <summary>
        /// Retourne le nom de joueur
        /// </summary>
        /// <returns>string nom</returns>
        public string ObtenirNom()
        {
            return this.m_Nom;
        }

        /// <summary>
        /// Retourne le tableau des scores du joueur
        /// </summary>
        /// <returns>string[9] scores</returns>
        public string[] ObtenirScore()
        {
            return m_Scores;
        }

        /// <summary>
        /// Modifie le score du joueur si c'est premier essaie victorieux ou s'il a fait un meilleur score.
        /// </summary>
        /// <param name="index">Index à modifier dans le tableau des scores.</param>
        /// <param name="temp">La valeur à remplacer.</param>
        /// <returns></returns>
        public bool ModifierScore(int index, string temp)
        {
            if (Double.Parse(m_Scores[index]) > Double.Parse(temp) || m_Scores[index] == "00.00")
            {
                m_Scores[index] = temp;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Est utilisé pour écrire dans le fichier texte.
        /// </summary>
        /// <returns>string nom_joueur et scores[]</returns>
        public override string ToString()
        {
            return this.m_Nom + ";" + m_Scores[0] + "," + m_Scores[1] + ',' + m_Scores[2] + ',' +
                m_Scores[3] + ',' + m_Scores[4] + ',' + m_Scores[5] + ',' + m_Scores[6] + ',' + m_Scores[7] + ',' + m_Scores[8];
        }

        /// <summary>
        /// Dessine avec espacement automatique le tableau des scores.
        /// </summary>
        /// <returns>string ligne des scores d'un joueur.</returns>
        public string FormatClassement()
        {
            string espacement = "";
            string formatClassement = this.m_Nom + espacement + "   | ";

            for (short x = 1; x <= (10 - this.m_Nom.Length); x++)
                espacement += " ";

            foreach (string s in m_Scores)
            {
                if (s.Length < 5)
                    for (int x = 0; x < (5 - s.Length); x++)
                        formatClassement += "0";
                formatClassement += s + "   ";
            }
            return espacement + formatClassement + "\b\b|";
        }
    }
}
