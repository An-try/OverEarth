using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public static class JsonDataProvider
    {
        public static UserProfilesData GetUserProfilesData()
        {
            string userDataFilePath = FilePathConstants.UserDataFilePath;

            if (File.Exists(userDataFilePath))
            {
                string dataJson = File.ReadAllText(userDataFilePath);
                return JsonUtility.FromJson<UserProfilesData>(dataJson);
            }
            else
            {
                JsonDataWriter.CreateInitialUserProfilesFile();

                string dataJson = File.ReadAllText(userDataFilePath);
                return JsonUtility.FromJson<UserProfilesData>(dataJson);
            }
        }
    }
}
