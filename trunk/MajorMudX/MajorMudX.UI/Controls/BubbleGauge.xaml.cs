using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace MajorMudX.UI.Controls
{
    public partial class BubbleGauge : UserControl, INotifyPropertyChanged
    {
        #region BubbleColor

        public static readonly DependencyProperty BubbleColorProperty =
            DependencyProperty.Register("BubbleColor", typeof(Color), typeof(BubbleGauge),
            new PropertyMetadata(Colors.Green, new PropertyChangedCallback(OnColorChanged)));

        public static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
        }

        public Color BubbleColor
        {
            get { return (Color)GetValue(BubbleColorProperty); }
            set { SetValue(BubbleColorProperty, value); }
        }

        #endregion

        #region BubbleMaxValue

        public static readonly DependencyProperty BubbleMaxValueProperty =
            DependencyProperty.Register("BubbleMaxValue", typeof(int), typeof(BubbleGauge),
            new PropertyMetadata(100, new PropertyChangedCallback(OnBubbleMaxValueChanged)));

        public static void OnBubbleMaxValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
        }

        public int BubbleMaxValue
        {
            get { return (int)GetValue(BubbleMaxValueProperty); }
            set { SetValue(BubbleMaxValueProperty, value); }
        }

        #endregion

        #region BubbleCurrentValue

        public static readonly DependencyProperty BubbleCurrentValueProperty =
            DependencyProperty.Register("BubbleCurrentValue", typeof(int), typeof(BubbleGauge),
            new PropertyMetadata(100, new PropertyChangedCallback(OnBubbleCurrentValueChanged)));

        public static void OnBubbleCurrentValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            BubbleGauge gauge = obj as BubbleGauge;
            gauge.BubbleCurrentValue = (int)e.NewValue;
        }

        public int BubbleCurrentValue
        {
            get { return (int)GetValue(BubbleCurrentValueProperty); }
            set
            {
                SetValue(BubbleCurrentValueProperty, value);
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("FillPercent"));
            }
        }

        #endregion

        public double FillPercent
        {
            get { return BubbleCurrentValue / (double)BubbleMaxValue; }
        }

        public BubbleGauge()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
