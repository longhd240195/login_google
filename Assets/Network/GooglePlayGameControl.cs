using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GooglePlayGameControl : MonoBehaviour
{
    [SerializeField] string idTest;
    static PlayGamesPlatform platform;
    bool isNetworkAvailable => Application.internetReachability != NetworkReachability.NotReachable;

    public void Sign()
    {
        InitPlayGamesPlatform();
    }

    void AutoLogin()
    {
        AutoLoginGPS();
    }

    void InitPlayGamesPlatform()
    {
        if (isNetworkAvailable)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.InitializeInstance(config);
            platform = PlayGamesPlatform.Activate();
            DOVirtual.DelayedCall(0.5f, AutoLogin);
        }
    }

    void AutoLoginGPS()
    {
        if (isNetworkAvailable)
        {
            if (platform != null)
            {
                platform.Authenticate(SignInInteractivity.CanPromptAlways, (result =>
                {
                    Debug.Log($"result: {result.ToString()}");
                    if (result == SignInStatus.Success)
                    {
                        Debug.Log("Login success");
                        ActionOnSignIn();
                    }
                    else
                    {
                        //TODO: login fail here
                    }
                }));
            }
            else
            {
                Debug.Log("Refesh InitPlayGamesPlatform");
                InitPlayGamesPlatform();
            }
        }
        else
        {
            Debug.Log($"Request login here because fail internet access");
            //TODO: Request login here because fail internet access
        }
    }

    public void Logout(Action onCompleted = null)
    {
        platform.SignOut();
        onCompleted?.Invoke();
    }

    void LoginGuest()
    {
        ActionOnSignIn();
    }

    void ActionOnSignIn()
    {
        var id = GetUserId();
        Debug.Log("id: " + id);
    }

    public string GetUserName()
    {
        return platform.localUser.authenticated ? platform.localUser.userName : string.Empty;
    }

    public string GetUserEmail()
    {
        return ((PlayGamesLocalUser)platform.localUser).Email;
    }

    public string GetUserId()
    {
        return platform.localUser.authenticated ? platform.localUser.id : SystemInfo.deviceUniqueIdentifier;
    }

    string GetUidLocal()
    {
        return string.Empty;
    }
}
