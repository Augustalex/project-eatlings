using System;
using Unity.Mathematics;
using UnityEngine;

public class ItemHolderParent : MonoBehaviour
{
    // Public
    [SerializeField] private Animator animator;
    [SerializeField] private ItemHolder itemHolder;


    private float _layerWeightLeft;
    private float _layerWeightRight;
    private float _layerWeightLeftAnim;
    private float _layerWeightRightAnim;
    private static readonly int Planting = Animator.StringToHash("Planting");
    private FarmerMovement _movement;
    private static readonly int Watering = Animator.StringToHash("Watering");
    private static readonly int ToolWatercan = Animator.StringToHash("ToolWatercan");

    private void Awake()
    {
        _movement = GetComponent<FarmerMovement>();
    }

    private void OnEnable()
    {
        itemHolder.DidHoldItem += ItemHolderOnDidHoldItem;
        itemHolder.DidDropItem += ItemHolderOnDidDropItem;
        itemHolder.DidUseItem += ItemHolderOnDidUseItem;
        itemHolder.UsedItem += ItemHolderOnUsedItem;
    }

    private void ItemHolderOnDidUseItem(ItemHolder.ItemActivity activity)
    {
        if (activity == ItemHolder.ItemActivity.Plant)
        {
            _layerWeightLeft = 0;
            _layerWeightRight = 0;
            animator.SetTrigger(Planting);
        }
        
        else
        {
            Debug.Log("No animation for activity: " + activity);
        }
    }
    
    
    private void ItemHolderOnUsedItem(ItemHolder.ItemActivity activity) {
        if (activity == ItemHolder.ItemActivity.Watering)
        {
            _layerWeightLeft = 1;
            _layerWeightRight = 1;
            animator.SetBool(Watering, true);
        }
    }

    private void OnDisable()
    {
        itemHolder.DidHoldItem -= ItemHolderOnDidHoldItem;
    }

    private void ItemHolderOnDidDropItem()
    {
        _layerWeightLeft = 0;
        _layerWeightRight = 0;
        animator.SetBool(ToolWatercan, false);
    }

    private void ItemHolderOnDidHoldItem(ItemHolder.ItemActivity activity)
    {
        _layerWeightLeft = 0;
        _layerWeightRight = 1;
        if (activity == ItemHolder.ItemActivity.Watering) {
            _layerWeightLeft = 1;
            animator.SetBool(ToolWatercan, true);
        }
        
    }

    private void Update()
    {
        // TODO: optimize
        // if (_layerWeightAnim <= 0 || _layerWeightAnim >= 1)
        // {
        //     
        // }
        _layerWeightLeftAnim = math.lerp(_layerWeightLeftAnim, _layerWeightLeft, 0.2f);
        _layerWeightRightAnim = math.lerp(_layerWeightRightAnim, _layerWeightRight, 0.2f);
        int _LeftArm = 1;
        int _RightArm = 2;
        animator.SetLayerWeight(_LeftArm, _layerWeightLeftAnim);
        animator.SetLayerWeight(_RightArm, _layerWeightRightAnim);
    }
}
