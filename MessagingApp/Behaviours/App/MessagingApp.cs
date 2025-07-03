using MessagingApp.Utilities;
using MonkePhone.Behaviours;
using UnityEngine;
using UnityEngine.UI;

namespace MessagingApp.Behaviours.App
{
    public class MessagingApp : PhoneApp
    {
        public override string AppId => "Messaging";

        public Text _Header, ht;
        public GameObject _newPage, _newPageT, gameobjecta, gameobjectb;

        public override void Initialize()
        {
            _Header = transform.Find("Header").GetComponent<Text>();
            _Header.transform.localPosition = new Vector3(0f, 42.3818f, 0f);

            _newPage = new GameObject("Info");
            _newPage.transform.SetParent(transform, false);
            _newPage.transform.position = new Vector3(-65.7571f, 11.6977f, -80.0866f);
            _newPage.transform.position = new Vector3(-65.7571f, 11.6977f, -80.0866f);
            _newPage.transform.SetSiblingIndex(2);

            Transform htp = transform.Find("Header");

            _newPageT = Instantiate(htp.gameObject, _newPage.transform, false);
            _newPageT.name = "Header";
            _newPageT.transform.localPosition = new Vector3(-6.3145f, 35.0311f, 0f);
            _newPageT.transform.localScale = new Vector3(0.4544f, 0.4544f, 0.7362f);

            ht = _newPageT.GetComponent<Text>();
            ht.alignment = TextAnchor.UpperLeft;
            ht.horizontalOverflow = HorizontalWrapMode.Overflow;
            ht.verticalOverflow = VerticalWrapMode.Overflow;

            gameobjecta = transform.Find("Chat Messages").gameObject;
            gameobjecta.SetActive(false);

            gameobjectb = transform.Find("Chat Box").gameObject;
            gameobjectb.SetActive(false);
        }

        void Update()
        {
            if (UnityEngine.InputSystem.Keyboard.current.oKey.isPressed)
            {
                AppOpened();
            }
        }

        public override void AppOpened()
        {
            base.AppOpened();

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
            // hey instead of doing this cause ye we will just dup more of the text alright then add the shit to it.
            ht.text = $"Status: {UserAuth.Instance.status}\r\n\r\nUsername: {UserAuth.Instance.userName} \r\nFriend Code: {UserAuth.Instance.userCode}";
        }
    }
}