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
using MMX.Core.API.Infrastructure.ViewModels;
using MMX.Core.API.Infrastructure.Command;
using MMX.Core.API.Infrastructure.Services;

namespace MMX.ViewModels
{
    public class ShellViewModel : ViewModelBase, ITitleViewModel, IFooterViewModel
    {
        public ShellViewModel()
        {
            BuildTitleCommands();
            BuildFooterCommands();
        }

        #region Properties

        #endregion

        #region Commands

        #endregion

        #region ITitleViewModel

        void BuildTitleCommands()
        {
            CloseCommand = new DelegateCommand(
                new Action<object>((o) => { Application.Current.MainWindow.Close(); }),
                new Func<object, bool>((o) => { return true; }));

            MaxRestoreCommand = new DelegateCommand(
                new Action<object>((o) =>
                {
                    WindowState current = Application.Current.MainWindow.WindowState;
                    Application.Current.MainWindow.WindowState = current == WindowState.Maximized ?
                        WindowState.Normal : WindowState.Maximized;
                }),
                new Func<object, bool>((o) => { return true; }));

            MinimizeCommand = new DelegateCommand(
                new Action<object>((o) => { Application.Current.MainWindow.WindowState = WindowState.Minimized; }),
                new Func<object, bool>((o) => { return true; }));

            DragMoveCommand = new DelegateCommand(
                new Action<object>((o) => { Application.Current.MainWindow.DragMove(); }),
                new Func<object, bool>((o) => { return true; }));

        }

        public ICommand CloseCommand { get; set; }

        public ICommand MaxRestoreCommand { get; set; }

        public ICommand MinimizeCommand { get; set; }

        public ICommand DragMoveCommand { get; set; }

        #endregion

        #region IFooterViewModel

        void BuildFooterCommands()
        {
            DragResizeCommand = new DelegateCommand(
                new Action<object>((o) => { Application.Current.MainWindow.DragResize(WindowResizeEdge.BottomRight); }),
                new Func<object, bool>((o) => { return true; }));

            ResizeEnterMouseHoverCommand = new DelegateCommand(
                new Action<object>((o) =>
                {
                    MouseEventArgs e = o as MouseEventArgs;
                    if (e != null)
                    {
                        FrameworkElement ctl = e.OriginalSource as FrameworkElement;
                        if (ctl != null)
                            ctl.Cursor = Application.Current.MainWindow.WindowState != WindowState.Maximized ?
                                Cursors.SizeNWSE : Cursors.Arrow;
                    }
                }),
                new Func<object, bool>((o) => { return true; }));
        }

        public ICommand DragResizeCommand { get; set; }

        public ICommand ResizeEnterMouseHoverCommand { get; set; }

        #endregion
    }
}
