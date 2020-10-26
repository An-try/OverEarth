using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class UserDataManager : Singleton<UserDataManager>
    {
        private static UserProfilesData _userProfilesData = new UserProfilesData();



        public static void SaveProfile(UserProfile newUserProfile)
        {
            if (ProfileExists(newUserProfile.Name, out UserProfile oldUserProfile))
            {
                RewriteUserProfileWithNewProfile(oldUserProfile, newUserProfile);
            }
            else
            {
                SaveNewUserProfile(newUserProfile);
            }
        }

        public static void DeleteProfile(string profileName)
        {
            if (ProfileExists(profileName, out UserProfile userProfile))
            {
                _userProfilesData.UserProfiles.Remove(userProfile);
            }
            JsonDataWriter.SaveUserProfilesData(_userProfilesData);
        }



        private static void RewriteUserProfileWithNewProfile(UserProfile oldUserProfile, UserProfile newUserProfile)
        {
            oldUserProfile.RewriteWithProfile(newUserProfile);
            JsonDataWriter.SaveUserProfilesData(_userProfilesData);
        }

        private static void SaveNewUserProfile(UserProfile newUserProfile)
        {
            _userProfilesData.UserProfiles.Add(newUserProfile);
            JsonDataWriter.SaveUserProfilesData(_userProfilesData);
        }

        private static bool ProfileExists(string profileName, out UserProfile sameUserProfile)
        {
            sameUserProfile = null;
            UserProfile userProfile = _userProfilesData.UserProfiles.Find(profile => profile.Name == profileName);
            if (userProfile != null)
            {
                sameUserProfile = userProfile;
                return true;
            }
            return false;
        }
    }
}
