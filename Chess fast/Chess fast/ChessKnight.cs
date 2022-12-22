using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_fast
{
    using System;
    using System.Collections.Generic;
    public class Prog
    {


        // Class for storing a cell's data
        public class cell
        {
            public int x, y;
            public int dis;
            public int prevx;
            public int prevy;

            public cell(int x, int y, int dis, int prevx, int prevy)
            {
                this.x = x;
                this.y = y;
                this.dis = dis;
                this.prevx = prevx;
                this.prevy = prevy; 
            }
        }

        // Utility method returns true
        // if (x, y) lies inside Board
        static bool isInside(int x, int y, int N)
        {
            if (x >= 1 && x <= N && y >= 1 && y <= N)
                return true;
            return false;
        }

        // Method returns minimum step
        // to reach target position
        public static int minStepToReachTarget(int[] knightPos,
                                        int[] targetPos, int N)
        {
            string[,] board =new string[N,N];
            // x and y direction, where a knight can move
            int[] dx = { -2, -1, 1, 2, -2, -1, 1, 2 };
            int[] dy = { -1, -2, -2, -1, 1, 2, 2, 1 };
            
            for (int i = 0; i < N; i++)
            {
               for (int j = 0; j <  N; j++)
                {
                    board[i, j] = " 0 " ;
                }
            }
            board[knightPos[0], knightPos[1]] = " K ";
            board[targetPos[0], targetPos[1]] = " T ";
            // queue for storing states of knight in board
            Queue<cell> q = new Queue<cell>();
            List<cell> p = new List<cell>();
            // push starting position of knight with 0 distance
            q.Enqueue(new cell(knightPos[0], knightPos[1], 0,0,0));

            cell t;
            int x, y;
            bool[,] visit = new bool[N + 1, N + 1];

            // make all cell unvisited
            for (int i = 1; i <= N; i++)
                for (int j = 1; j <= N; j++)
                    visit[i, j] = false;

            // visit starting state
            visit[knightPos[0], knightPos[1]] = true;


            while (q.Count != 0)
            {
                t = q.Peek();
                p.Add(t);
                q.Dequeue();

                if (t.x == targetPos[0] && t.y == targetPos[1])
                {
                    int a = t.prevx;
                    int b = t.prevy;
                    board[t.x, t.y] =$" T " ;
                    int distance = t.dis-1;
                    //search for the previous steps
                    while(distance!=0)
                    {
                        foreach (cell e in p)
                        {
                            if (e.y == b && e.x == a && distance == e.dis)
                            {
                                Console.WriteLine(distance);
                                board[a, b] = $" {e.dis.ToString()} ";
                                a = e.prevx;
                                b = e.prevy;
                                distance--;
                            }
                        }
                    }
                    Console.Write("  ");
                    for(int i= 0; i < N; i++)
                    {
                        if (i < 10)
                            Console.Write($"--{i}-");
                        else if (i < 100)
                            Console.Write($"-{i}-");
                    }
                    Console.WriteLine();

                    //draw the board
                    for (int i = N-1; i >=0; i--)
                    {
                        if(i<10)
                        Console.Write($" {i}|");
                        else if(i<100)
                            Console.Write($"{i}|");
                        for (int j = 0; j < N; j++)
                        {
                            
                            Console.Write($"{board[i, j]}|");
                        }
                        Console.WriteLine();
                        Console.Write("  ");
                        for (int j=0; j < N; j++)
                        {
                            Console.Write("----");
                        }
                        Console.WriteLine();

                    }
                    put_Into_file(board, N);
                    return t.dis;
                }
                   

                // loop for all reachable states
                for (int i = 0; i < 8; i++)
                {
                    x = t.x + dx[i];
                    y = t.y + dy[i];
                    
                    // If reachable state is not yet visited and
                    // inside board, push that state into queue
                    if (isInside(x, y, N) && !visit[x, y])
                    {
                        visit[x, y] = true;
                        q.Enqueue(new cell(x, y, t.dis + 1,t.x,t.y));
                    }
                }
            }
            return int.MaxValue;
        }
        static void put_Into_file(string[,] board, int N)
        {
            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;
            try
            {
                ostrm = new FileStream("C:\\Redirect.txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
            }
            catch (Exception d)
            {
                Console.WriteLine("Cannot open Redirect.txt for writing");
                Console.WriteLine(d.Message);
                return;
            }
            Console.SetOut(writer);
            Console.Clear();
            Console.Write("  ");
            for (int i = 0; i < N; i++)
            {
                if (i < 10)
                    Console.Write($"--{i}-");
                else if (i < 100)
                    Console.Write($"-{i}-");
            }
            Console.WriteLine();

            //draw the board
            for (int i = N - 1; i >= 0; i--)
            {
                if (i < 10)
                    Console.Write($" {i}|");
                else if (i < 100)
                    Console.Write($"{i}|");
                for (int j = 0; j < N; j++)
                {

                    Console.Write($"{board[i, j]}|");
                }
                Console.WriteLine();
                Console.Write("  ");
                for (int j = 0; j < N; j++)
                {
                    Console.Write("----");
                }
                Console.WriteLine();

            }
            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
            Console.WriteLine("Done");

            var doc = new Aspose.Words.Document("C:\\Redirect.txt");

            doc.Watermark.Remove();

            //Aspose.Words.Document extractedPage = doc.ExtractPages(0, 1);
            doc.Save("C:\\Doc.bmp");



        }

        // Driver code

    }


}
