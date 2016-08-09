using System;

namespace RPGGame
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var engine = vRPGEngine.Engine.Instance;

            using (var game = new RPGGame())
            {
                engine.InsertGame(game);
                engine.Start();
            }
        }
    }
}
