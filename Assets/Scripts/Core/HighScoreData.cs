using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Represents a single high score entry with player name and score.
/// </summary>
[Serializable]
public class HighScoreEntry
{
    /// <summary>
    /// The player's name or initials.
    /// </summary>
    public string playerName;

    /// <summary>
    /// The player's score.
    /// </summary>
    public int score;
}

/// <summary>
/// Container class for a list of high score entries.
/// Used for serialization and storage of multiple high scores.
/// </summary>
[Serializable]
public class HighScoreData
{
    /// <summary>
    /// List of high score entries.
    /// </summary>
    public List<HighScoreEntry> entries = new List<HighScoreEntry>();
}
