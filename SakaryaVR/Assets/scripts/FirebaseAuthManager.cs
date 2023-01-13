using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase;
using Firebase.Auth;

/*
    FireBase'nin i�inde "Authentication" sistemi var. Projemizde ilk ba�ta onu kullan�yorduk fakat kullanmay� b�rakt�k.
    Kodlar�n� silmek yerine olur da ileride kullanmay� d���n�r�z diye burada b�rak�yoruz....
 */


public class FirebaseAuthManager : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Firebase Veritaban�nda Hata: " + dependencyStatus);
            }
        });
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("��k�� Yap�ld� " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Giri� Yap�ld� " + user.UserId);
            }
        }
    }

    public void Login()
    {
        StartCoroutine(LoginAsync(emailLoginField.text, passwordLoginField.text));
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);
        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);
            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;
            string failedMessage = "Giri� Ba�ar�s�z! Nedeni: ";
            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email Ge�ersiz.";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "�ifre Ge�ersiz.";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email Ge�ersiz.";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "�ifre Ge�ersiz.";
                    break;
                default:
                    failedMessage = "Giri� Ba�ar�s�z.";
                    break;
            }
            Debug.Log(failedMessage);
        }
        else
        {
            user = loginTask.Result;
            Debug.Log("Giri� Yap�ld�.");
            SceneManager.LoadScene("Game");
        }
    }
}
