using System.Collections;


public class CellularAutomatonGenerator : UnityEngine.MonoBehaviour
{

	public int width = 50;
	public int height = 50;
	
	public float initialAliveChance = 0.4f;

	public int underPopulationPoint = 4;
	public int birthPoint = 4;

	private bool genWall = true;

	private bool[,] newWorld;
	private bool[,] oldWorld;

	private ArrayList wallList = new ArrayList();

	public UnityEngine.GameObject wall;

	// Use this for initialization
	void Start () {
		oldWorld = new bool[width, height];
	}
	
	public void stepSimulation()
	{
		newWorld = new bool[width, height];

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				int neighbors = countAliveNeighbors(i, j);

				if (oldWorld[i, j]) 
				{
					if(neighbors < underPopulationPoint)
					{
						newWorld[i, j] = false;
					}
					else
					{
						newWorld[i, j] = true;
					}
				}
				else
				{
					if(neighbors > birthPoint)
					{
						newWorld[i, j] = true;
					}
				}
			}
		}

		oldWorld = newWorld;

		updateWalls();
	}

	private int countAliveNeighbors(int row, int col)
	{
		int count = 0;

		for(int i = row - 1; i <= row + 1; i++)
		{
			for(int j = col - 1; j <= col + 1; j++)
			{
				// Don't count self as neighbor
				if (i == row && j == col) break;

				// Count out of bounds as Dead
				if(i > 0 && i < width && j > 0 && j < height && oldWorld[i, j])
				{
					count++;
				} 
			}
		}

		return count;
	}

	public void initializeWorld()
	{
		oldWorld = new bool[width, height];

		System.Random rand = new System.Random();

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (rand.NextDouble() < initialAliveChance)
				{
					oldWorld[i, j] = true;
				}
			}
		}

		updateWalls();
	}

	public void switchWalls()
	{
		genWall = !genWall;
		updateWalls();
	}
	
	private void updateWalls()
	{
		foreach (UnityEngine.GameObject item in wallList)
		{
			Destroy(item);
		}

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (oldWorld[i, j] == genWall)
				{
					wallList.Add(Instantiate(wall, new UnityEngine.Vector3(i * wall.transform.localScale.x, 0, j * wall.transform.localScale.z), new UnityEngine.Quaternion()));
				}
			}
		}

	}

	// Update is called once per frame
	void Update () {
	}
}
