using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI player_Lv;
    [SerializeField] private TextMeshProUGUI player_CurExp;
    [SerializeField] private TextMeshProUGUI player_NextExp;
    [SerializeField] private TextMeshProUGUI player_Atk;
    [SerializeField] private TextMeshProUGUI player_Def;
    [SerializeField] private TextMeshProUGUI player_Vgr;
    [SerializeField] private TextMeshProUGUI Player_Mnt;
    [SerializeField] private TextMeshProUGUI end_Lv;
    [SerializeField] private TextMeshProUGUI end_Time;
    [SerializeField] private TextMeshProUGUI cur_Exp;
    [SerializeField] private TextMeshProUGUI total_Exp;
    public void statText(float lv, float curexp, PlayerStat Stat)//float nextexp,float damage, float Df, float vgr, float mnt)
    {
        player_Lv.text = "���緹�� : " + lv;
        player_CurExp.text = "���� ����ġ : " + curexp.ToString("N0");
        player_NextExp.text = "�ʿ� ����ġ : " + Stat.NextExp.ToString("N0");
        player_Vgr.text = "����� : " + Stat.vgr;
        Player_Mnt.text = "���ŷ� : " + Stat.mnt.ToString("N0");
        player_Atk.text = "���ݷ� : " + Stat.str.ToString("N0");
        player_Def.text = "���� : " + Stat.ind.ToString("N0");
    }

    public void EndText(float lv, float time)
    {
        end_Lv.text = "ĳ���� ���� : " + lv;
        end_Time.text = "�Ҹ�ð� : " + time.ToString("N1");
    }

    public void  Exp(float curexp, float totalexp)//float nextexp,float damage, float Df, float vgr, float mnt)
    {
        total_Exp.text = "�̹� ���� ����ġ : " + totalexp.ToString("N0");
        cur_Exp.text = "���� ����ġ : " + curexp.ToString("N0");
    }
}
