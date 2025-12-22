using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace League_Analyser.View.UserControls
{
    public partial class ScrollBar : UserControl
    {
        private bool isDragging = false;
        public double thumbPositionOld;
        private double mousePositionOld;

        public struct ScrollBarPoints
        {
            public double beginingPoint;
            public double middlePoint;
            public double endPoint;
        }

        public event EventHandler<ScrollBarPoints>? ScrollBarMoved;

        public ScrollBar()
        {
            InitializeComponent();
        }

        private void Thumb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            mousePositionOld = e.GetPosition(localGrid).X;
            thumbPositionOld = Thumb.Margin.Left;
            Thumb.CaptureMouse();
            e.Handled = true;
        }

        private void Thumb_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging) return;
            double mousePosition = e.GetPosition(localGrid).X;
            MoveScrollThumb(mousePosition - mousePositionOld);
        }

        private void Thumb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            try { Thumb.ReleaseMouseCapture(); } catch { }
        }

        private void Thumb_LostMouseCapture(object sender, MouseEventArgs e)
        {
            isDragging = false;
            try { Thumb.ReleaseMouseCapture(); } catch { }
        }

        public void SetThumbWidth(double percent, double position)
        {
            if (percent > 1)
            {
                Thumb.Width = Width;
                Thumb.Margin = new Thickness(0, 0, 0, 0);
                return;
            }
            else if (percent < 0) return;

            double newWidth = Width * percent;
            double widthDelta = Thumb.Width - newWidth;

            double newMargin;
            if (position < 0) newMargin = Thumb.Margin.Left + (widthDelta / 2);
            else
            {
                newMargin = position * Width - (newWidth / 2);
            }

            if (newMargin < 0) newMargin = 0;
            else if ((newMargin + newWidth) > Width) newMargin = Width - newWidth;

            Thumb.Width = newWidth;
            Thumb.Margin = new Thickness(newMargin, 0, 0, 0);

            ScrollBarPoints scrollBarPoints_t = new ScrollBarPoints();
            scrollBarPoints_t.middlePoint = (Thumb.Margin.Left + (Thumb.Width / 2)) / Width;
            scrollBarPoints_t.beginingPoint = Thumb.Margin.Left / Width;
            scrollBarPoints_t.endPoint = (Thumb.Margin.Left + Thumb.Width) / Width;
            if (scrollBarPoints_t.beginingPoint < 0) scrollBarPoints_t.beginingPoint = 0;
            if (scrollBarPoints_t.endPoint > 1) scrollBarPoints_t.endPoint = 1;

            ScrollBarMoved?.Invoke(this, scrollBarPoints_t);
        }

        public void MoveScrollThumb(double value)
        {
            double newX = thumbPositionOld + value;

            if (newX < 0) newX = 0;
            else if ((newX + Thumb.Width) > Width) newX = Width - Thumb.Width;
            Thumb.Margin = new Thickness(newX, 0, 0, 0);

            ScrollBarPoints scrollBarPoints_t = new ScrollBarPoints();
            scrollBarPoints_t.middlePoint = (Thumb.Margin.Left + (Thumb.Width / 2)) / Width;
            scrollBarPoints_t.beginingPoint = Thumb.Margin.Left / Width;
            scrollBarPoints_t.endPoint = (Thumb.Margin.Left + Thumb.Width) / Width;
            if (scrollBarPoints_t.beginingPoint < 0) scrollBarPoints_t.beginingPoint = 0;
            if (scrollBarPoints_t.endPoint > 1) scrollBarPoints_t.endPoint = 1;

            ScrollBarMoved?.Invoke(this, scrollBarPoints_t);
        }
    }
}
