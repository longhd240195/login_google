using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GooglePlayGameControl : MonoBehaviour
{
    public void SocialLogin()
    {
        Social.localUser.Authenticate(success => {
            if (success)
            {
                Debug.Log("Authentication successful");
                string userInfo = "Username: " + Social.localUser.userName +
                                  "\nUser ID: " + Social.localUser.id +
                                  "\nIsUnderage: " + Social.localUser.underage +
                                  "\nToken ID: " + Social.localUser.id;
                Debug.Log(userInfo);
            }
            else
                Debug.Log("Authentication failed");
        });

    }

    public void Login()
    {
        PlayGamesPlatform.Activate();

        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        Debug.Log("status: " + status);
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
        }
        else
        {
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
        }
    }

    public void ShowToken()
    {
        Debug.Log("PlayGamesPlatform.Instance.localUser.id: " + PlayGamesPlatform.Instance.localUser.id);
    }
}
