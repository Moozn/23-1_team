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
        player_Lv.text = "현재레벨 : " + lv;
        player_CurExp.text = "소지 경험치 : " + curexp.ToString("N1");
        player_NextExp.text = "필요 경험치 : " + Stat.NextExp.ToString("N1");
        player_Vgr.text = "생명력 : " + Stat.vgr;
        Player_Mnt.text = "정신력 : " + Stat.mnt.ToString("N1");
        player_Atk.text = "공격력 : " + Stat.str.ToString("N1");
        player_Def.text = "방어력 : " + Stat.ind.ToString("N1");
    }
}
