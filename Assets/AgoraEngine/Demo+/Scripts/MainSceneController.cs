using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif
using agora_gaming_rtc;


public enum TestSceneEnum
{
    AppScreenShare,
    DesktopScreenShare,
    Transcoding,
    InjectStream,
    One2One
};


/// <summary>
///    TestHome serves a game controller object for this application.
/// </summary>
public class MainSceneController : MonoBehaviour
{

    // Use this for initialization
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList();
#endif
    static IVideoChatClient app = null;

    private string HomeSceneName = "MainScene";

    // PLEASE KEEP THIS App ID IN SAFE PLACE
    // Get your own App ID at https://dashboard.agora.io/
    [Header("Agora Properties")]
    [SerializeField]
    private string AppID = "dd287741ae8845f489ab4c99cc91aa91";
    [SerializeField]
    private string channelName;
    [SerializeField]
    private string token = "";

    [Header("Photon Related")]
    [SerializeField]
    GameObject loadingScreen;
    [SerializeField]
    GameObject startScreen;

    [Header("UI")]
    [SerializeField] GameObject modUI;
    [SerializeField] GameObject guestUI;

    public enum UserChoice
    {
        Default,
        Traditional,
        VR
    }

    public enum UserType
    {
        Default,
        Mod,
        Guest
    }

    public UserChoice userChoice;
    public UserType userType;

    private bool _initialized = false;

    void Awake()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
		permissionList.Add(Permission.Microphone);         
		permissionList.Add(Permission.Camera);               
#endif

        // keep this alive across scenes
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        CheckAppId();
        //ConnectToServer();
    }

    void Update()
    {
        CheckPermissions();

    }

    public int GetChoice()
    {
        if (userChoice == UserChoice.Traditional)
            return 0;
        else
            return 1;
    }

    public bool IsMod()
    {
        if (userType == UserType.Mod)
            return true;
        else
            return false;
    }


    private void CheckAppId()
    {
        Debug.Assert(AppID.Length > 10, "Please fill in your AppId first on Game Controller object.");
        if (AppID.Length > 10)
        {
            _initialized = true;
        }
    }


    /// <summary>
    ///   Checks for platform dependent permissions.
    /// </summary>
    private void CheckPermissions()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        foreach(string permission in permissionList)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {                 
				Permission.RequestUserPermission(permission);
			}
        }
#endif
    }


    //private void LoadLastChannel()
    //{
    //    string channel = PlayerPrefs.GetString("ChannelName");
    //    if (!string.IsNullOrEmpty(channel))
    //    {
    //        GameObject go = GameObject.Find("ChannelName");
    //        InputField field = go.GetComponent<InputField>();

    //        field.text = channel;
    //    }
    //}

    //private void SaveChannelName()
    //{
    //    if (!string.IsNullOrEmpty(channelInputField.text))
    //    {
    //        PlayerPrefs.SetString("ChannelName", channelInputField.text);
    //        PlayerPrefs.Save();
    //    }
    //}
  

    public void HandleSceneButtonClick(int sceneEnum)
    {
        // get parameters (channel name, channel profile, etc.)
        TestSceneEnum scenename = (TestSceneEnum)sceneEnum;
        string sceneFileName = string.Format("{0}Scene", scenename.ToString());
        string channelName = this.channelName;
        //string token = this.token;

        if (string.IsNullOrEmpty(channelName))
        {
            Debug.LogError("Channel name can not be empty!");
            return;
        }

        if (!_initialized)
        {
            Debug.LogError("AppID null or app is not initialized properly!");
            return;
        }

        switch (scenename)
        {
            case TestSceneEnum.DesktopScreenShare:
                app = new DesktopScreenShare(); // create app
                break;
        }

        if (app == null) return;

        app.OnViewControllerFinish += OnViewControllerFinish;
        // load engine
        app.LoadEngine(AppID, token);
        // join channel and jump to next scene
        app.Join(channelName);
        //SaveChannelName();
        SceneManager.sceneLoaded += OnLevelFinishedLoading; // configure GameObject after scene is loaded
        SceneManager.LoadScene(sceneFileName, LoadSceneMode.Single);
    }


    public void InstantiateUI()
    {
        if (userType == UserType.Mod)
            Instantiate(modUI);
        if (userType == UserType.Guest)
            Instantiate(guestUI);
    }

    void ShowVersion()
    {
        GameObject go = GameObject.Find("VersionText");
        if (go != null)
        {
            Text text = go.GetComponent<Text>();
            var engine = IRtcEngine.GetEngine(AppID);
            Debug.Assert(engine != null, "Failed to get engine, appid = " + AppID);
            text.text = IRtcEngine.GetSdkVersion();
        }
    }

    //bool _previewing = false;
    //public void HandlePreviewClick(Button button)
    //{
    //    if (!_initialized) return;
    //    if (_echoTesting) {
    //        Debug.LogWarning("Echo testing! can't do preview right now!");
    //        return;
	   // }
    //    var engine = IRtcEngine.GetEngine(AppID);
    //    _previewing = !_previewing;
    //    previewImage.GetComponent<VideoSurface>().SetEnable(_previewing);
    //    if (_previewing)
    //    {
    //        engine.EnableVideo();
    //        engine.EnableVideoObserver();
    //        engine.StartPreview();
    //        button.GetComponentInChildren<Text>().text = "StopPreview";

    //        // VideoManager only works after video enabled
    //        CheckDevices(engine);
    //    }
    //    else
    //    {
    //        engine.DisableVideo();
    //        engine.DisableVideoObserver();
    //        engine.StopPreview();
    //        button.GetComponentInChildren<Text>().text = "StartPreview";
    //    }
    //}

    public void OnViewControllerFinish()
    {
        if (!ReferenceEquals(app, null))
        {
            app = null; // delete app
            SceneManager.LoadScene(HomeSceneName, LoadSceneMode.Single);
        }
        Destroy(gameObject);
    }

    public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        // Stop preview
        //if (_previewing)
        //{
        //    var engine = IRtcEngine.QueryEngine();
        //    if (engine != null)
        //    {
        //        engine.StopPreview();
        //        _previewing = false;
        //    }
        //}

        if (!ReferenceEquals(app, null))
        {
            app.OnSceneLoaded(); // call this after scene is loaded
        }
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnApplicationPause(bool paused)
    {
        if (!ReferenceEquals(app, null))
        {
            app.EnableVideo(paused);
        }
    }

    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit, clean up...");
        //if (_previewing)
        //{
        //    var engine = IRtcEngine.QueryEngine();
        //    if (engine != null)
        //    {
        //        engine.StopPreview();
        //        _previewing = false;
        //    }
        //}
        if (!ReferenceEquals(app, null))
        {
            app.UnloadEngine();
        }
        IRtcEngine.Destroy();
    }

    void Exit()
    {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // Gracefully quit on OS like Android, so OnApplicationQuit() is called
            Application.Quit();
#endif

    }

    /// <summary>
    ///   This method shows the CheckVideoDeviceCount API call.  It should only be used
    //  after EnableVideo() call.
    /// </summary>
    /// <param name="engine">Video Engine </param>
    void CheckDevices(IRtcEngine engine)
    {
        VideoDeviceManager deviceManager = VideoDeviceManager.GetInstance(engine);
        deviceManager.CreateAVideoDeviceManager();

        int cnt = deviceManager.GetVideoDeviceCount();
        Debug.Log("Device count =============== " + cnt);
    }
}
