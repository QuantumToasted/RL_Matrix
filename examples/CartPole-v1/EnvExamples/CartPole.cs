using Gym.Environments.Envs.Classic;
using Gym.Rendering.WinForm;
using OneOf;
using RLMatrix;

#if OLD
public class CartPole : IEnvironment<float[]>
#else
public class CartPole : IEnvironmentAsync<float[]>
#endif
{
    public int stepCounter { get; set; }
    public int maxSteps { get; set; }
    public bool isDone { get; set; }
    public OneOf<int, (int, int)> stateSize { get; set; }
    public int[] actionSize { get; set; }

    CartPoleEnv myEnv;

    private float[] myState;

    public CartPole()
    {
        //TODO: it should be more obvious to put Initalise method in constructor or something since its not called later. 
        //Create base constructor that calls initialise method if not initialised???
        Initialise();
    }

#if OLD
    public float[] GetCurrentState()
#else
    public Task<float[]> GetCurrentState()
#endif
    {
        if (myState == null)
            myState = new float[4] { 0, 0, 0, 0 };
        
#if OLD
        return myState;
#else
        return Task.FromResult(myState);
#endif
    }

    public void Initialise()
    {
        myEnv = new CartPoleEnv(WinFormEnvViewer.Factory);
        stepCounter = 0;
        maxSteps = 100000;
        stateSize = myEnv.ObservationSpace.Shape.Size;
        actionSize = new int[] { myEnv.ActionSpace.Shape.Size };
        myEnv.Reset();
        isDone = false;
    }

#if OLD
    public void Reset()
#else
    public Task Reset()
#endif
    {
        myEnv.Reset();
        isDone = false;
        stepCounter = 0;
#if !OLD
        return Task.CompletedTask;
#endif
    }

#if OLD
    public float Step(int[] actionsIds)
#else
    public async Task<(float, bool)> Step(int[] actionsIds)
#endif
    {
        var actionId = actionsIds[0];

        var (observation, reward, _done, information) = myEnv.Step(actionId);
        SixLabors.ImageSharp.Image img = myEnv.Render();
        myState = observation.ToFloatArray();
        isDone = _done;

        if (stepCounter > maxSteps)
            isDone = true;

        if (isDone)
        {
            reward = 0;
        }

#if OLD
        return reward;
#else
        if (isDone)
            await Reset();

        return (reward, isDone);
#endif
    }
}
