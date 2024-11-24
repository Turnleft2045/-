using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour,ISaveable
{
    [Header("事件监听")]
    public VoidEventSO newGameEvent;
    [Header("基本属性")]

    public float maxHealth;

    public float currentHealth;

    [Header("无敌帧")]

    public float invulnerableDration;
    private float invulnerableCounter;
    public bool invulnerable;

    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;
    public UnityEvent<Character> OnHealthChage;
    private void NewGame()
    {
        currentHealth = maxHealth;
       
        OnHealthChage?.Invoke(this);

    }

    private void Update()
    {
        if(invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter<=0)
            {
                invulnerable = false;
            }
        }
    }

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Water"))
          {
            if (currentHealth > 0)
            {
                currentHealth = 0;
                OnHealthChage?.Invoke(this);
                OnDie?.Invoke();
            }
           
        }
    }

    public void TakeDamage(Attack attacker)
    {
        if (invulnerable)
            return;
        if (currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable();

            //执行受伤
            OnTakeDamage?.Invoke(attacker.transform);
            OnHealthChage?.Invoke(this);
        }
        else if(currentHealth!=0)
        {
            //执行死亡
            currentHealth = 0;
            OnDie?.Invoke();
            OnHealthChage?.Invoke(this);
        }
    }

    private void TriggerInvulnerable()
    {
        if (!invulnerable)
        {   
            invulnerable = true;
            invulnerableCounter = invulnerableDration;
        }

    }

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }


    public void GetSaveData(Data data)
    {
        if(data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            data.characterPosDict[GetDataID().ID] = transform.position;
            data.floatSavedData[GetDataID().ID + "health"] = this.currentHealth;
        }
        else
        {
            data.characterPosDict.Add(GetDataID().ID, transform.position);
            data.floatSavedData.Add(GetDataID().ID + "health", this.currentHealth);
            
        }
    }

    public void LoadData(Data data)
    {
        if(data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            transform.position = data.characterPosDict[GetDataID().ID];
            this.currentHealth = data.floatSavedData[GetDataID().ID + "health"];

            OnHealthChage?.Invoke(this);
        }
    }



}
