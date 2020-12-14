using PersonalAgenda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PersonalAgenda
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityPage : ContentPage
    {
        Agenda a;
        public ActivityPage(Agenda agenda)
        {
            InitializeComponent();
            a = agenda;
        }
        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var activity = (Activity)BindingContext;
            await App.Database.SaveActivityAsync(activity);
            listView.ItemsSource = await App.Database.GetActivitiesAsync();
        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            Activity act;
            if (e.SelectedItem != null)
            {
                act = e.SelectedItem as Activity;
                var nactivity = new NoteActivity()
                {
                    AgendaID = a.ID,
                    ActivityID = act.ID
                };
                await App.Database.SaveNoteActivityAsync(nactivity);
                act.NoteActivities = new List<NoteActivity> { nactivity };

                await Navigation.PopAsync();
            }
        }

            async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var activity = (Activity)BindingContext;
            await App.Database.DeleteActivityAsync(activity);
            listView.ItemsSource = await App.Database.GetActivitiesAsync();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listView.ItemsSource = await App.Database.GetActivitiesAsync();
        }


        }
}