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

    public void statText(float lv,float curexp, PlayerStat Stat)//float nextexp,float damage, float Df, float vgr, float mnt)
    {
        player_Lv.text = "���緹�� : " + lv;
        player_CurExp.text = "���� ����ġ : " + curexp.ToString("N1");
        player_NextExp.text = "�ʿ� ����ġ : " + Stat.NextExp.ToString("N1");
        player_Vgr.text = "����� : " + Stat.vgr;
        Player_Mnt.text = "���ŷ� : " + Stat.mnt.ToString("N1");
        player_Atk.text = "���ݷ� : " + Stat.str.ToString("N1");
        player_Def.text = "���� : " + Stat.ind.ToString("N1");
    }
}
