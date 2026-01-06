using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private List<WeaponBase> weapons = new List<WeaponBase>();


    private void Awake()
    {
        if(weapons == null || weapons.Count == 0)
        {
            WeaponBase[] found = GetComponentsInChildren<WeaponBase>(true);
            if(found != null)
            {
                weapons = new List<WeaponBase>(found);
            }
        }
    }


    public void Register(WeaponBase weapon)
    {
        if (weapon != null)
        {
            if(weapons.Contains(weapon) == false)
            {
                weapons.Add(weapon);
            }
        }
    }


    // T : Template.
    public void EnableWeapon<T>(bool enabled) where T : WeaponBase 
    {
        for(int i=0; i < weapons.Count; ++i)
        {
            T w = weapons[i] as T;
            if(w != null)
            {
                w.GameObject().SetActive(enabled);
            }
        }
    }


    //WeaponBase로부터 상속받은 모든 ClassType을 반환
    public T GetWeapon<T>() where T : WeaponBase
    {
        for (int i = 0; i < weapons.Count; ++i)
        {
            T w = weapons[i] as T;
            if(w != null)
            {
                return w;
            }
        }

        return null;
    }


    //업그레이드 관련 함수. 업그레이드를 하면 내가 가진 모든 무기에 업그레이드 수치 적용
    public void AddProjectileCountAll(int add)
    {
        for (int i = 0; i < weapons.Count; ++i)
        {
            int cur = weapons[i].GetProjectileCount();
            weapons[i].SetProjectileCount(Mathf.Max(1, cur + add));
        }
    }


    public void ReduceCooldownAll(float delta)
    {
        for(int i = 0; i < weapons.Count; ++i) //내가 가진 무기 개수만큼 for문을 돌림
        {
            float cur = weapons[i].GetAttackCooldown();
            float next = Mathf.Max(0.05f, cur - delta); 
            weapons[i].SetAttackCooldown(next);
        }
    }


    public void AddDamageAll(float add)
    {
        for (int i = 0; i < weapons.Count; ++i)
        {
            float cur = weapons[i].GetDamage();
            weapons[i].SetDamage(cur + add);
        }    
    }


    public void AddProjectileSpeedAll(float add)
    {
        for (int i = 0; i < weapons.Count; ++i)
        {
            float cur = weapons[i].GetProjectileSpeed();
            weapons[i].SetProjectileSpeed(cur + add);
        }
    }
}
