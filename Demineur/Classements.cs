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
            LireFichierClassement();
            Console.WriteLine("Nous contruisons le classement...");
            Thread.Sleep(2000);
        }

        public void AfficherClassement()//top 10 par difficulte
        {

        }

        public void MettreAJourJoueuer(string NomJoueur, string Temps, string Difficulte)
        {

        }

        public void LireFichierClassement(){

            string cheminFichier = @"..\..\..\classement\classement.txt";
            m_ListeJoueurs = new List<Joueur>();
            string[] tableauScore = new string[9];
            StreamReader sr;
            FileStream fs;
            string ligne;           
                      
            if(File.Exists(cheminFichier)){
                fs = File.OpenRead(cheminFichier);
                sr = new StreamReader(fs,UTF8Encoding.UTF8);
                while ((ligne = sr.ReadLine()) != null){
                    string nomJoueur = ligne.Split(';',StringSplitOptions.RemoveEmptyEntries)[0];
                    short count = 0;
                    foreach(string note in ligne.Substring(nomJoueur.Length+1).Split(',',9,StringSplitOptions.RemoveEmptyEntries)){                      
                        tableauScore[count] = note;
                        count++;
                    }
                    m_ListeJoueurs.Add(new Joueur(nomJoueur, tableauScore));                  
                }
            } 
            else
                File.Create(cheminFichier);
        }
    }
}
