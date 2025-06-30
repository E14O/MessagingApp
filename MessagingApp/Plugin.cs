using System;
using System.Collections;
using System.IO;
using BepInEx;
using MessagingApp.Utilities;
using MonkePhone.Behaviours;
using MonkePhone.Behaviours.Apps;
using MonkePhone.Tools;
using UnityEngine;

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

                StartCoroutine(PhoneHandlerInit());
            }
            catch
            {
                Logging.Error($"{Constants.Name} Unavailable, Account data could not be loaded.");
            }
        }

        private IEnumerator PhoneHandlerInit()
        {
            yield return new WaitUntil(() => PhoneHandler.Instance != null);

            try
            {
                foreach (Transform App in PhoneHandler.Instance.Phone.transform.Find("Canvas"))
                {
                    switch (App.name)
                    {
                        case "MessagingApp":
                            App.gameObject.SetActive(false);
                            App.GetComponent<MonkePhone.Behaviours.Apps.MessagingApp>().Destroy();
                            PhoneHandler.Instance.CreateApp<Behaviours.Apps.MessagingApp>(App.gameObject);
                            break;
                    }
                }

                foreach (Transform Icon in PhoneHandler.Instance.Phone.transform.Find("Canvas/Home Screen/Grid"))
                {
                    switch (Icon.name)
                    {
                        case "MessagingIcon":
                            Icon.gameObject.SetActive(true);
                            break;
                    }
                }
                Logging.Info($"{Constants.Name} Successfully Created!");
            }
            catch (Exception e)
            {
                Logging.Error($"{Constants.Name} Unable To Be Created!, Error Code: {e}");
            }
        }
    }
}
