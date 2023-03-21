using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Weapons.ThrowableWeapons
{
    public class ThrowableWeaponBase : WeaponBase
    {
        [SerializeField] protected int maxAmount;
        protected int CurrentAmount;

        public override void Attack()
        {
        }

        //TODO: add weapon stacking
    }
}