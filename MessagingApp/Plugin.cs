using System;
using System.Collections;
using BepInEx;
using MonkePhone.Behaviours;
using UnityEngine;

namespace MessagingApp
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        void Start() => StartCoroutine(PhoneHandlerInit());

        private IEnumerator PhoneHandlerInit()
        {
            yield return new WaitUntil(() => PhoneHandler.Instance.Phone != null);

            try
            {
                foreach (Transform App in PhoneHandler.Instance.Phone.transform.Find("Canvas"))
                {
                    switch (App.name)
                    {
                        case "MessagingApp":
                            App.gameObject.SetActive(false);
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

                Logger.LogInfo("MessagingApp Successfully Created!");
            }
            catch (Exception e)
            {
                Logger.LogError($"MessagingApp Unable To Be Created!, Error Code:{e}");
            }
        }
    }
}
