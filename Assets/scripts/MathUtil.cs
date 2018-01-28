using UnityEngine;

public static class MathUtil {

	public static float[] GenerateNumsThatAddToSum (int n, float desiredSum)
	{
		float[] v = new float[n];
		float[] u = new float[n];
		float sum = 0.0f;
		for (int i = 0; i < n; i++)
		{
			v[i] = Random.value;
			u[i] = -Mathf.Log(v[i]);
			sum = sum + u[i];
		}

		float[] p = new float[n];
		float total = 0.0f;
		for (int i = 0; i < n; i++)
		{
			p[i] = (u[i] / sum) * desiredSum;
			total = total + p[i];
		}

		return p;
	}

	// only accurate if n is even lmfao
	// early return if n is odd OR < 2 (because why even bother if n < 2??)
	public static float[] GenerateNumsThatAverageToVal (int n, float avg, float min, float max)
	{
		if (n < 2 || n % 2 != 0) 
		{
			Debug.LogError("Bad input on GenerateNumsThatAverageToVal - defaulting to {avg}");
			return new float[] {avg};
		}

		float[] output = new float[n];

		for (int i = 0; i < n; i += 2)
		{
			// 1. generate a random number
			output[i] = Random.Range(min, max);
			if (i + 1 == n) break;
			// 2. generate a number to bring total back to average
			output[i + 1] = 2*avg - output[i];
		}

		return output;
	}
}
