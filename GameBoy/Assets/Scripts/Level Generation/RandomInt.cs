using System.Collections;
using UnityEngine;

//RandomInt is used to get a random integer between a range of values
public class RandomInt
{
    public int min;
    public int max;

    public RandomInt (int min, int max)
    {
        this.min=min;
        this.max=max;
    }

    public int getRandom (){
        return Random.Range(min,max+1);
    }
}



