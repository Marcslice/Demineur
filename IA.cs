using System;
using System.IO;
using System.Reflection;



namespace Demineur
{
    public class IA
    {
        FileInfo dll;
        Type[] classes;
        object[] ligCol;
        dynamic bot;
        public IA(int lignes, int colonnes)
        {
            dll = new FileInfo(@"..\..\..\dll\IntelligenceArtificielDemineur.dll");
            classes = Assembly.LoadFile(dll.FullName).GetTypes(); // Load les classes du dll.
            ligCol = new object[2] { lignes, colonnes }; // Envoyer au constructeur de l'IA externe.
            bot = Activator.CreateInstance(classes[0], ligCol); //classes[0] pour la premiere classe du dll.     
        }

        public int[] JouerTour(string grille)
        {
            MethodInfo IATour = classes[0].GetMethod("MeilleurCoup");
            return IATour.Invoke(bot, new object[1] { grille });
        }
    }
}
