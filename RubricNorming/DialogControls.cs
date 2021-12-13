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

        public static async void OneButton(string title, string content, string closeButtonText)
        {
            ContentDialog oneButton = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = closeButtonText
            };

            DialogResult = await oneButton.ShowAsync();
        }
    }
}
