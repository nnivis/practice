using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Reflection;
using Microsoft.Win32;
using System.IO;

namespace Reflection
{
    public partial class MainWindow : Window
    {
        Assembly asm;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.dll)|*.dll|All files (*.dll)|*.dll";
            if (openFileDialog.ShowDialog() == true)
            {
                string sSelectedFile = openFileDialog.FileName;
                asm = Assembly.LoadFrom(sSelectedFile); //загружаем динамически сборку
                FileName.Text = sSelectedFile;
                Type[] types = asm.GetTypes();
                foreach (Type t in types) //начинаем выводить данные
                {
                    txtEditor.Text += "---Класс: " + t.Name + "---\n"; //пишем имена найденных классов в загруженном файле
                    txtEditor.Text += "Свойства:\n";
                    foreach (PropertyInfo prop in t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static))
                    {
                        txtEditor.Text += $"{prop.PropertyType} {prop.Name} {{"; //пишем Тип и Имя_свойства
                        if (prop.CanRead) txtEditor.Text += "get;"; // если свойство доступно для чтения
                        if (prop.CanWrite) txtEditor.Text += "set;"; // если свойство доступно для записи
                        txtEditor.Text += "}\n";
                    }
                    txtEditor.Text += "Методы:\n";
                    foreach (MethodInfo method in t.GetMethods())
                    {
                        txtEditor.Text += $"{method.ReturnType.Name} {method.Name} (";
                        //получаем все параметры
                        ParameterInfo[] parameters = method.GetParameters();
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            var param = parameters[i]; //получаем модификаторы
                            string modificator = "";
                            if (param.IsIn) modificator = "in";
                            else if (param.IsOut) modificator = "out";

                            txtEditor.Text += $"{param.ParameterType.Name} {modificator} {param.Name}";
                            // если параметр имеет значение по умолчанию
                            if (param.HasDefaultValue) txtEditor.Text += $"={param.DefaultValue}";
                            // если не последний параметр, добавляем запятую
                            if (i < parameters.Length - 1) txtEditor.Text += ", ";
                        }
                        txtEditor.Text += ")\n";
                    }
                }
            }
        }
        private void btnMethodComplete_Click(object sender, RoutedEventArgs e)
        {
            if (txtEditor.Text == "") MessageBox.Show("Вы не загрузили файл");
            else
            {
                if (MethodText.Text.IndexOf('.') == -1 || MethodText.Text.IndexOf('(') == -1 || MethodText.Text.IndexOf(')') == -1) MessageBox.Show("Убедитесь, что вы правильно написали команду: Имя_типа.Имя_Метода(арг1, арг2,...)");
                else
                {
                    if (MethodText.Text != "")
                    {
                        var TypeClass = MethodText.Text.Substring(0, MethodText.Text.IndexOf('.'));
                        int k = MethodText.Text.IndexOf('.');
                        var MethodName = MethodText.Text.Substring(k + 1, MethodText.Text.IndexOf('(') - k - 1);
                        int p = MethodText.Text.IndexOf('(');
                        var Param = MethodText.Text.Substring(p + 1, MethodText.Text.IndexOf(')') - p - 1);
                        string[] param = new string[3];  //проблема в параметрах
                        int j = 0;
                        for (int i = p + 1; i < MethodText.Text.IndexOf(')') - p - 1; i++)
                        {
                            param[j] = MethodText.Text.Substring(i, MethodText.Text.IndexOf(',') - i - 2);
                            j++;
                        }
                        ///////////////////////////
                        Type[] types = asm.GetTypes();
                        MethodInfo t;
                        foreach (Type ty in types) //начинаем выполнять метод
                        {
                            if (Convert.ToString(ty).Substring(18, Convert.ToString(ty).Length - 18) == TypeClass)
                            {
                                t = ty.GetMethod(MethodName);
                                if (Convert.ToString(ty.GetMethod(MethodName)) == "") { MessageBox.Show("Вы ввели несуществующий метод"); break; }
                                else
                                {
                                    if (t.ReturnType.Name == "Void") { MessageBox.Show("Результат: OK"); break; }
                                    else
                                    {
                                        t.Invoke(ty, param);
                                        MessageBox.Show(Convert.ToString(t.Invoke(ty, param)));
                                        break;
                                    }
                                }
                            }
                            else { MessageBox.Show("Вы ввели несуществующий класс"); break; }
                        }
                    }
                    else MessageBox.Show("Вы не заполнили текстбокс");
                }
            }
        }
    }
}
//необходимо исправить у второй кнопки:
//1) улучшить проверку правильности заполнения второго текстбокса (валид-я?)
//2) подобрать другой способ вытягивания названия методов из классов (число 18)
//! 3) дописать выполнение методов с аргументами (и проверку неверно введенных аргументов в текстбокс - это относится к 1 пункту)
//
//необходимо исправить в работе:
//1) улучшить вид кода в MainVVindovv.xaml
//?
