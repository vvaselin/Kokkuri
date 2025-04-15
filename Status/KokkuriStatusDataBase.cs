using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "KokkuriDataBase")]
public class KokkuriStatusDataBase : ScriptableObject
{
    public List<KokkuriStatus> kokkuris = new List<KokkuriStatus>();
}
