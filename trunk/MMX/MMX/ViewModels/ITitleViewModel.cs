using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MMX.ViewModels
{
    public interface ITitleViewModel
    {
        ICommand CloseCommand { get; set; }
        ICommand MaxRestoreCommand { get; set; }
        ICommand MinimizeCommand { get; set; }
        ICommand DragMoveCommand { get; set; }
    }
}
