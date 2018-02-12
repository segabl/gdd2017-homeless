using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace KarmaSystem
{
  public class KarmaController : PausableObject
  {
    private Dictionary<GameObject,SocialStatus> socialStatusList;


    protected override void updatePausable()
    {

    }

    public void Awake()
    {
      socialStatusList = new Dictionary<GameObject, SocialStatus>();
      GameObject[] characters = GameObject.FindGameObjectsWithTag("character");
      foreach (GameObject character in characters)
      {
        socialStatusList.Add(character, new SocialStatus(new Dictionary<GameObject, Relationship>(),new Reputation()));
      }
    }

    public void DebugKarmaList()
    {
      foreach (GameObject g1 in socialStatusList.Keys)
      {
        SocialStatus k = socialStatusList[g1];
        Debug.Log("Charater: " + g1.name);
        Debug.Log("Reputation: Charity: " + k.reputation.charity);
        Debug.Log("Reputation: Reliability: " + k.reputation.reliability);
        Debug.Log("Reputation: Criminality: " + k.reputation.criminality);
        Debug.Log("Reputation: Cruelty: " + k.reputation.cruelty);
        Debug.Log("Relationships: ");
        foreach (GameObject g2 in k.relationships.Keys)
        {
          Relationship r = k.relationships[g2];
          Debug.Log(" " + g1.name + "->" + g2.name + ": Trust: " + r.trust);
          Debug.Log(" " + g1.name + "->" + g2.name + ": Affection: " + r.affection);
        }
      }
    }
  }
  internal class SocialStatus //Links relationships and reputation to a character
  {
    internal Dictionary<GameObject, Relationship> relationships;
    internal Reputation reputation;

    internal SocialStatus(Dictionary<GameObject,Relationship> relationships_, Reputation reputation_)
    {
      relationships = relationships_;
      reputation = reputation_;
    }
  }
  internal class Relationship //inter-character social values
  {
    internal Relationship(int trust_=SocialConstants.defaultTrust, int affection_=SocialConstants.defaultAffection)
    {
      trust = trust_;
      affection = affection_;
    }
    internal int trust { get; set; }
    internal int affection { get; set; }

  }
  internal class Reputation //general social values
  {
    internal Reputation(int charity_=SocialConstants.defaultCharity, int reliability_=SocialConstants.defaultReliability,
      int criminality_=SocialConstants.defaultCriminality, int cruelty_=SocialConstants.defaultCruelty)
    {
      charity = charity_;
      reliability = reliability_;
      criminality = criminality_;
      cruelty = cruelty_;
    }

    internal int charity { get; set; }
    internal int reliability { get; set; }
    internal int criminality { get; set; }
    internal int cruelty { get; set; }


  }


  internal class SocialEffector
  {
    protected ReputationEffector reputationEffector;
    protected RelationshipEffector relationshipEffector;
    public SocialEffector(ReputationEffector reputationEffector_= null, RelationshipEffector relationshipEffector_=null)
    {
      reputationEffector = reputationEffector_;
      relationshipEffector = relationshipEffector_;
    }
    internal void Apply(SocialStatus socialStatus)
    {
      if (reputationEffector != null)
        reputationEffector.Apply(socialStatus.reputation);
      if (relationshipEffector != null && relationshipEffector.target != null)
        relationshipEffector.Apply(socialStatus.relationships[relationshipEffector.target]);
    }
  }
  internal class ReputationEffector
  {
    protected int charityEffector, reliabilityEffector, criminalityEffector, crueltyEffector;
    public ReputationEffector(int charityEffector_=0, int reliabilityEffector_=0,
      int criminalityEffector_=0, int crueltyEffector_=0)
    {
      charityEffector = charityEffector_;
    }
    internal void Apply(Reputation reputation)
    {
      reputation.charity += charityEffector; //no overflow protection
      reputation.reliability += reliabilityEffector;
      reputation.criminality += criminalityEffector;
      reputation.cruelty += crueltyEffector;

      PropertyInfo[] properties = typeof(Reputation).GetProperties();
      foreach (PropertyInfo property in properties) 
      {
        if ((int)property.GetValue(reputation, null) < 0) 
          property.SetValue(reputation, 0, null);
        if ((int)property.GetValue(reputation, null) > 99)
          property.SetValue(reputation, 99, null);
      }

    }
  }
  internal class RelationshipEffector
  {
    internal GameObject target;
    protected int trustEffector, affectionEffector;
    public RelationshipEffector(GameObject target_=null, int trustEffector_=0, int affectionEffector_=0)
    {
      target = target_;
      trustEffector = trustEffector_;
      affectionEffector = affectionEffector_;
    }
    internal void Apply(Relationship relationship)
    {
      relationship.trust += trustEffector; //no overflow protection
      relationship.affection += affectionEffector;

      PropertyInfo[] properties = typeof(Relationship).GetProperties();
      foreach (PropertyInfo property in properties)
      {
        if ((int)property.GetValue(relationship, null) < 0)
          property.SetValue(relationship, 0, null);
        if ((int)property.GetValue(relationship, null) > 99)
          property.SetValue(relationship, 99, null);
      }

    }
  }
  internal static class SocialConstants //defines constants, social actions and decisions
  {
    internal const int defaultTrust = 0;
    internal const int defaultAffection = 0;
    internal const int defaultCharity = 5;
    internal const int defaultReliability = 3;
    internal const int defaultCriminality = 1;
    internal const int defaultCruelty = 0;


    //Some exemplar actions
    internal static readonly SocialEffector sharingFood = new SocialEffector(new ReputationEffector(1,0,0,0),
      new RelationshipEffector(null,1,1));
    internal static readonly SocialEffector stealingBeerFromShop = new SocialEffector(new ReputationEffector(-1, -1, 1, 0),
      new RelationshipEffector(null, -5, -5));
  }

}