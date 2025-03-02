using Gym.Environments.Envs.Classic;
using Gym.Rendering.WinForm;
using OneOf;
using RLMatrix;

#if OLD
public class CartPole2d : IEnvironment<float[,]>
#else
public class CartPole2d : IEnvironmentAsync<float[,]>
#endif
{
    public int stepCounter { get; set; }
    public int maxSteps { get; set; }
    public bool isDone { get; set; }
    public OneOf<int, (int, int)> stateSize { get; set; }
    public int[] actionSize { get; set; }

    CartPoleEnv myEnv;

    private float[,] myState;

    public CartPole2d()
    {
        //TODO: it should be more obvious to put Initalise method in constructor or something since its not called later. 
        //Create base constructor that calls initialise method if not initialised???
        Initialise();
    }

#if OLD
    public float[,] GetCurrentState()
#else
    public Task<float[,]> GetCurrentState()
#endif
    {
        if (myState == null)
            myState = new float[2, 2];

        var myStateCopy = new float[2, 2];

        for (int i = 0; i < myState.GetLength(0); i++)
        {
            for (int j = 0; j < myState.GetLength(1); j++)
            {
                myStateCopy[i, j] = myState[i, j];
            }
        }

#if OLD
        return myStateCopy;
#else
        return Task.FromResult(myStateCopy);
#endif
    }

    public void Initialise()
    {
        myEnv = new CartPoleEnv(WinFormEnvViewer.Factory);
        stepCounter = 0;
        maxSteps = 100000;
        stateSize = (2, 2);
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
        myState = new float[2, 2];
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

        SixLabors.ImageSharp.Image img = myEnv.Render(); //returns the image that was rendered.
                                                         // form.Invoke(new Action(() => ChangeImage(img)));


        // Thread.Sleep(1); //this is to prevent it from finishing instantly !

        float[] observationArray = observation.ToFloatArray();
        myState[0, 0] = observationArray[0];
        myState[0, 1] = observationArray[1];
        myState[1, 0] = observationArray[2];
        myState[1, 1] = observationArray[3];
        isDone = _done;

        if (stepCounter > maxSteps)
            isDone = true;
        
#if OLD
        return reward;
#else
        if (isDone)
            await Reset();
        
        return (reward, isDone);
#endif
    }


    ~CartPole2d()
    {
        myEnv.CloseEnvironment();
        myEnv.Dispose();
    }
}
