#if WINDOWS10_0_19041_0
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Windows.System;
#endif


namespace TetrisMaui;
public partial class MainPage : ContentPage
{
    GameDrawable drawable = new();
    IDispatcherTimer timer;
    bool isAccelerating = false;
    readonly TimeSpan normalSpeed = TimeSpan.FromMilliseconds(500);
    readonly TimeSpan fastSpeed = TimeSpan.FromMilliseconds(50);
    int score = 0;
    bool isPaused = false;
    public MainPage()
    {
        InitializeComponent();
        Drawable = drawable;
        PreviewDrawable = new PreviewDrawable(drawable);
        BindingContext = this;
        

        timer = Dispatcher.CreateTimer();
        timer.Interval = normalSpeed;
        timer.Tick += OnTimerTick;
        timer.Start();

    }

    public IDrawable PreviewDrawable { get; set; }
    public IDrawable Drawable { get; set; }
    async void OnTimerTick(object? sender, EventArgs e)
    {
        bool isBlocked = drawable.BlockCells.Any(cell =>
        cell.Y + 1 >= drawable.Rows ||  // 到底了
        drawable.FixedBlocks.Any(fixedCell => fixedCell.X == cell.X && fixedCell.Y == cell.Y + 1)  // 下方有固定方块
    );

        if (isBlocked)
        {
            drawable.FixedBlocks.AddRange(
            drawable.BlockCells.Select(c => new Point((int)MathF.Round(c.X), (int)MathF.Round(c.Y))));

            int gained = drawable.ClearFullLines();
            scoreLabel.Text = $"得分：{drawable.Score}";

            bool isOverTop = drawable.FixedBlocks.Any(cell => cell.Y <= 0);
            if (isOverTop)
            {
                timer.Stop();
                await GameOverPopup();
                return;
            }

            // 重置游戏

        
        drawable.BlockCells = drawable.NextBlock;
        drawable.NextBlock = GameDrawable.GenerateRandomBlock();
        nextView.Invalidate();

        }
        else
        {
            for (int i = 0; i < drawable.BlockCells.Count; i++)
            {
                var cell = drawable.BlockCells[i];
                drawable.BlockCells[i] = new PointF(cell.X, cell.Y + 1);
            }
        }

        gameView.Invalidate();
    }

    async Task GameOverPopup()
    {
        bool restart = await this.DisplayAlert(
            "游戏结束",
            $"你堆到了顶部！\n最终得分：{drawable.Score}",
            "重新开始",
            "退出");

        if (restart)
            ResetGame();
        else
            await this.DisplayAlert("提示", "请手动关闭程序退出", "确定");
    }

    void ResetGame()
    {
        drawable.Reset(); // 新增方法，清空状态并重新生成初始块
        scoreLabel.Text = "得分：0";
        timer.Interval = normalSpeed;
        timer.Start();
        gameView.Invalidate();
        nextView.Invalidate();
    }

    void OnPauseClicked(object sender, EventArgs e)
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            timer.Stop();
            pauseButton.Text = "继续";
        }
        else
        {
            timer.Start();
            pauseButton.Text = "暂停";
        }
    }
    void OnRestartClicked(object sender, EventArgs e)
    {
        if (timer.IsRunning)
            timer.Stop();

        ResetGame();
    }

#if WINDOWS10_0_19041_0
    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        var window =Platform.CurrentWindow;
        var nativeWindow = window?.Handler?.PlatformView as Microsoft.UI.Xaml.Window;

        if (nativeWindow?.Content is FrameworkElement root)
        {
            root.PreviewKeyDown += OnKeyDown;
            root.PreviewKeyUp += OnKeyUp;
            Dispatcher.Dispatch(() => root.Focus(FocusState.Programmatic));
            System.Diagnostics.Debug.WriteLine("已绑定 PreviewKeyDown 到 view");
        }
    }

 

    private void OnKeyDown(object sender, KeyRoutedEventArgs e)
    {

        System.Diagnostics.Debug.WriteLine($"你按下了：{e.Key}");

        if (e.Key == VirtualKey.Down && !isAccelerating)
        {
            isAccelerating = true;
            timer.Interval = fastSpeed;
        }

        if (e.Key == VirtualKey.Left)
        {
            if (drawable.BlockCells.All(cell =>
                cell.X > 0 &&
                !drawable.FixedBlocks.Any(f => f.X == cell.X - 1 && f.Y == cell.Y)))
            {
                MoveBlock(-1);
            }
        }
        else if (e.Key == VirtualKey.Right)
        {
            if (drawable.BlockCells.All(cell =>
                cell.X < drawable.Columns - 1 &&
                !drawable.FixedBlocks.Any(f => f.X == cell.X + 1 && f.Y == cell.Y)))
            {
                MoveBlock(1);
            }
        }


        else if (e.Key == VirtualKey.R || e.Key == VirtualKey.Up)
        {
            drawable.RotateBlock();
        }
        gameView.Invalidate();
    }

    private void OnKeyUp(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Down && isAccelerating)
        {
            isAccelerating = false;
            timer.Interval = normalSpeed;
        }
    }

    void MoveBlock(int dx)
    {
        for (int i = 0; i < drawable.BlockCells.Count; i++)
        {
            var cell = drawable.BlockCells[i];
            drawable.BlockCells[i] = new PointF(cell.X + dx, cell.Y);
        }
    }


#endif


}




