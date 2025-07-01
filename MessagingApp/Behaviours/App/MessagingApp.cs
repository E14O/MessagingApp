
using MessagingApp.Utilities;
using MonkePhone.Behaviours;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

namespace MessagingApp.Behaviours.Apps
{
    public class MessagingApp : PhoneApp
    {
        public override string AppId => "Messaging";

        public Text _Header;
        public GameObject _newPage, _newPageT;

        public override void Initialize()
        {
            _Header = transform.Find("Header").GetComponent<Text>();
            _Header.transform.localPosition = new Vector3(0f, 42.3818f, 0f);

            _newPage = new GameObject("Info");
            _newPage.transform.SetParent(gameObject.transform, true);

            _newPageT = Instantiate(transform.Find("Header").gameObject);
            _newPageT.transform.SetParent(_newPage.transform, true);
            _newPageT.transform.SetSiblingIndex(2);

        }

        public override void AppOpened()
        {
            base.AppOpened();

            transform.GetChild(3 & 4).gameObject.SetActive(false);
            _newPageT.name = "loading...";

            RefreshApp();
        }

        /*public override void ButtonClick(PhoneUIObject phoneUIObject, bool isLeftHand)
         {
             switch (phoneUIObject.name)
             {
                 case "ExampleButton":
                     RefreshApp();
                     break;
             }
         }*/

        private void RefreshApp()
        {

        }
    }
}