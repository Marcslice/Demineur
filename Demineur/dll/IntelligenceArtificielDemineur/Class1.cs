using System;

namespace IntelligenceArtificielDemineur
{
    public class IA
    {

        short m_Lignes, m_Colonnes;
        public IA(short p_Lignes, short p_Colonnes) {
            m_Lignes = p_Lignes;
            m_Colonnes = p_Colonnes;
        }

        public void TourIA(string grille) {
            Console.WriteLine("l'ia a joué.");
        }
    }
}
