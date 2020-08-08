using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CharacterStateUI : MonoBehaviour
{
    public TMP_Text turnText;
    public TMP_Text damageText;
    public TMP_Text armourText;

    public Slider healthSlider;
    public Image healthFill;

    public Gradient healthGradient;

    private RewindUI context;

    private CharacterSnapshot snapshot;

    public void SetUI(CharacterSnapshot snapshot, RewindUI context)
    {
        this.context = context;
        this.snapshot = snapshot;
        turnText.text = $"Turn {snapshot.Turn}";
        damageText.text = $"+{snapshot.Damage}";
        armourText.text = $"+{snapshot.Armour}";
        healthSlider.maxValue = snapshot.MaxHealth;
        healthSlider.value = snapshot.Health;
        healthFill.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }


    // public IEnumerable<DamageInfo> WeaponDamage
    // {
    //     get
    //     {
    //         return (EquipmentSlots[(int)EquipmentSlot.Weapon] as Weapon)
    //             ?.Damage
    //             .GroupBy(p => p.DamageType)
    //             .Select(g => new DamageInfo { DamageType = g.Key, Value = g.Sum(p => p.Value) });
    //     }
    // }

    // public IEnumerable<DamageInfo> ArmourResistances
    // {
    //     get
    //     {
    //         return EquipmentSlots
    //             .OfType<Armour>()
    //             .SelectMany(p => p.Resistances)
    //             .GroupBy(p => p.DamageType)
    //             .Select(g => new DamageInfo { DamageType = g.Key, Value = g.Sum(p => p.Value) });
    //     }
    // }

    public void SetState()
    {
        context.SetState(snapshot);
    }
}
