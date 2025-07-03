using System.Net.Http;
using System.Threading.Tasks;
using MonkePhone.Tools;
using Photon.Pun;
using UnityEngine;

namespace MessagingApp.Utilities
{
    public class UserAuth : MonoBehaviour
    {
        public static UserAuth Instance { get; private set; }
        public GameObject COCTextObj, COCHTextObj;
        public bool HasAccount = false;
        string info = "";
        string setName = "";
        string code = "";
        string userId = "";
        public string userName = "";
        public string playerId = "";
        public string userCode = "";
        public string status = "";
        public void Start()
        {
            Instance = this;
            AuthInit();
        }

        public async void AuthInit()
        {
            try
            {
                var response = await new HttpClient().GetAsync("https://gist.github.com/E14O/2fc8e12c2c54da568c66fede2b1ec5f0/raw");
                response.EnsureSuccessStatusCode();
                info = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Logging.Error($"ERROR SENDING REQUEST {e}");
                return;
            }

            while (!PhotonNetwork.IsConnectedAndReady)
            {
                Logging.Log("Waiting For Photon To Initialise..");
                await Task.Yield();
            }

            string currentPlayerID = PhotonNetwork.LocalPlayer.UserId;
            Logging.Log(info);

            foreach (string line in info.Split('\n'))
            {
                var parts = line.Trim().Split(',');
                if (parts.Length < 4)
                    continue;

                userId = parts[0].Trim();
                userName = parts[1].Trim();
                playerId = parts[2].Trim().Replace("\uFEFF", ""); // some google dock told me this will work lets see (Removes A Byte Order Mark (BOM) That Was Fucking Up Our System)
                userCode = parts[3].Trim();

                Logging.Log($"{userId} {userName} {playerId} {userCode}");

                if (playerId == currentPlayerID)
                {
                    setName = userName;
                    code = userCode;
                    Logging.Log($"SetName: {setName}, Code: {code}");
                    HasAccount = true;

                    if (string.IsNullOrEmpty(setName))
                    {
                        status = "No Account";
                        HasAccount = false;
                        Logging.Log("You Do Not Have A Account!");
                    }
                    else
                    {
                        status = "Authenticated";
                    }

                    break;     
                }
            }
        }

        // When a user taps in there friends code it needs to check through the data base and find that code then send a message to another gist saying "{PlayerAsCode} Wants To Freind {PlayerBCode}" Then Player B Will Check If There Code Is On There Every 15s To Reduce Lag And If It Is Display As A Notif On MonkePhone + InfoWatch
    }
}


