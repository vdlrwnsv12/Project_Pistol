using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("타겟 ID 지상 타겟 = 0, 공중 타겟: 1, 민간인 타겟: 2")]
    public int targetID;

    [Header("데이터")]
    public string Type;
    public int Hp;
    public int Score;
    // public void Init(TargetData data)
    // {
    //     ID = data.ID;
    //     Type = data.Type;
    //     Hp = data.Hp;
    //     Score = data.Score;

    // }
    public Animator anim;

    void Start()
    {
      anim.GetComponent<Animator>();
      LoadDataByID(targetID);
    }

    public void TakeDamage(int amount, Collider hitCollider)
    {
        if(hitCollider != null && hitCollider.name == "Head")
        {
            amount = Mathf.RoundToInt(amount * 1.6f);
            Debug.Log($"헤드샷 데미지: {amount}");
        }
        Hp -= amount;
        
        if(Hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"Die{name},{Type},{Score}");

        anim.SetBool("Die", true);
    }

    private void LoadDataByID(int id)
    {
        TextAsset TargetDataJson = Resources.Load<TextAsset>("Data/JSON/TargetData");
        if(TargetDataJson == null)
        {
            return;
        }

        TargetDataList dataList = JsonUtility.FromJson<TargetDataList>(TargetDataJson.text);

        foreach (var data in dataList.Targets)
        {
            if(data.ID == id)
            {
                Init(data);
                return;
            }
        }
        Debug.Log("id에 맞는 데이터 찾을수없음");
    }
    public void Init(TargetData data)
    {
        Type = data.Type;
        Hp = data.Hp;
        Score = data.Score;
        Debug.Log($"타겟 초기화 ID: {data.ID},Type: {Type}, HP: {Hp}, Score: {Score} ");
    }

}
