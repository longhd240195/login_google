#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;

using Mono.Cecil.Cil;
#endif

#if UNITY_IOS
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.Native;
using System.Text;

#endif

using TMPro;
using UnityEngine;

public class GooglePlayGameControl : MonoBehaviour
{
    private string Token;
    [SerializeField]private TextMeshProUGUI tmp;

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
        var config = new PlayGamesClientConfiguration.Builder()
            .RequestIdToken()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
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
        Social.localUser.Authenticate(success =>
        {
            Log("Result login: " + success);
            if (success)
            {
                //Call Unity Authentication SDK to sign in or link with Google.
                Log("PlayGamesPlatform: " + PlayGamesPlatform.Instance.IsAuthenticated());
                Log("PlayGamesPlatform: " + PlayGamesPlatform.Instance.GetUserDisplayName());
                Log("PlayGamesPlatform: " + PlayGamesPlatform.Instance.GetUserId());
                Token = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
                Log("Login with Google done. IdToken: " + ((PlayGamesLocalUser)Social.localUser).GetIdToken());
            }
            else
            {
                Log("Unsuccessful login");
            }
        });

        //PlayGamesPlatform.Instance.ManuallyAuthenticate((success) =>
        //    {
        //        Log("SignInStatus: " + success);
        //        Log("PlayGamesPlatform: " + PlayGamesPlatform.Instance.IsAuthenticated());
        //        Log("PlayGamesPlatform: " + PlayGamesPlatform.Instance.GetUserDisplayName());
        //        Log("PlayGamesPlatform: " + PlayGamesPlatform.Instance.GetUserId());
        //        if (success == SignInStatus.Success)
        //        {
        //            Log("Login with Google Play games successful.");

        //            PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
        //            {
        //                Log("Authorization code: " + code);
        //                Token = code;
        //                Log("Token: " + Token);
        //                // This token serves as an example to be used for SignInWithGooglePlayGames
        //            });
        //        }
        //        else
        //        {
        //            LogError("Failed to retrieve Google play games authorization code");
        //            Log("Login Unsuccessful");
        //        }
        //    });

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
                    Log("Sign-in with Apple successfully done. IDToken: " + idToken);
                    Token = idToken;
                }
                else
                {
                    Log("Sign-in with Apple error. Message: appleIDCredential is null");
                    LogError("Retrieving Apple Id Token failed.");
                }
            },
            error =>
            {
                Log("Sign-in with Apple error. Message: " + error);
                Log("Sign-in with Apple error. Message: " + error.LocalizedDescription);
                Log("Sign-in with Apple error. Message: " + error.LocalizedRecoveryOptions);
                Log("Sign-in with Apple error. Message: " + error.LocalizedFailureReason);
                Log("Sign-in with Apple error. Message: " + error.LocalizedRecoverySuggestion);
                LogError("Retrieving Apple Id Token failed.");
            }
        );
#endif

    }

    public void TryGetToken()
    {
#if UNITY_ANDROID
        //Log("Try get token...");
        
        //PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
        //{
        //    Log("Authorization code: " + code);
        //    Token = code;
        //    Log("Token: " + Token);
        //    // This token serves as an example to be used for SignInWithGooglePlayGames
        //});
#endif
    }

    public void Logout()
    {
#if UNITY_ANDROID
#endif
    }

    public void Log(string message)
    {
        tmp.text += "\n" + message;
    }

    public void LogError(string message)
    {
        tmp.text += "\n<color=red>" + message + "</color>";
    }

    public void MovePage(int diff)
    {
        tmp.pageToDisplay += diff;
    }

    public void ShowToken()
    {
        Log("Token: " + Token);
    }
}
