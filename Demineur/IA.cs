﻿using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using System;



namespace Demineur
{
    public class IA
    {
        FileInfo dll;
        Type[] classes;
        object[] ligCol;
        dynamic bot;
        public IA(short lignes, short colonnes) {
            dll = new FileInfo(@"..\..\..\dll\IntelligenceArtificielDemineur.dll"); 
            classes = Assembly.LoadFile(dll.FullName).GetTypes(); // Load les classes du dll.
            ligCol = new object[2] {lignes,colonnes}; // Envoyer au constructeur de l'IA externe.
            bot = Activator.CreateInstance(classes[0],ligCol); //classes[0] pour la premiere classe du dll.     
        }

        public int[] JouerTour(string grille) {
            MethodInfo IATour = classes[0].GetMethod("TourIA");
            IATour.Invoke(bot, new object[1] { grille });
            return null; // pour le moment
        }
    }
}
