using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GooglePlayGameControl : MonoBehaviour
{
    private string Token;

    void Awake()
    {
        PlayGamesPlatform.Activate();
    }

    public void LoginGooglePlayGames()
    {
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login with Google Play games successful.");

                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log("Authorization code: " + code);
                    Token = code;
                    // This token serves as an example to be used for SignInWithGooglePlayGames
                });
            }
            else
            {
                Debug.LogError("Failed to retrieve Google play games authorization code");
                Debug.Log("Login Unsuccessful");
            }
        });
    }

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
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login with Google Play games successful.");

                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log("Authorization code: " + code);
                    Token = code;
                    // This token serves as an example to be used for SignInWithGooglePlayGames
                });
            }
            else
            {
                Debug.LogError("Failed to retrieve Google play games authorization code");
                Debug.Log("Login Unsuccessful");
            }
        });
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
        Debug.Log("PlayGamesPlatform.Instance.localUser.id: " + Token);
    }
}
