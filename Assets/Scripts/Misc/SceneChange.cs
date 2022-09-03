using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneChange
{
    private static GameManager gameManager;
    public static bool initialized = false;
    public static bool update = false;
    public static Vector3 playerLoc;
    public static float currency;
    public static float totalEarnings;
    public static List<Inventory_Item> items;

    public static Skill leadership;
    public static Skill teamwork;
    public static Skill technology;
    public static Skill professionalism;
    public static Skill communication;
    public static Skill critThinking;

    public static string playerName;

    public static void Initialize()
    {
        if (!initialized)
        {

            currency = Player_Inventory.instance.currency;
            totalEarnings = Player_Inventory.instance.totalEarnings;
            items = Player_Inventory.instance.items;

            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            leadership = gameManager.Leadership;
            teamwork = gameManager.Teamwork;
            technology = gameManager.Technology;
            professionalism = gameManager.Professionalism;
            communication = gameManager.Communication;
            critThinking = gameManager.CritThinking;

            playerName = gameManager.Name;

            initialized = true;
        }

    }
    public static void PreSceneChange()
    {
        playerLoc = GameManager.instance.player_character.gameObject.transform.position;
        currency = Player_Inventory.instance.currency;
        totalEarnings = Player_Inventory.instance.totalEarnings;
        items = Player_Inventory.instance.items;

        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager = GameManager.instance;
        leadership = gameManager.Leadership;
        teamwork = gameManager.Teamwork;
        technology = gameManager.Technology;
        professionalism = gameManager.Professionalism;
        communication = gameManager.Communication;
        critThinking = gameManager.CritThinking;

        playerName = gameManager.Name;
    }

    public static void PostSceneChange()
    {
        update = true;
        Player_Inventory.instance.currency = currency;
        Player_Inventory.instance.totalEarnings = totalEarnings;
        Player_Inventory.instance.items = items;
        GameManager.instance.player_character.gameObject.transform.position = playerLoc;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.Leadership = leadership;
        gameManager.Teamwork = teamwork;
        gameManager.Technology = technology;
        gameManager.Professionalism = professionalism;
        gameManager.Communication = communication;
        gameManager.CritThinking = critThinking;
    }

    public static void RefreshInventoryUI()
    {
        if (update)
        {
            Player_Inventory.instance.UI.update_currency(currency);
            Player_Inventory.instance.UI.Update_UI();
            update = false;
        }
    }

}
