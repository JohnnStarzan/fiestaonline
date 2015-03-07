using System;

namespace Launcher
{
    public class APIFailResult
    {
        public APIData data;
        public string stat;

        public class APIData
        {
            public uint code;
            public string error;
        }
    }
}

