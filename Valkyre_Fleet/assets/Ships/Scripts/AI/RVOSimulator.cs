/* */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// This is going to be attached to some global game object. Loads all agents for the agents to check global vars.
/// </summary>
public class RVOSimulator
{
	public RVOAgent defaultAgent;
	public RVOKdTree kdTree;
	public float globalTime;
	public float timeStep;
	public List<RVOAgent> agents;

	int RVO_ERROR = int.MaxValue;

	private static RVOSimulator instance = new RVOSimulator();

	public static RVOSimulator Instance
	{
		get
		{
			return instance;
		}
	}


	/// <summary>
	/// Prevents a default instance of the <see cref="RVOSimulator"/> class from being created.
	/// </summary>
	RVOSimulator()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="RVOSimulator"/> class.
	/// </summary>
	/// <param name="timeStep">The time step.</param>
	/// <param name="neighborDist">The neighbor dist.</param>
	/// <param name="maxNeighbors">The maximum neighbors.</param>
	/// <param name="timeHorizon">The time horizon.</param>
	/// <param name="radius">The radius.</param>
	/// <param name="maxSpeed">The maximum speed.</param>
	/// <param name="velocity">The velocity.</param>
	RVOSimulator(float timeStep, float neighborDist, int maxNeighbors, float timeHorizon, float radius, float maxSpeed, Vector3 velocity)
	{
		kdTree = new RVOKdTree();
		defaultAgent = new RVOAgent();

		defaultAgent.maxNeighbors = maxNeighbors;
		defaultAgent.maxSpeed = maxSpeed;
		defaultAgent.neighborDist = neighborDist;
		defaultAgent.radius = radius;
		defaultAgent.timeHorizon = timeHorizon;
		defaultAgent.velocity = velocity;
	}

	/// <summary>
	/// Adds a new agent with default properties to the simulation.
	/// </summary>
	/// <param name="position">position  The three-dimensional starting position of this agent.</param>
	/// <returns>The number of the agent, or RVO::RVO_ERROR when the agent defaults have not been set.</returns>
	public int addAgent(Vector3 position)
	{
		if (defaultAgent == null)
		{
			return RVO_ERROR;
		}

		// Unity should handle this.
		RVOAgent agent = new RVOAgent();

		agent.position = position;
		agent.maxNeighbors = defaultAgent.maxNeighbors;
		agent.maxSpeed = defaultAgent.maxSpeed;
		agent.neighborDist = defaultAgent.neighborDist;
		agent.radius = defaultAgent.radius;
		agent.timeHorizon = defaultAgent.timeHorizon;
		agent.velocity = defaultAgent.velocity;

		agent.id = agents.Count;

		agents.Add(agent);

		return agents.Count - 1;
	}

	/// <summary>
	/// Adds a new agent to the simulation.
	/// </summary>
	/// <param name="position">The three-dimensional starting position of this agent.</param>
	/// <param name="neighborDist">The maximum distance (center point to center point) to other agents this agent takes into account in the navigation. The larger this number, the longer the running time of the simulation. If the number is too low, the simulation will not be safe. Must be non-negative.</param>
	/// <param name="maxNeighbors">The maximum number of other agents this agent takes into account in the navigation. The larger this number, the longer the running time of the simulation. If the number is too low, the simulation will not be safe.</param>
	/// <param name="timeHorizon">The minimum amount of time for which this agent's velocities that are computed by the simulation are safe with respect to other agents. The larger this number, the sooner this agent will respond to the presence of other agents, but the less freedom this agent has in choosing its velocities. Must be positive.</param>
	/// <param name="radius">The radius of this agent. Must be non-negative.</param>
	/// <param name="maxSpeed">The maximum speed of this agent. Must be non-negative.</param>
	/// <param name="velocity">The initial three-dimensional linear velocity of this agent (optional).</param>
	/// <returns>The number of the agent.</returns>
	public int addAgent(Vector3 position, float neighborDist, int maxNeighbors, float timeHorizon, float radius, float maxSpeed, Vector3 velocity)
	{
		RVOAgent agent = new RVOAgent();
		agent.position = position;
		agent.maxNeighbors = maxNeighbors;
		agent.maxSpeed = maxSpeed;
		agent.neighborDist = neighborDist;
		agent.radius = radius;
		agent.timeHorizon = timeHorizon;
		agent.velocity = velocity;

		agent.id = agents.Count;

		agents.Add(agent);


		return agents.Count - 1;
	}

	/// <summary>
	/// Lets the simulator perform a simulation step and updates the three-dimensional position and three-dimensional velocity of each agent.
	/// </summary>
	void Update()
	{
		kdTree.buildAgentTree();

		for (int i = 0; i < agents.Count; ++i)
		{
			agents[i].computeNeighbors();
			agents[i].computeNewVelocity();
		}


		// Let unity handle this.
		for (int i = 0; i < agents.Count; ++i)
		{
			//agents[i].update();
		}

		globalTime += Time.deltaTime;
	}

	/// <summary>
	/// Returns the specified agent neighbor of the specified agent.
	/// </summary>
	/// <param name="agentNo">The number of the agent whose agent neighbor is to be retrieved.</param>
	/// <param name="neighborNo">The number of the agent neighbor to be retrieved.</param>
	/// <returns>The number of the neighboring agent.</returns>
	public int getAgentAgentNeighbor(int agentNo, int neighborNo)
	{
		return agents[agentNo].maxNeighbors;
	}

	/// <summary>
	/// Returns the maximum neighbor count of a specified agent.
	/// </summary>
	/// <param name="agentNo">agentNo  The number of the agent whose maximum neighbor count is to be retrieved.</param>
	/// <returns>The present maximum neighbor count of the agent.</returns>
	public int getAgentMaxNeighbors(int agentNo)
	{
		return agents[agentNo].maxNeighbors;
	}

	/// <summary>
	/// Returns the maximum speed of a specified agent.
	/// </summary>
	/// <param name="agentNo">agentNo  The number of the agent whose maximum speed is to be retrieved.</param>
	/// <returns>The present maximum speed of the agent.</returns>
	public float getAgentMaxSpeed(int agentNo)
	{
		return agents[agentNo].maxSpeed;
	}

	/// <summary>
	/// Returns the maximum neighbor distance of a specified agent.
	/// </summary>
	/// <param name="agentNo">agentNo  The number of the agent whose maximum neighbor distance is to be retrieved.</param>
	/// <returns>The present maximum neighbor distance of the agent.</returns>
	public float getAgentNeighborDist(int agentNo)
	{
		return agents[agentNo].neighborDist;
    }

	/// <summary>
	/// Returns the count of agent neighbors taken into account to compute the current velocity for the specified agent.
	/// </summary>
	/// <param name="agentNo">The number of the agent whose count of agent neighbors is to be retrieved.</param>
	/// <returns>The count of agent neighbors taken into account to compute the current velocity for the specified agent.</returns>
	public int getAgentNumAgentNeighbors(int agentNo)
	{
		return agents[agentNo].agentNeighbors.Count;
	}

	/// <summary>
	/// Returns the count of ORCA constraints used to compute the current velocity for the specified agent.
	/// </summary>
	/// <param name="agentNo">The number of the agent whose count of ORCA constraints is to be retrieved.</param>
	/// <returns>The count of ORCA constraints used to compute the current velocity for the specified agent.</returns>
	int getAgentNumORCAPlanes(int agentNo)
	{
		return agents[agentNo].orcaPlanes.Count;
	}

	/// <summary>
	/// Gets the agent orca plane.
	/// </summary>
	/// <param name="agentNo">The number of the agent whose ORCA constraint is to be retrieved.</param>
	/// <param name="planeNo">The number of the ORCA constraint to be retrieved.</param>
	/// <returns>A plane representing the specified ORCA constraint.</returns>
	/// <remarks>The halfspace to which the normal of the plane points is the region of permissible velocities with respect to the specified ORCA constraint.</remarks>
	RVOPlane getAgentORCAPlane(int agentNo, int planeNo)
	{
		return agents[agentNo].orcaPlanes[planeNo];
	}

	/// <summary>
	/// Returns the three-dimensional position of a specified agent.
	/// </summary>
	/// <param name="agentNo">agentNo  The number of the agent whose three-dimensional position is to be retrieved.</param>
	/// <returns>The present three-dimensional position of the (center of the) agent.</returns>
	Vector3 getAgentPosition(int agentNo)
	{
		return agents[agentNo].position;
	}
	
	/// <summary>
	/// Returns the three-dimensional preferred velocity of a specified agent.
	/// </summary>
	/// <param name="agentNo">The number of the agent whose three-dimensional preferred velocity is to be retrieved.</param>
	/// <returns>The present three-dimensional preferred velocity of the agent.</returns>
	Vector3 getAgentPrefVelocity(int agentNo)
	{
		return agents[agentNo].preferedVelocity;
	}

	/// <summary>
	/// Returns the radius of a specified agent.
	/// </summary>
	/// <param name="agentNo">The number of the agent whose radius is to be retrieved.</param>
	/// <returns>The present radius of the agent.</returns>
	float getAgentRadius(int agentNo)
	{
		return agents[agentNo].radius;
	}

	/// <summary>
	/// Returns the time horizon of a specified agent.
	/// </summary>
	/// <param name="agentNo">The number of the agent whose time horizon is to be retrieved.</param>
	/// <returns>The present time horizon of the agent.</returns>
	float getAgentTimeHorizon(int agentNo)
	{
		return agents[agentNo].timeHorizon;
	}

	/// <summary>
	/// Returns the three-dimensional linear velocity of a specified agent.
	/// </summary>
	/// <param name="agentNo">The number of the agent whose three-dimensional linear velocity is to be retrieved.</param>
	/// <returns>The present three-dimensional linear velocity of the agent.</returns>
	Vector3 getAgentVelocity(int agentNo)
	{
		return agents[agentNo].velocity;
	}
	
	/// <summary>
	/// Returns the global time of the simulation.
	/// </summary>
	/// <returns>The present global time of the simulation (zero initially).</returns>
	float getGlobalTime()
	{
		return globalTime;
	}
	
	/// <summary>
	/// Returns the count of agents in the simulation.
	/// </summary>
	/// <returns>The count of agents in the simulation.</returns>
	int getNumAgents()
	{
		return agents.Count;
	}
	
	/// <summary>
	/// Returns the time step of the simulation.
	/// </summary>
	/// <returns>The present time step of the simulation.</returns>
	float getTimeStep()
	{
		return Time.deltaTime;
	}

	/// <summary>
	/// Removes an agent from the simulation.
	/// </summary>
	/// <param name="agentNo">The number of the agent that is to be removed.</param>
	/// <remarks>After the removal of the agent, the agent that previously had number getNumAgents() - 1 will now have number agentNo.</remarks>
	void removeAgent(int agentNo)
	{
		agents.Remove(agents[agentNo]);
	}

	/// <summary>
	/// Sets the default properties for any new agent that is added.
	/// </summary>
	/// <param name="neighborDist">The default maximum distance (center point to center point) to other agents a new agent takes into account in the navigation. The larger this number, the longer he running time of the simulation. If the number is too low, the simulation will not be safe. Must be non-negative.</param>
	/// <param name="maxNeighbors">The default maximum number of other agents a new agent takes into account in the navigation. The larger this number, the longer the running time of the simulation. If the number is too low, the simulation will not be safe.</param>
	/// <param name="timeHorizon">The default minimum amount of time for which a new agent's velocities that are computed by the simulation are safe with respect to other agents. The larger this number, the sooner an agent will respond to the presence of other agents, but the less freedom the agent has in choosing its velocities. Must be positive.</param>
	/// <param name="radius">The default radius of a new agent. Must be non-negative.</param>
	/// <param name="maxSpeed">The default maximum speed of a new agent. Must be non-negative.</param>
	/// <param name="velocity">The default initial three-dimensional linear velocity of a new agent (optional).</param>
	void setAgentDefaults(float neighborDist, int maxNeighbors, float timeHorizon, float radius, float maxSpeed, Vector3 velocity)
	{
		if (defaultAgent == null)
		{
			defaultAgent = new RVOAgent();
		}

		defaultAgent.maxNeighbors = maxNeighbors;
		defaultAgent.maxSpeed = maxSpeed;
		defaultAgent.neighborDist = neighborDist;
		defaultAgent.radius = radius;
		defaultAgent.timeHorizon = timeHorizon;
		defaultAgent.velocity = velocity;
	}
	
	/// <summary>
	/// Sets the maximum neighbor count of a specified agent.
	/// </summary>
	/// <param name="agentNo">The number of the agent whose maximum neighbor count is to be modified.</param>
	/// <param name="maxNeighbors">The replacement maximum neighbor count.</param>
	void setAgentMaxNeighbors(int agentNo, int maxNeighbors)
	{
		agents[agentNo].maxNeighbors = maxNeighbors;
	}

	/// <summary>
	/// Sets the maximum speed of a specified agent.
	/// </summary>
	/// <param name="agentNo">The number of the agent whose maximum speed is to be modified.</param>
	/// <param name="maxSpeed">The replacement maximum speed. Must be non-negative.</param>
	void setAgentMaxSpeed(int agentNo, float maxSpeed)
	{
		agents[agentNo].maxSpeed = maxSpeed;
	}
	
	/// <summary>
	/// Sets the maximum neighbor distance of a specified agent.
	/// </summary>
	/// <param name="agentNo">The number of the agent whose maximum neighbor distance is to be modified.</param>
	/// <param name="neighborDist">The replacement maximum neighbor distance. Must be non-negative.</param>
	void setAgentNeighborDist(int agentNo, float neighborDist)
	{
		agents[agentNo].neighborDist = neighborDist;
	}

	/// <summary>
	/// Sets the three-dimensional position of a specified agent.
	/// </summary>
	/// <param name="agentNo">The number of the agent whose three-dimensional position is to be modified.</param>
	/// <param name="position">The replacement of the three-dimensional position.</param>
	void setAgentPosition(int agentNo, Vector3 position)
	{
		agents[agentNo].position = position;
	}
	
	/// <summary>
	/// Sets the three-dimensional preferred velocity of a specified agent.
	/// </summary>
	/// <param name="agentNo">The number of the agent whose three-dimensional preferred velocity is to be modified.</param>
	/// <param name="prefVelocity">The replacement of the three-dimensional preferred velocity.</param>
	void setAgentPrefVelocity(int agentNo, Vector3 prefVelocity)
	{
		agents[agentNo].preferedVelocity = prefVelocity;
	}

	/// <summary>
	/// Sets the radius of a specified agent.
	/// </summary>
	/// <param name="agentNo">The number of the agent whose radius is to be modified.</param>
	/// <param name="radius">The replacement radius. Must be non-negative.</param>
	void setAgentRadius(int agentNo, float radius)
	{
		agents[agentNo].radius = radius;
	}
	
	/// <summary>
	/// Sets the time horizon of a specified agent with respect to other agents.
	/// </summary>
	/// <param name="agentNo">The number of the agent whose time horizon is to be modified.</param>
	/// <param name="timeHorizon">The replacement time horizon with respect to other agents. Must be positive.</param>
	void setAgentTimeHorizon(int agentNo, float timeHorizon)
	{
		agents[agentNo].timeHorizon = timeHorizon;
	}

	/// <summary>
	/// Sets the three-dimensional linear velocity of a specified agent.
	/// </summary>
	/// <param name="agentNo">The number of the agent whose three-dimensional linear velocity is to be modified.</param>
	/// <param name="velocity">velocity  The replacement three-dimensional linear velocity.</param>
	void setAgentVelocity(int agentNo, Vector3 velocity)
	{
		agents[agentNo].velocity = velocity;
	}
	
	/// <summary>
	/// Sets the time step of the simulation.
	/// </summary>
	/// <param name="timeStep">timeStep  The time step of the simulation. Must be positive.</param>
	void setTimeStep(float timeStep)
	{
		this.timeStep = Time.deltaTime;
	}
}