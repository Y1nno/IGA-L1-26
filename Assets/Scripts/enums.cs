using UnityEngine;

public enum IngredientType
{
    Topping,
}

public enum RecipeType
{
    Pizza,
}

public enum StepType
{
    Chopping,
    Cooking,
    Plating
}
public enum StationType
{
    CuttingBoard,
    Stove,
    Plate
}

public enum BasketType
{
    Unlimited,
    Limited
}

public enum ProcessingStationState
{
    Empty,
    Full,
    Processing,
    Ready,
    Ruined
}

public enum ProcessingType
{
    Manual,
    Automatic
}

public enum ToolHolderState
{
    Empty,
    Occupied
}