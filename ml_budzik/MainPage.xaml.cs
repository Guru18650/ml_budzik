using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ml_budzik
{
    public partial class MainPage : ContentPage
    {
        private Timer timer;

        public ObservableCollection<Alarm> Alarms = new ObservableCollection<Alarm>();
        public MainPage()
        {
            InitializeComponent();
            loadAlarms();
            list.ItemsSource = Alarms;

            timer = new Timer(1000);
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        public void saveAlarms()
        {
            Preferences.Set("alarms1", JsonConvert.SerializeObject(Alarms));
        }
        public void loadAlarms()
        {
            Alarms = JsonConvert.DeserializeObject<ObservableCollection<Alarm>>(Preferences.Get("alarms1", "[]"));
        }
        private void new_Clicked(object sender, EventArgs e)
        {
            Alarms.Add(new Alarm() { Name = "Nowy", On = false, Time = DateTime.Now.TimeOfDay });
            saveAlarms();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            Grid g = (Grid)bt.Parent;
            CheckBox cb = (CheckBox)g.Children[0];
            Entry ee = (Entry)g.Children[1];
            TimePicker tp = (TimePicker)g.Children[2];
            Alarms.Remove(Alarms.First(item => item.Name == ee.Text && item.Time == tp.Time && item.On == cb.IsChecked));
            saveAlarms() ;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                bP.Title = "Budzik ("+DateTime.Now.ToString("HH:mm:ss")+")";
                foreach (var item in Alarms)
                {
                    if (item.On && item.Time.Minutes == DateTime.Now.TimeOfDay.Minutes && item.Time.Hours == DateTime.Now.TimeOfDay.Hours && item.Time.Seconds == DateTime.Now.TimeOfDay.Seconds)
                    {
                        await Navigation.PushAsync(new AlarmPage(item.Name));
                        break;
                    }
                }
            });
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            saveAlarms();
        }

        private void TimePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            saveAlarms();
        }
    }

    public class Alarm
    {
        public string Name { get; set; }
        public TimeSpan Time { get; set; }
        public bool On {  get; set; }

    }
}