using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace RaysHotDogs
{
    [Activity(Label = "RaysHotDogs", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private Button orderButton;
        private Button cartButton;
        private Button aboutButton;
        private Button mapButton;
        private Button takePictureButton;

        protected override void OnCreate(Bundle savedInstancesState)
        {
            base.OnCreate(savedInstancesState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.MainMenu);

            FindViews();

            HandleEvents();
        }

        private void FindViews()
        {
            orderButton = FindViewById<Button>(Resource.Id.orderButton);
            cartButton = FindViewById<Button>(Resource.Id.cartButton);
            aboutButton = FindViewById<Button>(Resource.Id.aboutButton);
            mapButton = FindViewById<Button>(Resource.Id.mapButton);
            takePictureButton = FindViewById<Button>(Resource.Id.takePictureButton);
        }

        private void HandleEvents()
        {
            orderButton.Click += OrderButton_Click;
            aboutButton.Click += AboutButton_Click;
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(AboutActivity));
            StartActivity(intent);
        }

        private void OrderButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(HotDogMenuActivity));
            StartActivity(intent);
        }
    }
}

