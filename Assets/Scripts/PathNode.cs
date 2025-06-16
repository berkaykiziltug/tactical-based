using UnityEngine;

public class PathNode
{
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;

    private PathNode cameFromPathNode;
    
    
    public PathNode(GridPosition gridPosion)
    {
        this.gridPosition = gridPosion;
    }
    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public int GetGCost()
    {
        return gCost;
    }
    public int GetFCost()
    {
        return fCost;
    }
    public int GetHCost()
    {
        return hCost;
    }
}
