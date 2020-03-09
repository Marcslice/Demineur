using System;

namespace Demineur
{
    class Demineur
    {
        bool m_EnMarche;
        Partie m_Partie;
        Classements m_Classement;

        Demineur()
        {
            m_EnMarche = true;
            //m_Partie = new Partie();

            m_Classement = new Classements();
            Menu.AfficherMenu();
            m_Partie = new Partie(Menu.OptionDePartie());
        }



        static void Main()
        {
            Demineur m_Demineur = new Demineur();
        }
    }
}
