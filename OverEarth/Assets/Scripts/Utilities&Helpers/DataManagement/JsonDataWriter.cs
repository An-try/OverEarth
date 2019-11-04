using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public static class JsonDataWriter
    {
        public static void CreateInitialUserProfilesFile()
        {
            string userDataFilePath = FilePathConstants.UserDataFilePath;

            UserProfilesData userProfilesData = new UserProfilesData();

            string newJsonData = JsonUtility.ToJson(userProfilesData, true);
            File.WriteAllText(userDataFilePath, newJsonData);
        }

        public static void SaveUserProfilesData(UserProfilesData userProfilesData)
        {
            string userDataFilePath = FilePathConstants.UserDataFilePath;

            string newJsonData = JsonUtility.ToJson(userProfilesData, true);
            File.WriteAllText(userDataFilePath, newJsonData);
        }
    }
}
