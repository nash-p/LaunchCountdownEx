using LaunchCountDown.Common;
using LaunchCountDown.Windows;
using PluginFramework;

namespace LaunchCountDown.Toolbar
{
    class LaunchCountDownToolbar : MonoBehaviourExtended
    {
        private IButton _launchButton;
        private MonoBehaviourWindow _window;

        public override void Awake()
        {
            _window = GetComponent<MainWindow>();

            if (ToolbarManager.ToolbarAvailable)
            {
                _launchButton = ToolbarManager.Instance.add("LaunchCountDownEx", "launchButton");
                _launchButton.TexturePath = "LaunchCountDownEx/Icons/launch_icon_normal";
                _launchButton.ToolTip = "LaunchCountDownExtended";
                _launchButton.OnClick += _launchButton_OnClick;
                _launchButton.Visibility = new GameScenesVisibility(GameScenes.FLIGHT);
                _launchButton.Enabled = true;
            }
        }
      
        void _launchButton_OnClick(ClickEvent e)
        {
            _window.Visible = !_window.Visible;
            _launchButton.TexturePath = _window.Visible
            ? "LaunchCountDownEx/Icons/launch_icon_normal"
            : "LaunchCountDownEx/Icons/launch_icon_disabled";
        }

        internal void SetEnable(bool flag)
        {
            _launchButton.Enabled = flag;
        }

        public override void OnDestroy()
        {
            if (!ToolbarManager.ToolbarAvailable) return;
            if (_launchButton == null) return;

            _launchButton.OnClick -= _launchButton_OnClick;
            _launchButton.Destroy();
        }
    }
}
