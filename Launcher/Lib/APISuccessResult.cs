using System;

namespace Launcher
{
    public class APISuccessResult
    {
        public APIData data;
        public string stat;

        public class APIData
        {
            public string token;
        }
    }
}

