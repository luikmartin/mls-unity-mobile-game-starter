using System;
using Unity.Services.Core;
using Unity.Services.Mediation;
using UnityEngine;

public abstract class AdBase<AD_TYPE, AD_INTERFACE> : MonoBehaviour where AD_TYPE : AD_INTERFACE where AD_INTERFACE : IDisposable
{
    private string _iosGameId;
    private string _androidGameId;

    [Header("Ad Unit Ids"), Tooltip("Android Ad Unit Ids")]
    public string androidAdUnitId;
    [Tooltip("iOS Ad Unit Ids")]
    public string iosAdUnitId;

    public AD_TYPE ad;


    private void Awake()
    {
        try
        {
            var config = Utils.LoadConfig<Config>(Constants.CONFIG_PATH);

            _iosGameId = config.IOS_GAME_ID;
            _androidGameId = config.ANDROID_GAME_ID;
        }
        catch (Exception e)
        {
            Debug.LogError("Missing configuration file! " + e.Message);
            Debug.LogWarning("Make sure you have 'Config.json' file present in the project root. Use 'Config.example.json' as a base");
        }
    }

    public async void Start()
    {
        try
        {
            Debug.Log("Initializing...");
            await UnityServices.InitializeAsync(GetGameId());
            Debug.Log("Initialized!");

            InitializationComplete();
        }
        catch (Exception e)
        {
            InitializationFailed(e);
        }
    }

    public void OnDestroy()
    {
        ad?.Dispose();
    }

    InitializationOptions GetGameId()
    {
        var initializationOptions = new InitializationOptions();

#if UNITY_IOS
            if (!string.IsNullOrEmpty(_iosGameId))
            {
                initializationOptions.SetGameId(_iosGameId);
            }
#elif UNITY_ANDROID
        if (!string.IsNullOrEmpty(_androidGameId))
        {
            initializationOptions.SetGameId(_androidGameId);
        }
#endif
        return initializationOptions;
    }

    public abstract void LoadAd();

    public void AdLoaded(object sender = null, EventArgs args = null)
    {
        Debug.Log("Ad loaded");
    }

    public void AdClosed(object sender, EventArgs args)
    {
        Debug.Log("Ad closed");
    }

    public void AdFailedLoad(LoadFailedException e)
    {
        Debug.Log("Failed to load ad");
        Debug.Log(e.Message);
    }

    public void AdFailedLoad(object sender, LoadErrorEventArgs e)
    {
        Debug.Log("Failed to load ad");
        Debug.Log(e.Message);
    }

    public abstract void InitializationComplete();

    public void InitializationFailed(Exception error)
    {
        var initializationError = SdkInitializationError.Unknown;

        if (error is InitializeFailedException initializeFailedException)
        {
            initializationError = initializeFailedException.initializationError;
        }
        Debug.Log($"Initialization Failed: {initializationError}:{error.Message}");
    }

    public void ImpressionEvent(object sender, ImpressionEventArgs args)
    {
        var impressionData = args.ImpressionData != null
            ? Utils.ToJson<ImpressionData>(args.ImpressionData)
            : null;
        Debug.Log($"Impression event from ad unit id {args.AdUnitId} : {impressionData}");
    }
}