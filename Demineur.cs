using System;
namespace Demineur
{
    /// <summary>
    /// Point d'entrée du programme.
    /// On a une seule instance de Demineur.
    /// Demineur peut créer une seule partie à la fois.
    /// </summary>
    class Demineur
    {

        Partie m_Partie;
        Classements m_Classement;
        Demineur()
        {
            short choix;
            m_Classement = new Classements();

            do
            {
                choix = Menu.AfficherMenu();
                switch (choix)

                {
                    case 1:
                        m_Partie = new Partie(Menu.DemanderNom(), Menu.OptionDePartie());
                        if (m_Partie.CommencerPartie())
                        { // Si partie retourne true, alors le joueur a batu son record.
                            m_Classement.MettreAJourJoueur(m_Partie.InfoDepartie());
                            m_Classement.SauvegardeDuClassement();
                        }
                        break;
                    case 2:
                        short visionnement = 0;
                        do
                            visionnement = (short)Menu.AfficherClassement(m_Classement.ToString(visionnement), visionnement);
                        while (visionnement != 0);
                        break;
                    case 3:
                        break;
                }
            }
            while (choix != 3);
        }

        static void Main()
        {
            Demineur m_Demineur = new Demineur();
        }
    }
}
