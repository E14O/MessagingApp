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
        public bool HasAccount = false;
        string info = "";
        string setName = "";
        string code = "";
        string userId = "";
        public string userName = "";
        public string playerId = "";
        public string userCode = "";
        public string status = "";

        // some time ill rego over this code i hate it so much.

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

            Logging.Log(info);

            foreach (string line in info.Split('\n'))
            {
                var parts = line.Trim().Split(',');
                if (parts.Length < 4)
                    continue;

                string id = parts[0].Trim();
                string name = parts[1].Trim();
                string pid = parts[2].Trim().Replace("\uFEFF", "");
                string ucode = parts[3].Trim();

                Logging.Log($"{id} {name} {pid} {ucode}");

                if (pid == PhotonNetwork.LocalPlayer.UserId)
                {
                    userId = id;
                    userName = name;
                    playerId = pid;
                    userCode = ucode;

                    setName = userName;
                    code = userCode;
                    Logging.Log($"SetName: {setName}, Code: {code}");
                    HasAccount = true;
                    break;
                }
            }

            if (string.IsNullOrEmpty(setName))
            {
                status = "No Account\r\n\r\nYou Do Not Have A\r\nAccount. Create An\r\nAccount By Joining\r\nThe Discord Server\r\n\r\n<color=yellow>https://discord.gg/</color>\r\n<color=yellow>tbHvpqF5qy</color>";
                HasAccount = false;
                Logging.Log("you have no stinking account");
            }
            else
            {
                status = "Logged In";
                Logging.Log("user innnn go go!");
            }
        }
        // When a user taps in there friends code it needs to check through the data base and find that code then send a message to another gist saying "{PlayerAsCode} Wants To Freind {PlayerBCode}" Then Player B Will Check If There Code Is On There Every 15s To Reduce Lag And If It Is Display As A Notif On MonkePhone + InfoWatch
    }
}


