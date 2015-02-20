using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LugusRandomGeneratorGrid : ILugusRandomGenerator
{
	protected float _xDir;
	protected float _yDir;
	protected float _zDir;
	protected float _width = 1f;
	protected float _height = 1f;
	protected float _depth = 1f;

	protected int _rows;
	public int Rows
	{
		get
		{
			return _rows;
		}
		set
		{
			_rows = value;
			_yDir = _height / _rows;
		}
	}

	protected int _columns;
	public int Columns
	{
		get
		{
			return _columns;
		}
		set
		{
			_columns = value;
			_xDir = _width / _columns;
		}
	}

	protected int _layers;
	public int Stacks
	{
		get
		{
			return _layers;
		}
		set
		{
			_layers = value;
			_zDir = _depth / _layers;
		}
	}

	protected float _spread = 0.4f;
	public float Spread
	{
		get
		{
			return _spread;
		}
		set
		{
			_spread = value;
		}
	}

	protected Vector3[,,] _grid;
	public Vector3[,,] Grid
	{
		get
		{
			return _grid;
		}
	}

	protected int _currentX = 0;
	protected int _currentY = 0;
	protected int _currentZ = 0;

	public LugusRandomGeneratorGrid(int rows, int columns): this(rows, columns, 10, System.DateTime.Now.Millisecond){}
	public LugusRandomGeneratorGrid(int rows, int columns, int layers): this(rows, columns, layers, System.DateTime.Now.Millisecond){}
	public LugusRandomGeneratorGrid(int rows, int columns, int layers, int seed)
	{
		_dr = new DataRange(0, columns * rows * layers); 
		SetSeed(seed);

		_xDir = _width / columns;
		_yDir = _height / rows;
		_zDir = _depth / layers;
	
		_rows = rows;
		_columns = columns;
		_layers = layers;

		_grid = Get3DScatterGrid();
	}

	public new Vector3 GetValue ()
	{
		Vector3 nextValue = _grid[_currentX,_currentY,_currentZ];
		_currentX++;
		if (_currentX%_columns == 0) 
		{
			_currentX = 0;
			_currentY++;
			if (_currentY%_rows == 0) 
			{
				_currentY = 0;
				_currentZ++;
				if (_currentZ%_layers == 0) 
				{
					_currentZ=0;
				}
			}
		}
		return nextValue;
	}

	//source : http://www.gamasutra.com/view/feature/130071/random_scattering_creating_.php?page=2
	public Vector3[,] Get2DScatterGrid(float scale = 1f)
	{
		Vector3[,] grid = new Vector3[_columns, _rows];
		
		for (int ix = 0; ix < _columns; ix++) 
		{
			for (int iy = 0; iy < _rows; iy++) 
			{ 
				grid[ix,iy] = scale * new Vector3(
					_xDir * ix + GetValue(-_spread, _spread) *_xDir, 
					_yDir * iy + GetValue(-_spread, _spread) *_yDir);
			}
		}

		return grid;
	}

	public List<Vector3> Get2DScatterGridList(float scale = 1f)
	{
		List<Vector3> result = new List<Vector3>();

		foreach (Vector3 v in Get2DScatterGrid(scale))
		{
			result.Add(v);
		}

		return result;
	}

	public Vector3 [,,] Get3DScatterGrid(float scale = 1f)
	{
		Vector3[,,] grid = new Vector3[_columns, _rows, _layers];
		for (int ix = 0; ix <_columns; ix++) 
		{
			for (int iy = 0; iy <_rows; iy++) 
			{ 
				for (int iz = 0; iz < _layers; iz++) 
				{
					grid[ix,iy,iz] = scale * new Vector3(
						_xDir * ix + GetValue(-_spread, _spread) *_xDir,
						_yDir * iy + GetValue(-_spread, _spread) *_yDir,
						_zDir * iz + GetValue(-_spread, _spread) *_zDir);
				}
			}
		}

		return grid;
	}

	public List<Vector3> Get3DScatterGridList(float scale = 1f)
	{
		List<Vector3> result = new List<Vector3>();
		
		foreach (Vector3 v in Get3DScatterGrid(scale))
		{
			result.Add(v);
		}
		
		return result;
	}
	
	public override void SetSeed (int seed)
	{
		base.SetSeed (seed);
		ResetGrid();
	}
	public override void Reset ()
	{
		base.Reset ();
		ResetGrid();
	}
	public void ResetGrid()
	{
		_currentX=0;
		_currentY=0;
		_currentZ=0;
		_grid = Get3DScatterGrid();
	}
}