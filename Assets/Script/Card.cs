using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum UnitType
{
    None,
    Mask
}
public class Card : MonoBehaviour
{
    public UnitType unitType;
    public string cardName;
    [Multiline]
    public string cardDescription;
    [Multiline]
    public string maskDescription;
    [Header("text")]
    public TextMesh unitNameText;

    [Header("Mask")]
    public SpriteRenderer MaskRend;
    public SpriteRenderer LockRend;
    [Header("cost")]
    private int currentCost;
    public int cost;

    public TextMesh costText;
    [Header("Grid")]
    public int x = -1;
    public int y = -1;
    [Header("resources")]
    public int coin;
    public int food;
    public int water;
    [Header("Sale resources")]
    public int Salecoin;
    public int Salefood;

    [Header("Particles")]
    public ParticleSystem coinParticle;
    public ParticleSystem foodParticle;
    public ParticleSystem waterParticle;
    [Header("effect")]
    public Animator SquareEffect;

    public Place place;
    public bool isWear;
    public bool isLock;

    public List<IAct> acts = new List<IAct>();
    private readonly List<Type> actTypes = new List<Type>
    {
        typeof(TargetAmount),
        typeof(TargetCard),

        typeof(SetCardNearby),

        typeof(DetectAdjacentCard),
        typeof(DetectNRowCard),
        typeof(DetectSameMaskCard),

        typeof(CountEmptySlot),
        typeof(CountMyCostCard),
        typeof(CountNCostCard),
        typeof(CountTargetCardCost),
        typeof(CountAdjacentCard),

        typeof(LessCoin),
        typeof(LessFood),
        typeof(LinkFood),
        typeof(LinkCoin),
        typeof(LinkWater),
        typeof(EarnFood),
        typeof(EarnCoin),
        typeof(EarnWater),
        typeof(LinkToss),
        typeof(WhileCostDone),
        typeof(RunAdjacentCard),
        typeof(MultipleResources),
        typeof(MultipleRunCost)
    };

    private void Awake()
    {
        acts = GetComponents<IAct>().OrderBy(a => actTypes.IndexOf(a.GetType())).ToList();
        currentCost = cost;
        costText.text = currentCost.ToString();
    }
    private void Start()
    {
        Salecoin = coin;
        Salefood = food;
        unitNameText.text = cardName;
    }

    public void AddTotalCost(int plusCost)
    {
        Debug.Log($"AddTotalCost {plusCost}");
        cost += plusCost;
        currentCost = cost;
        costText.text = currentCost.ToString();
    }

    public void Run()
    {
        if (currentCost <= 0) return;

        foreach (var act in acts)
        {
            Debug.Log(act);
            SquareEffect.Play("Action");
            SFXManager.instance.PlaySFX(2);
            if (!act.Run(this))
            {
                return;
            }
        }
        //FinalizeResources();
        //GameManager.gameManager.CheckDayEnd();
        currentCost--;
        costText.text = currentCost.ToString();

        CardManager.cardManager.UpdateCard(this);
    }

    public void Adjustment()
    {
        foreach (var act in acts)
        {
            act.Adjustment(this);
        }
    }

    public void UpdateCard(Card changedCard)
    {
        foreach (var act in acts)
        {
            act.UpdateCard(changedCard);
        }
    }

    public void AddRunActionToQueue()
    {
        CardManager.cardManager.RunCard(x, y);
    }

    public void DisableCollider()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }

    public void ToogleLock()
    {
        isLock = !isLock;
        if (LockRend != null)
        {
            LockRend.enabled = isLock;
        }
    }

    public void Select()
    {
        if (place == null)
        {
            Debug.Log($"선택됨{gameObject}");
            CardManager.cardManager.SelectingCard(gameObject);
            return;
        }
    }

    public void AddAct(IAct newAct)
    {
        acts.Add(newAct);
        acts = acts.OrderBy(a => actTypes.IndexOf(a.GetType())).ToList();
    }

    public void AddMask(Card card)
    {
        if (card == null) return;

        isWear = true;

        // Copy mask sprite/visual
        if (MaskRend != null && card.MaskRend != null)
        {
            MaskRend.sprite = card.MaskRend.sprite;
            MaskRend.enabled = true;
        }

        // Copy mask description
        if (!string.IsNullOrEmpty(card.maskDescription))
        {
            maskDescription = card.maskDescription;
        }

        // Two-pass copy of IAct components so references to other Act components
        // (e.g. TargetCard) are remapped to the newly created instances on this card.
        var srcActs = card.acts.Where(a => a != null).ToList();

        // first pass: create components of the same types and remember mapping
        var mapping = new Dictionary<IAct, Component>();
        foreach (var srcAct in srcActs)
        {
            var t = srcAct.GetType();
            var newComp = (Component)gameObject.AddComponent(t);
            mapping[srcAct] = newComp;
        }

        // second pass: copy fields, remapping component references to newly created ones
        foreach (var srcAct in srcActs)
        {
            var t = srcAct.GetType();
            var newCompObj = mapping[srcAct];

            var fields = t.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            foreach (var f in fields)
            {
                if (f.IsStatic) continue;
                if (Attribute.IsDefined(f, typeof(NonSerializedAttribute))) continue;

                try
                {
                    var val = f.GetValue(srcAct);

                    // If the field is a reference to one of the source acts, remap to the new one
                    if (val is IAct referencedAct && mapping.ContainsKey(referencedAct))
                    {
                        f.SetValue(newCompObj, mapping[referencedAct]);
                        continue;
                    }

                    // If the field is a Component reference (but not part of src acts), try to
                    // find or add an equivalent component on this GameObject.
                    if (val is Component compVal)
                    {
                        var existing = gameObject.GetComponent(compVal.GetType());
                        if (existing == null)
                        {
                            existing = (Component)gameObject.AddComponent(compVal.GetType());
                        }
                        f.SetValue(newCompObj, existing);
                        continue;
                    }

                    // Primitive/serializable fields: copy value directly
                    f.SetValue(newCompObj, val);
                }
                catch (Exception)
                {
                    // ignore fields that can't be copied
                }
            }

            if (newCompObj is IAct newAct)
            {
                AddAct(newAct);
            }
        }
    }
    public int GetCurrentCost()
    {
        return currentCost;
    }

    public void SetCurrentCost(int newCost)
    {
        currentCost = newCost;
        costText.text = currentCost.ToString();
    }

    public void PlayCoinParticle()
    {
        if (coinParticle != null)
        {
            coinParticle.Play();
        }
    }

    public void PlayFoodParticle()
    {
        if (foodParticle != null)
        {
            foodParticle.Play();
        }
    }

    public void PlayWaterParticle()
    {
        if (waterParticle != null)
        {
            waterParticle.Play();
        }
    }
}
