using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[Serializable]
public class RanksData
{
    Dictionary<RankType, Rank> ranks = new();
}

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class Rank
{
    [JsonProperty] public RankType type;
    [JsonProperty] public int level;
    [JsonProperty] private bool automate;

    public string Name => Localization.TranslateDefault($"RANKS_NAME_{type.ToString().ToUpper()}");
    public string Desc => GetDesc(type, level); // TODO Renamme to NextLevelDesc?

    public string GetDesc(RankType argType, int argLevel) => Localization.TranslateDefault($"RANKS_DESC_{argType.ToString().ToUpper()}_{argLevel}");

    public bool Unlocked() // TODO
    {
        /*
        tier() { return player.ranks.rank.gte(3) || player.ranks.tier.gte(1) || player.mainUpg.atom.includes(3) || tmp.radiation.unl || tmp.inf_unl },
        tetr() { return player.mainUpg.atom.includes(3) || tmp.radiation.unl || tmp.inf_unl },
        pent() { return tmp.radiation.unl || tmp.inf_unl },
        hex() { return tmp.chal13comp || tmp.inf_unl },
        */

        return type switch
        {
            RankType.Rank => true,
            RankType.Tier => true,
            // ...
            _ => false,
        };
    }

    public double Effect(RankType argType, int argLevel) // TODO
    {
        /*
         rank: {
            '3'() {
                let ret = player.build.mass_1.amt.div(20)
                return ret
            },
            '5'() {
                let ret = player.build.mass_2.amt.div(40)
                return ret
            },
            '6'() {
                let ret = player.ranks.rank.add(1).pow(player.ranks.rank.gte(17)?player.ranks.rank.add(1).root(3):2)
                return ret
            },
            '40'() {
                let ret = player.ranks.rank.root(2).div(100)
                if (player.ranks.rank.gte(90)) ret = player.ranks.rank.root(1.6).div(100)
                if (player.ranks.rank.gte(220)) ret = player.ranks.rank.div(100)
                return ret
            },
            '45'() {
                let ret = player.ranks.rank.add(1).pow(1.5)
                return ret
            },
            '300'() {
                let ret = player.ranks.rank.add(1)
                return ret
            },
            '380'() {
                let ret = E(10).pow(player.ranks.rank.sub(379).pow(1.5).pow(player.ranks.tier.gte(55)?RANKS.effect.tier[55]():1).softcap(1000,0.5,0))
                return ret
            },
            '800'() {
                let ret = E(1).sub(player.ranks.rank.sub(799).mul(0.0025).add(1).softcap(1.25,0.5,0).sub(1)).max(0.75)
                return ret
            },
        },
        tier: {
            '4'() {
                let ret = E(0)
                if (player.ranks.tier.gte(12)) ret = player.ranks.tier.mul(0.1)
                else ret = player.ranks.tier.mul(0.05).add(1).softcap(1.4,0.75,0).sub(1)
                return ret
            },
            '6'() {
                let ret = E(2).pow(player.ranks.tier)
                if (player.ranks.tier.gte(8)) ret = ret.pow(RANKS.effect.tier[8]())
                return overflow(ret,'ee100',0.5).overflow('ee40000',0.25,2)
            },
            '8'() {
                let ret = player.bh.dm.max(1).log10().add(1).root(2)
                return ret.overflow('ee5',0.5)
            },
            '55'() {
                let ret = player.ranks.tier.max(1).log10().add(1).root(4)
                return ret
            },
        },
        tetr: {
            '2'() {
                let ret = player.build.mass_3.amt.div(400)
                if (ret.gte(1) && hasPrestige(0,15)) ret = ret.pow(1.5)
                return ret
            },
            '4'() {
                let ret = E(0.96).pow(player.ranks.tier.pow(1/3))
                return ret
            },
            '5'() {
                let ret = player.ranks.tetr.pow(4).softcap(1000,0.25,0)
                return ret
            },
        },
        pent: {
            '2'() {
                let ret = E(1.3).pow(player.ranks.tetr.softcap(12e10,0.1,0))
                return ret
            },
            '4'() {
                let ret = player.supernova.times.add(1).root(5)
                return ret
            },
            '5'() {
                let ret = E(1.05).pow(player.ranks.pent.min(1500))
                return ret
            },
            '8'() {
                let ret = E(1.1).pow(player.ranks.pent)
                return ret
            },
        },
        hex: {
            '4'() {
                let hex = player.ranks.hex
                let ret = hex.mul(.2).add(1)
                if (hex.gte(43)) ret = ret.pow(hex.min(1e18).div(10).add(1).root(2))
                return overflow(ret,1e11,0.5)
            },
        },
         */

        return argType switch
        {
            RankType.Rank => argLevel switch
            {
                3 => 1,
                5 => 1,
                6 => 1,
                // ...
                _ => 0,
            },
            RankType.Tier => argLevel switch
            {
                4 => 1,
                6 => 1,
                8 => 1,
                // ...
                _ => 0,
            },
            // ...
            _ => 0,
        };
    }

    public bool UnlockedAutomation() // TODO
    {
        /*rank() { return player.mainUpg.rp.includes(5) || tmp.inf_unl },
        tier() { return player.mainUpg.rp.includes(6) || tmp.inf_unl },
        tetr() { return player.mainUpg.atom.includes(5) || tmp.inf_unl },
        pent() { return hasTree("qol8") || tmp.inf_unl },
        hex() { return true },*/

        bool autoRankUnlocked = type switch
        {
            RankType.Rank => true,
            RankType.Tier => true,
            // ...
            _ => false,
        };

        return autoRankUnlocked && automate;
    }

    public void Reset() // TODO
    {
        /*
         rank() {
            player.mass = E(0)
            for (let x = 1; x <= UPGS.mass.cols; x++) BUILDINGS.reset("mass_"+x)
        },
        tier() {
            player.ranks.rank = E(0)
            this.rank()
        },
        tetr() {
            player.ranks.tier = E(0)
            this.tier()
        },
        pent() {
            player.ranks.tetr = E(0)
            this.tetr()
        },
        hex() {
            player.ranks.pent = E(0)
            this.pent()
        },
        */
        switch (type)
        {
            case RankType.Rank:
                // ...
                break;

            case RankType.Tier:
                // ...
                break;
        }
    }
}

[JsonConverter(typeof(StringEnumConverter))]
public enum RankType
{ 
    Rank = 0,
    Tier,
    Tetr,
    Pent,
    Hex
}
