using System;
using System.Collections.Generic;
using System.Text;

namespace Demineur
{
    public class Case
    {
        bool bombe;
        char nbDanger;
        bool[] casesVoisines;

        public Case()
        {

        }

        public bool EsTuBombe()
        {
            return true;
        }

        public char CombienDanger()
        {
            return '0';
        }
    }
}
