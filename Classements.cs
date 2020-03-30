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

            int index = ((Int32.Parse(info[1]) - (Int32.Parse(info[1]) / 2 + 3))) * 3 + (Int32.Parse(info[2]) - 1);

            if ((aModifier = m_ListeJoueurs.Find(j => j.ObtenirNom() == info[0])) != null)
            {
                if (aModifier.ModifierScore(index, info[3]))
                    InterfaceUsager.MessageNouveauRecord();
            }
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
        /// </summary>
        /// <param name="indexFacile">0, 3, 6 sont les scores en mode facile dans le tableau de joueur.</param>
        /// <returns>List trié de joueur.</returns>
        List<Joueur> triFacile(short indexFacile)
        {
            List<Joueur> listTrier = new List<Joueur>(m_ListeJoueurs);

            for (int x = 0; x < listTrier.Count - 1; x++)
            {
                for (int y = x + 1; y < listTrier.Count; y++)
                {
                    if (Double.Parse(listTrier[x].ObtenirScore()[indexFacile]) > Double.Parse(listTrier[y].ObtenirScore()[indexFacile]) && listTrier[y].ObtenirScore()[indexFacile] != "00.00")
                    {
                        Joueur aBouger = listTrier[x];
                        listTrier[x] = listTrier[y];
                        listTrier[y] = aBouger;
                    }
                    if (listTrier[x].ObtenirScore()[indexFacile] == "00.00")
                    {
                        listTrier.RemoveAt(x);
                        y -= 1;
                    }
                }
            }
            if (listTrier.Count == 1)
                if (listTrier[0].ObtenirScore()[indexFacile] == "00.00")
                    listTrier.RemoveAt(0);

            if (listTrier.Count < 10 && listTrier.Count > 0)
                return listTrier.GetRange(0, listTrier.Count - 1);
            else if (listTrier.Count > 9)
                return listTrier.GetRange(0, 9);
            else
                return listTrier;
        }

        /// <summary>
        /// Tri le tableau de joueur selon le tri demandé et le retourne
        /// au toString() pour créer ce qui sera affiché à l'écran.
        /// </summary>
        /// <param name="indexFacile">1, 4, 7 sont les scores en mode Normal dans le tableau de joueur.</param>
        /// <returns>List trié de joueur.</returns>
        List<Joueur> triNormal(short indexNormal)
        {
            List<Joueur> listTrier = new List<Joueur>(m_ListeJoueurs);

            for (int x = 0; x < listTrier.Count - 1; x++)
            {
                for (int y = x + 1; y < listTrier.Count; y++)
                {
                    if (Double.Parse(listTrier[x].ObtenirScore()[indexNormal]) > Double.Parse(listTrier[y].ObtenirScore()[indexNormal]) && listTrier[y].ObtenirScore()[indexNormal] != "00.00")
                    {
                        Joueur aBouger = listTrier[x];
                        listTrier[x] = listTrier[y];
                        listTrier[y] = aBouger;
                    }

                    if (listTrier[x].ObtenirScore()[indexNormal] == "00.00")
                    {
                        listTrier.RemoveAt(x);
                        y -= 1;
                    }
                }
            }
            if (listTrier.Count == 1)
                if (listTrier[0].ObtenirScore()[indexNormal] == "00.00")
                    listTrier.RemoveAt(0);


            if (listTrier.Count < 10 && listTrier.Count > 0)
                return listTrier.GetRange(0, listTrier.Count - 1);
            else if (listTrier.Count > 9)
                return listTrier.GetRange(0, 9);
            else
                return listTrier;
        }

        /// <summary>
        /// Tri le tableau de joueur selon le tri demandé et le retourne
        /// au toString() pour créer ce qui sera affiché à l'écran.
        /// </summary>
        /// <param name="indexFacile">2, 5, 8 sont les scores en mode Difficile dans le tableau de joueur.</param>
        /// <returns>List trié de joueur.</returns>
        List<Joueur> triDifficile(short indexDifficile)
        {
            List<Joueur> listTrier = new List<Joueur>(m_ListeJoueurs);

            for (int x = 0; x < listTrier.Count - 1; x++)
            {
                for (int y = x + 1; y < listTrier.Count; y++)
                {
                    if (Double.Parse(listTrier[x].ObtenirScore()[indexDifficile]) > Double.Parse(listTrier[y].ObtenirScore()[indexDifficile]) && listTrier[y].ObtenirScore()[indexDifficile] != "00.00")
                    {
                        Joueur aBouger = listTrier[x];
                        listTrier[x] = listTrier[y];
                        listTrier[y] = aBouger;
                    }
                    if (listTrier[x].ObtenirScore()[indexDifficile] == "00.00")
                    {
                        listTrier.RemoveAt(x);
                        y -= 1;
                    }
                }
            }
            if (listTrier.Count == 1)
                if (listTrier[0].ObtenirScore()[indexDifficile] == "00.00")
                    listTrier.RemoveAt(0);

            if (listTrier.Count < 10 && listTrier.Count > 0)
                return listTrier.GetRange(0, listTrier.Count - 1);
            else if (listTrier.Count > 9)
                return listTrier.GetRange(0, 9);
            else
                return listTrier;
        }

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
        /// Retourne le classement sous forme de string
        /// </summary>
        /// <returns>string : classement</returns>
        ///         /// <summary>
        /// Est appelé après la partie.
        /// Permet de sauvegarder automatiquement les informations des joueurs du classement.
        /// </summary>
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
                        foreach (Joueur j in triFacile(x))
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
                        foreach (Joueur j in triNormal(x))
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
                        foreach (Joueur j in triDifficile(x))
                            classement += j.FormatClassement() + "\n";
                        classement += "\n\n";
                    }
                    return classement;
            }
            return classement;
        }
    }
}
