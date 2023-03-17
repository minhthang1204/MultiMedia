#region Using

using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

/// <summary>
///     Mono singleton Class. Extend this class to make singleton component.
///     Example:
///     <code>
/// public class Foo : MonoSingleton<Foo>
/// </code>
///     . To get the instance of Foo class, use <code>Foo.instance</code>
///     Override <code>Init()</code> method instead of using <code>Awake()</code>
///     from this class.
/// </summary>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    [SerializeField] private bool _deactivateOnLoad;
    [SerializeField] private bool _dontDestroyOnLoad;

    private bool _isInitialized;

    public static T instance
    {
        get
        {
            // Instance required for the first time, we look for it
            if (_instance == null)
            {
                var instances = Resources.FindObjectsOfTypeAll<T>();
                if (instances == null || instances.Length == 0)
                {
                    return null;
                }

                _instance = instances.FirstOrDefault(i => i.gameObject.scene.buildIndex != -1);
                _instance?.Init();
            }
            return _instance;
        }
    }

    /// <summary>
    ///     This function is called when the instance is used the first time
    ///     Put all the initializations you need here, as you would do in Awake
    /// </summary>
    public void Init()
    {
        if (_isInitialized)
        {
            return;
        }

        if (_dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }

        if (_deactivateOnLoad)
        {
            gameObject.SetActive(false);
        }

        if (gameObject == null)
        {
            return;
        }

        SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;

        InternalInit();
        _isInitialized = true;
    }

    // If no other monobehaviour request the instance in an awake function
    // executing before this one, no need to search the object.
    protected virtual void Awake()
    {
        if (_instance == null || !_instance || !_instance.gameObject)
        {
            _instance = (T)this;
        }
        else if (_instance != this)
        {
            Debug.LogError($"Another instance of {GetType()} already exist! Destroying self...");
            Destroy(this);
            return;
        }
        _instance.Init();
    }

    protected abstract void InternalInit();

    protected abstract void InternalOnDestroy();

    protected abstract void InternalOnDisable();

    protected abstract void InternalOnEnable();

    protected void OnDestroy()
    {
        // Clear static listener OnDestroy
        SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;

        StopAllCoroutines();
        InternalOnDestroy();
        if (_instance != this)
        {
            return;
        }
        _instance = null;
        _isInitialized = false;
    }

    protected void OnDisable()
    {
        InternalOnDisable();
    }

    protected void OnEnable()
    {
        InternalOnEnable();
    }

    /// Make sure the instance isn't referenced anymore when the user quit, just in case.
    private void OnApplicationQuit()
    {
        _instance = null;
    }

    private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
    {
        // Sanity
        if (!instance || !gameObject || gameObject == null)
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            _instance = null;
            return;
        }

        // On scene change, do not nullify instance on the following conditions:
        // 1) Singleton is marked "don't destroy on load"
        if (_dontDestroyOnLoad)
        {
            return;
        }

        SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
        _instance = null;
    }
}