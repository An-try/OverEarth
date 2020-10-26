using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public static class FilePathConstants
    {
        private static string UserDataFileName => "user_data.json";
        private static string UserDataFolder => Application.persistentDataPath;

        public static string UserDataFilePath => UserDataFolder + "/" + UserDataFileName;
    }
}
