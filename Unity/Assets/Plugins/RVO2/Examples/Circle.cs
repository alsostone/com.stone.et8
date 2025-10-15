/*
 * Circle.cs
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

/*
 * Example file showing a demo with 250 agents initially positioned evenly
 * distributed on a circle attempting to move to the antipodal position on the
 * circle.
 */

#define RVOCS_OUTPUT_TIME_AND_POSITIONS

using System;
using System.Collections.Generic;
using TrueSync;

namespace RVO
{
    class Circle
    {
        /* Store the goals of the agents. */
        readonly IList<TSVector2> goals;
        
        readonly Simulator simulator;

        Circle()
        {
            goals = new List<TSVector2>();
            simulator = new Simulator();
        }

        void setupScenario()
        {
            /* Specify the global time step of the simulation. */
            simulator.setTimeStep(0.25f);

            /*
             * Specify the default parameters for agents that are subsequently
             * added.
             */
            simulator.setAgentDefaults(15.0f, 10, 10.0f, 10.0f, 1.5f, 2.0f, new TSVector2(0.0f, 0.0f));

            /*
             * Add agents, specifying their start position, and store their
             * goals on the opposite side of the environment.
             */
            for (int i = 0; i < 250; ++i)
            {
                simulator.addAgent(200.0f *
                    new TSVector2((float)Math.Cos(i * 2.0f * Math.PI / 250.0f),
                        (float)Math.Sin(i * 2.0f * Math.PI / 250.0f)));
                goals.Add(-simulator.getAgentPosition(i));
            }
        }

#if RVOCS_OUTPUT_TIME_AND_POSITIONS
        void updateVisualization()
        {
            /* Output the current global time. */
            Console.Write(simulator.getGlobalTime());

            /* Output the current position of all the agents. */
            for (int i = 0; i < simulator.getNumAgents(); ++i)
            {
                Console.Write(" {0}", simulator.getAgentPosition(i));
            }

            Console.WriteLine();
        }
#endif

        void setPreferredVelocities()
        {
            /*
             * Set the preferred velocity to be a vector of unit magnitude
             * (speed) in the direction of the goal.
             */
            for (int i = 0; i < simulator.getNumAgents(); ++i)
            {
                TSVector2 goalVector = goals[i] - simulator.getAgentPosition(i);

                if (RVOMath.absSq(goalVector) > 1.0f)
                {
                    goalVector = RVOMath.normalize(goalVector);
                }

                simulator.setAgentPrefVelocity(i, goalVector);
            }
        }

        bool reachedGoal()
        {
            /* Check if all agents have reached their goals. */
            for (int i = 0; i < simulator.getNumAgents(); ++i)
            {
                if (RVOMath.absSq(simulator.getAgentPosition(i) - goals[i]) > simulator.getAgentRadius(i) * simulator.getAgentRadius(i))
                {
                    return false;
                }
            }

            return true;
        }

        public static void Main(string[] args)
        {
            Circle circle = new();

            /* Set up the scenario. */
            circle.setupScenario();

            /* Perform (and manipulate) the simulation. */
            do
            {
#if RVOCS_OUTPUT_TIME_AND_POSITIONS
                circle.updateVisualization();
#endif
                circle.setPreferredVelocities();
                circle.simulator.doStep();
            }
            while (!circle.reachedGoal());
        }
    }
}
