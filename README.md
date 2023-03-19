# NPC_in_2DExplorer_by_ML
A level boss NPC developed in the 2D explorer game using the application of Machine learning algorithms.

The game environments used here are developed using the Unity3D game engine. There are 
free game object assets available in Unity3D website which can be imported into our unity 
project. This game asset contains 
most of the necessary game objects such as tiles, a player character, enemies, interactable 
(e.g., Info post, Teleporter), background arts, and sound effects that are required to develop 
a good real time 2D platformer game.

After starting the training, the Python API will try to connect to the Unity game environment using the 
Communicator. After connecting to the environment, the agent sends the observation it gets 
from the environment to the Python API and based on the observation, the neural network 
will send back some actions using appropriate RL algorithm given in the configuration file. 

This is the character that is been trained with ML algorithms. This acts as a level boss in game. 
The main objective of this is to find the player character and attack it. Here instead of player, 
a scripted bot character called “Chomper” (which is denoted as enemy in the Figure 4) is used 
in the training and testing.
For getting the observations from the environment, a 2D ray perception sensor component is 
added to the agent as shown in the Figure. From the figure it can be seen that there are 3 
rays, one ray is going towards top, another ray is going towards left hitting the enemy and 
another ray is going towards right hitting the Obstacle object.
![image](https://user-images.githubusercontent.com/16830321/226207214-67e2397f-d120-4df7-a6b1-8169e1f340ad.png)
The agent can receive 2 discrete branches of actions from the Neural network, one branch contains the values for movement that is “Nothing”, “Backward”, “Forward”, “Sit” and another branch contains values for “Nothing” “Melee attack”, “Ranged attack”, “Interaction”.

A final environment is developed which is built complex enough to test the abilities to search and attack the enemy from a starting point. The main objective of the agent is search and kill the Chomper which is located at the 3rd floor

![image](https://user-images.githubusercontent.com/16830321/226207375-8e0ebbcb-fe91-4fc8-931f-071cd8977670.png)

The generated model from this training session has been saved and applied to the agent, it was observed that it is navigating to the top floor and attacks as shown in figure 21. Finally, the machine learning model which controls the agent to travel to the top floor and attacks the Chomper has been obtained and saved. 
![image](https://user-images.githubusercontent.com/16830321/226207572-ec00e1f2-4bd4-4236-92c3-18d546dbd7d2.png)

As there is not any proper definition or test cases defined to assess the realistic behaviour, it is difficult to verify it here.  As discussed in the section 2.3, to be realistic it should act like human being. When the agent behaviour is compared with human, it does unnecessary activities like attacking and shooting even it does not detect the presence of Chomper. Based on this, it can be said that the expected realistic behaviour has not been obtained but the agent still can accomplish its task as a level boss.
The agent completed its task of navigating through the teleporters and attacking its enemy (Chomper). Also 2 distinct behaviours were obtained by agent. The primary or first behaviour was obtained in the phase 6 training where the agent navigates to top most floor and attacks the enemy. The second behaviour was obtained in the phase 5 training with the usage of SAC algorithm, where the agent navigates to the second floor and attacks the enemy. While doing this it only uses its ranged attack on the enemy in the sitting position. On the other hand, in the primary behaviour, the agent uses both the Melee attack and ranged attack. This was the main difference between the first and second behaviour.
The performance of the AI agent in this environment is described as follows.
•	Set-up: A environment where the agent can travel through the teleporters and can attack the enemy.
•	Goal: The agent needs to learn to travel through the teleporters and attack the enemy.
•	Agents: The environment contain 1 agent.
•	Agent Reward Function (independent):
o	+1 for killing the enemy (Chomper)
o	-1 for getting killed
o	+0.1 for attacking the enemy (Chomper)
o	-0.2 for getting attacked by the enemy (Chomper)
o	-0.00001 for Melee attack 
o	-0.00001 for Ranged attack
o	-0.001 for colliding with the obstacle
•	Behaviour Parameters:
o	Vector Observation space: Size of 11, corresponding to 3 ray casts each can detect the enemy (Chomper). Additionally, the global position of the agent.
o	Actions: 2 discrete action branches:
	Movement and sitting (4 possible actions: Forward, Backwards, Sitting and No Action)
	Attacks and Interaction (4 possible actions: Melee Attack, Ranged Attack, Interact and No Action)
o	Visual Observations: None
•	Benchmark Mean Reward: 1.08

