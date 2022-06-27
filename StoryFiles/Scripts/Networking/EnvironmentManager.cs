using StoryFiles;
using UnityEngine;

namespace StoryFiles
{
    public class EnvironmentManager
    {


        private const string devURL = "https://gateway.dev.storyfile.com";
        private const string stgURL = "https://gateway.stg.storyfile.com";
        private const string prodURL = "https://gateway.prod.storyfile.com";

        private const string socketDevURL = "wss://storyfile-exhibit.dev.storyfile.com/speech2text";
        private const string socketStgURL = "wss://storyfile-exhibit.stg.storyfile.com/speech2text";
        private const string socketProdURL = "wss://exhibit.storyfile.com/speech2text";

        private const string startPath = "/api/v1/dialogue/start";
        private const string interactPath = "/api/v1/dialogue/interact";
        private const string configPath = "/api/v1/webapp/{0}/config";

        public static string DialogStartUrl(Environment environment)
        {
            switch (environment)
            {
                case Environment.Development:
                    return devURL + startPath;
                case Environment.Stage:
                    return stgURL + startPath;
                case Environment.Production:
                    return prodURL + startPath;
            }
            return "";
        }

        public static string InteractUrl(Environment environment)
        {
            switch (environment)
            {
                case Environment.Development:
                    return devURL + interactPath;
                case Environment.Stage:
                    return stgURL + interactPath;
                case Environment.Production:
                    return prodURL + interactPath;
            }
            return "";
        }

        public static string ConfigUrl(Environment environment, int id)
        {
            switch (environment)
            {
                case Environment.Development:
                    return devURL + string.Format(configPath, id);
                case Environment.Stage:
                    return stgURL + string.Format(configPath, id);
                case Environment.Production:
                    return prodURL + string.Format(configPath, id);
            }
            return "";
        }

        public static string SpeachSocketUrl(Environment environment)
        {
            switch (environment)
            {
                case Environment.Development:
                    return socketDevURL;
                case Environment.Stage:
                    return socketStgURL;
                case Environment.Production:
                    return socketProdURL;
            }
            return "";
        }
    }
}
