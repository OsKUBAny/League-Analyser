using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace League_Analyser.View.UserControls
{
    public partial class InfoPanel : UserControl
    {
        private Info info;
        private int timerCloseTime;
        private bool isClosing = false;
        private Storyboard Animation_SlideIn;
        private Storyboard Animation_SlideOut;
        private Storyboard Animation_Progress = new Storyboard();
        private DoubleAnimation varProgressAnimation = new DoubleAnimation
        {
            From = 50,
            To = 0,
            AutoReverse = false
        };

        public InfoPanel(Info classReference, Info.PromptParameters promptParams)
        {
            InitializeComponent();
            info = classReference;

            InfoPanelGrid.Background = promptParams.color;
            timerCloseTime = promptParams.time;
            infoIcon.Source = promptParams.icon;
            buttonClose.Visibility = promptParams.closeVisibility;
            progressBar.Visibility = promptParams.closeVisibility;
            textBlock_title.Text = promptParams.title;
            textBlock_message.Text = promptParams.message;

            Animation_SlideIn = (Storyboard)this.Resources["SlideIn"];
            Animation_SlideOut = (Storyboard)this.Resources["SlideOut"];

            varProgressAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(timerCloseTime));
            varProgressAnimation.Completed += ClosePrompt;
            Storyboard.SetTarget(varProgressAnimation, progressBar);
            Storyboard.SetTargetProperty(varProgressAnimation, new PropertyPath(WidthProperty));
            Animation_Progress.Children.Add(varProgressAnimation);
        }

        // Starts all animations and enables prompt, called from Info.ManagePrompt when this is being enabled.
        public void StartPrompt()
        {
            Animation_SlideIn.Begin();
            Animation_Progress.Begin();
        }

        // Pauses remaining time when cursor enters into prompt.
        private void panel_info_MouseEnter(object sender, MouseEventArgs e)
        {
            Animation_Progress.Pause();
        }

        // Resumes timer when cursor leaves prompt.
        private void panel_info_MouseLeave(object sender, MouseEventArgs e)
        {
            Animation_Progress.Resume();
        }

        // Initialize closing operation by starting slide out animation.
        public void ClosePrompt(object sender, EventArgs e)
        {
            if (isClosing == true) return;
            isClosing = true;
            Animation_SlideOut.Begin();
        }

        // Close prompt by removing it from mainGrid. Called when button "close" is pressed or when slide out animation fishishes.
        private void SlideOutAnimation_Completed(object sender, EventArgs e)
        {
            if (this.Parent != null) ((Panel)this.Parent).Children.Remove(this);
            info.ManagePrompt(true);
        }
    }
}
