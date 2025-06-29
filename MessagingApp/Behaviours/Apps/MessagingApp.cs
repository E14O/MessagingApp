
using MonkePhone.Behaviours;
using MonkePhone.Behaviours.UI;
using MonkePhone.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace MessagingApp.Behaviours.Apps
{
    public class MessagingApp : PhoneApp
    {
        public override string AppId => "Messaging";

        private Text _Header;

        public override void Initialize()
        {
            _Header = transform.Find("Header").GetComponent<Text>();
            _Header.transform.localPosition = new Vector3(0f, 42.3818f, 0f);
        }

      /*  public override void AppOpened()
        {
            base.AppOpened();

            // this gets called once the app is opened so if u want some code to run here u can 
            // most of the time this void isnt used so you can remove it.

            RefreshApp();
        }

        public override void ButtonClick(PhoneUIObject phoneUIObject, bool isLeftHand)
        {
            switch (phoneUIObject.name)
            {
                case "ExampleButton":
                    RefreshApp();
                    break;
            }
        }

        private void RefreshApp()
        {
            // e.g what code u want cause like so epic
        }*/
    }
}