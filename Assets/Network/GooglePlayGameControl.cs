#if UNITY_ANDROID
using System;
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

using System.Threading.Tasks;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class GooglePlayGameControl : MonoBehaviour
{
    private string Token;
    [SerializeField]private TextMeshProUGUI tmp;
    [SerializeField]private Button btnAppleAutoLogin;

#if UNITY_IOS
    IAppleAuthManager m_AppleAuthManager;
#endif


    void Awake()
    {
        Initialize();
        btnAppleAutoLogin.onClick.AddListener(ApplePlayLogin);
    }

    void Update()
    {
#if UNITY_IOS
        // Updates the AppleAuthManager instance to execute
        // pending callbacks inside Unity's execution loop
        if (this.m_AppleAuthManager != null)
        {
            this.m_AppleAuthManager.Update();
        }
#endif

    }

    public void Initialize()
    {
#if UNITY_ANDROID
        //var config = new PlayGamesClientConfiguration.Builder()
        //    .RequestServerAuthCode(true)
        //    .RequestIdToken()
        //    .Build();

        //PlayGamesPlatform.InitializeInstance(config);
        //PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
#endif
        Log("Initialize");

#if UNITY_IOS
        var deserializer = new PayloadDeserializer();
        m_AppleAuthManager = new AppleAuthManager(deserializer);
#endif
    }

    public void LoginGooglePlayGames()
    {
#if UNITY_ANDROID
        //Social.localUser.Authenticate(success =>
        //{
        //    Log("Google v10.14");
        //    Log("Result login: " + success);
        //    if (success)
        //    {
        //        //Call Unity Authentication SDK to sign in or link with Google.
        //        Log("PlayGamesPlatform: " + PlayGamesPlatform.Instance.IsAuthenticated());
        //        Log("PlayGamesPlatform: " + PlayGamesPlatform.Instance.GetUserDisplayName());
        //        Log("PlayGamesPlatform: " + PlayGamesPlatform.Instance.GetUserId());
        //        Token = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
        //        Log("Login with Google done. IdToken: " + ((PlayGamesLocalUser)Social.localUser).GetIdToken());
        //        string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
        //        Log("Auth code: " + authCode);
        //    }
        //    else
        //    {
        //        Log("Unsuccessful login");
        //    }
        //});

        PlayGamesPlatform.Instance.Authenticate((success) =>
            {
                Log("Google v11.01");
                Log("SignInStatus: " + success);
                Log("GetUserDisplayName: " + PlayGamesPlatform.Instance.GetUserDisplayName());
                Log("GetUserId: " + PlayGamesPlatform.Instance.GetUserId());
                if (success == SignInStatus.Success)
                {
                    Log("Login with Google Play games successful.");
                    
                    PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                    {
                        Token = code;
                        Log("Token: " + Token);
                        // This token serves as an example to be used for SignInWithGooglePlayGames
                    });
                }
                else
                {
                    LogError("Failed to retrieve Google play games authorization code");
                    Log("Login Unsuccessful");
                }
            });

#endif

#if UNITY_IOS
        // Initialize the Apple Auth Manager
        Log("normal login apple");
        if (m_AppleAuthManager == null)
        {
            Initialize();
        }

        // Set the login arguments
        var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

        // Perform the login
        m_AppleAuthManager.LoginWithAppleId(loginArgs, credential =>
            {
                var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {
                    var idToken = Encoding.UTF8.GetString(appleIdCredential.IdentityToken, 0, appleIdCredential.IdentityToken.Length);
                    Token = idToken;

                    Log("Sign-in with Apple successfully done. IdentityToken: " + Token);

                    if (appleIdCredential.AuthorizationCode != null)
                    {
                        var authorizationCode = Encoding.UTF8.GetString(appleIdCredential.AuthorizationCode, 0, appleIdCredential.AuthorizationCode.Length);
                        Log("Sign-in with Apple successfully done. authorizationCode: " + authorizationCode);
                    }
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

    private void ApplePlayLogin()
    {
#if UNITY_IOS
        var quickLoginArgs = new AppleAuthQuickLoginArgs();
        Log("Auto login apple");
        m_AppleAuthManager.QuickLogin(
            quickLoginArgs,
            credential =>
            {
                // Received a valid credential!
                // Try casting to IAppleIDCredential or IPasswordCredential

                // Previous Apple sign in credential
                var appleIDCredential = credential as IAppleIDCredential;
                if (appleIDCredential != null)
                {
                    var idToken = Encoding.UTF8.GetString(appleIDCredential.IdentityToken, 0, appleIDCredential.IdentityToken.Length);
                    Log("Sign-in with Apple successfully done. IDToken: " + idToken);
                    Token = idToken;
                }
                else
                {
                    Log("Sign-in with Apple error. Message: appleIDCredential is null");
                    LogError("Retrieving Apple Id Token failed.");
                }
                // Saved Keychain credential (read about Keychain Items)
                var passwordCredential = credential as IPasswordCredential;
            },
            error =>
            {
                // Quick login failed. The user has never used Sign in With Apple on your app. Go to login screen
            });

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
