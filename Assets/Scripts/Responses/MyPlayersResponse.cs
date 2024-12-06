
using System.Collections.Generic;

[System.Serializable]
public class MyPlayersResponse
{
    public string message;
    public List<TeamPlayer> data;
}

[System.Serializable]
public class TeamPlayer
{
    public Player players;
}

[System.Serializable]
public class Player
{
    public string playerName;
    public PlayerStats playerStats;
}

[System.Serializable]
public class PlayerStats
{
    public int technique;
    public int pass;
    public int pace;
    public int agility;
    public int defense;
    public int finishing;
    public int stamina;
}