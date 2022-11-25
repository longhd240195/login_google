#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

#if UNITY_IOS
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.Native;
using System.Text;

using UnityEditor.PackageManager;
#endif

using UnityEngine;
using Debug = UnityEngine.Debug;

public class GooglePlayGameControl : MonoBehaviour
{
    private string Token;

#if UNITY_IOS
    IAppleAuthManager m_AppleAuthManager;
#endif


    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Activate();
#endif

#if UNITY_IOS
        var deserializer = new PayloadDeserializer();
        m_AppleAuthManager = new AppleAuthManager(deserializer);
#endif
    }

    public void LoginGooglePlayGames()
    {
#if UNITY_ANDROID

        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            Debug.Log("SignInStatus: " + success);
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login with Google Play games successful.");

                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log("Authorization code: " + code);
                    Token = code;
                    Debug.Log("Token: " + Token);
                    // This token serves as an example to be used for SignInWithGooglePlayGames
                });
            }
            else
            {
                Debug.LogError("Failed to retrieve Google play games authorization code");
                Debug.Log("Login Unsuccessful");
            }
        });
#endif

#if UNITY_IOS
        // Initialize the Apple Auth Manager
        if (m_AppleAuthManager == null)
        {
            Initialize();
        }

        // Set the login arguments
        var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

        // Perform the login
        m_AppleAuthManager.LoginWithAppleId(loginArgs, credential =>
            {
                var appleIDCredential = credential as IAppleIDCredential;
                if (appleIDCredential != null)
                {
                    var idToken = Encoding.UTF8.GetString(appleIDCredential.IdentityToken,0,appleIDCredential.IdentityToken.Length);
                    Debug.Log("Sign-in with Apple successfully done. IDToken: " + idToken);
                    Token = idToken;
                }
                else
                {
                    Debug.Log("Sign-in with Apple error. Message: appleIDCredential is null");
                    Debug.LogError("Retrieving Apple Id Token failed.");
                }
            },
            error =>
            {
                Debug.Log("Sign-in with Apple error. Message: " + error);
                Debug.LogError("Retrieving Apple Id Token failed.");
            }
        );
#endif

    }


    public void ShowToken()
    {
        Debug.Log("Token: " + Token);
    }
}
