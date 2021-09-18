using System.Collections.Generic;
using UnityEngine;

public class TicTacToe : MonoBehaviour
{
    public static readonly int PIECE_PLAYER_HUMAN = 0;
    public static readonly int PIECE_PLAYER_CPU = 1;

    public int m_CurrentPlayer = 0;
    public GameObject[] m_Pieces;
    private TNode[,] m_Grid = new TNode[3, 3];
    public List<TMove> m_Moves = new List<TMove>();

    public bool IsValidMove(int x, int y)
    {
        return m_Grid[x, y] == null;
    }

    public TNode GetBestMove()
    {
        int max = int.MinValue;
        int index = -1;

        for (int i = 0; i < m_Moves.Count; i++)
        {
            if (max < m_Moves[i].score && IsValidMove(m_Moves[i].node.x, m_Moves[i].node.y))
            {
                max = m_Moves[i].score;
                index = i;
            }
        }

        if (index > -1)
        {
            return m_Moves[index].node;
        }

        var moves = GetAvailableMoves();
        return moves[Random.Range(0, moves.Count)];
    }

    public void CpuInput()
    {
        CallMinimax(0, 1);
        var move = GetBestMove();
        PushPieceInBoard(move.x, move.y, PIECE_PLAYER_CPU);
        m_CurrentPlayer = 0;
    }

    public void PushPieceInBoard(int x, int y, int player)
    {
        // X = 0; Y = 0
        // X = 0; Y = 2
        var position = new Vector3(x, 2 - y, 0.0f);
        Instantiate(m_Pieces[player], position, Quaternion.identity, transform);
        m_Grid[x, y] = new TNode() { x = x, y = y, player = player };
        Debug.Log($"[{x}, {y}] = {player}");
    }

    public void PlayerInput(KeyCode key, int x, int y)
    {
        if (Input.GetKeyDown(key) && IsValidMove(x, y))
        {
            PushPieceInBoard(x, y, PIECE_PLAYER_HUMAN);
            m_CurrentPlayer++;
        }
    }

    private void Update()
    {
        if (IsGameOver()) return;

        if (m_CurrentPlayer == PIECE_PLAYER_HUMAN)
        {
            PlayerInput(KeyCode.Q, 0, 0);
            PlayerInput(KeyCode.W, 0, 1);
            PlayerInput(KeyCode.E, 0, 2);
            PlayerInput(KeyCode.A, 1, 0);
            PlayerInput(KeyCode.S, 1, 1);
            PlayerInput(KeyCode.D, 1, 2);
            PlayerInput(KeyCode.Z, 2, 0);
            PlayerInput(KeyCode.X, 2, 1);
            PlayerInput(KeyCode.C, 2, 2);
        }
        else
        {
            CpuInput();
        }
    }

    public bool HasWon(int player)
    {
        // Linhas
        if (m_Grid[0, 0]?.player == player && m_Grid[0, 1]?.player == player && m_Grid[0, 2]?.player == player) return true;
        if (m_Grid[1, 0]?.player == player && m_Grid[1, 1]?.player == player && m_Grid[1, 2]?.player == player) return true;
        if (m_Grid[2, 0]?.player == player && m_Grid[2, 1]?.player == player && m_Grid[2, 2]?.player == player) return true;

        // Colunas
        if (m_Grid[0, 0]?.player == player && m_Grid[1, 0]?.player == player && m_Grid[2, 0]?.player == player) return true;
        if (m_Grid[0, 1]?.player == player && m_Grid[1, 1]?.player == player && m_Grid[2, 1]?.player == player) return true;
        if (m_Grid[0, 2]?.player == player && m_Grid[1, 2]?.player == player && m_Grid[2, 2]?.player == player) return true;

        // Diagonal
        if (m_Grid[0, 0]?.player == player && m_Grid[1, 1]?.player == player && m_Grid[2, 2]?.player == player) return true;
        if (m_Grid[0, 2]?.player == player && m_Grid[1, 1]?.player == player && m_Grid[2, 0]?.player == player) return true;

        return false;
    }

    public List<TNode> GetAvailableMoves()
    {
        List<TNode> moves = new List<TNode>();
        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                if (m_Grid[row, column] == null)
                {
                    TNode node = new TNode();
                    node.x = row;
                    node.y = column;
                    moves.Add(node);
                }
            }
        }
        return moves;
    }

    public bool IsGameOver()
    {
        if (HasWon(PIECE_PLAYER_HUMAN))
        {
            Debug.Log("You Won!");
            return true;
        }

        if (HasWon(PIECE_PLAYER_CPU))
        {
            Debug.Log("You Lost!");
            return true;
        }

        if (GetAvailableMoves().Capacity == 0)
        {
            Debug.Log("It's a draw!");
            return true;
        }

        return false;
    }

    public int MinValue(List<int> list)
    {
        int min = int.MaxValue;
        int index = -1;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] < min)
            {
                min = list[i];
                index = i;
            }
        }

        return list[index];
    }

    public int MaxValue(List<int> list)
    {
        int max = int.MinValue;
        int index = -1;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] > max)
            {
                max = list[i];
                index = i;
            }
        }

        return list[index];
    }

    public int Minimax(int depth, int player)
    {
        if (HasWon(0)) return +1;
        if (HasWon(1)) return -1;

        var moves = GetAvailableMoves();
        if (moves.Capacity == 0) return 0;

        var scores = new List<int>();
        for (int i = 0; i < moves.Count; i++)
        {
            var point = moves[i];
            var node = new TNode()
            {
                x = point.x,
                y = point.y,
                player = player
            };

            m_Grid[node.x, node.y] = node;
            int nextPlayer = ++player % 2;
            int score = Minimax(depth + 1, nextPlayer);
            scores.Add(score);

            if (depth == 0)
            {
                TMove move = new TMove(node, score);
                m_Moves.Add(move);
            }

            m_Grid[node.x, node.y] = null;
        }

        return player == 0 ? MaxValue(scores) : MinValue(scores);
    }

    public void CallMinimax(int depth, int player)
    {
        m_Moves.Clear();
        Minimax(depth, player);
    }
}