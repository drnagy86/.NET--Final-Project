using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubricNorming
{
    internal class DialogControls
    {
        internal static ContentDialogResult DialogResult { get; set; }

        public static async void OneButton(string title, string content, string closeButtonText = "Okay")
        {
            ContentDialog oneButton = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = closeButtonText
            };

            DialogResult = await oneButton.ShowAsync();
        }

        public static async void OkayCancel(string title, string content, string primaryButtonText = "Okay", string closeButtonText = "Cancel")
        {
            ContentDialog oneButton = new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = primaryButtonText,
                CloseButtonText = closeButtonText
            };

            DialogResult = await oneButton.ShowAsync();
        }

        public static async void YesNo(string title, string content, string primaryButtonText = "Yes", string closeButtonText = "No")
        {
            ContentDialog oneButton = new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = primaryButtonText,
                CloseButtonText = closeButtonText
            };

            DialogResult = await oneButton.ShowAsync();
        }
    }
}
