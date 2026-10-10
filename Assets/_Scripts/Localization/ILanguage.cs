using UnityEngine;
using System.Collections;

public interface ILanguage
{
    string Prefix{ get; }
    string GetString(string key);
}
