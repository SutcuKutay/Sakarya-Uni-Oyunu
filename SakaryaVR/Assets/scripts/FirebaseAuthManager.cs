using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase;
using Firebase.Auth;

/*
    FireBase'nin içinde "Authentication" sistemi var. Projemizde ilk baþta onu kullanýyorduk fakat kullanmayý býraktýk.
    Kodlarýný silmek yerine olur da ileride kullanmayý düþünürüz diye burada býrakýyoruz....
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
                Debug.LogError("Firebase Veritabanýnda Hata: " + dependencyStatus);
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
                Debug.Log("Çýkýþ Yapýldý " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Giriþ Yapýldý " + user.UserId);
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
            string failedMessage = "Giriþ Baþarýsýz! Nedeni: ";
            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email Geçersiz.";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Þifre Geçersiz.";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email Geçersiz.";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Þifre Geçersiz.";
                    break;
                default:
                    failedMessage = "Giriþ Baþarýsýz.";
                    break;
            }
            Debug.Log(failedMessage);
        }
        else
        {
            user = loginTask.Result;
            Debug.Log("Giriþ Yapýldý.");
            SceneManager.LoadScene("Game");
        }
    }
}
