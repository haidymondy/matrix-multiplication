using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


namespace Problem
{
    // ***************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // ***************************************
    public static class MatrixMultiplication
    {

        #region YOUR CODE IS HERE

        //Your Code is Here:
        //==================
        /// <summary>
        /// Multiply 2 square matrices in an efficient way [Strassen's Method]
        /// </summary>
        /// <param name="M1">First square matrix</param>
        /// <param name="M2">Second square matrix</param>
        /// <param name="N">Dimension (power of 2)</param>
        /// <returns>Resulting square matrix</returns>

        public static int[,] add(int[,] M1, int[,] M2, int n)
        {
            int[,] final = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    final[i, j] = M1[i, j] + M2[i, j];
                }
            }
            return final;
        }
        public static int[,] sub(int[,] M1, int[,] M2, int n)
        {
            int[,] final = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    final[i, j] = M1[i, j] - M2[i, j];
                }
            }
            return final;
        }
        
        public static int[,] brute_force(int[,] m1, int[,] m2, int[,] final, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int k = 0; k < n; k++)
                    {
                        final[i, j] += m1[i, k] * m2[k, j];
                    }
                }
            }
            return final;
        }

        public static int[,] combine_mat(int[,] Q1, int[,] Q2, int[,] Q3, int[,] Q4, int[,] final, int n)
        {
            // int[,] final = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    final[i, j] = Q1[i, j];
                    final[i, j + n] = Q2[i, j];
                    final[i + n, j] = Q3[i, j];
                    final[i + n, j + n] = Q4[i, j];
                }
            }
            return final;
        }

        static public int[,] MatrixMultiply(int[,] M1, int[,] M2, int N)
        {
            //REMOVE THIS LINE BEFORE START CODING
            // throw new NotImplementedException();

            int[,] array1D = new int[N, N];
            int[,] final = new int[N, N];
            int N_new = N / 2;

            int[,] M1Q1 = new int[N_new, N_new];
            int[,] M1Q2 = new int[N_new, N_new];
            int[,] M1Q3 = new int[N_new, N_new];
            int[,] M1Q4 = new int[N_new, N_new];

            int[,] M2Q1 = new int[N_new, N_new];
            int[,] M2Q2 = new int[N_new, N_new];
            int[,] M2Q3 = new int[N_new, N_new];
            int[,] M2Q4 = new int[N_new, N_new];

            int[,] finalQ1 = new int[N_new, N_new];
            int[,] finalQ2 = new int[N_new, N_new];
            int[,] finalQ3 = new int[N_new, N_new];
            int[,] finalQ4 = new int[N_new, N_new];

            if (N % 2 != 0 || N<0)
            {
                throw new Exception("N should be even and positive");
            }
            
            if (N < 128)
            {
                final = brute_force(M1, M2, final, N);
                return final;
            }

            //divide 2 mtrx into 4
            for (int i = 0; i < N_new; i++)
            {
                for (int j = 0; j < N_new; j++)
                {
                    M1Q1[i, j] = M1[i, j];
                    M1Q2[i, j] = M1[i, j + (N_new)];
                    M1Q3[i, j] = M1[i + (N_new), j];
                    M1Q4[i, j] = M1[i + (N_new), j + (N_new)];

                    M2Q1[i, j] = M2[i, j];
                    M2Q2[i, j] = M2[i, j + (N_new)];
                    M2Q3[i, j] = M2[i + (N_new), j];
                    M2Q4[i, j] = M2[i + (N_new), j + (N_new)];
                }
            }

            Task<int[,]> taskP = Task.Run(() => MatrixMultiply(add(M1Q1, M1Q4, N_new), add(M2Q1, M2Q4, N_new), N_new));
            Task<int[,]> taskQ = Task.Run(() => MatrixMultiply(add(M1Q3, M1Q4, N_new), M2Q1, N_new));
            Task<int[,]> taskR = Task.Run(() => MatrixMultiply(M1Q1, sub(M2Q2, M2Q4, N_new), N_new));
            Task<int[,]> taskS = Task.Run(() => MatrixMultiply(M1Q4, sub(M2Q3, M2Q1, N_new), N_new));
            Task<int[,]> taskT = Task.Run(() => MatrixMultiply(add(M1Q1, M1Q2, N_new), M2Q4, N_new));
            Task<int[,]> taskU = Task.Run(() => MatrixMultiply(sub(M1Q3, M1Q1, N_new), add(M2Q1, M2Q2, N_new), N_new));
            Task<int[,]> taskV = Task.Run(() => MatrixMultiply(sub(M1Q2, M1Q4, N_new), add(M2Q3, M2Q4, N_new), N_new));

            Task.WaitAll(taskP, taskQ, taskR, taskS, taskT, taskU, taskV);

            int[,] p = taskP.Result;
            int[,] q = taskQ.Result;
            int[,] r = taskR.Result;
            int[,] s = taskS.Result;
            int[,] t = taskT.Result;
            int[,] u = taskU.Result;
            int[,] v = taskV.Result;


            Task<int[,]> task2 = Task.Run(() => add(r, t, N_new));
            Task<int[,]> task3 = Task.Run(() => add(q, s, N_new));
            Task<int[,]> task1 = Task.Run(() => add(add(p, s, N_new), sub(v, t, N_new), N_new));
            Task<int[,]> task4 = Task.Run(() => add(add(p, r, N_new), sub(u, q, N_new), N_new));

            Task.WaitAll(task1, task2, task3, task4);

            finalQ2 = task2.Result;
            finalQ3 = task3.Result;
            finalQ4 = task4.Result;
            finalQ1 = task1.Result;

            // combine into final
           final = combine_mat(finalQ1, finalQ2, finalQ3, finalQ4, final, N_new);
           
            return final;

            #endregion
        }
    }
}