<<<<<<< HEAD
# RVE-Simulator



To conduct experiments using the Risk–Value Estimation (RVE) model, please run the simulation environment in Unity. The platform allows adjustment of key parameters through the following scripts:

Configurable Parameters:



&nbsp;   TaskGenerator.cs



&nbsp;       Random\_Seed: Controls randomness in task generation.



&nbsp;       Task\_Count: Number of tasks in the simulation.



&nbsp;       Bot\_Count: Number of robots in the swarm.



&nbsp;   GameTimer.cs



&nbsp;       Total\_Time: Duration of each simulation run (in seconds).



&nbsp;   WorkController.cs



&nbsp;       Decision\_Strategy: Task selection strategy (e.g., RVE, Greedy, Proximity).



&nbsp;       Alpha (α): Time hunger coefficient.



&nbsp;       Gamma (γ₀): Initial risk aversion coefficient.



&nbsp;   BotInfor.cs



&nbsp;       Learning\_Speed (k): Controls how quickly robots improve at tasks.



&nbsp;       Nmax: Maximum number of successes required to reach peak performance.



&nbsp;       Nmin: Minimum threshold for learning activation.



&nbsp;       Smax: Maximum success coefficient.



&nbsp;       Smin: Initial success coefficient.



Usage:



&nbsp;   Open the project in Unity.



&nbsp;   Locate the above scripts in the project hierarchy or prefeb.



&nbsp;   Modify parameter values as needed for your experiment.



&nbsp;   Run the simulation to collect results.





