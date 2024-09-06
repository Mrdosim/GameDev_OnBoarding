using UnityEngine;
using TMPro;
using Firebase.Auth;
using Firebase;
using System.Threading;
using UnityEngine.SceneManagement;

public class FirebaseAuthManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;

    public TMP_InputField email;
    public TMP_InputField password;

    private SynchronizationContext _syncContext;

    private void Start()
    {
        _syncContext = SynchronizationContext.Current;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase is ready.");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    public void LogIn()
    {
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Login was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Login encountered an error: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                Debug.Log("Login task completed successfully.");
                AuthResult result = task.Result;
                FirebaseUser newUser = result.User;
                Debug.Log("User logged in successfully: " + newUser.Email);

                _syncContext.Post(_ =>
                {
                    Debug.Log("Attempting to load scene: MainScene");
                    try
                    {
                        SceneManager.LoadScene("MainScene");
                        Debug.Log("Scene load attempted.");
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("Scene loading failed: " + e.Message);
                    }
                }, null);
            }
        });
    }


    public void Join()
    {
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                return;
            }
            if (task.IsFaulted)
            {
                return;
            }

            AuthResult result = task.Result;
            FirebaseUser newUser = result.User;
        });
    }
    




    public void LogOut()
    {
        auth.SignOut();
    }
}