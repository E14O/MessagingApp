using System.Net.Http;
using System.Threading.Tasks;
using MonkePhone.Tools;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace MessagingApp.Utilities
{
    public class UserAuth : MonoBehaviour
    {
        public TextMeshPro COC, COCH;
        public GameObject COCTextObj, COCHTextObj;
        public bool HasAccount = false;
        string info = "";
        string setName = "";
        string code = "";

        public void Start()
        {
            AuthInit();
        }

        public async void AuthInit()
        {
            // Setting The COC Stuff

            COCTextObj = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/COCBodyText_TitleData");
            COC = COCTextObj.GetComponent<TextMeshPro>();
            COC.text = "Loading Your Infomation....";
            COCHTextObj = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/CodeOfConductHeadingText");
            COCH = COCHTextObj.GetComponent<TextMeshPro>();
            COCH.text = "MonkeMessaging - \nStatus: Loading User Data";

            // This is where im setting all your account details if you have a account

            try
            {
                // This sends out the request to my gist
                var response = await new HttpClient().GetAsync("https://gist.github.com/E14O/2fc8e12c2c54da568c66fede2b1ec5f0/raw");
                response.EnsureSuccessStatusCode();
                // gets the content! YAY!
                info = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                // if the gist cannot be accessed for any reason this error will show in logs
                Logging.Error($"ERROR SENDING REQUEST {e}");
                return;
            }

            // gets the players current player id so we can try and find your account

            while (!PhotonNetwork.IsConnectedAndReady)
            {
                Logging.Log("Waiting For Photon To Initialise..");
                await Task.Yield();
            }

            string currentPlayerID = PhotonNetwork.LocalPlayer.UserId;
            Logging.Log(info);

            // splitting the data up 
            foreach (string line in info.Split('\n'))
            {
                var parts = line.Trim().Split(',');
                if (parts.Length < 4)
                    continue;

                string userId = parts[0].Trim();
                string username = parts[1].Trim();
                string playerId = parts[2].Trim().Replace("\uFEFF", ""); // some google dock told me this will work lets see (Removes A Byte Order Mark (BOM) That Was Fucking Up Our System)
                string userCode = parts[3].Trim();

                Logging.Log($"{userId} {username} {playerId} {userCode}");

              /// Checks for the player ID for the user.
                if (playerId == currentPlayerID)
                {
                    setName = username;
                    code = userCode;
                    Logging.Log($"SetName: {setName}, Code: {code}"); /// If the user does have an account their information will be displayed.
                    HasAccount = true;
                    COC.text = $"\n\nUsername: {setName}\nFriend Code: {code}\n\nIf Something Is Wrong Please Contact Support :)";
                    COCH.text = "MonkeMessaging - \nStatus: Logged In";
                    break;
                }
            }

            if (string.IsNullOrEmpty(setName))
            {
                COC.text = "\n\nYou Do Not Have A Account Join The Discord (https://discord.gg/tbHvpqF5qy) And Do ?CreateAccount!\n\nIf Something Is Wrong Please Contact Support :)";
                COCH.text = "MonkeMessaging - \nStatus: No Account";
                Logging.Log("You Do Not Have A Account!"); /// The user does not have an account if this log is displayed.
            }
        }

        // When a user taps in there friends code it needs to check through the data base and find that code then send a message to another gist saying "{PlayerAsCode} Wants To Freind {PlayerBCode}" Then Player B Will Check If There Code Is On There Every 15s To Reduce Lag And If It Is Display As A Notif On MonkePhone + InfoWatch
    }
}


