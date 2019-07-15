using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace linear1
{
    class Program
    {
        /// <summary>
        /// Dùng phương pháp tuyến tính tối ưu 3x + y với yêu cầu sau:
        /// 0	≤	x	≤	1
        /// 0	≤	y	≤	2
        /// 	x + y	≤	2
        /// 	
        /// Kết quả:
        /// - Giá trị: 4
        /// - x = 1
        /// - y = 1
        /// 
        /// Main steps in solving the problem
        /// - Create the variables
        /// - Define the constraints
        /// - Define the objective function
        /// - Declare the solver—the method that implements an algorithm for finding the optimal solution
        /// - Invoke the solver and display the results
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Create the linear solver with the GLOP backend.
            var solver = new Google.OrTools.LinearSolver.Solver("SimpleLpProgram", Google.OrTools.LinearSolver.Solver.GLOP_LINEAR_PROGRAMMING);

            // Create the variables x and y.
            var x = solver.MakeNumVar(0.0, 1.0, "x");
            var y = solver.MakeNumVar(0.0, 2.0, "y");

            Console.WriteLine("Number of variables = " + solver.NumVariables());

            // Create a linear constraint, 0 <= x + y <= 2.
            var ct = solver.MakeConstraint(0.0, 2.0, "ct");
            ct.SetCoefficient(x, 1);
            ct.SetCoefficient(y, 1);

            Console.WriteLine("Number of constraints = " + solver.NumConstraints());

            // Create the objective function, 3 * x + y.
            var objective = solver.Objective();
            objective.SetCoefficient(x, 3);
            objective.SetCoefficient(y, 1);
            objective.SetMaximization();

            solver.Solve();

            Console.WriteLine("Solution:");
            Console.WriteLine("Objective value = " + objective.Value());
            Console.WriteLine("x = " + x.SolutionValue());
            Console.WriteLine("y = " + y.SolutionValue());

            Console.ReadLine();
        }
    }
}
