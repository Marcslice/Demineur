using System;
using System.Collections.Generic;
using System.Globalization;
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
        }

        /// <summary>
        /// Est appelé si le joueur gagne une partie.
        /// S'il bat son record, le temps sera mis à jour.
        /// Si le joueur n'existait pas déjà, il sera créé.
        /// index est le calcule permettant de passer d'un tableau 2D en tableau 1D,
        /// le calcule permet de modifier le bon score dans le tableau de score du joueur.
        /// </summary>
        /// <param name="info">info[] {nom, grosseur, difficulté, tempsEnMinutes}</param>
        public void MettreAJourJoueur(string[] info)
        {
            Joueur aModifier;

            int index = ((Int32.Parse(info[1]) - (Int32.Parse(info[1]) / 2 + 3))) * 3 + (Int32.Parse(info[2]) - 1);

            if ((aModifier = m_ListeJoueurs.Find(j => j.ObtenirNom() == info[0])) != null)
                aModifier.ModifierScore(index, info[3]);
            else
                m_ListeJoueurs.Add(new Joueur(info[0], index, info[3]));
        }

        /// <summary>
        /// Est appelé lors de la construction de Classement.
        /// Créer la liste de joueur si le fichier text existe.
        /// Créer un fichier text vide si il n'existe pas.
        /// </summary>
        void FichierClassement()
        {

            string cheminFichier = @"..\..\..\classement\classement.txt"; // en mode visual studio
            //string cheminFichier = @".\classement\classement.txt"; // en mode exe

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
        /// Tri le tableau de joueur selon le tri demandé et le retourne
        /// au toString() pour créer ce qui sera affiché à l'écran.
        /// Les joueurs dont le score est de 00.00 ne sera pas afficher pour une catégorie donnée.
        /// Seulement les 10 premiers joueur d'une catégorie seront affichés.
        /// </summary>
        /// <param name="indexDifficulte">
        /// 0, 3, 6 sont les scores en mode facile dans le tableau de joueur.
        /// 1,4,7 sont les scores en mode Normal dans le tableau de joueur.
        /// 2,5,8 sont les scores en mode Difficile dans le tableau de joueur.
        /// </param>
        /// <returns>List trié de joueur.</returns>
        List<Joueur> trierClassement(short indexDifficulte)
        {
            List<Joueur> listTrier = new List<Joueur>(m_ListeJoueurs);
            for (int x = 0; x < listTrier.Count - 1; x++)
            {
                for (int y = x + 1; y < listTrier.Count; y++)
                {
                    if (Double.Parse(listTrier[x].ObtenirScore()[indexDifficulte], CultureInfo.InvariantCulture) > Double.Parse(listTrier[y].ObtenirScore()[indexDifficulte], CultureInfo.InvariantCulture))
                    {
                        Joueur aBouger = listTrier[x];
                        listTrier[x] = listTrier[y];
                        listTrier[y] = aBouger;
                    }
                }
            }

            while (listTrier.Count > 0 && listTrier[0].ObtenirScore()[indexDifficulte] == "00.00")
                listTrier.RemoveAt(0);

            if (listTrier.Count < 10 && listTrier.Count > 0)
                return listTrier.GetRange(0, listTrier.Count);
            else if (listTrier.Count > 9)
                return listTrier.GetRange(0, 9);
            else
                return listTrier;
        }

        /// <summary>
        /// Écrit dans le document classement.txt
        /// </summary>
        public void SauvegardeDuClassement()
        {
            string cheminFichier = @"..\..\..\classement\classement.txt"; // en mode visual studio
            //string cheminFichier = @".\classement\classement.txt"; // en mode exe
            FileStream fs = File.OpenWrite(cheminFichier);
            StreamWriter sw = new StreamWriter(fs, UTF8Encoding.UTF8);
            foreach (Joueur j in m_ListeJoueurs)
                sw.WriteLine(j.ToString());

            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// Retourne le classement sous forme de string
        /// </summary>
        /// <returns>string : classement</returns>
        public string ToString(short tri)
        {
            string classement = "";
            switch (tri)
            {
                case 0:
                    foreach (Joueur j in m_ListeJoueurs)
                        classement += j.FormatClassement() + "\n";
                    return classement;
                case 1:
                    for (short x = 0; x < 7; x += 3)
                    {
                        switch (x)
                        {
                            case 0:
                                classement += "\n                                       Facile et Petite grille                        \n\n";
                                break;
                            case 3:
                                classement += "\n                                       Facile et Moyenne grille                       \n\n";
                                break;
                            case 6:
                                classement += "\n                                       Facile et Grande grille                        \n\n";
                                break;
                        }

                        foreach (Joueur j in trierClassement(x))
                            classement += j.FormatClassement() + "\n";
                        classement += "\n\n";
                    }
                    return classement;
                case 2:
                    for (short x = 1; x < 8; x += 3)
                    {
                        switch (x)
                        {
                            case 1:
                                classement += "\n                                       Normal et Petite grille                        \n\n";
                                break;
                            case 4:
                                classement += "\n                                       Normal et Moyenne grille                       \n\n";
                                break;
                            case 7:
                                classement += "\n                                       Normal et Grande grille                        \n\n";
                                break;
                        }
                        foreach (Joueur j in trierClassement(x))
                            classement += j.FormatClassement() + "\n";
                        classement += "\n\n";
                    }
                    return classement;
                case 3:
                    for (short x = 2; x < 9; x += 3)
                    {
                        switch (x)
                        {
                            case 2:
                                classement += "\n                                       Difficile et Petite grille                        \n\n";
                                break;
                            case 5:
                                classement += "\n                                       Difficile et Moyenne grille                       \n\n";
                                break;
                            case 8:
                                classement += "\n                                       Difficile et Grande grille                        \n\n";
                                break;
                        }
                        foreach (Joueur j in trierClassement(x))
                            classement += j.FormatClassement() + "\n";
                        classement += "\n\n";
                    }
                    return classement;
            }
            return classement;
        }
    }
}
