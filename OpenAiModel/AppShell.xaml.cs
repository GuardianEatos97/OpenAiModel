using OpenAiModel.Views;
namespace OpenAiModel
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute("loadsheddingquestion", typeof(YodaQuestionPage));
            Routing.RegisterRoute("loadsheddinganswer", typeof(YodaAnswerPage));
        }
    }
}
