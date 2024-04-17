using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class npc_control : MonoBehaviour
{
    public RectTransform NPC_A;
    public RectTransform NPC_B;
    public RectTransform NPC_C;

    public float transitionTime = 0.2f;


    private RectTransform currentNPC;
    public TMP_Text NPC_name;
    public TMP_Text NPC_dialog;

    void Start()
    {
        transitionTime = 0.2f;
        currentNPC = NPC_A;
        NPC_name.text = "��������";
        NPC_dialog.text = "����... �ڳ��� �ֹ�Į... �� ���ƺ��̴µ�!\n���� �ѹ� �������� �ǰڳ�?";
        NPC_B.position = NPC_B.position + new Vector3(700, 0, 0);
        NPC_C.position = NPC_C.position + new Vector3(700, 0, 0);
    }

    public void ShowImageA()
    {
        if (currentNPC == NPC_A) return;
        NPC_name.text = "";
        NPC_dialog.text = "";
        StartCoroutine(TransitionImage(currentNPC, NPC_A));
        NPC_name.text = "��������";
        NPC_dialog.text = "����... �ڳ��� �ֹ�Į... �� ���ƺ��̴µ�!\n���� �ѹ� �������� �ǰڳ�?";
        currentNPC = NPC_A;
    }
    public void ShowImageB()
    {
        if (currentNPC == NPC_B) return;
        NPC_name.text = "";
        NPC_dialog.text = "";
        StartCoroutine(TransitionImage(currentNPC, NPC_B));
        NPC_name.text = "���°�����";
        NPC_dialog.text = "���� �Ʒ��� ���������� ���Գ�.";
        currentNPC = NPC_B;

    }
    public void ShowImageC()
    {
        if (currentNPC == NPC_C) return;
        NPC_name.text = "";
        NPC_dialog.text = "";
        StartCoroutine(TransitionImage(currentNPC, NPC_C));
        NPC_name.text = "��������";
        NPC_dialog.text = "�������~!";
        currentNPC = NPC_C;
    }

    IEnumerator TransitionImage(RectTransform currentImage, RectTransform nextImage)
    {
        // ���� �̹����� ���������� �̵�
        float elapsedTime = 0;
        Vector3 startPos = currentImage.position;
        Vector3 endPos = startPos + new Vector3(700, 0, 0);

        while (elapsedTime < transitionTime)
        {
            currentImage.position = Vector3.Lerp(startPos, endPos, elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        currentImage.position = endPos;

        // ���� �̹����� ȭ�� ���� �ٱ����� �߾����� �̵�
        elapsedTime = 0;
        startPos = nextImage.position;
        endPos = startPos - new Vector3(700, 0, 0);

        while (elapsedTime < transitionTime)
        {
            nextImage.position = Vector3.Lerp(startPos, endPos, elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        nextImage.position = endPos;
    }
}
