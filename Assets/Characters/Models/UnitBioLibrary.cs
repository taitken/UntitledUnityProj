
using System.Collections.Generic;
using UnityEngine;

namespace Unit.Models
{
    public static class UnitBioLibrary
    {
        private static IList<string> maleNames = new List<string>{
            "Jim", "Plim", "Bobell", "Jobly", "Wuzark", "Slizzik", "Preek", "Jolkin", "Puzzard"
        };
        private static IList<string> femaleNames = new List<string>{
            "Jimess", "Plum", "Boell", "Jonl", "Wuzly", "Slizzla", "Preen", "Jolee", "Puzzarl"
        };
        private static IList<string> usedMaleNames = new List<string>();
        private static IList<string> usedFemaleNames = new List<string>();

        public static string GetName(eCharacterSex sex)
        {
            if (sex == eCharacterSex.Male)
            {
                int index = Random.Range(0, UnitBioLibrary.maleNames.Count);
                string charName = UnitBioLibrary.maleNames[index];
                UnitBioLibrary.maleNames.RemoveAt(index);
                UnitBioLibrary.usedMaleNames.Add(charName);
                if (UnitBioLibrary.maleNames.Count == 0)
                {
                    UnitBioLibrary.maleNames = usedMaleNames;
                    UnitBioLibrary.usedMaleNames = new List<string>();
                }
                return charName;
            }
            else
            {
                int index = Random.Range(0, UnitBioLibrary.femaleNames.Count);
                string charName = UnitBioLibrary.femaleNames[index];
                UnitBioLibrary.femaleNames.RemoveAt(index);
                UnitBioLibrary.usedFemaleNames.Add(charName);
                if (UnitBioLibrary.femaleNames.Count == 0)
                {
                    UnitBioLibrary.femaleNames = usedFemaleNames;
                    UnitBioLibrary.usedFemaleNames = new List<string>();
                }
                return charName;
            }
        }

        public static eCharacterSex GetSex()
        {
            return (eCharacterSex)Random.Range(0, 2);
        }
    }
}