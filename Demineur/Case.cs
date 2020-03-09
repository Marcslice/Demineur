using System;
using System.Collections.Generic;
using System.Text;

namespace Demineur
{
    public class Case
    {
        bool esTuBombe;
        short nbDanger;
        Case[] casesVoisines;

        public Case()
        {
            esTuBombe = false;
            nbDanger = 0;
            casesVoisines = new Case[8]{null, null, null, null, null, null, null, null};
        }

        public short Value
        {
            get { CombienDanger();
                return nbDanger; }
        }

        public bool Bombe
        {
            set { esTuBombe = value; }
            get { return esTuBombe; }
        }

        public Case this[int i]
        {
            set {casesVoisines[i] = value; }
        }

        public void CombienDanger()
        {
            for (int i = 0; i < 8; i++)
            {
                if(casesVoisines[i].Bombe)
                {
                    nbDanger++;
                }
            }
        }

    }
}
