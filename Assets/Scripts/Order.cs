using System.ComponentModel.Design;
using UnityEngine;

public class Order : OrderDishParentObj
{
    public string orderID;
    public Recipe Recipe;
    public Dish Dish;
    public int score;

    public void OnSubmitOrder(Dish dish)
    {
        this.Dish = dish;
        // TODO: Additional logic for submitting the order
        CompareOrderToDish();
    }

    private void CompareOrderToDish()
    {
        // TODO: Logic to compare the order to the dish
    }
}
