using System;
using System.Collections.Generic;
using System.IO;
using System.Text;



namespace Demineur
{
    /// <summary>
    /// Classement est une extension de démineur. 
    /// Elle permet de créer et lire le fichier texte contenant les données des joueurs.
    /// </summary>
    public class Classements
    {
        List<Joueur> m_ListeJoueurs;

        public Classements()
        {
            m_ListeJoueurs = new List<Joueur>();
            FichierClassement();
            Console.WriteLine("Nous contruisons le classement...");
        }

        /// <summary>
        /// Est appelé si le joueur bat son record.
        /// index est le calcule permettant de passer d'un tableau 2D en tableau 1D,
        ///     le calcule permet de modifier le bon score dans le tableau de score du joueur.
        /// </summary>
        /// <param name="info">info[] {nom, grosseur, difficulté, tempsEnMinutes}</param>
        public void MettreAJourJoueur(string[] info)
        {
            Joueur aModifier;
            if ((aModifier = m_ListeJoueurs.Find(j => j.ObtenirNom() == info[0])) != null)
            {
                int index = ((Int32.Parse(info[1]) - (Int32.Parse(info[1]) / 2 + 3))) * 3 + (Int32.Parse(info[2]) - 1);
                if (aModifier.ModifierScore(index, info[3]))
                    InterfaceUsager.MessageNouveauRecord();
            }
            else
            {
                m_ListeJoueurs.Add(new Joueur(info[0], Int32.Parse(info[1]) - 1 * 3 + Int32.Parse(info[2]) - 1, info[3]));
                //add sort and filter
            }
        }

        /// <summary>
        /// Est appelé lors de la construction de Classement.
        /// Créer la liste de joueur si le fichier text existe.
        /// Créer un fichier text vide si il n'existe pas.
        /// </summary>
        public void FichierClassement()
        {

            string cheminFichier = @"..\..\..\classement\classement.txt";

            if (File.Exists(cheminFichier))
            {
                FileStream fs = File.OpenRead(cheminFichier);
                StreamReader sr = new StreamReader(fs, UTF8Encoding.UTF8);

                string ligne;
                ligne = sr.ReadLine();
                while (ligne != null && ligne != "")
                {
                    DeStringAJoueur(ligne);
                    ligne = sr.ReadLine();
                }
                sr.Close();
                fs.Close();
            }
            else
            {
                var nouveauClassement = File.Create(cheminFichier);
                nouveauClassement.Close();
            }
        }

        /// <summary>
        /// Est appelé après la partie.
        /// Permet de sauvegarder automatiquement les informations des joueurs du classement.
        /// </summary>
        public void SauvegardeDuClassement()
        {
            string cheminFichier = @"..\..\..\classement\classement.txt";
            FileStream fs = File.OpenWrite(cheminFichier);
            StreamWriter sw = new StreamWriter(fs, UTF8Encoding.UTF8);
            foreach (Joueur j in m_ListeJoueurs)
                sw.WriteLine(j.ToString());

            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// Créer les joueurs à partir du fichier texte.
        /// </summary>
        /// <param name="joueurStats">Ligne lu par le programme.</param>
        void DeStringAJoueur(string joueurStats)
        {
            string nomJoueur = joueurStats.Split(';', StringSplitOptions.RemoveEmptyEntries)[0];
            string[] tableauScore = new string[9];
            short index = 0;
            foreach (string note in joueurStats.Substring(nomJoueur.Length + 1).Split(',', 9, StringSplitOptions.RemoveEmptyEntries))
            {
                tableauScore[index] = note;
                index++;
            }
            m_ListeJoueurs.Add(new Joueur(nomJoueur, tableauScore));
        }

        /// <summary>
        /// Retourne le classement sous forme de string
        /// </summary>
        /// <returns>string : classement</returns>
        public override string ToString()
        {
            string classement = "";

            foreach (Joueur j in m_ListeJoueurs)
                classement += j.FormatClassement() + "\n";
            return classement;
        }
    }
}
