using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSerializable
{
    [System.Serializable]
    public class Outfit
    {
        public int Id;
        public string Name;
        public List<string> Item;
        public bool IsSelect;
    }

    [System.Serializable]
    public class Renting
    {
        public string Id;
        public double Time;

        public Renting() { }

        public Renting(string id, double time)
        {
            Id = id;
            Time = time;
        }
    }

    public string Id = "hihidochoo";
    public string Name = "";
    public bool IsAds = true;
    public int Level;
    public int Gold;
    public int Cash;
    public int Gem;
    public int AvatarID = 2;
    public int TotalGames;
    public int TotalWins = 0;
    public int HighestSocre = 0;
    public List<string> Boughts;
    public List<Renting> Rentings;
    public string Language;
    //public List<string> Items;
    public List<string> Events;
    public List<Outfit> Outfits;
    public List<string> NonConsumerPackages;

    public Outfit GetOutfit()
    {
        for(int i = 0; i < Outfits.Count; i++)
        {
            if (Outfits[i].IsSelect)
                return Outfits[i];
        }
        return null;
    }

    public void SetOutfit(int Id)
    {
        for (int i = 0; i < Outfits.Count; i++)
        {
            if (Outfits[i].Id == Id)
                Outfits[i].IsSelect = true;
            else
                Outfits[i].IsSelect = false;
        }
    }

    public int GetOutfitId()
    {
        for (int i = 0; i < Outfits.Count; i++)
        {
            if (Outfits[i].IsSelect == true)
                return i;
        }
        Outfits[0].IsSelect = true;
        return 0;
    }

    public void SetOutfitName(string Name)
    {
        for (int i = 0; i < Outfits.Count; i++)
        {
            if (Outfits[i].IsSelect)
                Outfits[i].Name = Name;
        }
    }

    public PlayerSerializable()
    {
        Id = SystemInfo.deviceUniqueIdentifier;
        IsAds = true;
        Level = 0;
        Gold = 0;
        Cash = 0;
        Gem = 0;
        AvatarID = 2;
        TotalGames = 0;
        TotalWins = 0;
        HighestSocre = 0;
        Name = "You";
        Boughts = new List<string>();
        Rentings = new List<Renting>();
        //Items = new List<string>() { "toc8|hair", "ao21|top", "quan25|bottom", "giay15|shoes" };
        Events = new List<string>();
        Outfits = new List<Outfit>();

        Outfit outfit = new Outfit();
        outfit.Id = Outfits.Count;
        outfit.Name = $"Outfit {outfit.Id}";
        outfit.Item = new List<string>() { "toc8|hair", "ao21|top", "quan25|bottom", "giay15|shoes" };
        outfit.IsSelect = true;
        Outfits.Add(outfit);

        NonConsumerPackages = new List<string>();
    }
}

