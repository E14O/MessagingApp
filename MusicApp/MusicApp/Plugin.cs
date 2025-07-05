using System;
using System.Collections;
using BepInEx;
using MonkePhone.Behaviours;
using Photon.Pun;
using UnityEngine;

namespace MusicApp
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
                StartCoroutine(PhoneHandlerInit());
            }
            catch
            {
                Logger.LogError($"{Constants.Name} Unavailable, could not start Init.");
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
                        case "MusicApp":
                            app.gameObject.SetActive(false);
                            app.GetComponent<MonkePhone.Behaviours.Apps.MusicApp>().Destroy();
                            PhoneHandler.Instance.CreateApp<Behaviours.App.MusicApp>(app.gameObject);
                            Logger.LogInfo($"Creating App...");
                            break;
                    }
                }

                foreach (Transform Icon in PhoneHandler.Instance.Phone.transform.Find("Canvas/Home Screen/Grid"))
                {
                    switch (Icon.name)
                    {
                        case "MonkeMusic":
                            Icon.gameObject.SetActive(true);
                            //  Icon.gameObject.GetComponent<MonkePhone.Behaviours.UI.PhoneAppIcon>().Destroy();
                            // Icon.gameObject.AddComponent<PhoneAppIcon>().appId = "Music";
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
