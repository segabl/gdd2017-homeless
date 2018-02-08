using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KarmaSystem
{
  public class KarmaController : PausableObject
  {
    private List<Karma> karmas;

    protected override void updatePausable()
    {
      
    }

    void awake()
    {
      GameObject[] characters = GameObject.FindGameObjectsWithTag("character");
      foreach (GameObject character in characters)
      {
        karmas.Add(new Karma());
      }
    }

  }
  internal class Karma
  {
    public GameObject character;
    public List<Relationship> relationships;
    public Reputation reputation;
  }
  internal class Relationship
  {
    public GameObject character;
    public int trust { get; set; }
    public int affection { get; set; }

  }
  class Reputation
  {
    public int charity { get; set; }
    public int reliability { get; set; }
    public int criminality { get; set; }
    public int cruelty { get; set; }
  }
}