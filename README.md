# TetrisMaui - A Modern Tetris Game Built with .NET MAUI

TetrisMaui is a modern, lightweight, and interactive implementation of the classic Tetris game, built entirely with .NET MAUI for Windows. It features a clean UI, keyboard and button controls, smooth gameplay, and intuitive score tracking.

---

## Features

- **Gameplay**
  - Includes all 7 classic Tetromino shapes: I, O, T, L, J, S, Z
  - Automatic downward movement and collision detection
  - Rotation and lateral movement support
  - Line detection and automatic clearing

- **User Interaction**
  - **Keyboard Controls**:
    - `¡û` : Move Left
    - `¡ú` : Move Right
    - `¡ý` : Accelerate Down
    - `¡ü` or `R` : Rotate
  - **Buttons**:
    - Pause the game
    - Restart the game

- **Game UI**
  - "Next Block" preview panel
  - Real-time score display
  - Game over alert with restart option

---

## UI Design

- **Background**: #D0DBF6 (Soft muted blue)
- **Blocks**: #7299FD (Current), #768DF9 (Fixed)
- **Text**: #34354B (Dark Blue Gray)
- **Grid Lines**: #7299FD (Light blue lines)
- **Buttons**: Rounded corners, soft blue background

Layout Structure:

- Left: Game board
- Right: Preview area, score, and controls
- Fully centered layout, responsive to screen sizes

## Technology Stack

| Technology | Purpose |
|------------|---------|
| .NET MAUI  | Cross-platform UI framework |
| C#         | Game logic |
| XAML       | UI layout and design |
| ICanvas / GraphicsView | Custom rendering for blocks and game board |

## Project Structure
TetrisMaui/
©À©¤©¤ MainPage.xaml             
©À©¤©¤ MainPage.xaml.cs        
©À©¤©¤ GameDrawable.cs          
©À©¤©¤ PreviewDrawable.cs        
©À©¤©¤ Resources/               
©¸©¤©¤ App.xaml / App.xaml.cs 

### Prerequisites

- **Operating System**: Windows 10 or later (WinUI 3 support required)
- **IDE**: Visual Studio 2022 (v17.8 or higher)
- **Workloads**:
  ```bash
  dotnet workload install maui
  dotnet workload install maui-windows

### License
- **This project is licensed under the MIT License.

### Contact
- **For issues, suggestions, or contributions, feel free to open an issue or pull request.

