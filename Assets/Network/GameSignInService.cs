#if UNITY_ANDROID
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

#if UNITY_IOS
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.Native;
using System.Text;
#endif

using System;
using System.Threading.Tasks;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class GameSignInService
{
    private static string Token;
    private static bool initialize;
    private string a = "56682896863-fio4e7bb2bmj8cd5gnpntphm5nb5qv2g.apps.googleusercontent.com";
    private string b = "56682896863-h80ikbpcq55a9cj7s4q2d5lpat160a01.apps.googleusercontent.com";
#if UNITY_IOS
    IAppleAuthManager m_AppleAuthManager;
#endif

    public void Initialize()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Activate();
#endif

#if UNITY_IOS
        var deserializer = new PayloadDeserializer();
        m_AppleAuthManager = new AppleAuthManager(deserializer);
#endif
        initialize = true;
        Log("Initialize login google/apple");
    }

    public void LoginGooglePlayGames(Action<string> callback)
    {
        if (!initialize)
            Initialize();

#if UNITY_ANDROID
        CheckNull(callback);

        //PlayGamesPlatform.Instance.Authenticate((success) =>
        //    {
        //        Log("Google v11.01");
        //        Log("SignInStatus: " + success);
        //        Log("GetUserDisplayName: " + PlayGamesPlatform.Instance.GetUserDisplayName());
        //        Log("GetUserId: " + PlayGamesPlatform.Instance.GetUserId());
        //        if (success == SignInStatus.Success)
        //        {
        //            Log("Login with Google Play games successful.");
                    
        //            PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
        //            {
        //                Token = code;
        //                Log("Token: " + Token);
        //                callback?.Invoke(Token);
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
    }

    private void AppleQuickLogin(Action<string> callback)
    {
        if (!initialize)
            Initialize();
        CheckNull(callback);

#if UNITY_IOS
        var quickLoginArgs = new AppleAuthQuickLoginArgs();
        Log("Auto login apple");

        m_AppleAuthManager.QuickLogin(quickLoginArgs,credential =>
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
                    callback?.Invoke(Token);
                }
                else
                {
                    Log("Sign-in with Apple error. Message: appleIDCredential is null");
                    LogError("Retrieving Apple Id Token failed.");
                }
            },
            error =>
            {
                // Quick login failed. The user has never used Sign in With Apple on your app. Go to login screen
            });

#endif
    }

    private void AppleNormalLogin(Action<string> callback)
    {
        if (!initialize)
            Initialize();
        CheckNull(callback);

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

    private void CheckNull(object obj)
    {
        if(obj == null)
            LogError("!!!! Null callback !!!!");
    }

    public void Logout()
    {
#if UNITY_ANDROID
#endif

#if UNITY_IOS
        
#endif
    }

    public void Log(string message)
    {
        Debug.Log(message);
    }

    public void LogError(string message)
    {
        Debug.LogError(message);
    }
}
