using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DaraMemo.Enums {

    public static class StatusExtensions {
        public static ImageSource GetIcon(this Status status) {
            var key = status switch {
                Status.Active => "ActiveIcon",
                Status.Afk => "AfkIcon",
                Status.Break => "BreakIcon",
                _ => throw new ArgumentOutOfRangeException()
            };

            return (ImageSource)Application.Current.Resources[key];
        }
    }

}
