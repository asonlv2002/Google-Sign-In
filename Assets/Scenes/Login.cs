using Google;
using System.Threading.Tasks;
using UnityEngine;

public class Login : MonoBehaviour
{
    string webClientID = "296351000649-3pc46c85crce5imm9sulfofk37qq0d7p.apps.googleusercontent.com";
    private GoogleSignInConfiguration config;
    private void Awake()
    {
        config = new GoogleSignInConfiguration()
        {
            WebClientId = webClientID,
            RequestIdToken = true,
            UseGameSignIn = false,
            RequestEmail = true,
        };
    }

    public async void LoginGoogle()
    {
        GoogleSignIn.Configuration = config;
        await GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished, TaskScheduler.Default);
    }

    private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (var enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.LogError("Google Sign-In failed: " + error.Status + " " + error.Message);

                    // Kiểm tra nếu lỗi là do người dùng hủy
                    if (error.Status == GoogleSignInStatusCode.CANCELED)
                    {
                        Debug.LogWarning("Sign-In was canceled by the user.");
                    }
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.LogWarning("Google Sign-In was canceled.");
        }
        else
        {
            Debug.Log("Welcome: " + task.Result.DisplayName + "!");
            Debug.Log("Email: " + task.Result.Email);
            Debug.Log("IdToken: " + task.Result.IdToken);
        }
    }
}
