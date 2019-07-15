using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace linear2
{
    class Program
    {
        /// <summary>
        /// feasible_region.png
        /// Dùng phương pháp tuyến tính Glop tối ưu 3x + 4y với yêu cầu sau:
        /// x + 2y	≤	14
        /// 3x – y	≥	0
        /// x – y	≤	2
        /// 
        /// Kết quả:
        /// x = 6.0
        /// y = 4.0
        /// Giá trị = 34.0
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var solver = new Google.OrTools.LinearSolver.Solver("LinearProgrammingExample", Google.OrTools.LinearSolver.Solver.GLOP_LINEAR_PROGRAMMING);
            // x and y are continuous non-negative variables.
            var x = solver.MakeNumVar(0.0, double.PositiveInfinity, "x");
            var y = solver.MakeNumVar(0.0, double.PositiveInfinity, "y");

            // x + 2y <= 14.
            var c0 = solver.MakeConstraint(double.NegativeInfinity, 14.0);
            c0.SetCoefficient(x, 1);
            c0.SetCoefficient(y, 2);

            // 3x - y >= 0.
            var c1 = solver.MakeConstraint(0.0, double.PositiveInfinity);
            c1.SetCoefficient(x, 3);
            c1.SetCoefficient(y, -1);

            // x - y <= 2.
            var c2 = solver.MakeConstraint(double.NegativeInfinity, 2.0);
            c2.SetCoefficient(x, 1);
            c2.SetCoefficient(y, -1);

            // Objective function: 3x + 4y.
            var objective = solver.Objective();
            objective.SetCoefficient(x, 3);
            objective.SetCoefficient(y, 4);
            objective.SetMaximization();

            solver.Solve();

            Console.WriteLine("Number of variables = " + solver.NumVariables());
            Console.WriteLine("Number of constraints = " + solver.NumConstraints());
            // The value of each variable in the solution.
            Console.WriteLine("Solution:");
            Console.WriteLine("x = " + x.SolutionValue());
            Console.WriteLine("y = " + y.SolutionValue());
            // The objective value of the solution.
            Console.WriteLine("Optimal objective value = " + solver.Objective().Value());

            Console.ReadLine();
        }
    }
}
