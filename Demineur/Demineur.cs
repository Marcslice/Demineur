using System;

namespace Demineur
{
    class Demineur
    {

        Partie m_Partie;
        Classements m_Classement;

        Demineur()
        {
            short choix;
            m_Classement = new Classements();
        
            do
                choix = Menu.AfficherMenu();
            while (choix == 4);
            
            switch (choix)
            {
                case 1:
                    m_Partie = new Partie(Menu.OptionDePartie());     
                    break;
                case 2:
                    Menu.AfficherClassement();
                    break;
                case 3:
                    break;
            }
        }

        static void Main()
        {
            Demineur m_Demineur = new Demineur();
        }
    }
}
