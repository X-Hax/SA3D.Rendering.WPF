using SA3D.Rendering.Input;
using SA3D.Rendering.Input.Settings;
using System;
using System.Configuration;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace SA3D.Rendering.WPF.XAML
{
	/// <summary>
	/// Input setting control.
	/// </summary>
	public partial class UcInputSetting : UserControl
	{
		private readonly PropertyInfo _property;
		private readonly InputCode _defaultCode;
		private readonly InputCodeAttribute _attribute;
		private readonly WndInputSettings _window;

		/// <summary>
		/// Currently set input code.
		/// </summary>
		public InputCode Code
		{
			get => (InputCode)(_property.GetValue(_window.Settings) ?? InputCode.None);
			set => _property.SetValue(_window.Settings, value);
		}

		/// <summary>
		/// All available input codes.
		/// </summary>
		public static InputCode[] InputCodes;

		static UcInputSetting()
		{
			InputCodes = Enum.GetValues<InputCode>();
		}

		/// <summary>
		/// Creates a new input setting.
		/// </summary>
		/// <param name="window"></param>
		/// <param name="field"></param>
		/// <exception cref="InvalidOperationException"></exception>
		public UcInputSetting(WndInputSettings window, PropertyInfo field)
		{
			_window = window;

			_property = field;
			_attribute = field.GetCustomAttribute<InputCodeAttribute>() 
				?? throw new InvalidOperationException();

			DefaultSettingValueAttribute defaultSetting = _property.GetCustomAttribute<DefaultSettingValueAttribute>() 
				?? throw new InvalidOperationException();

			_defaultCode = Enum.Parse<InputCode>(defaultSetting.Value);

			ToolTip = _attribute.Description;
			InitializeComponent();

			OptionName.Text = _attribute.Name;
		}

		private void Reset_Click(object sender, RoutedEventArgs e)
		{
			InputCodeSelection.SelectedItem = _defaultCode;
		}

		private void Record_Click(object sender, RoutedEventArgs e)
		{
			RecordButton.Visibility = Visibility.Collapsed;
			RecordText.Visibility = Visibility.Visible;
			_window.Recording = this;
		}

		internal void FinishRecording()
		{
			RecordButton.Visibility = Visibility.Visible;
			RecordText.Visibility = Visibility.Collapsed;
		}

	}
}
