using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
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
using corel = Corel.Interop.VGCore;

namespace GameInCorel
{

    public partial class DockerUI : UserControl
    {
        public static corel.Application corelApp;
        private Game game;
        private corel.Document doc;
        public DockerUI(corel.Application app)
        {
            DockerUI.corelApp = app;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           // corel.StructCreateOptions opt = new corel.StructCreateOptions();
           // opt.Units = corel.cdrUnit.cdrPixel;
           // opt.Name = "Game";
           // opt.Resolution = 96;
           // //opt.PageHeight = 25.0;
           //// opt.PageWidth = 25.0;



             doc = DockerUI.corelApp.CreateDocument();
            doc.Unit = corel.cdrUnit.cdrPixel;
            doc.Name = "Game";
            doc.ActivePage.SizeWidth = 250;
            doc.ActivePage.SizeHeight = 250;
            //doc.ActivePage.Resolution = 96;

            this.game = new Game();

            DockerUI.corelApp.ActiveDocument.DrawingOriginX += DockerUI.corelApp.ActivePage.LeftX;
            DockerUI.corelApp.ActiveDocument.DrawingOriginY += DockerUI.corelApp.ActivePage.BottomY;


            this.game.Start();


         
        }

        
    }
}
