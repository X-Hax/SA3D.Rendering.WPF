using SA3D.Rendering.Input;
using SA3D.Rendering.Input.Settings;
using System.Configuration;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SA3D.Rendering.WPF.XAML
{
	/// <summary>
	/// Window for configuring input settings. Gets automatically generated for the supplied input settings.
	/// </summary>
	public class WndInputSettings : Window
	{
		private UcInputSetting? _recording;

		internal UcInputSetting? Recording
		{
			get => _recording;
			set
			{
				_recording?.FinishRecording();
				_recording = value;
			}
		}

		internal ApplicationSettingsBase Settings { get; }

		/// <summary>
		/// Creates a new window for configuring input settings.
		/// </summary>
		/// <param name="settings">The settings to configure.</param>
		public WndInputSettings(ApplicationSettingsBase settings)
		{
			// This window is being generated, so that the controls dont need to be handled manually
			// the scrollviewer and stackpanel are not worth to create an xaml file for tbh
			Title = "Control Settings";
			Width = 410;
			Height = 650;
			MinWidth = Width;
			MaxWidth = Width;
			Closing += (e, o) => settings.Save();
			Settings = settings;

			ScrollViewer scroll = new()
			{
				VerticalScrollBarVisibility = ScrollBarVisibility.Auto
			};
			Content = scroll;

			StackPanel container = new();
			scroll.Content = container;

			UcSettingsCategory? panel = null;

			PropertyInfo[] props = settings.GetType().GetTypeInfo().GetProperties();
			foreach(PropertyInfo field in props)
			{
				InputCodeCategoryAttribute? titleAttr = field.GetCustomAttribute<InputCodeCategoryAttribute>();
				if(titleAttr != null)
				{
					panel = new(titleAttr.Title);
					container.Children.Add(panel);
				}

				InputCodeAttribute? attr = field.GetCustomAttribute<InputCodeAttribute>();
				if(attr != null)
				{
					panel?.Container.Children.Add(new UcInputSetting(this, field));
				}
			}

		}

		/// <inheritdoc/>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if(Recording != null)
			{
				Recording.InputCodeSelection.SelectedItem = (InputCode)e.Key;
				Recording = null;
			}
		}
	}
}
