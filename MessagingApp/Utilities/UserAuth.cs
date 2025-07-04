using System;
using System.Net.Http;
using System.Threading.Tasks;
using MonkePhone.Tools;
using Photon.Pun;
using UnityEngine;
using static FriendBackendController;

namespace MessagingApp.Utilities
{
    public class UserAuth : MonoBehaviour
    {
        public static UserAuth Instance { get; private set; }

        public bool HasAccount = false;
        public string UserName = "";
        public string PlayerId = "";
        public string UserCode = "";
        public string Status = "";
        public string AccountInfoDisplay = "";

        private string info = "";
        private string setName = "";
        private string code = "";
        private string userId = "";

        private static readonly HttpClient httpClient = new();
        private const string AuthUrl = "https://gist.github.com/E14O/2fc8e12c2c54da568c66fede2b1ec5f0/raw";

        private void Start()
        {
            Instance = this;
            AuthInit();
        }

        public async void AuthInit()
        {
            try
            {
                var response = await httpClient.GetAsync(AuthUrl);
                response.EnsureSuccessStatusCode();
                info = await response.Content.ReadAsStringAsync();
                Logging.Log("httpClient Successfully sent request.");
            }
            catch (HttpRequestException e)
            {
                Logging.Error($"Error has occurred with httpClient sending request: {e}");
                return;
            }

            while (!PhotonNetwork.IsConnectedAndReady)
            {
                Logging.Log("httpClient waiting for PhotonNetwork to initialize...");
                await Task.Yield();
            }

            Logging.Log("Received Auth Info:\n" + info);

            foreach (string rawLine in info.Split('\n'))
            {
                var line = rawLine.Trim('\uFEFF').Trim();
                var parts = line.Split(',');

                if (parts.Length < 4)
                    continue;

                string ID = parts[0].Trim();
                string OuserName = parts[1].Trim();
                string OplayerID = parts[2].Trim();
                string OuserCode = parts[3].Trim();

                Logging.Log($"Parsed: ID: {ID}, UserName: {OuserName}, PlayerID: {OplayerID}, FriendCode: {OuserCode}");

                if (OplayerID == PhotonNetwork.LocalPlayer.UserId)
                {
                    userId = ID;
                    UserName = OuserName;
                    PlayerId = OuserCode;
                    UserCode = OuserCode;

                    setName = UserName;
                    code = UserCode;

                    HasAccount = true;
                    Logging.Log($"Authenticated as: {setName}, FriendCode: {code}");
                    break;
                }
            }

            if (string.IsNullOrEmpty(setName))
            {
                Status = "No Account";
                HasAccount = false;
                Logging.Log("No account found for this user.");
            }
            else
            {
                Status = "Logged In";
                Logging.Log("User successfully authenticated.");
            }

            switch (Status)
            {
                case "No Account":
                    AccountInfoDisplay = "Status: No Account\r\n\r\nYou Do Not Have A\r\nAccount. Create An\r\nAccount By Joining\r\nThe Discord Server\r\n\r\n<color=yellow>https://discord.gg/</color>\r\n<color=yellow>tbHvpqF5qy</color>";
                    break;
                case "Logged In":
                    AccountInfoDisplay = $"Status: {Status}\n\nUsername: {UserName}\nFriend Code: {UserCode}";
                    break;
            }
        }
    }
}
