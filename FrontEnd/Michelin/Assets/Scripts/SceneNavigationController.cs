using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneNavigationController : MonoBehaviour
{
    public TextMeshProUGUI warningText; // 경고 메시지를 표시할 텍스트

    // 주막 버튼 이벤트
    public void OnTavernButtonClicked()
    {
        if (GameMgr.Instance.time >= 0 && GameMgr.Instance.time < 12) // 낮 시간인 경우
        {
            warningText.text = "현재는 주막 운영 시간이 아닙니다.";
        }
        else
        {
            warningText.text = "";
            SceneManager.LoadScene("Restraunt"); // 주막 씬으로 이동
        }
    }

    // 사냥터 버튼 이벤트
    public void OnHuntingGroundButtonClicked()
    {
        if (GameMgr.Instance.time >= 12 && GameMgr.Instance.time <= 24) // 밤 시간인 경우
        {
            warningText.text = "현재는 호랑이 출몰 시간입니다. 사냥터에 들어갈 수 없습니다.";
        }
        else
        {
            warningText.text = "";
            GameMgr.Instance.inventoryControl.draggable.resetPos();
            SceneManager.LoadScene("Hunting Ground"); // 사냥터 씬으로 이동
        }
    }
}
