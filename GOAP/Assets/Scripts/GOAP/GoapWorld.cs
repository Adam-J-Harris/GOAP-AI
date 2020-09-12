using System.Collections.Generic;
using UnityEngine;

public sealed class GoapWorld
{
    private static readonly GoapWorld instance = new GoapWorld();

    private static GoapWorldStates world;

    private static Queue<GameObject> patients;
    private static Queue<GameObject> cubicles;

    static GoapWorld()
    {
        world = new GoapWorldStates();

        patients = new Queue<GameObject>();
        cubicles = new Queue<GameObject>();

        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cubicle");

        foreach (GameObject c in cubes)
            cubicles.Enqueue(c);

        if (cubes.Length > 0)
            world.ModifyState("FreeCubicle", cubes.Length);

        // Speed up the Game State
        Time.timeScale = 5.0f;
    }

    private GoapWorld()
    {

    }

    public void AddPatient(GameObject patient)
    {
        patients.Enqueue(patient);
    }

    public GameObject RemovePatient()
    {
        if (patients.Count == 0)
            return null;

        return patients.Dequeue();
    }

    public void AddCubicle(GameObject cubicle)
    {
        cubicles.Enqueue(cubicle);
    }

    public GameObject RemoveCubicle()
    {
        if (cubicles.Count == 0)
            return null;

        return cubicles.Dequeue();
    }

    public static GoapWorld Instance
    {
        get { return instance; }
    }

    public GoapWorldStates GetWorld()
    {
        return world;
    }
}
