using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Xmazon
{
	public partial class Register : ContentPage
	{
		public Register ()
		{
			InitializeComponent ();
		}

		async void OnSubscibeButtonClicked (object sender, EventArgs e)
		{
			await Navigation.PopAsync ();
		}
	}
}

