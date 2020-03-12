using System.Collections.Generic;
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

        }

        public void AfficherClassement()//top 10 par difficulte
        {

        }

        public void MettreAJour(string NomJoueur, string Temps, string Difficulte)
        {

        }

        public void LireFichierClassement(){
            string cheminFichier = @".\classement\classement.txt";
            if(File.Exists(cheminFichier)){
                FileStream fs = File.OpenRead(cheminFichier);
                StreamReader sr = new StreamReader(fs);
                Console.WriteLine(sr.ReadLine());
            } 
            else{
                //create File and load empty list
                FileStream fs = File.Create(cheminFichier);
                m_ListeJoueurs = new List<Joueur>();
            }

        }
    }
}
