using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Score
{
    public Score()
    {
        UserName = "";
        Value = 0;
    }

    public string UserName;
    public int Value;
}

public  class Leaderboard
{
    public Score PlayerScore = null;
    public List<Score> FriendsLeaderboard = new List<Score>();
    public List<Score> WorldLeaderboard = new List<Score>();
}

public class LeaderboardController
{
    #region STATICS
    FacebookPlugin m_facebookPlugin {get { return FacebookPlugin.GetInstance(); } }
    App42Plugin m_app42Plugin { get { return App42Plugin.GetInstance(); } }
    #endregion

    static LeaderboardController s_instance = new LeaderboardController();
    public static LeaderboardController GetInstance() { return s_instance; }

    Dictionary<GameManager.kind, Leaderboard> m_leaderboards = new Dictionary<GameManager.kind, Leaderboard>();

    static string GetScoreName(GameManager.kind kind)
    {
        if (GameConstants.IsBeta)
            return "BETA";

        if (kind == GameManager.kind.Survival)
            return "Bubbler Survival";
        if (kind == GameManager.kind.Timed)
            return "Bubbler Timed";

        return "";
    }

    string GetLocalScoreSK(GameManager.kind gameKind)
    {
        var saveKey = SaveKeys.SK_HIGHSCORE_SURVIVAL;

        if (gameKind == GameManager.kind.Timed)
        {
            saveKey = SaveKeys.SK_HIGHSCORE_TIMED;
        }

        if (GameConstants.IsBeta)
            saveKey = SaveKeys.SK_HIGHSCORE_BETA;

        return saveKey;
    }

    public void GetLeaderboard(GameManager.kind gameKind, DisposableAction<Leaderboard> onLeaderboardsReceived)
    {
        if (!m_leaderboards.ContainsKey(gameKind))
        {
            m_leaderboards[gameKind] = new Leaderboard();

            PlayerScore(gameKind, (playerScore) =>
                {
                    m_leaderboards[gameKind].PlayerScore = playerScore;
                    SecureData.Save(GetLocalScoreSK(gameKind), playerScore.Value.ToString());  

                    LeaderboardRunner.Create(gameKind, m_leaderboards[gameKind], (lbUpdated) =>
                        {
                            if(onLeaderboardsReceived != null)
                                onLeaderboardsReceived.Run(lbUpdated);
                        });                     

                });
        }
        else
            if(onLeaderboardsReceived != null)
                onLeaderboardsReceived.Run(m_leaderboards[gameKind]);
    }

    void PlayerScore(GameManager.kind gameKind, Action<Score> onFinishedLoading)
    {
        var score = new Score();

        score.UserName = m_facebookPlugin.UserId;

        m_app42Plugin.GetMyHighscore(GetScoreName(gameKind), m_facebookPlugin.UserId, (game) =>
            {
                if (game.scoreList.Count > 0)
                    score.Value = (int)game.scoreList[0].value;                 

                if (onFinishedLoading != null)
                    onFinishedLoading(score);

            }, (e) =>
            {
                if (onFinishedLoading != null)
                    onFinishedLoading(score);
            });
    }

    public void UpdatePlayerScore(GameManager.kind gameKind, int newPlayerScore)
    {
        if (m_leaderboards.ContainsKey(gameKind))
        {
            m_leaderboards[gameKind].PlayerScore.Value = newPlayerScore;

            var friendsLb = m_leaderboards[gameKind].FriendsLeaderboard;
            var worldLb = m_leaderboards[gameKind].WorldLeaderboard;

            var found = friendsLb.Find((s) => s.UserName.Equals(m_facebookPlugin.UserId));
            found.Value = newPlayerScore;

            found = worldLb.Find((s) => s.UserName.Equals(m_facebookPlugin.UserId));                 

            if (found != null)
                found.Value = newPlayerScore;
            else
            {
                if (worldLb.Count < m_app42Plugin.WORLD_LIMIT)
                    worldLb.Add(m_leaderboards[gameKind].PlayerScore);
                else if (newPlayerScore >= worldLb[worldLb.Count - 1].Value)
                {
                    worldLb[worldLb.Count - 1] = m_leaderboards[gameKind].PlayerScore;
                }
            }

            friendsLb.Sort((a,b) => b.Value.CompareTo(a.Value));
            worldLb.Sort((a,b) => b.Value.CompareTo(a.Value));
        }
        
    }

    public void SendHighscore(GameManager.kind gameKind, string userId, int score)
    {
        SecureData.Save(GetLocalScoreSK(gameKind), score.ToString());      
        App42Plugin.GetInstance().SendhighScore(GetScoreName(gameKind), FacebookPlugin.GetInstance().UserId, score);
    }

    public bool HigherThanLocalScore(GameManager.kind gameKind, int points)
    {
        var saveKey = GetLocalScoreSK(gameKind);

        int prevScore = 0;
        int.TryParse(SecureData.Get(saveKey, "0"), out prevScore);

        return points > prevScore;
    }

    public void ResetLeaderboards()
    {
        m_leaderboards.Clear();
    }

    public void ResetLocalScores()
    {
        SecureData.Save(SaveKeys.SK_HIGHSCORE_BETA, (0).ToString());   
        SecureData.Save(SaveKeys.SK_HIGHSCORE_TIMED, (0).ToString());   
        SecureData.Save(SaveKeys.SK_HIGHSCORE_SURVIVAL, (0).ToString());   
    }

    class LeaderboardRunner : MonoBehaviour
    {
        #region STATICS
        FacebookPlugin m_facebookPlugin {get { return FacebookPlugin.GetInstance(); } }
        App42Plugin m_app42Plugin { get { return App42Plugin.GetInstance(); } }
        #endregion

        public static void Create(GameManager.kind gameKind, Leaderboard leaderboard, Action<Leaderboard> onFinishedLeaderboard)
        {
            var go = new GameObject("__leaderboard_runner");
            var runner = go.AddComponent<LeaderboardRunner>();

            runner.StartCoroutine(runner.LoadLbs(gameKind, leaderboard, onFinishedLeaderboard));
        }

        IEnumerator LoadLbs(GameManager.kind gameKind, Leaderboard leaderboard, Action<Leaderboard> onFinishedLeaderboard)
        {
            DontDestroyOnLoad(this.gameObject);

            var playerScore = leaderboard.PlayerScore;

            var routine1 = StartCoroutine(GetFriendsLeaderboard(gameKind, (friendsLB) =>
                    {
                        if (!friendsLB.Exists((score) => score.UserName.Equals(playerScore.UserName)))
                            friendsLB.Add(playerScore);

                        leaderboard.FriendsLeaderboard = friendsLB;
                    }));


            var routine2 = StartCoroutine(GetWorldLeaderboard(gameKind, (worldLB) =>
                    {
                        if (worldLB.Count < m_app42Plugin.WORLD_LIMIT && !worldLB.Exists((score) => score.UserName.Equals(playerScore.UserName)))
                            worldLB.Add(playerScore);

                        leaderboard.WorldLeaderboard = worldLB;

                    }));

            yield return routine1;
            yield return routine2;

            if (onFinishedLeaderboard != null)
            {
                onFinishedLeaderboard(leaderboard);
            }

            Destroy(gameObject);
        }

        IEnumerator GetFriendsLeaderboard(GameManager.kind gameKind, Action<List<Score>> onFinishedLoading)
        {
            var finished = false;

            var scoreList = new List<Score>();

            m_app42Plugin.GetHighScoreFacebookFriends(GetScoreName(gameKind), m_facebookPlugin.AccessToken, (game) =>
                {
                    var scores = game.GetScoreList();

                    for (int i = 0; i < scores.Count; i++)
                    {
                        var score = new Score();
                        score.UserName = scores[i].userName;
                        score.Value = (int)scores[i].value;

                        scoreList.Add(score);                       
                    }

                    finished = true;

                }, (e) =>
                {
                    finished = true;
                });

            while (!finished)
                yield return null;

            if (onFinishedLoading != null)
                onFinishedLoading(scoreList);
        }

        IEnumerator GetWorldLeaderboard(GameManager.kind gameKind, Action<List<Score>> onFinishedLoading)
        {
            var finished = false;

            var scoreList = new List<Score>();

            m_app42Plugin.GetHighScore(GetScoreName(gameKind), (game) =>
                {
                    var scores = game.GetScoreList();

                    for (int i = 0; i < scores.Count; i++)
                    {
                        var score = new Score();
                        score.UserName = scores[i].userName;
                        score.Value = (int)scores[i].value;

                        scoreList.Add(score);

                    }

                    finished = true;

                }, (e) =>
                {
                    finished = true;
                });

            while (!finished)
                yield return null;

            if (onFinishedLoading != null)
            {
                onFinishedLoading(scoreList);
            }
        }
    }
}
