using System;
using UnityEngine;

public static class MinMaxAI
{
    // Board
    // 1= AI, -1 = Player, 0 = Empty space
    private static readonly int[][] Wins = new int[][]
    {
        new[] { 0, 1, 2 }, new[] { 3, 4, 5 }, new[] { 6, 7, 8 },
        new[] { 0, 3, 6 }, new[] { 1, 4, 7 }, new[] { 2, 5, 8 },
        new[] { 0, 4, 8 }, new[] { 2, 4, 6 }
    };

    public static int Win(int[] board)
    {
        foreach (var line in Wins)
        {
            int a = line[0], b = line[1], c = line[2];
            if (board[a] != 0 && board[a] == board[b] && board[b] == board[c])
                return board[c];
        }
        return 0; // draw or game continues.
    }

    public static int MinMax(int[] board, int player)
    {
        int winner = Win(board);
        if (winner != 0)
            return winner * player;

        int move = -1;
        int score = -2;

        for (int i = 0; i < 9; i++)
        {
            if (board[i] == 0)
            {
                board[i] = player;
                int thisScore = -MinMax(board, -player);
                if (thisScore > score)
                {
                    score = thisScore;
                    move = i;
                }
                board[i] = 0;
            }
        }
        if (move == -1)
            return 0; // draw

        return score;
    }

    public static int ComputerMove(int[] board)
    {
        int move = -1;
        int score = -2;

        for (int i = 0; i < 9; i++)
        {
            if (board[i]== 0)
            {
                board[i] = 1;
                int tempScore = -MinMax(board, -1);
                board[i] = 0;
                if (tempScore > score)
                {
                    score = tempScore;
                    move = i;
                }
            }
        }
        return move;
    }

}

