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

            m_Classement = new Classements();

            switch (Menu.AfficherMenu()) {
                case 1 :
                    m_Partie = new Partie(Menu.OptionDePartie());
                    break;
                case 2  :
                    Menu.AfficherClassement();
                    break;
                case 3:
                    break;
                case 4:
                    Menu.AfficherMenu();
                    break;
            }
        }



        static void Main()
        {
            Demineur m_Demineur = new Demineur();
        }
    }
}
