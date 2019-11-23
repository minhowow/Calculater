using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Calculater;

namespace CalculaterWPF
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Block.TryParse(textbox.Text, out var block)&&double.TryParse(multiplyerBox.Text,out var multiplyer))
                {
                    textboxShow.Content = "f(x) = "+block.ToString();
                    canvas.Children.Clear();
                    double width = ActualWidth / 2;
                    double height = ActualHeight / 2;
                    double gap = 1;
                    canvas.Children.Add(new Line()
                    {
                        X1 = -width,
                        X2 = width,
                        Y1 = 0,
                        Y2 = 0,
                        Stroke = Brushes.Black,
                        StrokeThickness = 2
                    });
                    canvas.Children.Add(new Line()
                    {
                        X1 = 0,
                        X2 = 0,
                        Y1 = -height,
                        Y2 = height,
                        Stroke = Brushes.Black,
                        StrokeThickness = 2
                    });
                    for (int i = -(int)(height / multiplyer); i <= (int)(height / multiplyer); i++)
                    {
                        if (i != 0)
                        {
                            canvas.Children.Add(new Line()
                            {
                                X1 = -width,
                                X2 = width,
                                Y1 = i * multiplyer,
                                Y2 = i * multiplyer,
                                Stroke = Brushes.LightGray
                            });
                        }
                    }
                    for (int i = -(int)(width / multiplyer); i <= (int)(width / multiplyer); i++)
                    {
                        if (i != 0)
                        {
                            canvas.Children.Add(new Line()
                            {
                                X1 = i * multiplyer,
                                X2 = i * multiplyer,
                                Y1 = -height,
                                Y2 = height,
                                Stroke = Brushes.LightGray
                            });
                        }
                    }
                    for (double i = -width; i <= width; i += gap)
                    {
                        canvas.Children.Add(new Line()
                        {
                            X1 = i,
                            X2 = i + gap,
                            Y1 = block.Calculate(i / multiplyer) * multiplyer,
                            Y2 = block.Calculate((i + gap) / multiplyer) * multiplyer,
                            Stroke = Brushes.Red
                        });
                    }
                }
            }
            catch (Exception)
            {
                textboxShow.Content += "(오류)";
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TextBox_TextChanged(null, null);
        }
    }
}
