using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    public int playerAttack = 5;
    public int numCoins = 0;
    public int playerDefense = 5;
    public float playerMaxHealth = 20;
    public float playerCurrentHealth = 20;
    public Vector3 playerPosition = new Vector3(-7, 0.3f, 100);

    public bool isNewGame = true;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [System.Serializable]
    class SaveData
    {
        public float maxHealth;
        public float currentHealth;
        public int attack;
        public int defense;
        public int numCoins;
        public Vector3 position;
    }

    public void SavePlayerData()
    {
        SaveData saveData = new SaveData();
        saveData.attack = playerAttack;
        saveData.defense = playerDefense;
        saveData.maxHealth = playerMaxHealth;
        saveData.currentHealth = playerCurrentHealth;
        saveData.position = playerPosition;
        saveData.numCoins = numCoins;

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/playerSaveFile.json", json);
    }

    public void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/playerSaveFile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            playerMaxHealth = saveData.maxHealth;
            playerCurrentHealth = saveData.currentHealth;
            playerDefense = saveData.defense;
            playerPosition = saveData.position;
            playerAttack = saveData.attack;
            numCoins = saveData.numCoins;
        }
    }

}
