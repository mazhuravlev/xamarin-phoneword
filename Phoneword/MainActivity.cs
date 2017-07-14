using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Telephony;

namespace Phoneword
{
    [Activity(Label = "Phoneword", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            var phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            var translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            var callButton = FindViewById<Button>(Resource.Id.CallButton);

            callButton.Enabled = false;

            var translatedNumber = "";

            translateButton.Click += (sender, args) =>
            {

                translatedNumber = PhonewordTranslator.ToNumber(phoneNumberText.Text);
                if (string.IsNullOrEmpty(translatedNumber))
                {
                    callButton.Enabled = false;
                    callButton.Text = "Call";
                }
                else
                {
                    callButton.Enabled = true;
                    callButton.Text = $"Call {translatedNumber}";
                }
            };

            callButton.Click += (sender, args) =>
            {
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Call " + translatedNumber + "?");
                callDialog.SetNeutralButton("Call", delegate
                {
                    var callIntent = new Intent(Intent.ActionCall);
                    callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
                    StartActivity(callIntent);
                });
                callDialog.SetNegativeButton("Cancel", delegate { });

                callDialog.Show();
            };
        }
    }
}

