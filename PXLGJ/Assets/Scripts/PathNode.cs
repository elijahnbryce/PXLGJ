public class PathNode
{
    public int x, y;
    public int gCost, hCost, fCost;
    public bool isWalkable = true;
    public PathNode searchedFrom;

    public PathNode (int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void CalcF()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}
