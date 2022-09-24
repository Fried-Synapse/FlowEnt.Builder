namespace FriedSynapse.FlowEnt.Editor
{
    internal class PreviewableControlSection : AbstractControlSection
    {
        protected override void Bind()
        {
            if (Controllable.IsSeekable)
            {
                ControlBar.OnValueChanging += (_) =>
                {
                    if (IsBuilding)
                    {
                        StartPreview();
                    }
                };
            }
            base.Bind();
        }

        protected override void OnPlayPause()
        {
            switch (Controllable.PlayState)
            {
                case PlayState.Waiting:
                case PlayState.Playing:
                    Controllable.Pause();
                    break;
                default:
                    if (IsBuilding)
                    {
                        StartPreview();
                    }
                    else
                    {
                        Controllable.Resume();
                    }
                    break;
            }
        }

        protected override void OnNextFrame()
        {
            if (IsBuilding)
            {
                StartPreview();
                Controllable.Pause();
            }
            base.OnNextFrame();
        }

        protected override void OnStop()
        {
            PreviewController.Stop();
        }

        private void StartPreview()
        {
            PreviewController.Start(new PreviewController.Options()
            {
                Animation = (AbstractAnimation)Controllable,
                OnStop = UpdatePlayState
            });
        }
    }
}
