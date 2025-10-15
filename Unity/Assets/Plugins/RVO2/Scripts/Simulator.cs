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
        internal IList<Agent> agents_;
        internal IList<Obstacle> obstacles_;
        internal KdTree kdTree_;
        internal FP timeStep_;

        private IList<int> freeIndexs_;
        private IDictionary<long, int> agentId2index_;

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
         * <param name="isStatic"></param>
         * <param name="agentNo">External specified id</param>
         */
        public long addAgent(TSVector2 position, FP neighborDist, int maxNeighbors, FP timeHorizon, FP timeHorizonObst, FP radius, FP maxSpeed, TSVector2 velocity, bool isStatic, long agentNo = -1)
        {
            int index = -1;
            Agent agent = null;
            if (freeIndexs_.Count > 0)
            {
                index = freeIndexs_[^1];
                freeIndexs_.RemoveAt(freeIndexs_.Count - 1);
                agent = agents_[index];
            }
            else
            {
                index = agents_.Count;
                agent = new Agent();
                agents_.Add(agent);
            }

            agent.id = agentNo == -1 ? agents_.Count : agentNo;
            agent.maxNeighbors_ = maxNeighbors;
            agent.maxSpeed_ = maxSpeed;
            agent.neighborDist_ = neighborDist;
            agent.position = position;
            agent.radius_ = radius;
            agent.timeHorizon_ = timeHorizon;
            agent.timeHorizonObst_ = timeHorizonObst;
            agent.velocity_ = velocity;
            agent.isRemoved = false;
            agent.isStatic = isStatic;
            agentId2index_.Add(agent.id, index);

            return agent.id;
        }

        /**
         * <summary>Adds a new obstacle to the simulation.</summary>
         *
         * <returns>The number of the first vertex of the obstacle, or -1 when
         * the number of vertices is less than two.</returns>
         *
         * <param name="vertices">List of the vertices of the polygonal obstacle
         * in counterclockwise order.</param>
         *
         * <remarks>To add a "negative" obstacle, e.g. a bounding polygon around
         * the environment, the vertices should be listed in clockwise order.
         * </remarks>
         */
        public int addObstacle(IList<TSVector2> vertices)
        {
            if (vertices.Count < 2)
            {
                return -1;
            }

            int obstacleNo = obstacles_.Count;

            for (int i = 0; i < vertices.Count; ++i)
            {
                Obstacle obstacle = new();
                obstacle.point_ = vertices[i];

                if (i != 0)
                {
                    obstacle.previous_ = obstacles_[obstacles_.Count - 1];
                    obstacle.previous_.next_ = obstacle;
                }

                if (i == vertices.Count - 1)
                {
                    obstacle.next_ = obstacles_[obstacleNo];
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

                obstacle.id_ = obstacles_.Count;
                obstacles_.Add(obstacle);
            }

            return obstacleNo;
        }
        
        public void removeAgent(long agentNo)
        {
            int index = agentId2index_[agentNo];
            agents_[index].isRemoved = true;
            
            freeIndexs_.Add(index);
            agentId2index_.Remove(agents_[index].id);
        }
        
        public IList<Agent> GetAgents()
        {
            return agents_;
        }

        /**
         * <summary>Clears the simulation.</summary>
         */
        public void Clear()
        {
            freeIndexs_.Clear();
            for (int index = 0; index < agents_.Count; index++) {
                freeIndexs_.Add(index);
            }

            agents_.Clear();
            agentId2index_.Clear();
        }

        /**
         * <summary>Performs a simulation step and updates the two-dimensional
         * position and two-dimensional velocity of each agent.</summary>
         *
         * <returns>The global time after the simulation step.</returns>
         */
        public void doStep()
        {
            kdTree_.buildAgentTree(agents_);
            
            for (int i = 0; i < agents_.Count; ++i)
            {
                agents_[i].computeNeighbors(kdTree_);
                agents_[i].computeNewVelocity(timeStep_);
                agents_[i].update(timeStep_);
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
            return agents_[agentId2index_[agentNo]].agentNeighbors_[neighborNo].Value.id;
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
            return agents_[agentId2index_[agentNo]].agentNeighbors_.Count;
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
            return agents_[agentId2index_[agentNo]].obstacleNeighbors_.Count;
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
        public int getAgentObstacleNeighbor(long agentNo, int neighborNo)
        {
            return agents_[agentId2index_[agentNo]].obstacleNeighbors_[neighborNo].Value.id_;
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
            return agents_[agentId2index_[agentNo]].orcaLines_;
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
        public TSVector2 getObstacleVertex(int vertexNo)
        {
            return obstacles_[vertexNo].point_;
        }

        /**
         * <summary>Returns the number of the obstacle vertex succeeding the
         * specified obstacle vertex in its polygon.</summary>
         *
         * <returns>The number of the obstacle vertex succeeding the specified
         * obstacle vertex in its polygon.</returns>
         *
         * <param name="vertexNo">The number of the obstacle vertex whose
         * successor is to be retrieved.</param>
         */
        public int getNextObstacleVertexNo(int vertexNo)
        {
            return obstacles_[vertexNo].next_.id_;
        }

        /**
         * <summary>Returns the number of the obstacle vertex preceding the
         * specified obstacle vertex in its polygon.</summary>
         *
         * <returns>The number of the obstacle vertex preceding the specified
         * obstacle vertex in its polygon.</returns>
         *
         * <param name="vertexNo">The number of the obstacle vertex whose
         * predecessor is to be retrieved.</param>
         */
        public int getPrevObstacleVertexNo(int vertexNo)
        {
            return obstacles_[vertexNo].previous_.id_;
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
         * <summary>Processes the obstacles that have been added so that they
         * are accounted for in the simulation.</summary>
         *
         * <remarks>Obstacles added to the simulation after this function has
         * been called are not accounted for in the simulation.</remarks>
         */
        public void processObstacles()
        {
            kdTree_.buildObstacleTree(obstacles_);
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
            agents_[agentId2index_[agentNo]].position = position;
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
            agents_[agentId2index_[agentNo]].prefVelocity = prefVelocity;
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
            agents_ = new List<Agent>();
            kdTree_ = new KdTree();
            obstacles_ = new List<Obstacle>();
            timeStep_ = FP.EN1;

            freeIndexs_ = new List<int>();
            agentId2index_ = new Dictionary<long, int>();
        }
    }
}
