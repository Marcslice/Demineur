namespace Demineur
{
    /// <summary>
    /// Représente une case du démineur. Le grille génére sa grille en créant "Lignes X Colonne" cases. 
    /// Bool esTuBombe détermine si un case est une bombe ou non.
    /// Bool estOuverte détermine si la case est ouverte ou non.
    /// int nbDanger détermine combien de bombe entoure la case ouverte.
    /// Case[] casesVoisines contient les cases qui entourent cette case.
    /// </summary>
    public class Case
    {
        bool esTuBombe, estOuverte;
        int nbDanger;
        Case[] casesVoisines;

        /// <summary>
        /// Construit une case fermé sans bombe avec un danger par défaut de 0.
        /// Case 0 = NW | 1 = N | 2 = NE | 3 = W | 4 = E | 5 = SW | 6 = S | 7 = SE
        /// </summary>
        public Case()
        {
            esTuBombe = false;
            estOuverte = false;
            nbDanger = 0;
            casesVoisines = new Case[8] { null, null, null, null, null, null, null, null };
        }

        /// <summary>
        /// Retourne la valeur numérique du danger autour de la case.
        /// </summary>
        public int Value
        {
            get { return nbDanger; }
            set { nbDanger = value; }
        }

        /// <summary>
        /// Retourne si la case contient une bombe.
        /// </summary>
        public bool Bombe
        {
            set { esTuBombe = value; }
            get { return esTuBombe; }
        }

        /// <summary>
        /// Retourne l'état de la case. (Ouverte/Fermé)
        /// </summary>
        public bool Ouvert
        {
            set { estOuverte = value; }
            get { return estOuverte; }
        }

        /// <summary>
        /// Permet de naviguer les voisins d'une case.
        /// </summary>
        /// <param name="i">index</param>
        /// <returns>Case : Retourne une case</returns>
        public Case this[int i]
        {
            get { return casesVoisines[i]; }
            set { casesVoisines[i] = value; }
        }

        /// <summary>
        /// Affecte au tableau casesVoisines les voisins de la case active.
        /// </summary>
        /// <param name="i">Index du voisin</param>
        /// <param name="voisin">Case voisine</param>
        public void SetCase(int i, Case voisin)
        {
            this.casesVoisines[i] = voisin;
        }

        /// <summary>
        /// Parcour les voisin d'une case pour affecter la valeur de danger à la case active.
        /// </summary>
        /// <returns>Int : nbDanger</returns>
        public int CalculerDanger()
        {
            nbDanger = 0;
            for (int i = 0; i < 8; i++)
                if (casesVoisines[i] != null)
                    if (casesVoisines[i].Bombe)
                        nbDanger++;

            return nbDanger;
        }
    }
}
