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
                if (Block.TryParse(textbox.Text, out var block)&&double.TryParse(XmultiplyerBox.Text,out var Xmultiplyer) && double.TryParse(YmultiplyerBox.Text, out var Ymultiplyer))
                {
                    double width = ActualWidth / 2;
                    double height = ActualHeight / 2;
                    double gap = 1;
                    textboxShow.Content = "f(x) = "+block.ToString();
                    canvas.Children.Clear();

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
                    for (int i = -(int)(height / Ymultiplyer); i <= (int)(height / Ymultiplyer); i++)
                    {
                        if (i != 0)
                        {
                            canvas.Children.Add(new Line()
                            {
                                X1 = -width,
                                X2 = width,
                                Y1 = i * Ymultiplyer,
                                Y2 = i * Ymultiplyer,
                                Stroke = Brushes.LightGray
                            });
                        }
                    }
                    for (int i = -(int)(width / Xmultiplyer); i <= (int)(width / Xmultiplyer); i++)
                    {
                        if (i != 0)
                        {
                            canvas.Children.Add(new Line()
                            {
                                X1 = i * Xmultiplyer,
                                X2 = i * Xmultiplyer,
                                Y1 = -height,
                                Y2 = height,
                                Stroke = Brushes.LightGray
                            });
                        }
                    }
                    for (double i = -width; i <= width; i += gap)
                    {
                        try
                        {
                            canvas.Children.Add(new Line()
                            {
                                X1 = i,
                                X2 = i + gap,
                                Y1 = block.Calculate(i / Xmultiplyer) * Ymultiplyer,
                                Y2 = block.Calculate((i + gap) / Xmultiplyer) * Ymultiplyer,
                                Stroke = Brushes.Red
                            });
                        }
                        catch (Exception)
                        {


                        }
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
