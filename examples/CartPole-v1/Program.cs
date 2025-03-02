using RLMatrix;
#if !OLD
using RLMatrix.Agents.Common;
#endif
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//PPO
var optsppo = new PPOAgentOptions(
    batchSize: 24,           // Number of EPISODES agent interacts with environment before learning from its experience
    memorySize: 10000,       // Size of the replay buffer
    gamma: 0.99f,          // Discount factor for rewards
    gaeLambda: 0.95f,      // Lambda factor for Generalized Advantage Estimation
    lr: 3e-4f,            // Learning rate
    clipEpsilon: 0.2f,     // Clipping factor for PPO's objective function
    vClipRange: 0.2f,      // Clipping range for value loss
    cValue: 0.5f,          // Coefficient for value loss
    ppoEpochs: 20,            // Number of PPO epochs
    clipGradNorm: 0.5f    // Maximum allowed gradient norm
   );

#if OLD
var envppo = new List<IEnvironment<float[]>> { new CartPole(), new CartPole() };
var agent = new PPOAgent<float[]>(optsppo, envppo);
#else
var envppo = new List<IEnvironmentAsync<float[]>> { new CartPole(), new CartPole() };
var agent = new LocalDiscreteRolloutAgent<float[]>(optsppo, envppo);
#endif

for (int i = 0; i < 100_000; i++)
{
#if OLD
    //agent.Step();
#else
    //await agent.Step();
#endif
}

//DQN
#if OLD
var opts = new DQNAgentOptions(batchSize: 32, memorySize: 10000, gamma: 0.99f, epsStart: 1f, epsEnd: 0.05f, epsDecay: 50f, tau: 0.005f, lr: 1e-4f);
var env = new List<IEnvironment<float[,]>> { new CartPole2d(), new CartPole2d() };
var myAgent = new DQNAgent<float[,]>(opts, env);
#else
var opts = new DQNAgentOptions(batchSize: 32, memorySize: 10000, gamma: 0.99f, epsStart: 1f, epsEnd: 0.05f, epsDecay: 50f, tau: 0.005f, lr: 1e-4f);
var env = new List<IEnvironmentAsync<float[,]>> { new CartPole2d(), new CartPole2d() };
var myAgent = new LocalDiscreteRolloutAgent<float[,]>(opts, env);
#endif

for (int i = 0; i < 100_000; i++)
{
#if OLD
    myAgent.Step();
#else
    await myAgent.Step();
#endif
}

Console.ReadLine();