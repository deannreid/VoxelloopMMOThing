using UnityEngine;
using System.Collections;

public class database : MonoBehaviour 
{
    public static string user = "", name = "none";
    private string password = "", rePass = "", message = "";

    private bool register = false;

    private void OnGUI()
    {
		if (Application.platform == RuntimePlatform.Android) {
			Application.LoadLevel ("main");
		}

        if (message != "")
            GUILayout.Box(message);

        if (register)
        {
            GUILayout.Label("Username");
            user = GUILayout.TextField(user);
            GUILayout.Label("password");
            password = GUILayout.PasswordField(password, "*"[0]);
            GUILayout.Label("Re-password");
            rePass = GUILayout.PasswordField(rePass, "*"[0]);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Back"))
                register = false;

            if (GUILayout.Button("Register"))
            {
                message = "";

                if (user == "" || name == "" || password == "")
                    message += "Please enter all the fields \n";
                else
                {
                    if (password == rePass)
                    {
                        WWWForm form = new WWWForm();
                        form.AddField("user", user);
                        form.AddField("name", name);
                        form.AddField("password", password);
                        WWW w = new WWW("http://vl1.voxelloop.co.uk/login/register.php", form);
                        StartCoroutine(registerFunc(w));
                    }
                    else
                        message += "Your Password does not match \n";
                }
            }

            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.Label("User:");
            user = GUILayout.TextField(user);
            GUILayout.Label("Password:");
            password = GUILayout.PasswordField(password, "*"[0]);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Login"))
            {
                message = "";

                if (user == "" || password == "")
                    message += "Please enter all the fields \n";
                else
                {
                    WWWForm form = new WWWForm();
                    form.AddField("user", user);
                    form.AddField("password", password);
                    WWW w = new WWW("http://vl1.voxelloop.co.uk/login/login.php", form);
                    StartCoroutine(login(w));
                }
            }

            if (GUILayout.Button("Register"))
                register = true;

            GUILayout.EndHorizontal();
        }
    }

    IEnumerator login(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            if (w.text == "login-SUCCESS")
            {
                print("Login successful! Welcome " + user);
				Application.LoadLevel ("main");
            }
            else
                message += w.text;
        }
        else
        {
            message += "Dun-goofed (ERROR): " + w.error + "\n";
        }
    }

    IEnumerator registerFunc(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            message += w.text;
        }
        else
        {
            message += "Dun-gooofed (ERROR): " + w.error + "\n";
        }
    }
}
