using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;


public class GameDrawable : IDrawable
{
    public List<PointF> BlockCells = GenerateRandomBlock();
    public List<Point> FixedBlocks = new();
    public List<PointF> NextBlock = GenerateRandomBlock(); // 缓存下一个方块

    public float CellSize = 30; // 每格像素大小
    public int Rows = 20;
    public int Columns = 10;
    public int Score { get; private set; } = 0;
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {

        canvas.StrokeColor = Color.FromArgb("#7299FD");
        canvas.StrokeSize = 2;

        for (int row = 0; row <= Rows; row++)
        {
            float y = row * CellSize;
            canvas.DrawLine(0, y, Columns * CellSize, y);
        }

        for (int col = 0; col <= Columns; col++)
        {
            float x = col * CellSize;
            canvas.DrawLine(x, 0, x, Rows * CellSize);
        }

        canvas.FillColor = Color.FromArgb("#768DF9");
        foreach (var cell in FixedBlocks)
            canvas.FillRectangle((float)(cell.X * CellSize), (float)(cell.Y * CellSize), CellSize-1, CellSize-1);



        canvas.FillColor = Color.FromArgb("#7299FD");

        foreach (var cell in BlockCells)
        {
            canvas.FillRectangle(cell.X * CellSize, cell.Y * CellSize, CellSize - 1, CellSize - 1);
        }

    }

    public static List<PointF> GenerateRandomBlock()
    {
        var random = new Random();
        int type = random.Next(0, 7);
        List<PointF> block = new();

        float x = 4;

        switch (type)
        {
            case 0: //L
                block.Add(new PointF(x, 0));
                block.Add(new PointF(x, 1));
                block.Add(new PointF(x, 2));
                block.Add(new PointF(x + 1, 2));
                break;

            case 1: //J
                block.Add(new PointF(x + 1, 0));
                block.Add(new PointF(x + 1, 1));
                block.Add(new PointF(x + 1, 2));
                block.Add(new PointF(x, 2));
                break;

            case 2: //T
                block.Add(new PointF(x, 1));
                block.Add(new PointF(x + 1, 1));
                block.Add(new PointF(x + 2, 1));
                block.Add(new PointF(x + 1, 0));
                break;

            case 3://Z
                block.Add(new PointF(x, 0));
                block.Add(new PointF(x + 1, 0));
                block.Add(new PointF(x + 1, 1));
                block.Add(new PointF(x + 2, 1));
                break;

            case 4://S
                block.Add(new PointF(x + 1, 0));
                block.Add(new PointF(x + 2, 0));
                block.Add(new PointF(x, 1));
                block.Add(new PointF(x + 1, 1));
                break;

            case 5://O
                block.Add(new PointF(x, 0));
                block.Add(new PointF(x + 1, 0));
                block.Add(new PointF(x, 1));
                block.Add(new PointF(x + 1, 1));
                break;

            case 6: // I
                block.Add(new PointF(x, 0));
                block.Add(new PointF(x, 1));
                block.Add(new PointF(x, 2));
                block.Add(new PointF(x, 3));
                break;
        }
        return block;
    }
    public void RotateBlock()
    {
        var center = BlockCells[1]; 
        var rotated = new List<PointF>();

        foreach (var cell in BlockCells)
        {
            float dx = cell.X - center.X;
            float dy = cell.Y - center.Y;
            float newX = center.X - dy;
            float newY = center.Y + dx;
            rotated.Add(new PointF(newX, newY));
        }

        // 判断是否越界或撞到固定块
        bool valid = rotated.All(p =>
            p.X >= 0 && p.X < Columns &&
            p.Y >= 0 && p.Y < Rows &&
            !FixedBlocks.Any(f => f.X == p.X && f.Y == p.Y)
        );

        if (valid)
            BlockCells = rotated;
    }



    public int ClearFullLines()
    {
        // 找到所有满行的Y坐标
        var fullRows = FixedBlocks
            .GroupBy(cell => cell.Y)
            .Where(g => g.Count() >= Columns)
            .Select(g => g.Key)
            .ToList();

        // 如果有满行，则消除这些行并更新位置
        foreach (int row in fullRows.OrderBy(r => r))
        {
            // 删除该行
            FixedBlocks.RemoveAll(p => p.Y == row);

            // 将所有在该行之上的块下移 1
            for (int i = 0; i < FixedBlocks.Count; i++)
            {
                var cell = FixedBlocks[i];
                if (cell.Y < row)
                {
                    FixedBlocks[i] = new Point(cell.X, cell.Y + 1);
                }
            }
        }


        // 计分规则
        int lines = fullRows.Count;
        int add = lines switch { 1 => 100, 2 => 300, 3 => 500, _ => lines * 100 };
        Score += add;
        return add;
    }

    public void Reset()
    {
        BlockCells = GenerateRandomBlock();
        NextBlock = GenerateRandomBlock();
        FixedBlocks.Clear();
        Score = 0;
    }

}