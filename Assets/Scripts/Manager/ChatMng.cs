using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatMng : MonoBehaviour
{
    public TMPro.TextMeshProUGUI chatLogs;
    [SerializeField] TMPro.TMP_InputField chatInput;
    [SerializeField] Animator chatAnim;
    [SerializeField] GameObject chatPanel;
    public string myChatField;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //if (!chatPanel.activeSelf)

            // 기본상태에섯 엔터를 누르면 채팅 모드로 전환
            if (GameMng.I._keyMode.Equals(KEY_MODE.PLAYER_MODE))
            {
                GameMng.I._keyMode = KEY_MODE.TYPING_MODE;
                chatAnim.SetTrigger("ChatOpen");
                chatInput.readOnly = false;
                chatInput.Select();
                
                //chatInput.ActivateInputField();
                //chatInput.next();
                //chatPanel.SetActive(true);
            }

            // 입력한 것이 없고 그냥 엔터를 다시 눌렀다면 기본 상태로 전환
            else if (myChatField == "" && GameMng.I._keyMode.Equals(KEY_MODE.TYPING_MODE))
            {
                chatInput.readOnly = true;

                GameMng.I._keyMode = KEY_MODE.PLAYER_MODE;
                chatAnim.SetTrigger("ChatClose");
                //chatPanel.SetActive(false);
            }

            // 입력한 것이 있어서
            else if (myChatField != "" && GameMng.I._keyMode.Equals(KEY_MODE.TYPING_MODE))
            {
                chatInput.readOnly = true;
                GameMng.I._keyMode = KEY_MODE.PLAYER_MODE;
                chatAnim.SetTrigger("MessageOpen");
                NetworkMng.I.SendMsg(string.Format("CHAT:{0}", myChatField));
                chatLogs.text += string.Format("\n[{0}] : {1} ({2})", GameMng.I.userData.user_nickname, myChatField, System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute);
                myChatField = "";
                chatInput.text = "";
            }
        }
    }

    public void InputChat()
    {
        myChatField = chatInput.text;
    }

    public void newMessage(string nickName, string msg)
    {
        chatLogs.text += string.Format("\n[{0}] : {1} ({2})", nickName, msg, System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute);

        // 채팅 올라오는 효과음
        // SoundMng.I.PlayEffect()

        if (GameMng.I._keyMode.Equals(KEY_MODE.PLAYER_MODE))
            chatAnim.SetTrigger("MessageOpen");
    }
}
