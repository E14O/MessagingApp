using System;
using System.Collections;
using BepInEx;
using MessagingApp.Utilities;
using MonkePhone.Behaviours;
using MonkePhone.Behaviours.UI;
using Photon.Pun;
using UnityEngine;
using Viveport;

namespace MessagingApp
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }

        private void Start()
        {
            Instance = this;
            GorillaTagger.OnPlayerSpawned(Initialization);
        }

        private void Initialization()
        {
            try
            {
                new GameObject(Constants.Name, typeof(UserAuth));
                Logger.LogInfo("Loading UserAuth...");

                StartCoroutine(PhoneHandlerInit());
            }
            catch
            {
                Logger.LogError($"{Constants.Name} Unavailable, Account data could not be loaded.");
            }
        }

        private IEnumerator PhoneHandlerInit()
        {
            yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);

            try
            {
                foreach (Transform app in PhoneHandler.Instance.Phone.transform.Find("Canvas"))
                {
                    switch (app.name)
                    {
                        case "MessagingApp":
                           // app.gameObject.SetActive(false);
                            app.GetComponent<MonkePhone.Behaviours.Apps.MessagingApp>().Destroy();
                            PhoneHandler.Instance.CreateApp<Behaviours.Apps.MessagingApp>(app.gameObject);
                            Logger.LogInfo($"Creating App...");
                            break;
                    }
                }

                foreach (Transform Icon in PhoneHandler.Instance.Phone.transform.Find("Canvas/Home Screen/Grid"))
                {
                    switch (Icon.name)
                    {
                        case "MessagingIcon":
                            Icon.gameObject.SetActive(true);
                            Icon.gameObject.GetComponent<MonkePhone.Behaviours.UI.PhoneAppIcon>().Destroy();
                            Icon.gameObject.AddComponent<PhoneAppIcon>().appId = "Messaging";
                            break;

                    }
                }
                Logger.LogInfo($"{Constants.Name} Successfully Created!");
            }
            catch (Exception e)
            {
                Logger.LogError($"{Constants.Name} Unable To Be Created!, Error Code: {e}");
            }
        }
    }
}
