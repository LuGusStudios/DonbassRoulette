using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LugusRandom 
{
	private static LugusRandomDefault _instance = null;
	
	public static LugusRandomDefault use 
	{ 
		get 
		{
			if ( _instance == null )
			{
				_instance = new LugusRandomDefault();
			}

			return _instance; 
		}
	}
	
	public static void Change(LugusRandomDefault newInstance)
	{
		_instance = newInstance;
	}
}

public class LugusRandomDefault
{
	protected void OnDisable()
	{
		LugusRandom.Change(null);
	}
	
	protected void OnDestroy()
	{
		LugusRandom.Change(null);
	}
	
	protected LugusRandomGeneratorDistribution _gaussian;
	public LugusRandomGeneratorDistribution Gaussian
	{
		get
		{
			if(_gaussian == null)
			{
				_gaussian = new LugusRandomGeneratorDistribution(Distribution.Gaussian);
			}
			return _gaussian;
		}
	}

	protected LugusRandomGeneratorDistribution _exponential;
	public LugusRandomGeneratorDistribution Exponential
	{
		get
		{
			if(_exponential == null)
			{
				_exponential = new LugusRandomGeneratorDistribution(Distribution.Exponential);
			}
			return _exponential;
		}
	}

	protected LugusRandomGeneratorDistribution _triangular;
	public LugusRandomGeneratorDistribution Triangular
	{
		get
		{
			if(_triangular == null)
			{
				_triangular = new LugusRandomGeneratorDistribution(Distribution.Triangular);
			}
			return _triangular;
		}
	}

	protected LugusRandomGeneratorDistribution _doubleGauss;
	public LugusRandomGeneratorDistribution DoubleGaussian
	{
		get
		{
			if(_doubleGauss == null)
			{
				_doubleGauss = new LugusRandomGeneratorDistribution(Distribution.DoubleGaussian);
			}
			return _doubleGauss;
		}
	}

	protected LugusRandomGeneratorSequence _sequence;
	public LugusRandomGeneratorSequence Sequence
	{
		get
		{
			if(_sequence == null)
			{
				_sequence = new LugusRandomGeneratorSequence(0,10);
			}
			return _sequence;
		}
	}

	protected LugusRandomGeneratorGrid _grid;
	public LugusRandomGeneratorGrid Grid
	{
		get
		{
			if(_grid == null)
			{
				_grid =  new LugusRandomGeneratorGrid(10, 10, 10);
			}
			return _grid;
		}
	}

	protected LugusRandomGeneratorPerlin _perlin;
	public LugusRandomGeneratorPerlin Perlin
	{
		get
		{
			if(_perlin == null)
			{
				_perlin =  new LugusRandomGeneratorPerlin();
			}
			return _perlin;
		}
	}

	protected LugusRandomGeneratorGoldenRatio _goldenRatio;
	public LugusRandomGeneratorGoldenRatio GoldenRatio
	{
		get
		{
			if(_goldenRatio == null)
			{
				_goldenRatio = new LugusRandomGeneratorGoldenRatio();
			}
			return _goldenRatio;
		}
	}

	protected LugusRandomGeneratorUniform _uniform;
	public LugusRandomGeneratorUniform Uniform
	{
		get
		{
			if(_uniform == null)
			{
				_uniform = new LugusRandomGeneratorUniform();
			}
			return _uniform;
		}
	}

	public float GetValue()
	{
		return _uniform.GetValue();
	}

	public float Next(float max)
	{
		return _uniform.GetValue(max);
	}

	public float Next(float min, float max)
	{
		return _uniform.GetValue(min, max);
	}

	public void Reset()
	{
		_uniform.Reset();
	}

	public void ResetAll()
	{
		Uniform.Reset();
		Gaussian.Reset();
		DoubleGaussian.Reset();
		Exponential.Reset();
		Triangular.Reset();
		Sequence.Reset();
		Grid.Reset();
		Perlin.Reset();
		GoldenRatio.Reset();
	}

	public void SetSeed(int seed)
	{
		_uniform.SetSeed(seed);
	}

	public void SetRange(DataRange dr)
	{
		_uniform.Range = dr;
		Reset();
	}
}

