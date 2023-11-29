using System.Windows.Controls;

namespace SA3D.Rendering.WPF.XAML
{
	/// <summary>
	/// Interaction logic for UcSettingsCategory.xaml
	/// </summary>
	public partial class UcSettingsCategory : UserControl
	{
		/// <summary>
		/// Title of the category.
		/// </summary>
		public string Title { get; }

		/// <summary>
		/// Creates a new title for an input setting category.
		/// </summary>
		/// <param name="title"></param>
		public UcSettingsCategory(string title)
		{
			Title = title;
			InitializeComponent();
		}
	}
}
