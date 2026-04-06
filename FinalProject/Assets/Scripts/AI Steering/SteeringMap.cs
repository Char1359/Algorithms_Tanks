using NUnit.Framework;
using UnityEngine;

public class SteeringMap
{
    public float[] interest;
    public float[] danger;
    private float[] result;

    public float[] Result { get { return result; } }

    public SteeringMap(int count)
    {
        interest = new float[count];
        danger = new float[count];
        result = new float[count];
    }

    public void Reset()
    {
        System.Array.Clear(result, 0, result.Length);
        System.Array.Clear(interest, 0, interest.Length);
        System.Array.Clear(danger, 0, danger.Length);
    }

    public void Apply(float[] interest, float[] danger)
    {
        Assert.AreEqual(interest.Length, this.interest.Length, "Interest array should be the same size as the steering map's interest array.");
        Assert.AreEqual(danger.Length, this.danger.Length, "Danger array should be the same size as the steering map's danger array.");
        Assert.AreEqual(interest.Length, danger.Length, "Interest array and Danger array should be the same size.");

        for (int i = 0; i < interest.Length; i++)
        {
            if (interest[i] > this.interest[i])
            {
                this.interest[i] = interest[i];
            }

            if (danger[i] > this.danger[i])
            {
                this.danger[i] = danger[i];
            }
        }
    }

    public void Solve()
    {
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }
    }
}
