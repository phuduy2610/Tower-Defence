using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point
{
    public int X{get;set;}
    public int Y{get;set;}
    public Point(int X,int Y):this()
    {
        this.X = X;
        this.Y = Y;
    }   
}
