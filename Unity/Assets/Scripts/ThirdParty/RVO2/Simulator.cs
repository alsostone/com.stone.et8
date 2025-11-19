/*
 * Simulator.cs
 * RVO2 Library C#
 *
 * SPDX-FileCopyrightText: 2008 University of North Carolina at Chapel Hill
 * SPDX-License-Identifier: Apache-2.0
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Please send all bug reports to <geom@cs.unc.edu>.
 *
 * The authors may be contacted via:
 *
 * Jur van den Berg, Stephen J. Guy, Jamie Snape, Ming C. Lin, Dinesh Manocha
 * Dept. of Computer Science
 * 201 S. Columbia St.
 * Frederick P. Brooks, Jr. Computer Science Bldg.
 * Chapel Hill, N.C. 27599-3175
 * United States of America
 *
 * <http://gamma.cs.unc.edu/RVO2/>
 */

using System.Collections.Generic;
using TrueSync;

namespace RVO
{
    /**
     * <summary>Defines the simulation.</summary>
     */
    public class Simulator
    {
        internal PoolLinkedList<long, Agent> agents_;
        internal PoolLinkedList<(long, int), Obstacle> obstacles_;
        internal KdTree kdTree_;
        internal FP timeStep_;
        
        private bool agentDirty_ = false;
        private bool obstacleDirty_ = false;

        /**
         * <summary>Adds a new agent to the simulation.</summary>
         * 
         * <returns>The number of the agent.</returns>
         * 
         * <param name="position">The two-dimensional starting position of this
         * agent.</param>
         * <param name="neighborDist">The maximum distance (center point to
         * center point) to other agents this agent takes into account in the
         * navigation. The larger this number, the longer the running time of
         * the simulation. If the number is too low, the simulation will not be
         * safe. Must be non-negative.</param>
         * <param name="maxNeighbors">The maximum number of other agents this
         * agent takes into account in the navigation. The larger this number,
         * the longer the running time of the simulation. If the number is too
         * low, the simulation will not be safe.</param>
         * <param name="timeHorizon">The minimal amount of time for which this
         * agent's velocities that are computed by the simulation are safe with
         * respect to other agents. The larger this number, the sooner this
         * agent will respond to the presence of other agents, but the less
         * freedom this agent has in choosing its velocities. Must be positive.
         * </param>
         * <param name="timeHorizonObst">The minimal amount of time for which
         * this agent's velocities that are computed by the simulation are safe
         * with respect to obstacles. The larger this number, the sooner this
         * agent will respond to the presence of obstacles, but the less freedom
         * this agent has in choosing its velocities. Must be positive.</param>
         * <param name="radius">The radius of this agent. Must be non-negative.
         * </param>
         * <param name="maxSpeed">The maximum speed of this agent. Must be
         * non-negative.</param>
         * <param name="velocity">The initial two-dimensional linear velocity of
         * this agent.</param>
         * <param name="prefVelocity"></param>
         * <param name="agentNo">External specified id</param>
         */
        public void addAgent(TSVector2 position, FP neighborDist, int maxNeighbors, FP timeHorizon, FP timeHorizonObst, FP radius, FP maxSpeed, TSVector2 velocity, TSVector2 prefVelocity, long agentNo)
        {
            Agent agent = agents_.Add(agentNo);
            agent.id = agentNo;
            agent.maxNeighbors_ = maxNeighbors;
            agent.maxSpeed_ = maxSpeed;
            agent.neighborDist_ = neighborDist;
            agent.position = position;
            agent.radius_ = radius;
            agent.timeHorizon_ = timeHorizon;
            agent.timeHorizonObst_ = timeHorizonObst;
            agent.velocity = velocity;
            agent.prefVelocity = prefVelocity;
            agentDirty_ = true;
        }

        /**
         * <summary>Adds a new obstacle to the simulation.</summary>
         * 
         * <returns>The number of the first vertex of the obstacle, or -1 when
         * the number of vertices is less than two.</returns>
         * 
         * <param name="vertices">List of the vertices of the polygonal obstacle
         * in counterclockwise order.</param>
         * <param name="obstacleId"></param>
         * <remarks>To add a "negative" obstacle, e.g. a bounding polygon around
         * the environment, the vertices should be listed in clockwise order.
         * </remarks>
         */
        public void addObstacle(IList<TSVector2> vertices, long obstacleId)
        {
            if (vertices.Count < 2)
            {
                return;
            }
            
            Obstacle head = null;
            Obstacle previous = null;
            
            for (int i = 0; i < vertices.Count; ++i)
            {
                Obstacle obstacle = obstacles_.Add((obstacleId, i));
                obstacle.point_ = vertices[i];

                if (i == 0)
                {
                    head = obstacle;
                }
                else
                {
                    obstacle.previous_ = previous;
                    obstacle.previous_.next_ = obstacle;
                }
                
                if (i == vertices.Count - 1)
                {
                    obstacle.next_ = head;
                    obstacle.next_.previous_ = obstacle;
                }

                obstacle.direction_ = RVOMath.normalize(vertices[(i == vertices.Count - 1 ? 0 : i + 1)] - vertices[i]);

                if (vertices.Count == 2)
                {
                    obstacle.convex_ = true;
                }
                else
                {
                    obstacle.convex_ = (RVOMath.leftOf(vertices[(i == 0 ? vertices.Count - 1 : i - 1)], vertices[i], vertices[(i == vertices.Count - 1 ? 0 : i + 1)]) >= FP.Zero);
                }
                
                previous = obstacle;
            }
            
            obstacleDirty_ = true;
        }
        
        public void removeAgent(long agentNo)
        {
            agents_.Remove(agentNo);
            agentDirty_ = true;
        }
        
        public void removeObstacle(long obstacleId)
        {
            int index = 0;
            while (obstacles_.Remove((obstacleId, index))) {
                ++index;
            }
            obstacleDirty_ = true;
        }

        public PoolLinkedList<long, Agent> GetAllAgents()
        {
            return agents_;
        }
        
        public void ClearAllAgents()
        {
            agents_.Clear();
            agentDirty_ = true;
        }
        
        public void ClearAllObstacles()
        {
            obstacles_.Clear();
            obstacleDirty_ = true;
        }

        /**
         * <summary>Performs a simulation step and updates the two-dimensional
         * position and two-dimensional velocity of each agent.</summary>
         *
         * <returns>The global time after the simulation step.</returns>
         */
        public void doStep()
        {
            if (agentDirty_)
            {
                kdTree_.SetAgents(agents_);
                agentDirty_ = false;
            }
            if (obstacleDirty_)
            {
                kdTree_.SetObstacles(obstacles_);
                kdTree_.buildObstacleTree();
                obstacleDirty_ = false;
            }
            
            kdTree_.buildAgentTree();
            foreach (Agent agent in agents_)
            {
                agent.computeNeighbors(kdTree_);
                agent.computeNewVelocity(timeStep_);
                agent.update(timeStep_);
            }
        }

        /**
         * <summary>Returns the specified agent neighbor of the specified agent.
         * </summary>
         *
         * <returns>The number of the neighboring agent.</returns>
         *
         * <param name="agentNo">The number of the agent whose agent neighbor is
         * to be retrieved.</param>
         * <param name="neighborNo">The number of the agent neighbor to be
         * retrieved.</param>
         */
        public long getAgentAgentNeighbor(long agentNo, int neighborNo)
        {
            return agents_.Get(agentNo).agentNeighbors_[neighborNo].Value.id;
        }
        
        /**
         * <summary>Returns the count of agent neighbors taken into account to
         * compute the current velocity for the specified agent.</summary>
         *
         * <returns>The count of agent neighbors taken into account to compute
         * the current velocity for the specified agent.</returns>
         *
         * <param name="agentNo">The number of the agent whose count of agent
         * neighbors is to be retrieved.</param>
         */
        public int getAgentNumAgentNeighbors(long agentNo)
        {
            return agents_.Get(agentNo).agentNeighbors_.Count;
        }

        /**
         * <summary>Returns the count of obstacle neighbors taken into account
         * to compute the current velocity for the specified agent.</summary>
         *
         * <returns>The count of obstacle neighbors taken into account to
         * compute the current velocity for the specified agent.</returns>
         *
         * <param name="agentNo">The number of the agent whose count of obstacle
         * neighbors is to be retrieved.</param>
         */
        public int getAgentNumObstacleNeighbors(long agentNo)
        {
            return agents_.Get(agentNo).obstacleNeighbors_.Count;
        }

        /**
         * <summary>Returns the specified obstacle neighbor of the specified
         * agent.</summary>
         *
         * <returns>The number of the first vertex of the neighboring obstacle
         * edge.</returns>
         *
         * <param name="agentNo">The number of the agent whose obstacle neighbor
         * is to be retrieved.</param>
         * <param name="neighborNo">The number of the obstacle neighbor to be
         * retrieved.</param>
         */
        public Obstacle getAgentObstacleNeighbor(long agentNo, int neighborNo)
        {
            return agents_.Get(agentNo).obstacleNeighbors_[neighborNo].Value;
        }

        /**
         * <summary>Returns the ORCA constraints of the specified agent.
         * </summary>
         *
         * <returns>A list of lines representing the ORCA constraints.</returns>
         *
         * <param name="agentNo">The number of the agent whose ORCA constraints
         * are to be retrieved.</param>
         *
         * <remarks>The halfplane to the left of each line is the region of
         * permissible velocities with respect to that ORCA constraint.
         * </remarks>
         */
        public IList<Line> getAgentOrcaLines(long agentNo)
        {
            return agents_.Get(agentNo).orcaLines_;
        }

        /**
         * <summary>Returns the count of agents in the simulation.</summary>
         *
         * <returns>The count of agents in the simulation.</returns>
         */
        public int getNumAgents()
        {
            return agents_.Count;
        }

        /**
         * <summary>Returns the count of obstacle vertices in the simulation.
         * </summary>
         *
         * <returns>The count of obstacle vertices in the simulation.</returns>
         */
        public int getNumObstacleVertices()
        {
            return obstacles_.Count;
        }

        /**
         * <summary>Returns the two-dimensional position of a specified obstacle
         * vertex.</summary>
         *
         * <returns>The two-dimensional position of the specified obstacle
         * vertex.</returns>
         *
         * <param name="vertexNo">The number of the obstacle vertex to be
         * retrieved.</param>
         */
        public TSVector2 getObstacleVertex((long, int) vertexNo)
        {
            return obstacles_.Get(vertexNo).point_;
        }

        /**
         * <summary>Returns the time step of the simulation.</summary>
         *
         * <returns>The present time step of the simulation.</returns>
         */
        public FP getTimeStep()
        {
            return timeStep_;
        }

        /**
         * <summary>Performs a visibility query between the two specified points
         * with respect to the obstacles.</summary>
         *
         * <returns>A boolean specifying whether the two points are mutually
         * visible. Returns true when the obstacles have not been processed.
         * </returns>
         *
         * <param name="point1">The first point of the query.</param>
         * <param name="point2">The second point of the query.</param>
         * <param name="radius">The minimal distance between the line connecting
         * the two points and the obstacles in order for the points to be
         * mutually visible (optional). Must be non-negative.</param>
         */
        public bool queryVisibility(TSVector2 point1, TSVector2 point2, FP radius)
        {
            return kdTree_.queryVisibility(point1, point2, radius);
        }

        /**
         * <summary>Sets the two-dimensional position of a specified agent.
         * </summary>
         *
         * <param name="agentNo">The number of the agent whose two-dimensional
         * position is to be modified.</param>
         * <param name="position">The replacement of the two-dimensional
         * position.</param>
         */
        public void setAgentPosition(long agentNo, TSVector2 position)
        {
            agents_.Get(agentNo).position = position;
        }

        /**
         * <summary>Sets the two-dimensional preferred velocity of a specified
         * agent.</summary>
         *
         * <param name="agentNo">The number of the agent whose two-dimensional
         * preferred velocity is to be modified.</param>
         * <param name="prefVelocity">The replacement of the two-dimensional
         * preferred velocity.</param>
         */
        public void setAgentPrefVelocity(long agentNo, TSVector2 prefVelocity)
        {
            agents_.Get(agentNo).prefVelocity = prefVelocity;
        }

        /**
         * <summary>Sets the time step of the simulation.</summary>
         *
         * <param name="timeStep">The time step of the simulation. Must be
         * positive.</param>
         */
        public void setTimeStep(FP timeStep)
        {
            timeStep_ = timeStep;
        }

        /**
         * <summary>Constructs and initializes a simulation.</summary>
         */
        public Simulator()
        {
            agents_ = new PoolLinkedList<long, Agent>();
            kdTree_ = new KdTree();
            obstacles_ = new PoolLinkedList<(long, int), Obstacle>();
            timeStep_ = FP.EN1;
        }
    }
}
