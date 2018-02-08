using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KarmaSystem
{
  public class KarmaController : PausableObject
  {
    private List<Karma> karmaList;

    protected override void updatePausable()
    {
      
    }

    void awake()
    {
      karmaList = new List<Karma>();
      GameObject[] characters = GameObject.FindGameObjectsWithTag("character");
      foreach (GameObject character in characters)
      {
        karmaList.Add(new Karma(character,null,null));
      }
    }

    void debugKarmaList()
    {
      foreach (Karma k in karmaList)
      {
        Debug.Log("Charater: " + k.character.name);
        Debug.Log("Reputation: Charity: " + k.reputation.charity);
        Debug.Log("Reputation: Reliability: " + k.reputation.reliability);
        Debug.Log("Reputation: Criminality: " + k.reputation.criminality);
        Debug.Log("Reputation: Cruelty: " + k.reputation.cruelty);
        Debug.Log("Relationships: ");
        foreach (Relationship r in k.relationships)
        {
          Debug.Log(k.character.name + "->" + r.character.name + ": Trust: " + r.trust);
          Debug.Log(k.character.name + "->" + r.character.name + ": Affection: " + r.affection);
        }
      }
    }

  }
  internal class Karma //Links relationships and reputation to a character
  {
    public GameObject character;
    public List<Relationship> relationships;
    public Reputation reputation;

    public Karma(GameObject character_, List<Relationship> relationships_, Reputation reputation_)
    {
      character = character_;
      relationships = relationships_;
      reputation = reputation_;
    }
  }
  internal class Relationship //inter-character social values
  {
    public GameObject character;
    public int trust { get; set; }
    public int affection { get; set; }

  }
  internal class Reputation //general social values
  {
    public int charity { get; set; }
    public int reliability { get; set; }
    public int criminality { get; set; }
    public int cruelty { get; set; }
  }
}