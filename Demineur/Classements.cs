using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.IO;
using System;



namespace Demineur
{
    public class Classements
    {
        List<Joueur> m_ListeJoueurs;

        public Classements()
        {
            m_ListeJoueurs = new List<Joueur>();
            FichierClassement();
            Console.WriteLine("Nous contruisons le classement...");
            Thread.Sleep(2000);
        }

        public void AfficherClassement()//À trier
        {
            Console.Clear();
            Console.WriteLine("Joueur;Facile[p,m,g],Normal[p,m,g],Difficile[p,m,g]\n");
            foreach (Joueur j in m_ListeJoueurs)
                Console.WriteLine(j.ToString());
            
            Console.Write("\nAppuyez sur une touche pour revenir au menu principale.");
            Console.ReadKey();
        }

        public void MettreAJourJoueuer(string NomJoueur, string Temps, string Difficulte)
        {

        }
        /// <summary>
        /// Est appelé lors de la construction de Classement.
        /// Créer la liste de joueur si le fichier text existe.
        /// Créer un fichier text vide si il n'existe pas.
        /// </summary>
        public void FichierClassement(){

            string cheminFichier = @"..\..\..\classement\classement.txt";
                                                               
            if(File.Exists(cheminFichier)){            
                FileStream  fs = File.OpenRead(cheminFichier);
                StreamReader sr = new StreamReader(fs,UTF8Encoding.UTF8);
                string[] tableauScore = new string[9];
                string ligne; 
                while ((ligne = sr.ReadLine()) != null){
                    string nomJoueur = ligne.Split(';',StringSplitOptions.RemoveEmptyEntries)[0];
                    short index = 0;
                    foreach(string note in ligne.Substring(nomJoueur.Length+1).Split(',',9,StringSplitOptions.RemoveEmptyEntries)){                      
                        tableauScore[index] = note;
                        index++;
                    }
                    m_ListeJoueurs.Add(new Joueur(nomJoueur, tableauScore));                  
                }
            } 
            else
                File.Create(cheminFichier);
        }
    }
}
