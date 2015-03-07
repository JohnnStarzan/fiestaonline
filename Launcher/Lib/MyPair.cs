using System;

namespace Launcher
{
    public class MyPair
    {
        private string _game;
        private string _patchFile;

        public MyPair(string game, string patchFile)
        {
            this._game = game;
            this._patchFile = patchFile;
        }

        public string Game
        {
            get
            {
                return this._game;
            }
        }

        public string PatchFile
        {
            get
            {
                return this._patchFile;
            }
        }
    }
}

