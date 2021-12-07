﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Wowthing.Lib.Data;
using Wowthing.Lib.Enums;

namespace Wowthing.Lib.Models.Wow
{
    public class WowItem
    {
        [Key]
        public int Id { get; set; }
        public int ClassMask { get; set; }
        public long RaceMask { get; set; }
        public int Stackable { get; set; }
        public short ClassId { get; set; }
        public short SubclassId { get; set; }
        public short InventoryType { get; set; }
        public short ContainerSlots { get; set; }
        public WowQuality Quality { get; set; }
        public WowStat PrimaryStat { get; set; }

        public int CalculatedClassMask
        {
            get
            {
                if (ClassMask > 0)
                {
                    return ClassMask;
                }

                int classMask = 0;
                var itemStats = new HashSet<WowStat>(Hardcoded.StatToStats[PrimaryStat]);

                // Weapons with primary stats
                if (ClassId == 2 && PrimaryStat != WowStat.None)
                {
                    foreach (var classData in Hardcoded.Characters)
                    {
                        if (classData.WeaponTypes
                            .Any((t =>
                                    (int)t.Item1 == SubclassId &&
                                    t.Item2.Any(stat => itemStats.Contains(stat))
                            )))
                        {
                            classMask |= (int)classData.Mask;
                        }
                    }
                }
                // Armor types
                else if (ClassId == 4 && Hardcoded.ArmorSubclassCharacterMask.TryGetValue(SubclassId, out int armorMask))
                {
                    classMask = armorMask;
                }
                else if (ClassId == 4 && PrimaryStat != WowStat.None)
                {
                    foreach (var classData in Hardcoded.Characters)
                    {
                        if (classData.ArmorTypes
                            .Any((t =>
                                    (int)t.Item1 == SubclassId &&
                                    t.Item2.Any(stat => itemStats.Contains(stat))
                                )))
                        {
                            classMask |= (int)classData.Mask;
                        }
                    }
                }

                return classMask;
            }
        }
    }
}
