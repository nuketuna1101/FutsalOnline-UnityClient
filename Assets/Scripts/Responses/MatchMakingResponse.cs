using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatchMakingResponse
{
    public string message;
    public Match match;
    public User user;
    public Opponent opponent;
    public MatchResult matchResult;
}

[System.Serializable]
public class Match
{
    public int id;
    public string createdAt;
    public string updatedAt;
}

[System.Serializable]
public class User
{
    public int userId;
    public int userNewRating;
}

[System.Serializable]
public class Opponent
{
    public int userId;         
    public int opponentNewRating; 
}

[System.Serializable]
public class MatchResult
{
    public string message;
}