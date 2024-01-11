namespace ml_budzik;

public partial class AlarmPage : ContentPage
{
	public AlarmPage(string name)
	{
		InitializeComponent();
		ll.Text = name;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		Navigation.PopAsync();
    }
}