using Common.Logging;
using System.Net.Http.Headers;

namespace Agent
{
    internal static class StartMenu
    {
        private static string Header = @"
 ___ ___         __     __   __           __         ______         __           __   
|   |   |.---.-.|  |--.|  |_|  |--.-----.|  |--.    |   __ \.-----.|  |--.-----.|  |_ 
|   |   ||  _  ||    < |   _|  _  |  _  ||    <     |      <|  _  ||  _  |  _  ||   _|
 \_____/ |___._||__|__||____|_____|_____||__|__|    |___|__||_____||_____|_____||____|
    v.2.0.0 
";
        private static string MenuText = @"
Denne applikasjonen er utviklet for å overvåke og sjekke endringer i vaktplaner. 

1) Login Session
2) Automatisk sjekk av vaktplaner
3) Lagring av vaktplaner som PDF

Velg et alternativ ved å trykke på tilsvarende tall (1, 2, eller 3):
";
        public static int Print()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(Header);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(MenuText);
            return ReadKey();
        }

        /// <summary>
        /// Reads a key press from the console and returns the corresponding integer value if the key is '1', '2', or
        /// '3'.
        /// </summary>
        /// <remarks>The method does not display the pressed key on the console. If an invalid key is
        /// pressed, the user is prompted to try again, up to a maximum of three attempts.</remarks>
        /// <returns>An integer value of 1, 2, or 3 corresponding to the key pressed. Returns -1 if a valid key is not pressed
        /// within three attempts.</returns>
        private static int ReadKey()
        {
            int breakPoint = 3;

            while (breakPoint--  > 0)
            {
                var key = Console.ReadKey(true);
                if (key.KeyChar == '1' || key.KeyChar == '2' || key.KeyChar == '3')
                {
                    return int.Parse(key.KeyChar.ToString());
                }
                else
                {
                    Console.WriteLine("Ugyldig valg. Vennligst prøv igjen.");
                }
            }
            return -1;
        }
    }
}
