using System;
using System.Collections.Generic;
using System.Text;

namespace Demineur
{
    public class Case
    {
        bool esTuBombe;
        bool estOuverte;
        short nbDanger;
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
        public short Value
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

        public bool Ouvert
        {
            set { estOuverte = value; }
            get { return estOuverte; }
        }

        public bool estTuOuverte() {
            return estOuverte;
        }

        // public Case this[int i]
        // {
        //     set {casesVoisines[i] = value; }
        // }

        /// <summary>
        /// Affecte au tableau casesVoisines les voisins de la case active.
        /// </summary>
        /// <param name="i">Index du voisin</param>
        /// <param name="voisin">Case voisine</param>
        public void SetCase(int i, Case voisin) {
            this.casesVoisines[i] = voisin;
        }

        /*public void CombienDanger()
        {
            for (int i = 0; i < 8; i++)
            {
                if(casesVoisines[i].Bombe)
                {
                    nbDanger++;
                }
            }
        }*/
    }
}
